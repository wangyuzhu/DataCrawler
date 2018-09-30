using System;
using System.Collections.Generic;

namespace Apps.HtmlAgilityPackSpider.Model
{
    public class HttpRequestInfo
    {
        public string Url { set; get; }
        public string DataType { set; get; }
        public string Method { set; get; }
        public string Encode { set; get; }
        public string ContentType { set; get; }
        public string Cookie { set; get; }
        public string Host { set; get; }
        public string Refere { set; get; }
        public string UserAgent { set; get; }
        public bool UnUincode { set; get; } = true;
        public List<Dictionary<string,string>> Header { set; get; }
    }
}
