using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Page2Feed.Core.Model.Atom
{
    /// <remarks/>
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.w3.org/2005/Atom")]
    public class feedEntry
    {

        private string titleField;

        private feedEntryLink[] linkField;

        private string idField;

        private string updatedField;

        private string summaryField;

        private feedEntryContent contentField;

        private feedEntryAuthor authorField;

        /// <remarks/>
        public string title
        {
            get => titleField;
            set => titleField = value;
        }

        /// <remarks/>
        [XmlElement("link")]
        public feedEntryLink[] link
        {
            get => linkField;
            set => linkField = value;
        }

        /// <remarks/>
        public string id
        {
            get => idField;
            set => idField = value;
        }

        /// <remarks/>
        public string updated
        {
            get => updatedField;
            set => updatedField = value;
        }

        /// <remarks/>
        public string summary
        {
            get => summaryField;
            set => summaryField = value;
        }

        /// <remarks/>
        public feedEntryContent content
        {
            get => contentField;
            set => contentField = value;
        }

        /// <remarks/>
        public feedEntryAuthor author
        {
            get => authorField;
            set => authorField = value;
        }
    }
}