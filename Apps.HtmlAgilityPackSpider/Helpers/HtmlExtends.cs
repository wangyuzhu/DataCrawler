using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Apps.HtmlAgilityPackSpider.Helpers
{
    public static class HtmlExtends
    {
        public static string HtmlEncode(this string html)
        {
            return HttpUtility.HtmlEncode(html);
        }
        public static string HtmlDecode(this string html)
        {
            if (html != null && !string.IsNullOrEmpty(html.ToString()))
            {
                return HttpUtility.HtmlDecode(html.ToString());
            }
            else
            {
                return html;
            }
        }
        public static string UnUnicode(this string html)
        {
            string outStr = "";
            Regex reg = new Regex(@"(?i)\\u([0-9a-f]{4})");
            outStr = reg.Replace(html, delegate (Match m1)
            {
                return ((char)Convert.ToInt32(m1.Groups[1].Value, 16)).ToString();
            });
            return outStr;
        }
        public static string RemoveHTML(this string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                Regex regex = new Regex(@"<[^>]+>|</[^>]+>");
                str = regex.Replace(str, "");
            }
            return str;
        }

        /// <summary>
        /// 移除a标签，保留内容
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string RemoveHtmlLink(this string html)
        {
            html = Regex.Replace(html, @"<a\s*[^>]*>", "", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"</a>", "", RegexOptions.IgnoreCase);

            return html;
        }
    }
}
