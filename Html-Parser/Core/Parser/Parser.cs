using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Dom.Html;

namespace Parser.Core.Habra
{
    class Parser : IParser<string[]>
    {
        public string[] Parse(IHtmlDocument document)
        {
            var list  = new List<string>();


            var links = document.QuerySelectorAll("a")
                                 .Cast<IHtmlAnchorElement>()
                                 .Select(m => m.Href)
                                 .Where(item => item.Contains(CurUrl.url.Replace("http", "https")) || item.Contains(CurUrl.url))
                                 .ToList();
            foreach (var item in links)
            {
                if (!CurUrl.urls.Contains(item))
                {
                    CurUrl.changes.Add(item);
                }
                list.Add(item);
            }

            return list.ToArray();
        }
    }
}
