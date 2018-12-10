using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Page2Feed.Core.Model.Atom
{
    /// <remarks/>
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.w3.org/2005/Atom")]
    public class feedEntryAuthor
    {

        private string nameField;

        private string emailField;

        /// <remarks/>
        public string name
        {
            get => nameField;
            set => nameField = value;
        }

        /// <remarks/>
        public string email
        {
            get => emailField;
            set => emailField = value;
        }
    }
}