using System;
using System.Collections.Generic;
using System.Text;

namespace Cuyahoga.Core.Service.Versioning
{
    /// <summary>
    /// Provides meta information on how to version a given object
    /// </summary>
    public class VersioningInfo
    {
        private Type versionedType;
        private bool isList;
        private string idProperty;
        private VersioningInfo[] versionedChildTypes;

        /// <summary>
        /// Define child objects that are lists. Use the idProperty argument to indicate which 
        /// property defines the identifier for distinguishing objects in this list
        /// </summary>
        /// <param name="versionedType"></param>
        /// <param name="isList"></param>
        /// <param name="idProperty"></param>
        /// <param name="versionedChildTypes"></param>
        public VersioningInfo(Type versionedType, bool isList, string idProperty, params VersioningInfo[] versionedChildTypes)
        {
            this.versionedType = versionedType;
            this.isList = isList;
            this.idProperty = idProperty;
            this.versionedChildTypes = versionedChildTypes;
        }

        /// <summary>
        /// Define objects containing child objects that should get versioned as well
        /// </summary>
        /// <param name="versionedType"></param>
        /// <param name="versionedChildTypes"></param>
        public VersioningInfo(Type versionedType, params VersioningInfo[] versionedChildTypes)
            : this(versionedType, false, null, versionedChildTypes) { }

        /// <summary>
        /// Define flat objects that should get versioned
        /// </summary>
        /// <param name="versionedType"></param>
        public VersioningInfo(Type versionedType)
            : this(versionedType, false, null) { }

        /// <summary>
        /// The <see cref="System.Type">Type</see> of the object that should get versioned
        /// </summary>
        public Type VersionedType
        {
            get { return versionedType; }
        }

        /// <summary>
        /// True, if the <see cref="VersionedType">versioned type</see> is contained in a list
        /// </summary>
        public bool IsList
        {
            get { return isList; }
        }

        /// <summary>
        /// Name of the property that is used as identifier (not needed on flat objects)
        /// </summary>
        public string IdProperty
        {
            get { return idProperty; }
        }



        public VersioningInfo[] VersionedChildTypes
        {
            get { return versionedChildTypes; }
        }

    }
}
