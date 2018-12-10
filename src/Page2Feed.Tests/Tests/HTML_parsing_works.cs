using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using NUnit.Framework;
using Page2Feed.Core.Services;

// ReSharper disable InconsistentNaming

namespace Page2Feed.Web.Tests
{

    [TestFixture]
    public class HTML_parsing_works
    {

        [Test]
        public void HTML_summary_parsing_works()
        {
            var html = new HtmlDocument();
            html.Load(
                Path.Combine(
                    Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory,
                        "Tests"
                    ),
                    "sample.html"
                )
            );
            html.DocumentNode.Descendants()
                .Where(htmlNode => htmlNode.Name == "script" || htmlNode.Name == "style")
                .ToList()
                .ForEach(htmlNode => htmlNode.Remove());
            var texten = html.DocumentNode.InnerText.RegexReplace(@"\s+", " ");
            var text = html.DocumentNode.Descendants().Select(htmlNode => htmlNode.InnerText);
            var texts = string.Join(" ", text);
            var texts2 = texts.Replace("\r", " ");
            var texts3 = texts2.Replace("\n", " ");
            var texts4 = Regex.Replace(texts3, @"\s+", " ").Trim();
        }

    }
}
