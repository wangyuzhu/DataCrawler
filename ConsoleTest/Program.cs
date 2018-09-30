using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Apps.HtmlAgilityPackSpider;
using Apps.HtmlAgilityPackSpider.Model;
using Newtonsoft.Json;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            Console.WriteLine("Hello World!");
            aa a = new aa(new HttpRequestInfo { Url = "http://www.cngold.com.cn/yaowen/1.html" }, null);
            // aa a = new aa(new HttpRequestInfo { Url = "http://forex.cngold.com.cn/20180915d1711n306622568.html" }, null);
            a.Build().GetAwaiter();
            Console.WriteLine("dd");
            Console.ReadKey();
        }
    }

    public class aa : HtmlSpider
    {
        public aa(HttpRequestInfo request, object dataOption) : base(request, dataOption)
        {
        }

        /// <summary>
        /// 【自定义远程读取http内容(this.httpRequest)】
        /// </summary>
        /// <returns></returns>
        //public override async Task<string> SetHtml()
        //{
        //    return await this.GetHtml();
        //}

        public override async Task<List<HtmlFieldConfig>> GetListFields()
        {
            var encode = System.Text.Encoding.GetEncoding("GB2312");
            string html = await System.IO.File.ReadAllTextAsync("list.json", encode);
            // string html = await System.IO.File.ReadAllTextAsync("detail.json");
            return JsonConvert.DeserializeObject<List<HtmlFieldConfig>>(html);
        }

        public override Task Save(List<List<KeyValuePair<string, string>>> values)
        {
            // this.dataOption 区分来源
            Console.WriteLine(values);
            return Task.CompletedTask;
        }
    }
}
