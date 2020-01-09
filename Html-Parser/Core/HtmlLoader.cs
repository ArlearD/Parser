using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Parser.Core
{
    class HtmlLoader
    {
        readonly HttpClient client;

        public HtmlLoader()
        {
            client = new HttpClient();
        }

        public async Task<string> GetHtmlByUrl(string url)
        {
            var response = await client.GetAsync(url);
            string source = null;

            if(response != null && response.StatusCode == HttpStatusCode.OK)
            {
                source = await response.Content.ReadAsStringAsync();
            }

            return source;
        }
    }
}
