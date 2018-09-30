using System;
using System.Collections.Generic;
using System.Text;

namespace Apps.HtmlAgilityPackSpider.Model
{
    public class HtmlFieldConfig
    {
        private bool _multiple;
        private string _field;
        private string _resultdatatype;
        private string _xpath;
        private string _type;
        private string _attr;
        private List<KeyValuePair<string, string>> _replacewords;
        private string _removexpath;
        private bool _removehtmlattrs;
        private bool _removehtmllink;

        public bool Multiple
        {
            set { _multiple = value; }
            get { return _multiple; }
        }
        public string Field
        {
            set { _field = value; }
            get { return _field; }
        }
        public string ResultDataType
        {
            set { _resultdatatype = value; }
            get { return _resultdatatype; }
        }
        public string XPath
        {
            set { _xpath = value; }
            get { return _xpath; }
        }
        public string Type
        {
            set { _type = value; }
            get { return _type; }
        }
        public string Attr
        {
            set { _attr = value; }
            get { return _attr; }
        }
        public List<KeyValuePair<string, string>> ReplaceWords
        {
            set { _replacewords = value; }
            get { return _replacewords; }
        }
        public string RemoveXPath
        {
            set { _removexpath = value; }
            get { return _removexpath; }
        }
        public bool RemoveHtmlAttrs
        {
            set { _removehtmlattrs = value; }
            get { return _removehtmlattrs; }
        }
        public bool RemoveHtmlLink
        {
            set { _removehtmllink = value; }
            get { return _removehtmllink; }
        }
    }
}
