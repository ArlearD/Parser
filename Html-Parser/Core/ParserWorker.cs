using AngleSharp.Parser.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.Core
{
    class ParserWorker<T> where T : class
    {
        IParser<T> parser;


        bool isActive;

        public event Action<object, T> OnNewData;
        public event Action<object> OnCompleted;

        public ParserWorker(IParser<T> parser)
        {
            this.parser = parser;
        }

        public void Start()
        {
            isActive = true;
            Worker();
        }

        public void Abort()
        {
            isActive = false;
        }

        private async void Worker()
        {
            for (int i = 0; i < CurUrl.iter; i++)
            {
                if (i == 0)
                {
                    if (!isActive)
                    {
                        OnCompleted?.Invoke(this);
                        return;
                    }

                    var loader = new HtmlLoader();
                    var source = await loader.GetHtmlByUrl(CurUrl.url);
                    var domParser = new HtmlParser();

                    var document = await domParser.ParseAsync(source);

                    var result = parser.Parse(document);
                    foreach (var item in CurUrl.changes)
                    {
                        CurUrl.urls.Add(item);
                    }
                    OnNewData?.Invoke(this, result);

                }
                else
                {
                    foreach (var url in CurUrl.urls)
                    {
                        if (!isActive)
                        {
                            OnCompleted?.Invoke(this);
                            return;
                        }

                        var loader = new HtmlLoader();
                        var source = await loader.GetHtmlByUrl(url.Replace("https", "http"));
                        var domParser = new HtmlParser();

                        var document = await domParser.ParseAsync(source);

                        var result = parser.Parse(document);
                        OnNewData?.Invoke(this, result);
                    }
                    foreach (var item in CurUrl.changes)
                    {
                        CurUrl.urls.Add(item);
                    }
                }
            }
            OnCompleted?.Invoke(this);
            isActive = false;
        }


    }
}
