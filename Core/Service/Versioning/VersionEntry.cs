using System;
using System.Collections.Generic;
using System.Text;

using Cuyahoga.Core.Domain;

namespace Cuyahoga.Core.Service.Versioning
{
    public class VersionEntry
    {
        protected long id;
		protected string entryType;
        protected string entryKey;
        protected string entryValue;
        protected IList<int> versions;
        ContentItem versionedItem;
		
		#region Properties

        public virtual long Id
        {
            get { return this.id; }
            set { this.id = value; }
        }

        public virtual string EntryType
		{
			get{ return this.entryType; }
			set{ this.entryType = value; }
		}

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

        public virtual IList<int> Versions
        {
            get { return this.versions; }
            set { this.versions = value; }
        }

        public virtual ContentItem VersionedItem
        {
            get { return this.versionedItem; }
            set { this.versionedItem = value; }
        }


		#endregion

	
    }
}
