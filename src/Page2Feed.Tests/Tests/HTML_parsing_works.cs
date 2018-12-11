using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using AngleSharp.Dom;
using AngleSharp.Extensions;
using AngleSharp.Parser.Html;
using NUnit.Framework;
using Page2Feed.Core.Services;

// ReSharper disable InconsistentNaming

namespace Page2Feed.Tests.Tests
{

    [TestFixture]
    public class HTML_parsing_works
    {

        [Test]
        public void HTML_summary_parsing_works__AngleSharp()
        {
            var sampleHtmlPath = Path.Combine(
                Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    "Tests"
                ),
                "sample.html"
            );
            var html = new HtmlParser().Parse(File.OpenRead(sampleHtmlPath));
            foreach (var element in html.DocumentElement.Descendents<IElement>())
            {
                var scripts = element.GetElementsByTagName("script");
                foreach (var script in scripts)
                {
                    script.Parent.RemoveChild(script);
                }
                var styles = element.GetElementsByTagName("style");
                foreach (var style in styles)
                {
                    style.Parent.RemoveChild(style);
                }
            }

            var texts5 =
                html
                    .Body
                    .InnerText
                    .Replace("\r", " ")
                    .Replace("\n", " ")
                    .RegexReplace(@"\s+", " ")
                    .Trim()
                    ;
            var texts4 =
                Regex.Replace(
                    string.Join(
                        " ",
                        html.Body.InnerText
                    ).Replace("\r", " ").Replace("\n", " "), @"\s+", " ").Trim();
        }

    }

}
