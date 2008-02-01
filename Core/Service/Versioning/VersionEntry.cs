using System;
using System.Collections.Generic;
using System.Text;

using Cuyahoga.Core.Domain;

namespace Cuyahoga.Core.Service.Versioning
{
    public class VersionEntry
    {
       // protected long id;
		//protected string entryType;
        protected string entryKey;
        protected string entryValue;
		protected int validFromVersion;
		protected int? validToVersion;
        ContentItem versionedItem;
		
		#region Properties

		//public virtual long Id
		//{
		//    get { return this.id; }
		//    set { this.id = value; }
		//}


		//public virtual string EntryType
		//{
		//    get{ return this.entryType; }
		//    set{ this.entryType = value; }
		//}

		/// <summary>
		/// The entries key, e.g. Article.Comment[0].Text
		/// </summary>
        public virtual string EntryKey
        {
            get { return this.entryKey; }
            set { this.entryKey = value; }
        }

        public virtual string EntryValue
        {
            get { return this.entryValue; }
            set { this.entryValue = value; }
        }

        public virtual int ValidFromVersion
        {
            get { return this.validFromVersion; }
            set { this.validFromVersion= value; }
        }

		public virtual int ValidToVersion
		{
			get { return this.validToVersion; }
			set { this.validToVersion = value; }
		}

        public virtual ContentItem VersionedItem
        {
            get { return this.versionedItem; }
            set { this.versionedItem = value; }
        }


		#endregion

	
    }
}
