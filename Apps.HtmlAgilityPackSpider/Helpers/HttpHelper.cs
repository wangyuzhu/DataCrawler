using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Apps.HtmlAgilityPackSpider.Model;

namespace Apps.HtmlAgilityPackSpider.Helpers
{
    public static class HttpHelper
    {
        public static async Task<string> GetRemoteHttpString(this HttpRequestInfo httpRequest)
        {
            httpRequest.Method = httpRequest.Method ?? "get";
            httpRequest.Encode = httpRequest.Encode ?? "utf-8";

            Encoding encoding = Encoding.UTF8;
            try
            {
                encoding = httpRequest.Encode.ToLower() == "utf-8" ? Encoding.UTF8 : Encoding.GetEncoding(httpRequest.Encode);

                using (var c = new HttpClient())
                {
                    return await c.GetStringAsync(httpRequest.Url);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
