using System;
using System.Web;

using NUnit.Framework;
using Gentle.Framework;

using Cuyahoga.Core;
using Cuyahoga.Core.DAL;
using Cuyahoga.Modules;
using Cuyahoga.Modules.Articles;

namespace Cuyahoga.Test
{
	/// <summary>
	/// Summary description for ArticleTest.
	/// </summary>
	[TestFixture]
	public class ArticleTest
	{
		private Node _node;
		private Section _section;
		private User _admin;
		private User _author;

		[SetUp]
		public void Init()
		{
			// Setup a Cuyahoga context for the tests.
			
			this._admin = new User();
			this._admin.UserName = "testadmin";
			this._admin.Password = "fhjkd838";
			this._admin.Email = "testadmin@cuyahoga.org";
			CmsDataFactory.GetInstance().InsertUser(this._admin);
			
			this._author = new User();
			this._author.UserName = "testauthor";
			this._author.Password = "fhj38nd8";
			this._author.Email = "testauthor@cuyahoga.org";
			CmsDataFactory.GetInstance().InsertUser(this._author);
			
			this._node = new Node();
			this._node.Title = "testnode";
			this._node.ShortDescription = "testnode";
			this._node.Position = 9999;
			CmsDataFactory.GetInstance().InsertNode(this._node);

			this._section = new Section();
			this._section.Module = new ArticleModule();
			this._section.Module.ModuleId = 2; // HACK, Module.Id hardcoded
			this._section.Node = this._node;
			this._section.Title = "testsection";
			this._section.Position = 9999;
			CmsDataFactory.GetInstance().InsertSection(this._section);
		}

		[TearDown]
		public void Clear()
		{
			// Delete the Cuyahoga context.
			// We're assuming that all tests went well and no Articles are related.
			CmsDataFactory.GetInstance().DeleteSection(this._section);
			CmsDataFactory.GetInstance().DeleteNode(this._node);
			CmsDataFactory.GetInstance().DeleteUser(this._author);
			CmsDataFactory.GetInstance().DeleteUser(this._admin);
		}

		[Test]
		public void TestCRUD()
		{
			// New article without category
			Article article = new Article();
            article.Title = "testarticle";
			article.Content = "testcontent";
			article.Syndicate = false;
			article.DateOnline = DateTime.Now.AddDays(1);
			article.DateOffline = DateTime.Now.AddDays(50);
			article.CreatedBy = this._author;
			article.Section = this._section;
			Broker.Persist(article);
			Assert.IsTrue(article.Id != 0, "No id generated for the Article just inserted!" );
			// Retieve the new article
//			Key key = new Key(typeof(Article), true, "Id", article.Id);
//			Article newArticle = Broker.RetrieveInstance(typeof(Article), key) as Article;
//			Assert.AreEqual(article.Id, newArticle.Id);

            // Delete the article
			Broker.Remove(article);
		}
	}
}
