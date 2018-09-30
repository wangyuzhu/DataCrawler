using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Apps.HtmlAgilityPackSpider.Model;
using System.Linq;
using HtmlAgilityPack;
using Apps.HtmlAgilityPackSpider.Helpers;

namespace Apps.HtmlAgilityPackSpider
{
    public abstract class HtmlSpider
    {
        protected List<KeyValuePair<string, HtmlNodeCollection>> htmlNodes = null;
        protected HttpRequestInfo httpRequest { get; private set; }
        protected object dataOption { get; private set; }
        public HtmlSpider(HttpRequestInfo request, object _dataOption)
        {
            httpRequest = request;
            dataOption = _dataOption;
            htmlNodes = htmlNodes = new List<KeyValuePair<string, HtmlNodeCollection>>();
        }
        public virtual async Task<string> SetHtml()
        {
            return await this.GetHtml();
        }
        public abstract Task<List<HtmlFieldConfig>> GetListFields();

        public abstract Task Save(List<List<KeyValuePair<string, string>>> values);

        public virtual void F2()
        {
            Console.WriteLine("Base's virtual fucntion F2");
        }

        protected async Task<string> GetHtml()
        {
            return await httpRequest.GetRemoteHttpString();
        }

        public async Task Build()
        {
            List<List<KeyValuePair<string, string>>> values = new List<List<KeyValuePair<string, string>>>();

            var configs = await GetListFields();
            if (configs != null && configs.Count > 0)
            {
                var doc = new HtmlDocument();
                doc.LoadHtml((await SetHtml()).UnUnicode().HtmlDecode().SetHttpBase(httpRequest.Url));

                if (configs.Count(w => w.Multiple) > 1)
                {
                    throw new Exception("Multiple 只能由一条");
                }
                else
                {
                    if (configs.Exists(w => w.Multiple))
                    {
                        //多条数据抓取
                        foreach (var o in doc.DocumentNode.SelectNodes(configs.SingleOrDefault(w => w.Multiple).XPath))
                        {
                            var s = new List<KeyValuePair<string, string>>();

                            foreach (var c in configs.Where(w => !w.Multiple))
                            {
                                s.Add(new KeyValuePair<string, string>(c.Field, doc.GetNodeString(o, c)));
                            }

                            values.Add(s);
                        }
                    }
                    else
                    {
                        var s = new List<KeyValuePair<string, string>>();
                        foreach (var c in configs)
                        {
                            s.Add(new KeyValuePair<string, string>(c.Field, doc.GetNodeString(doc.DocumentNode, c)));
                        }
                        values.Add(s);
                    }
                    await Save(values);
                }
            }
        }
    }
}
