using System.Linq;
using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Extensions;
using AngleSharp.Parser.Html;
using Page2Feed.Web.App;

namespace Page2Feed.Web.Services.Background
{

    public class Html2TextConverter 
    {

        public async Task<string> Html2TextAsync(string html)
        {
            var htmlParser = new HtmlParser();
            var htmlDocument = await htmlParser.ParseAsync(html);
            #region Remove scripts and styles
            htmlDocument.Body.Descendents<IElement>()
                .Where(htmlNode =>
                    htmlNode.TagName == "script" ||
                    htmlNode.TagName == "style")
                .ToList()
                .ForEach(htmlNode =>
                    htmlNode.Parent.RemoveChild(htmlNode));
            #endregion
            #region Remove unnecessary whitespace
            var text =
                htmlDocument
                    .Body
                    .InnerText
                    .RegexReplace(
                        @"\s+",
                        " "
                    ).Trim();
            #endregion
            return text;
        }

    }

}