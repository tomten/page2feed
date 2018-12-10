using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Page2Feed.Web.Model.Atom
{
    /// <remarks/>
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.w3.org/2005/Atom")]
    public class feedEntryLink
    {

        private string hrefField;

        private string relField;

        private string typeField;

        /// <remarks/>
        [XmlAttribute]
        public string href
        {
            get => hrefField;
            set => hrefField = value;
        }

        /// <remarks/>
        [XmlAttribute]
        public string rel
        {
            get => relField;
            set => relField = value;
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