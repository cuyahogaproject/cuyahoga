using System;
using Cuyahoga.Core.Util;
using Lucene.Net.Index;
using Lucene.Net.Documents;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Store;
using log4net;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Search;

namespace Cuyahoga.Core.Service.Search
{
	/// <summary>
	/// The IndexBuilder class takes care of maintaining the fulltext index.
	/// </summary>
	public class IndexBuilder : IDisposable
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(IndexBuilder));

		private readonly ITextExtractor _textExtractor;
		private readonly Directory _indexDirectory;
		private IndexWriter _indexWriter;
		private bool _isClosed = false;
		private readonly bool _rebuildIndex;

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="physicalIndexDir">Location of the index files.</param>
		/// <param name="rebuildIndex">Flag to indicate if the index should be rebuilt. 
		/// <param name="textExtractor">The text extractor that can be used to extract text from content.</param>
		public IndexBuilder(string physicalIndexDir, bool rebuildIndex, ITextExtractor textExtractor)
		{
			this._indexDirectory = FSDirectory.GetDirectory(physicalIndexDir, false);
			this._rebuildIndex = rebuildIndex;
			this._textExtractor = textExtractor;

			InitIndexWriter();

			log.Info("IndexBuilder created.");
		}

		/// <summary>
		/// Initialize the IndexWriter depending on the rebuildIndex flag
		/// </summary>
		private void InitIndexWriter()
		{
			if (!IndexReader.IndexExists(this._indexDirectory) || this._rebuildIndex)
			{
				this._indexWriter = new IndexWriter(this._indexDirectory, new StandardAnalyzer(), true);
			}
			else
			{
				this._indexWriter = new IndexWriter(this._indexDirectory, new StandardAnalyzer(), false);
			}
		}

		/// <summary>
		/// Add content to be indexed.
		/// </summary>
		/// <param name="searchContent"></param>
		public void AddContent(SearchContent searchContent)
		{
			if (this._indexWriter == null)
			{
				InitIndexWriter();
			}
			this._indexWriter.AddDocument(BuildDocumentFromSearchContent(searchContent));
		}

		public void AddContent(IContentItem contentItem)
		{
			if (this._indexWriter == null)
			{
				InitIndexWriter();
			}
			this._indexWriter.AddDocument(BuildDocumentFromContentItem(contentItem, this._textExtractor));
		}

		/// <summary>
		/// Update existing content in the search index.
		/// </summary>
		/// <param name="searchContent"></param>
		public void UpdateContent(SearchContent searchContent)
		{
			if (this._rebuildIndex)
			{
				throw new InvalidOperationException("Cannot update documents when rebuilding the index.");
			}
			else
			{
				// First delete the old content
				DeleteContent(searchContent);
				// Now add the content again
				AddContent(searchContent);
			}
		}

		public void UpdateContent(IContentItem contentItem)
		{
			if (this._rebuildIndex)
			{
				throw new InvalidOperationException("Cannot update documents when rebuilding the index.");
			}
			else
			{
				// First delete the old content
				DeleteContent(contentItem);
				// Now add the content again
				AddContent(contentItem);
			}
		}

		/// <summary>
		/// Delete existing content from the search index.
		/// </summary>
		/// <param name="searchContent"></param>
		public void DeleteContent(SearchContent searchContent)
		{
			if (this._rebuildIndex)
			{
				throw new InvalidOperationException("Cannot delete documents when rebuilding the index.");
			}
			else
			{
				this._indexWriter.Close();
				this._indexWriter = null;

				// The path uniquely identifies a document in the index.
				Term term = new Term("path", searchContent.Path);
				IndexReader rdr = IndexReader.Open(this._indexDirectory);
				rdr.DeleteDocuments(term);
				rdr.Close();
			}
		}

		/// <summary>
		/// Delete existing content from the search index.
		/// </summary>
		/// <param name="contentItem"></param>
		public void DeleteContent(IContentItem contentItem)
		{
			if (this._rebuildIndex)
			{
				throw new InvalidOperationException("Cannot delete documents when rebuilding the index.");
			}
			else
			{
				this._indexWriter.Close();
				this._indexWriter = null;

				Term term = new Term("globalid", contentItem.GlobalId.ToString("N"));
				IndexReader rdr = IndexReader.Open(this._indexDirectory);
				rdr.DeleteDocuments(term);
				rdr.Close();
			}
		}

		/// <summary>
		/// Close the IndexWriter (saves the index).
		/// </summary>
		public void Close()
		{
			if (!this._isClosed && this._indexWriter != null)
			{
				//don't do this everytime 
				//this._indexWriter.Optimize();
				this._indexWriter.Close();
				this._isClosed = true;
				log.Info("New or updated search index written to disk.");
			}
		}


		private Document BuildDocumentFromContentItem(IContentItem contentItem, ITextExtractor textExtractor)
		{
			ISearchableContent searchInfo = contentItem as ISearchableContent;
			if (searchInfo == null) throw new ArgumentException("Argument must implement ISearchableContent");

			// Get the text of the content item to index
			string contentToIndex = searchInfo.ToSearchContent(textExtractor);
			// strip (x)html tags
			string plainTextContent = System.Text.RegularExpressions.Regex.Replace(contentToIndex, @"<(.|\n)*?>", string.Empty);
			// create the actual url
			string path = contentItem.GetContentUrl();
			// check that summary is not null. 
			string summary = contentItem.Summary ?? Text.TruncateText(plainTextContent, 200);

			Document doc = new Document();
			doc.Add(new Field("globalid", contentItem.GlobalId.ToString("N"), Field.Store.YES, Field.Index.UN_TOKENIZED));
			doc.Add(new Field("title", contentItem.Title, Field.Store.YES, Field.Index.TOKENIZED));
			doc.Add(new Field("summary", summary, Field.Store.YES, Field.Index.TOKENIZED));
			doc.Add(new Field("contents", plainTextContent, Field.Store.NO, Field.Index.TOKENIZED));
			doc.Add(new Field("author", contentItem.CreatedBy.FullName, Field.Store.YES, Field.Index.TOKENIZED));
			doc.Add(new Field("moduletype", contentItem.Section.ModuleType.Name, Field.Store.YES, Field.Index.UN_TOKENIZED));
			doc.Add(new Field("path", path, Field.Store.YES, Field.Index.UN_TOKENIZED));
			doc.Add(new Field("site", contentItem.Section.Node.Site.Id.ToString(), Field.Store.YES, Field.Index.UN_TOKENIZED));
			doc.Add(new Field("datecreated", contentItem.CreatedAt.ToString("u"), Field.Store.YES, Field.Index.UN_TOKENIZED));
			doc.Add(new Field("datemodified", contentItem.ModifiedAt.ToString("u"), Field.Store.YES, Field.Index.UN_TOKENIZED));
			if (contentItem.PublishedAt.HasValue)
			{
				doc.Add(new Field("datepublished", contentItem.PublishedAt.Value.ToString("u"), Field.Store.YES,
				                  Field.Index.UN_TOKENIZED));
			}
			// do not index the sectionid here (since it's used for access filtering)
			doc.Add(new Field("sectionid", contentItem.Section.Id.ToString(), Field.Store.YES, Field.Index.NO));

			foreach (Category cat in contentItem.Categories)
			{
				doc.Add(new Field("category", cat.Name, Field.Store.YES, Field.Index.UN_TOKENIZED));
			}

			foreach (Role viewRole in contentItem.ViewRoles)
			{
				doc.Add(new Field("viewroleid", viewRole.Id.ToString(), Field.Store.YES, Field.Index.UN_TOKENIZED));
			}

			foreach (CustomSearchField field in searchInfo.GetCustomSearchFields())
			{
				Field.Store store = field.IsStored ? Field.Store.YES : Field.Store.NO;
				Field.Index index = field.IsTokenized ? Field.Index.TOKENIZED : Field.Index.UN_TOKENIZED;
				if (field.FieldKey != null && field.FieldValue != null)
				{
					doc.Add(new Field(field.FieldKey, field.FieldValue, store, index));
				}
			}
			return doc;
		}

		private Document BuildDocumentFromSearchContent(SearchContent searchContent)
		{
			Document doc = new Document();
			doc.Add(new Field("title", searchContent.Title, Field.Store.YES, Field.Index.TOKENIZED));
			doc.Add(new Field("summary", searchContent.Summary, Field.Store.YES, Field.Index.TOKENIZED));
			doc.Add(new Field("contents", searchContent.Contents, Field.Store.NO, Field.Index.TOKENIZED));
			doc.Add(new Field("author", searchContent.Author, Field.Store.YES, Field.Index.TOKENIZED));
			doc.Add(new Field("moduletype", searchContent.ModuleType, Field.Store.YES, Field.Index.UN_TOKENIZED));
			doc.Add(new Field("path", searchContent.Path, Field.Store.YES, Field.Index.UN_TOKENIZED));
			doc.Add(new Field("category", searchContent.Category, Field.Store.YES, Field.Index.UN_TOKENIZED));
			doc.Add(new Field("site", searchContent.Site, Field.Store.YES, Field.Index.UN_TOKENIZED));
			doc.Add(new Field("datecreated", searchContent.DateCreated.ToString("u"), Field.Store.YES, Field.Index.UN_TOKENIZED));
			doc.Add(new Field("datemodified", searchContent.DateModified.ToString("u"), Field.Store.YES, Field.Index.UN_TOKENIZED));
			doc.Add(new Field("sectionid", searchContent.SectionId.ToString(), Field.Store.YES, Field.Index.UN_TOKENIZED));

			return doc;
		}

		#region IDisposable Members

		public void Dispose()
		{
			Close();
		}

		#endregion
	}
}
