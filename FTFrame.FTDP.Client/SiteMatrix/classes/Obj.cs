using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FTDPClient.Obj
{
    public class ComboItem
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public object Tag { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }
    public class RuleItem
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public bool Bind { get; set; } = false;
        public RuleItemType Type { get; set; }
        public List<RuleItem> Items { get; set; }
    }
    public enum RuleItemType
    {
        DIR,
        TABLE,
        COLUMN
    }
    public enum ApiType
    {
        LIST,
        DYVALUE,
        DATAOP
    }
    public class RuleInformation
    {
        public string Caption { get; set; }
        public string Type { get; set; }
        public ApiType ApiType { get; set; }
        public int Rank { get; set; }
        public bool IsText { get; set; } = false;
        public List<string[]> SubRules { get; set; }
        public string RuleId { get; set; }
    }
    public class RuleResult
    {
        public string PageId { get; set; }
        public string PagePath { get; set; }
        public string PageCaption { get; set; }
        public string ControlId { get; set; }
        public string ControlName { get; set; }
        public string ControlCaption { get; set; }
        public string PartId { get; set; }
        public string PartName { get; set; }
        public string PartXml { get; set; }
        public string ApiType { get; set; }
        public string ApiPath { get; set; }
        public string ApiCaption { get; set; }
        public string RuleDirId { get; set; }
        public string RuleDirCaption { get; set; }
        public string RuleTableId { get; set; }
        public string RuleTableBind { get; set; }
        public string RuleTableCaption { get; set; }
        public string RuleColumnId { get; set; }
        public string RuleColumnBind { get; set; }
        public string RuleColumnCaption { get; set; }
        public string RuleAtomId { get; set; }
        public string RuleAtomValue { get; set; }
        public string Message { get; set; }
        public List<(string JsonKey,string SetKey,string Desc)> ApiSet { get; set; }
        public object OtherResult { get; set; }
    }
    public class PartInformation
    {
        string PageId { get; set; }
        string PagePath { get; set; }
        string PageCaption { get; set; }
        string ControlId { get; set; }
        string ControlName { get; set; }
        string ControlCaption { get; set; }
        string PartId { get; set; }
        string PartName { get; set; }
    }
}
