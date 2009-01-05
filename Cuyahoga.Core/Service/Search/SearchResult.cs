using System;

namespace Cuyahoga.Core.Service.Search
{
    /// <summary>
    /// Class that holds the result of a single hit.
    /// </summary>
    public class SearchResult
    {
        private string _title;
        private string _summary;
        private string _author;
        private string _moduleType;
        private string _path;
        private string _category;
        private DateTime _dateCreated;
        private float _score;
        private float _boost;
        private int _sectionId;

        /// <summary>
        /// Property Title (string)
        /// </summary>
        public string Title
        {
            get { return this._title; }
            set { this._title = value; }
        }

        /// <summary>
        /// Property Summary (string)
        /// </summary>
        public string Summary
        {
            get { return this._summary; }
            set { this._summary = value; }
        }

        /// <summary>
        /// Property Author (string)
        /// </summary>
        public string Author
        {
            get { return this._author; }
            set { this._author = value; }
        }

        /// <summary>
        /// Property ModuleType (string)
        /// </summary>
        public string ModuleType
        {
            get { return this._moduleType; }
            set { this._moduleType = value; }
        }

        /// <summary>
        /// Property Path (string)
        /// </summary>
        public string Path
        {
            get { return this._path; }
            set { this._path = value; }
        }

        /// <summary>
        /// Property Category (string)
        /// </summary>
        public string Category
        {
            get { return this._category; }
            set { this._category = value; }
        }

        /// <summary>
        /// Property DateCreated (DateTime)
        /// </summary>
        public DateTime DateCreated
        {
            get { return this._dateCreated; }
            set { this._dateCreated = value; }
        }

        /// <summary>
        /// Property Score (float)
        /// </summary>
        public float Score
        {
            get { return this._score; }
            set { this._score = value; }
        }

        /// <summary>
        /// Property Boost (float)
        /// </summary>
        public float Boost
        {
            get { return this._boost; }
            set { this._boost = value; }
        }

        /// <summary>
        /// Property SectionId (int)
        /// </summary>
        public int SectionId
        {
            get { return this._sectionId; }
            set { this._sectionId = value; }
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public SearchResult()
        {
        }
    }
}
