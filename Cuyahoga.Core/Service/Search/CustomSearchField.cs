using System;
using System.Collections.Generic;
using System.Text;

namespace Cuyahoga.Core.Service.Search
{
    /// <summary>
    /// This class provides information about an object's property
    /// defining the way it will be indexed/retrieved by the search service
    /// </summary>
    public class CustomSearchField
    {
        public CustomSearchField(string fieldKey, string fieldValue, bool isTokenized, bool isStored)
        {
            this.fieldKey = fieldKey;
            this.fieldValue = fieldValue;
            this.isTokenized = isTokenized;
            this.isStored = isStored;
        }

        private string fieldKey;
        private string fieldValue;
        private bool isTokenized;
        private bool isStored;

        /// <summary>
        /// Defines the key to be used to index/retrieve the value
        /// </summary>
        public string FieldKey
        {
            get { return fieldKey; }
            set { fieldKey = value; }
        }
        /// <summary>
        /// The actual value to be indexed
        /// </summary>
        public string FieldValue
        {
            get { return fieldValue; }
            set { fieldValue = value; }
        }
        /// <summary>
        /// Defines if the value should be split into tokens
        /// </summary>
        public bool IsTokenized
        {
            get { return isTokenized; }
            set { isTokenized = value; }
        }
        /// <summary>
        /// Defines if the original value should be stored in the index
        /// </summary>
        public bool IsStored
        {
            get { return isStored; }
            set { isStored = value; }
        }

    }
}
