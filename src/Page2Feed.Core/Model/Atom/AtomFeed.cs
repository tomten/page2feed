using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Page2Feed.Core.Model.Atom
{
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.w3.org/2005/Atom")]
    [XmlRoot("feed", Namespace = "http://www.w3.org/2005/Atom", IsNullable = false)]
    public class AtomFeed
    {

        private string titleField;

        private string subtitleField;

        private feedLink[] linkField;

        private string idField;

        private string updatedField;

        private feedEntry[] entryField;

        /// <remarks/>
        public string title
        {
            get => titleField;
            set => titleField = value;
        }

        /// <remarks/>
        public string subtitle
        {
            get => subtitleField;
            set => subtitleField = value;
        }

        /// <remarks/>
        [XmlElement("link")]
        public feedLink[] link
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
        [XmlElement("entry")]
        public feedEntry[] entry
        {
            get => entryField;
            set => entryField = value;
        }
    }
}