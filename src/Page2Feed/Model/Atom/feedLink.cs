using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Page2Feed.Model.Atom
{
    /// <remarks/>
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.w3.org/2005/Atom")]
    public class feedLink
    {

        private string hrefField;

        private string relField;

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
    }
}