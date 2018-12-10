using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Page2Feed.Web.Model.Atom
{
    /// <remarks/>
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.w3.org/1999/xhtml")]
    [XmlRoot(Namespace = "http://www.w3.org/1999/xhtml", IsNullable = false)]
    public class div
    {

        private string pField;

        /// <remarks/>
        public string p
        {
            get => pField;
            set => pField = value;
        }
    }
}