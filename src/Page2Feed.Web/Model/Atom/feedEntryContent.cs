using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Page2Feed.Web.Model.Atom
{
    /// <remarks/>
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.w3.org/2005/Atom")]
    public class feedEntryContent
    {

        private string divField;

        private string typeField;

        /// <remarks/>
        [XmlElement(Namespace = "http://www.w3.org/1999/xhtml")]
        public string div
        {
            get => divField;
            set => divField = value;
        }

        /// <remarks/>
        [XmlAttribute]
        public string type
        {
            get => typeField;
            set => typeField = value;
        }
    }
}