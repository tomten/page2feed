using System.Threading.Tasks;

namespace Page2Feed.Core.Services.Interfaces
{

    public interface IHtml2TextConverter
    {

        Task<string> Html2TextAsync(string html);

    }

}