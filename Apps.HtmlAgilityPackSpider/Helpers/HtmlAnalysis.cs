using System;
using System.Collections.Generic;
using System.Text;
using HtmlAgilityPack;
using System.Linq;
using Apps.HtmlAgilityPackSpider.Model;

namespace Apps.HtmlAgilityPackSpider.Helpers
{
    public static class HtmlAnalysis
    {
        public static string GetNodeString(this HtmlDocument doc, HtmlNode parentNode, HtmlFieldConfig fieldConfig)
        {
            if (fieldConfig != null && !string.IsNullOrEmpty(fieldConfig.XPath))
            {
                if (doc != null || (parentNode != null))
                {
                    if (fieldConfig != null)
                    {
                        fieldConfig.Type = fieldConfig.Type ?? "html";
                    }

                    var node = doc.GetHtmlNode(parentNode, fieldConfig.XPath);

                    if (node != null)
                    {
                        if (!string.IsNullOrEmpty(fieldConfig.RemoveXPath))
                        {
                            var removeNodes = doc.GetHtmlNodes(node, fieldConfig.RemoveXPath);
                            if (removeNodes != null && removeNodes.Count > 0)
                            {
                                foreach (var o in removeNodes)
                                {
                                    o.Remove();
                                }
                            }
                        }

                        if (fieldConfig.RemoveHtmlAttrs)
                        {
                            node = node.RemoveHtmlAttrs();
                        }

                        string value = "";
                        if (!string.IsNullOrEmpty(fieldConfig.Attr))
                        {
                            var attr = node.Attributes[fieldConfig.Attr];
                            value = attr != null ? attr.Value : "";
                        }
                        else
                        {
                            if (fieldConfig.Type.ToLower() == "html")
                            {
                                value = node.InnerHtml;
                            }
                            else if (fieldConfig.Type.ToLower() == "text")
                            {
                                value = node.InnerText;
                            }
                        }
                        if (!string.IsNullOrEmpty(value) && fieldConfig.ReplaceWords != null && fieldConfig.ReplaceWords.Count > 0)
                        {
                            foreach (var o in fieldConfig.ReplaceWords)
                            {
                                value = value.Replace(o.Key, o.Value);
                            }
                        }
                        if (!string.IsNullOrEmpty(value))
                        {
                            if (fieldConfig.RemoveHtmlLink)
                            {
                                value = value.RemoveHtmlLink();
                            }
                            return value.Trim();
                        }

                    }
                }
            }
            return "";
        }

        public static HtmlNode GetHtmlNode(this HtmlDocument doc, HtmlNode parentNode, string xPath)
        {
            HtmlNode node = null;
            foreach (string path in xPath.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (xPath.StartsWith("//"))
                {
                    node = doc.DocumentNode.SelectSingleNode(path);
                }
                else
                {
                    node = parentNode.SelectSingleNode(path);
                }
                if (node != null)
                {
                    break;
                }
            }
            return node;
        }

        public static List<HtmlNode> GetHtmlNodes(this HtmlDocument doc, HtmlNode parentNode, string xPath)
        {
            HtmlNode node = null;
            List<HtmlNode> nodes = new List<HtmlNode>();

            foreach (string path in xPath.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (path.StartsWith("//"))
                {
                    node = doc.DocumentNode.SelectSingleNode(path);
                }
                else
                {
                    node = parentNode.SelectSingleNode(path);
                }
                if (node != null)
                {
                    nodes.Add(node);
                }
            }
            return nodes;
        }

        public static HtmlNode RemoveHtmlAttrs(this HtmlNode node, string[] baoliuAttr = null, bool removeEmptyNode = true, string[] canEmptyNodeTags = null)
        {
            baoliuAttr = baoliuAttr ?? new string[] { "src", "href" };
            canEmptyNodeTags = canEmptyNodeTags ?? new string[] { "img" };

            var nodes = node.Descendants();//.Where(w => w.NodeType == HtmlNodeType.Element);
            bool deleteNode = false;
            for (int n = 0; n < nodes.Count(); n++)
            {
                deleteNode = false;
                var obj = nodes.ElementAt(n);
                obj.InnerHtml = obj.InnerHtml.Trim();

                if (string.IsNullOrEmpty(obj.InnerText.Trim()))
                {
                    //删除空节点
                    if (!canEmptyNodeTags.Contains(obj.Name.ToLower()))
                    {
                        if (string.IsNullOrEmpty(obj.InnerHtml))
                        {
                            obj.Remove();
                            n--;
                            deleteNode = true;
                        }
                        else
                        {
                            var nc = obj.Descendants().Where(w => canEmptyNodeTags.Contains(w.Name));
                            if (nc == null || nc.Count() == 0)
                            {
                                obj.Remove();
                                n--;
                                deleteNode = true;
                            }

                        }

                    }
                }
                if (!deleteNode)
                {
                    if (obj.HasAttributes)
                    {
                        var ss = obj.Attributes.Where(w => !baoliuAttr.Contains(w.Name.ToLower())).ToList();

                        if (ss != null && ss.Count > 0)
                        {
                            ss.ForEach(o => { obj.Attributes.Remove(o); });
                        }
                    }
                }
            }
            return node;
        }

        public static string SetHttpBase(this string html, string baseUrl)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            try
            {
                foreach (var n in GetBaseUrlNodes())
                {
                    foreach (var link in doc.DocumentNode.Descendants(n.Key).ToArray())
                    {
                        string href = link.Attributes[n.Value].Value;
                        if (!string.IsNullOrEmpty(href) && !href.ToLower().StartsWith("javascript") && !href.ToLower().StartsWith("#"))
                        {
                            if (href.StartsWith("//"))
                            {
                                href = "http:" + href;
                            }
                            else
                            {
                                if (!href.Contains("://"))
                                {
                                    href = new Uri(new Uri(baseUrl), href).ToString();
                                }
                            }
                            if (href != link.Attributes[n.Value].Value)
                            {
                                html = html.Replace(link.Attributes[n.Value].Value, href);
                            }
                        }
                    }
                }
            }
            catch
            {

            }
            return html;
        }
        private static List<KeyValuePair<string, string>> GetBaseUrlNodes()
        {
            var res = new List<KeyValuePair<string, string>>();
            res.Add(new KeyValuePair<string, string>("a", "href"));
            res.Add(new KeyValuePair<string, string>("img", "src"));
            return res;
        }

    }
}
