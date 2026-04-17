using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace FTFrame.Server.Core.Model
{
    public class SiteInfo
    {
        public string _id { get; set; }
        public string _user { get; set; }
        public string _passwd { get; set; }
        public string _key { get; set; }
        public string _qianming { get; set; }
        public string _split { get; set; }
        public string _splitfile { get; set; }
        public string _splitserver { get; set; }
        public string _splitforeframe { get; set; }
        public string _codesetchange { get; set; }
        public DateTime? _datenow { get; set; }
    }
    public class DataRow
    {
        private string _rowid;
        private string _datatype;
        private int _length;
        private int _numpoint;
        private bool _allownull;
        private string _default;
        private bool _primary;
        private bool _index;
        public DataRow()
        {
        }
        public string rowid
        {
            get { return _rowid; }
            set { _rowid = value; }
        }
        public string datatype
        {
            get { return _datatype; }
            set { _datatype = value; }
        }
        public int length
        {
            get { return _length; }
            set { _length = value; }
        }
        public int numpoint
        {
            get { return _numpoint; }
            set { _numpoint = value; }
        }
        public bool allownull
        {
            get { return _allownull; }
            set { _allownull = value; }
        }
        public string Default
        {
            get { return _default; }
            set { _default = value; }
        }
        public bool primary
        {
            get { return _primary; }
            set { _primary = value; }
        }
        public bool index
        {
            get { return _index; }
            set { _index = value; }
        }
    }
    public class TreeNode
    {
        public string Text { get; set; }
        public object Tag { get; set; }
        public Color ForeColor { get; set; } = Color.Black;
        public TreeNode(string text)
        {
            Text = text;
        }
        private List<TreeNode> _Nodes = new List<TreeNode>();
        public List<TreeNode> Nodes
        {
            get
            {
                if (_Nodes == null) return new List<TreeNode>();
                return _Nodes;
            }
        }
        public void Add(TreeNode node)
        {
            if (_Nodes == null) _Nodes = new List<TreeNode>();
            _Nodes.Add(node);
        }
    }

    public class DPDataRow
    {
        public List<(DPItem OpItem, string Value, DPData Data)> ValueList { get; set; } = new List<(DPItem OpItem, string Value, DPData Data)>();
    }
    public class DPData
    {
        public string TableName { get; set; }
        public int LeiXing { get; set; }
        public string AdvSet { get; set; }
        public List<DPDataRow> DataRow { get; set; }
    }
    public class DPItem
    {
        public string Caption { get; set; }
        public string Name { get; set; }
        public string BindData { get; set; }
        public int LeiXing { get; set; }
        public string Validate { get; set; }
        public int Special { get; set; }
        public string Advance { get; set; }
        public bool IsJson { get; set; }
        //public List<DPItem> OpItemList { get; set; }
    }
}
