using System;
using System.Text;
using System.Reflection;
using System.Collections;
using System.ComponentModel;
using System.Runtime.InteropServices;
using FTDPClient.PropertyBagNameSpace;
using FTDPClient.consts;
using FTDPClient.functions;
using FTDPClient.forms;
using FTDPClient.PropertySpace.ControlInfo;
using mshtml;
using FTDPClient.database;
using System.Data.OleDb;
using System.Xml;
using FTDPClient.classes;
using System.Drawing;
using FTDPClient.Page;
using System.Windows.Forms;
using FTDPClient.controls;
namespace FTDPClient.PropertySpace.FormElement
{
    [System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
    public class func
    {
        public static string getID(IHTMLElement ele)
        {
            try
            {
                string id = str.getDecode(ele.id.Substring(7));
                id = id.Substring(0, id.IndexOf('|'));
                return id.Split(':')[2];
            }
            catch (Exception ex)
            {
                new error(ex);
                return "error";
            }
        }
        public static int getType(IHTMLElement ele)
        {
            try
            {
                string id = str.getDecode(ele.id.Substring(7));
                id = id.Substring(0, id.IndexOf('|'));
                return int.Parse(id.Split(':')[1]);
            }
            catch (Exception ex)
            {
                new error(ex);
                return 0;
            }
        }
        public static int getStatFilterType(IHTMLElement ele)
        {
            try
            {
                string id = str.getDecode(ele.id.Substring(7));
                return int.Parse(id.Substring(id.IndexOf('|')+1));
            }
            catch (Exception ex)
            {
                new error(ex);
                return 0;
            }
        }
        public static string getRule(IHTMLElement ele)
        {
            try
            {
                string id = str.getDecode(ele.id.Substring(7));
                if (id.IndexOf("@options") <= 0)
                {
                    return id.Substring(id.IndexOf('|') + 1);
                }
                else
                {
                    return id.Substring(id.IndexOf('|') + 1, id.IndexOf("@options") - id.IndexOf('|')-1);
                }
            }
            catch (Exception ex)
            {
                new error(ex);
                return "error";
            }
        }
        public static ArrayList getOptions(IHTMLElement ele)
        {
            //str|value|0#str|value|0
            //|->$$$$
            //#->!!!!
            try
            {
                string id = str.getDecode(ele.id.Substring(7));
                if (id.IndexOf("@options") <= 0)
                {
                    return new ArrayList();
                }
                else
                {
                    ArrayList al = new ArrayList();
                    string optionstr=id.Substring(id.IndexOf("@options")+8);
                    string[] options = optionstr.Split('#');
                    foreach (string option in options)
                    {
                        if (option.Trim().Equals("")) continue;
                        string[] items = option.Split('|');
                        al.Add(new string[] { items[0].Replace("$$$$", "|").Replace("!!!!", "#"), items[1].Replace("$$$$", "|").Replace("!!!!", "#"), items[2] });
                    }
                    return al;
                }
            }
            catch (Exception ex)
            {
                new error(ex);
                return new ArrayList();
            }
        }
        public static void setNewID(IHTMLElement ele, string nid)
        {
            try
            {
                string id = str.getDecode(ele.id.Substring(7));
                string id1 = id.Substring(0, id.IndexOf('|'));
                string id2 = id.Substring(id.IndexOf('|'));
                string newid = id1.Split(':')[0] + ":" + id1.Split(':')[1] + ":" + nid + id2;
                ele.id = "ftform_" + str.getEncode(newid);
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }
        public static void setNewType(IHTMLElement ele, int ntype)
        {
            try
            {
                string id = str.getDecode(ele.id.Substring(7));
                string id1 = id.Substring(0, id.IndexOf('|'));
                string id2 = id.Substring(id.IndexOf('|'));
                string newid = id1.Split(':')[0] + ":" + ntype + ":" + id1.Split(':')[2] + id2;
                ele.id = "ftform_" +str.getEncode(newid);
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }
        public static void setNewStatFilter(IHTMLElement ele, int ntype)
        {
            try
            {
                string id = str.getDecode(ele.id.Substring(7));
                string id1 = id.Substring(0, id.IndexOf('|'));
                string newid = id1+"|"+ntype;
                ele.id = "ftform_" +str.getEncode(newid);
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }
        public static void setNewRule(IHTMLElement ele, string rule)
        {
            try
            {
                string id = str.getDecode(ele.id.Substring(7));
                string id1 = id.Substring(0, id.IndexOf('|'));
                string newid=null;
                if (id.IndexOf("@options") <= 0)
                {
                    newid = id1 + "|" + rule;
                }
                else
                {
                    string optionstr = id.Substring(id.IndexOf("@options"));
                    newid = id1 + "|" + rule + optionstr;
                }
                ele.id = "ftform_" + str.getEncode(newid);
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }
        public static void setNewOptions(IHTMLElement ele, ArrayList options)
        {
            try
            {
                string id = str.getDecode(ele.id.Substring(7));
                string id1 = null;
                if (id.IndexOf("@options") <= 0)
                {
                    id1 = id;
                }
                else
                {
                    id1 = id.Substring(0, id.IndexOf("@options"));
                }
                string optionstr="";
                foreach (object o in options)
                {
                    optionstr += ((string[])o)[0].Replace("|", "$$$$").Replace("#", "!!!!") + "|" + ((string[])o)[1].Replace("|", "$$$$").Replace("#", "!!!!") + "|" + ((string[])o)[2] + "#";
                }
                string newid = id1 + "@options" + optionstr;
                ele.id = "ftform_" + str.getEncode(newid);
            }
            catch (Exception ex)
            {
                new error(ex);
            }
        }
    }
    [System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
    public class TextBox
    {
        public TextBox()
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl));
        }
        public static IHTMLElement curEle=null;
        public static PropertyBag Bag()
        {
            PropertyBag bag = new PropertyBag();
            bag.GetValue += new PropertySpecEventHandler(GetValue);
            bag.SetValue += new PropertySpecEventHandler(SetValue);
            string curcate = res.form.GetString("String47");
            bag.Properties.Add(new PropertySpec(res.form.GetString("String45"), typeof(string), curcate, res.form.GetString("String46"), func.getID(curEle)));
            int eletype = func.getType(curEle);
            bag.Properties.Add(new PropertySpec(res.form.GetString("String51"), typeof(string), curcate, res.form.GetString("String52"), (eletype == 0 ? res.form.GetString("String48") : (eletype == 1 ? res.form.GetString("String49") : res.form.GetString("String50"))), typeof(System.Drawing.Design.UITypeEditor), typeof(FormTextBoxType)));
            bag.Properties.Add(new PropertySpec(res.form.GetString("String53"), typeof(string), curcate, res.form.GetString("String54"), func.getRule(curEle), typeof(FTDPClient.classes.TextEditor), typeof(FTDPClient.classes.TextEditorConverter)));
            return bag;
        }
        private static void GetValue(object sender, PropertySpecEventArgs e)
        {
            string pName = e.Property.Name;
            if (pName.Equals(res.form.GetString("String45")))
            {
                e.Value = func.getID(curEle);
                return;
            }
            if (pName.Equals(res.form.GetString("String51")))
            {
                int eletype = func.getType(curEle);
                e.Value = (eletype == 0 ? res.form.GetString("String48") : (eletype == 1 ? res.form.GetString("String49") : res.form.GetString("String50")));
                return;
            }
            if (pName.Equals(res.form.GetString("String53")))
            {
                e.Value = func.getRule(curEle);
                return;
            }
        }
        private static void SetValue(object sender, PropertySpecEventArgs e)
        {
            string pName = e.Property.Name;
            string pValue = e.Value.ToString();
            if (pName.Equals(res.form.GetString("String45")))
            {
                if (!str.IsNatural(pValue))
                {
                    MsgBox.Warning("Invalid ID!");
                }
                else
                {
                    func.setNewID(curEle, pValue);
                }
                return;
            }
            if (pName.Equals(res.form.GetString("String51")))
            {
                int setvalue = (pValue.Equals(res.form.GetString("String48")) ? 0 : (pValue.Equals(res.form.GetString("String49")) ? 1 : 2));
                func.setNewType(curEle,setvalue);
                return;
            }
            if (pName.Equals(res.form.GetString("String53")))
            {
                func.setNewRule(curEle, pValue);
                return;
            }
        }
    }
    [System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
    public class Password
    {
        public Password()
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl));
        }
        public static IHTMLElement curEle = null;
        public static PropertyBag Bag()
        {
            PropertyBag bag = new PropertyBag();
            bag.GetValue += new PropertySpecEventHandler(GetValue);
            bag.SetValue += new PropertySpecEventHandler(SetValue);
            string curcate = res.form.GetString("String47");
            bag.Properties.Add(new PropertySpec(res.form.GetString("String45"), typeof(string), curcate, res.form.GetString("String46"), func.getID(curEle)));
            int eletype = func.getType(curEle);
            bag.Properties.Add(new PropertySpec(res.form.GetString("String51"), typeof(string), curcate, res.form.GetString("String52"), (eletype == 0 ? res.form.GetString("String48") : (eletype == 1 ? res.form.GetString("String49") : res.form.GetString("String50"))), typeof(System.Drawing.Design.UITypeEditor), typeof(FormTextBoxType)));
            bag.Properties.Add(new PropertySpec(res.form.GetString("String53"), typeof(string), curcate, res.form.GetString("String54"), func.getRule(curEle), typeof(FTDPClient.classes.TextEditor), typeof(FTDPClient.classes.TextEditorConverter)));
            return bag;
        }
        private static void GetValue(object sender, PropertySpecEventArgs e)
        {
            string pName = e.Property.Name;
            if (pName.Equals(res.form.GetString("String45")))
            {
                e.Value = func.getID(curEle);
                return;
            }
            if (pName.Equals(res.form.GetString("String51")))
            {
                int eletype = func.getType(curEle);
                e.Value = (eletype == 0 ? res.form.GetString("String48") : (eletype == 1 ? res.form.GetString("String49") : res.form.GetString("String50")));
                return;
            }
            if (pName.Equals(res.form.GetString("String53")))
            {
                e.Value = func.getRule(curEle);
                return;
            }
        }
        private static void SetValue(object sender, PropertySpecEventArgs e)
        {
            string pName = e.Property.Name;
            string pValue = e.Value.ToString();
            if (pName.Equals(res.form.GetString("String45")))
            {
                if (!str.IsNatural(pValue))
                {
                    MsgBox.Warning("Invalid ID!");
                }
                else
                {
                    func.setNewID(curEle, pValue);
                }
                return;
            }
            if (pName.Equals(res.form.GetString("String51")))
            {
                int setvalue = (pValue.Equals(res.form.GetString("String48")) ? 0 : (pValue.Equals(res.form.GetString("String49")) ? 1 : 2));
                func.setNewType(curEle, setvalue);
                return;
            }
            if (pName.Equals(res.form.GetString("String53")))
            {
                func.setNewRule(curEle, pValue);
                return;
            }
        }
    }
    [System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
    public class Flow
    {
        public Flow()
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl));
        }
        public static IHTMLElement curEle = null;
        public static PropertyBag Bag()
        {
            PropertyBag bag = new PropertyBag();
            bag.GetValue += new PropertySpecEventHandler(GetValue);
            bag.SetValue += new PropertySpecEventHandler(SetValue);
            string curcate = res.form.GetString("String47");
            int flowv=int.Parse(curEle.getAttribute("name", 0).ToString().Substring(13));
            bag.Properties.Add(new PropertySpec(res.form.GetString("String75"), typeof(int), curcate, res.form.GetString("String76"), flowv));
            string para = curEle.getAttribute("tag", 0) == null ? null : curEle.getAttribute("tag", 0).ToString();
            string sqlexec = str.GetSplitValue(para,0);
            string gourl = str.GetSplitValue(para, 1);
            bag.Properties.Add(new PropertySpec("Append SQL", typeof(string), curcate, "Execute this SQL when the operation finished,can use @session[] @request[] @requestForm[] @siteid@", sqlexec));
            bag.Properties.Add(new PropertySpec("Redirect URL", typeof(string), curcate, "Redirect this URL when the operation finished,can use @session[] @request[] @requestForm[] @siteid@,Multiple sqls split with #", gourl));
            return bag;
        }
        private static void GetValue(object sender, PropertySpecEventArgs e)
        {
            string pName = e.Property.Name;
            if (pName.Equals(res.form.GetString("String75")))
            {
                int flowv = int.Parse(curEle.getAttribute("name", 0).ToString().Substring(13));
                e.Value = flowv;
                return;
            }
            if (pName.Equals("Append SQL"))
            {
                string para = curEle.getAttribute("tag", 0) == null ? null : curEle.getAttribute("tag", 0).ToString();
                e.Value = str.GetSplitValue(para, 0);
                return;
            }
            if (pName.Equals("Redirect URL"))
            {
                string para = curEle.getAttribute("tag", 0) == null ? null : curEle.getAttribute("tag", 0).ToString();
                e.Value = str.GetSplitValue(para, 1);
                return;
            }
        }
        private static void SetValue(object sender, PropertySpecEventArgs e)
        {
            string pName = e.Property.Name;
            string pValue = e.Value.ToString();
            if (pName.Equals(res.form.GetString("String75")))
            {
                curEle.setAttribute("name", "flow_btn_val_"+pValue, 0);
                return;
            }
            if (pName.Equals("Append SQL"))
            {
                string para = curEle.getAttribute("tag", 0) == null ? null : curEle.getAttribute("tag", 0).ToString();
                curEle.setAttribute("tag", str.SetSplitValue(para, 2, 0, pValue), 0);
                return;
            }
            if (pName.Equals("Redirect URL"))
            {
                string para = curEle.getAttribute("tag", 0) == null ? null : curEle.getAttribute("tag", 0).ToString();
                curEle.setAttribute("tag", str.SetSplitValue(para, 2, 1, pValue), 0);
                return;
            }
        }
    }
    [System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
    public class FlowExt1
    {
        public FlowExt1()
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl));
        }
        public static IHTMLElement curEle = null;
        public static PropertyBag Bag()
        {
            PropertyBag bag = new PropertyBag();
            bag.GetValue += new PropertySpecEventHandler(GetValue);
            bag.SetValue += new PropertySpecEventHandler(SetValue);
            string curcate = res.form.GetString("String47");
            string[] strs = curEle.getAttribute("name", 0).ToString().Split('_');
            string flow = strs[4];
            int canmod = int.Parse(strs[5]);
            string extname = strs[6];
            if (extname == null) extname = "";
            bag.Properties.Add(new PropertySpec(res.form.GetString("String89"), typeof(string), curcate, res.form.GetString("String91"), flow));
            bag.Properties.Add(new PropertySpec(res.form.GetString("String93"), typeof(int), curcate, res.form.GetString("String94"), canmod));
            bag.Properties.Add(new PropertySpec(res.form.GetString("String90"), typeof(string), curcate, res.form.GetString("String92"), extname));
            string para = curEle.getAttribute("tag", 0) == null ? null : curEle.getAttribute("tag", 0).ToString();
            string sqlexec = str.GetSplitValue(para, 0);
            string gourl = str.GetSplitValue(para, 1);
            bag.Properties.Add(new PropertySpec("Append SQL", typeof(string), curcate, "Execute this SQL when the operation finished,can use @session[] @request[] @requestForm[] @siteid@", sqlexec));
            bag.Properties.Add(new PropertySpec("Redirect URL", typeof(string), curcate, "Redirect this URL when the operation finished,can use @session[] @request[] @requestForm[] @siteid@,Multiple sqls split with #", gourl));
            return bag;
        }
        private static void GetValue(object sender, PropertySpecEventArgs e)
        {
            string pName = e.Property.Name;
            if (pName.Equals(res.form.GetString("String89")))
            {
                string[] strs = curEle.getAttribute("name", 0).ToString().Split('_');
                string flow = strs[4];
                e.Value = flow;
                return;
            }
            if (pName.Equals(res.form.GetString("String93")))
            {
                string[] strs = curEle.getAttribute("name", 0).ToString().Split('_');
                int canmod = int.Parse(strs[5]);
                e.Value = canmod;
                return;
            }
            if (pName.Equals(res.form.GetString("String90")))
            {
                string[] strs = curEle.getAttribute("name", 0).ToString().Split('_');
                string extname = strs[6];
                if (extname == null) extname = "";
                e.Value = extname;
                return;
            }
            if (pName.Equals("Append SQL"))
            {
                string para = curEle.getAttribute("tag", 0) == null ? null : curEle.getAttribute("tag", 0).ToString();
                e.Value = str.GetSplitValue(para, 0);
                return;
            }
            if (pName.Equals("Redirect URL"))
            {
                string para = curEle.getAttribute("tag", 0) == null ? null : curEle.getAttribute("tag", 0).ToString();
                e.Value = str.GetSplitValue(para, 1);
                return;
            }
        }
        private static void SetValue(object sender, PropertySpecEventArgs e)
        {
            string pName = e.Property.Name;
            string pValue = e.Value.ToString();
            if (pName.Equals(res.form.GetString("String89")))
            {
                string[] strs = curEle.getAttribute("name", 0).ToString().Split('_');
                curEle.setAttribute("name", strs[0] + "_" + strs[1] + "_" + strs[2] + "_" + strs[3] + "_" + pValue + "_" + strs[5] + "_" + strs[6], 0);
                return;
            }
            if (pName.Equals(res.form.GetString("String93")))
            {
                string[] strs = curEle.getAttribute("name", 0).ToString().Split('_');
                curEle.setAttribute("name", strs[0] + "_" + strs[1] + "_" + strs[2] + "_" + strs[3] + "_" + strs[4] + "_" + pValue + "_" + strs[6], 0);
                return;
            }
            if (pName.Equals(res.form.GetString("String90")))
            {
                string[] strs = curEle.getAttribute("name", 0).ToString().Split('_');
                curEle.setAttribute("name", strs[0] + "_" + strs[1] + "_" + strs[2] + "_" + strs[3] + "_" + strs[4] + "_" + strs[5] + "_" + pValue, 0);
                return;
            }
            if (pName.Equals("Append SQL"))
            {
                string para = curEle.getAttribute("tag", 0) == null ? null : curEle.getAttribute("tag", 0).ToString();
                curEle.setAttribute("tag", str.SetSplitValue(para, 2, 0, pValue), 0);
                return;
            }
            if (pName.Equals("Redirect URL"))
            {
                string para = curEle.getAttribute("tag", 0) == null ? null : curEle.getAttribute("tag", 0).ToString();
                curEle.setAttribute("tag", str.SetSplitValue(para, 2, 1, pValue), 0);
                return;
            }
        }
    }
    [System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
    public class FlowExt2
    {
        public FlowExt2()
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl));
        }
        public static IHTMLElement curEle = null;
        public static PropertyBag Bag()
        {
            PropertyBag bag = new PropertyBag();
            bag.GetValue += new PropertySpecEventHandler(GetValue);
            bag.SetValue += new PropertySpecEventHandler(SetValue);
            string curcate = res.form.GetString("String47");
            string[] strs = curEle.getAttribute("name", 0).ToString().Split('_');
            string flow = strs[4];
            int canmod = int.Parse(strs[5]);
            string extname = strs[6];
            string extpname = strs[7];
            if (extpname == null) extpname = "";
            bag.Properties.Add(new PropertySpec(res.form.GetString("String891"), typeof(string), curcate, res.form.GetString("String911"), flow));
            bag.Properties.Add(new PropertySpec(res.form.GetString("String931"), typeof(int), curcate, res.form.GetString("String941"), canmod));
            bag.Properties.Add(new PropertySpec(res.form.GetString("String901"), typeof(string), curcate, res.form.GetString("String921"), extname));
            bag.Properties.Add(new PropertySpec(res.form.GetString("String95"), typeof(string), curcate, res.form.GetString("String96"), extpname));
            string para = curEle.getAttribute("tag", 0) == null ? null : curEle.getAttribute("tag", 0).ToString();
            string sqlexec = str.GetSplitValue(para, 0);
            string gourl = str.GetSplitValue(para, 1);
            bag.Properties.Add(new PropertySpec("Append SQL", typeof(string), curcate, "Execute this SQL when the operation finished,can use @session[] @request[] @requestForm[] @siteid@", sqlexec));
            bag.Properties.Add(new PropertySpec("Redirect URL", typeof(string), curcate, "Redirect this URL when the operation finished,can use @session[] @request[] @requestForm[] @siteid@,Multiple sqls split with #", gourl));
            return bag;
        }
        private static void GetValue(object sender, PropertySpecEventArgs e)
        {
            string pName = e.Property.Name;
            if (pName.Equals(res.form.GetString("String891")))
            {
                string[] strs = curEle.getAttribute("name", 0).ToString().Split('_');
                string flow = strs[4];
                e.Value = flow;
                return;
            }
            if (pName.Equals(res.form.GetString("String931")))
            {
                string[] strs = curEle.getAttribute("name", 0).ToString().Split('_');
                int canmod = int.Parse(strs[5]);
                e.Value = canmod;
                return;
            }
            if (pName.Equals(res.form.GetString("String901")))
            {
                string[] strs = curEle.getAttribute("name", 0).ToString().Split('_');
                string extname = strs[6];
                e.Value = extname;
                return;
            }
            if (pName.Equals(res.form.GetString("String95")))
            {
                string[] strs = curEle.getAttribute("name", 0).ToString().Split('_');
                string extpname = strs[7];
                if (extpname == null) extpname = "";
                e.Value = extpname;
                return;
            }
            if (pName.Equals("Append SQL"))
            {
                string para = curEle.getAttribute("tag", 0) == null ? null : curEle.getAttribute("tag", 0).ToString();
                e.Value = str.GetSplitValue(para, 0);
                return;
            }
            if (pName.Equals("Redirect URL"))
            {
                string para = curEle.getAttribute("tag", 0) == null ? null : curEle.getAttribute("tag", 0).ToString();
                e.Value = str.GetSplitValue(para, 1);
                return;
            }
        }
        private static void SetValue(object sender, PropertySpecEventArgs e)
        {
            string pName = e.Property.Name;
            string pValue = e.Value.ToString();
            if (pName.Equals(res.form.GetString("String891")))
            {
                string[] strs = curEle.getAttribute("name", 0).ToString().Split('_');
                curEle.setAttribute("name", strs[0] + "_" + strs[1] + "_" + strs[2] + "_" + strs[3] + "_" + pValue + "_" + strs[5] + "_" + strs[6] + "_" + strs[7], 0);
                return;
            }
            if (pName.Equals(res.form.GetString("String931")))
            {
                string[] strs = curEle.getAttribute("name", 0).ToString().Split('_');
                curEle.setAttribute("name", strs[0] + "_" + strs[1] + "_" + strs[2] + "_" + strs[3] + "_" + strs[4] + "_" + pValue + "_" + strs[6] + "_" + strs[7], 0);
                return;
            }
            if (pName.Equals(res.form.GetString("String901")))
            {
                string[] strs = curEle.getAttribute("name", 0).ToString().Split('_');
                curEle.setAttribute("name", strs[0] + "_" + strs[1] + "_" + strs[2] + "_" + strs[3] + "_" + strs[4] + "_" + strs[5] + "_" + pValue + "_" + strs[7], 0);
                return;
            }
            if (pName.Equals(res.form.GetString("String95")))
            {
                string[] strs = curEle.getAttribute("name", 0).ToString().Split('_');
                curEle.setAttribute("name", strs[0] + "_" + strs[1] + "_" + strs[2] + "_" + strs[3] + "_" + strs[4] + "_" + strs[5] + "_" + strs[6] + "_" + pValue, 0);
                return;
            }
            if (pName.Equals("Append SQL"))
            {
                string para = curEle.getAttribute("tag", 0) == null ? null : curEle.getAttribute("tag", 0).ToString();
                curEle.setAttribute("tag", str.SetSplitValue(para, 2, 0, pValue), 0);
                return;
            }
            if (pName.Equals("Redirect URL"))
            {
                string para = curEle.getAttribute("tag", 0) == null ? null : curEle.getAttribute("tag", 0).ToString();
                curEle.setAttribute("tag", str.SetSplitValue(para, 2, 1, pValue), 0);
                return;
            }
        }
    }
    [System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
    public class RowIndex
    {
        public RowIndex()
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl));
        }
        public static IHTMLElement curEle = null;
        public static PropertyBag Bag()
        {
            PropertyBag bag = new PropertyBag();
            bag.GetValue += new PropertySpecEventHandler(GetValue);
            bag.SetValue += new PropertySpecEventHandler(SetValue);
            string curcate = res.form.GetString("String47");
            int rowindex = int.Parse(curEle.getAttribute("name", 0).ToString().Substring(10));
            bag.Properties.Add(new PropertySpec(res.form.GetString("String85"), typeof(int), curcate, res.form.GetString("String86"), rowindex));
            return bag;
        }
        private static void GetValue(object sender, PropertySpecEventArgs e)
        {
            string pName = e.Property.Name;
            if (pName.Equals(res.form.GetString("String85")))
            {
                int rowindex = int.Parse(curEle.getAttribute("name", 0).ToString().Substring(10));
                e.Value = rowindex;
                return;
            }
        }
        private static void SetValue(object sender, PropertySpecEventArgs e)
        {
            string pName = e.Property.Name;
            string pValue = e.Value.ToString();
            if (pName.Equals(res.form.GetString("String85")))
            {
                curEle.setAttribute("name", "row_index_" + pValue, 0);
                return;
            }
        }
    }
    [System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
    public class RowRate
    {
        public RowRate()
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl));
        }
        public static IHTMLElement curEle = null;
        public static PropertyBag Bag()
        {
            PropertyBag bag = new PropertyBag();
            bag.GetValue += new PropertySpecEventHandler(GetValue);
            bag.SetValue += new PropertySpecEventHandler(SetValue);
            string curcate = res.form.GetString("String47");
            string[] rowrates = curEle.getAttribute("name", 0).ToString().Split('_');

            bag.Properties.Add(new PropertySpec(res.form.GetString("String81"), typeof(int), curcate, res.form.GetString("String83"), int.Parse(rowrates[1])));
            bag.Properties.Add(new PropertySpec(res.form.GetString("String82"), typeof(string), curcate, res.form.GetString("String84"), rowrates[2]));
            return bag;
        }
        private static void GetValue(object sender, PropertySpecEventArgs e)
        {
            string pName = e.Property.Name;
            string[] rowrates = curEle.getAttribute("name", 0).ToString().Split('_');
            if (pName.Equals(res.form.GetString("String81")))
            {
                e.Value = int.Parse(rowrates[1]);
                return;
            }
            if (pName.Equals(res.form.GetString("String82")))
            {
                e.Value = rowrates[2];
                return;
            }
        }
        private static void SetValue(object sender, PropertySpecEventArgs e)
        {
            string pName = e.Property.Name;
            string pValue = e.Value.ToString();
            string[] rowrates = curEle.getAttribute("name", 0).ToString().Split('_');
            if (pName.Equals(res.form.GetString("String81")))
            {
                curEle.setAttribute("name", rowrates[0] + "_" + pValue + "_" + rowrates[2], 0);
                return;
            }
            if (pName.Equals(res.form.GetString("String82")))
            {
                curEle.setAttribute("name", rowrates[0] + "_" + rowrates[1] + "_" + pValue, 0);
                return;
            }
        }
    }
    [System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
    public class Select
    {
        public Select()
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl));
        }
        public static IHTMLElement curEle = null;
        public static PropertyBag Bag()
        {
            PropertyBag bag = new PropertyBag();
            bag.GetValue += new PropertySpecEventHandler(GetValue);
            bag.SetValue += new PropertySpecEventHandler(SetValue);
            string curcate = res.form.GetString("String47");
            bag.Properties.Add(new PropertySpec(res.form.GetString("String45"), typeof(string), curcate, res.form.GetString("String46"), func.getID(curEle)));
            int eletype = func.getType(curEle);
            bag.Properties.Add(new PropertySpec(res.form.GetString("String51"), typeof(string), curcate, res.form.GetString("String52"), (eletype == 0 ? res.form.GetString("String48") : (eletype == 1 ? res.form.GetString("String49") : res.form.GetString("String50"))), typeof(System.Drawing.Design.UITypeEditor), typeof(FormTextBoxType)));
            bag.Properties.Add(new PropertySpec(res.form.GetString("String53"), typeof(string), curcate, res.form.GetString("String54"), func.getRule(curEle), typeof(FTDPClient.classes.TextEditor), typeof(FTDPClient.classes.TextEditorConverter)));
            bag.Properties.Add(new PropertySpec(res.form.GetString("String60"), typeof(string), curcate, res.form.GetString("String61"), "(Collection)", typeof(FTDPClient.classes.OptionEditor), typeof(FTDPClient.classes.OptionEditorConverter)));
            return bag;
        }
        private static void GetValue(object sender, PropertySpecEventArgs e)
        {
            string pName = e.Property.Name;
            if (pName.Equals(res.form.GetString("String45")))
            {
                e.Value = func.getID(curEle);
                return;
            }
            if (pName.Equals(res.form.GetString("String51")))
            {
                int eletype = func.getType(curEle);
                e.Value = (eletype == 0 ? res.form.GetString("String48") : (eletype == 1 ? res.form.GetString("String49") : res.form.GetString("String50")));
                return;
            }
            if (pName.Equals(res.form.GetString("String53")))
            {
                e.Value = func.getRule(curEle);
                return;
            }
            if (pName.Equals(res.form.GetString("String60")))
            {
                e.Value = func.getOptions(curEle);
                return;
            }
        }
        private static void SetValue(object sender, PropertySpecEventArgs e)
        {
            string pName = e.Property.Name;
            string pValue = e.Value.ToString();
            if (pName.Equals(res.form.GetString("String45")))
            {
                if (!str.IsNatural(pValue))
                {
                    MsgBox.Warning("Invalid ID!");
                }
                else
                {
                    func.setNewID(curEle, pValue);
                }
                return;
            }
            if (pName.Equals(res.form.GetString("String51")))
            {
                int setvalue = (pValue.Equals(res.form.GetString("String48")) ? 0 : (pValue.Equals(res.form.GetString("String49")) ? 1 : 2));
                func.setNewType(curEle, setvalue);
                return;
            }
            if (pName.Equals(res.form.GetString("String53")))
            {
                func.setNewRule(curEle, pValue);
                return;
            }
            if (pName.Equals(res.form.GetString("String60")))
            {
                func.setNewOptions(curEle, (ArrayList)e.Value);
                return;
            }
        }
    }
    [System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
    public class TextArea
    {
        public TextArea()
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl));
        }
        public static IHTMLElement curEle = null;
        public static PropertyBag Bag()
        {
            PropertyBag bag = new PropertyBag();
            bag.GetValue += new PropertySpecEventHandler(GetValue);
            bag.SetValue += new PropertySpecEventHandler(SetValue);
            string curcate = res.form.GetString("String47");
            bag.Properties.Add(new PropertySpec(res.form.GetString("String45"), typeof(string), curcate, res.form.GetString("String46"), func.getID(curEle)));
            int eletype = func.getType(curEle);
            bag.Properties.Add(new PropertySpec(res.form.GetString("String51"), typeof(string), curcate, res.form.GetString("String52"), (eletype == 0 ? res.form.GetString("String48") : (eletype == 1 ? res.form.GetString("String49") : res.form.GetString("String50"))), typeof(System.Drawing.Design.UITypeEditor), typeof(FormTextBoxType)));
            bag.Properties.Add(new PropertySpec(res.form.GetString("String53"), typeof(string), curcate, res.form.GetString("String54"), func.getRule(curEle), typeof(FTDPClient.classes.TextEditor), typeof(FTDPClient.classes.TextEditorConverter)));
            return bag;
        }
        private static void GetValue(object sender, PropertySpecEventArgs e)
        {
            string pName = e.Property.Name;
            if (pName.Equals(res.form.GetString("String45")))
            {
                e.Value = func.getID(curEle);
                return;
            }
            if (pName.Equals(res.form.GetString("String51")))
            {
                int eletype = func.getType(curEle);
                e.Value = (eletype == 0 ? res.form.GetString("String48") : (eletype == 1 ? res.form.GetString("String49") : res.form.GetString("String50")));
                return;
            }
            if (pName.Equals(res.form.GetString("String53")))
            {
                e.Value = func.getRule(curEle);
                return;
            }
        }
        private static void SetValue(object sender, PropertySpecEventArgs e)
        {
            string pName = e.Property.Name;
            string pValue = e.Value.ToString();
            if (pName.Equals(res.form.GetString("String45")))
            {
                if (!str.IsNatural(pValue))
                {
                    MsgBox.Warning("Invalid ID!");
                }
                else
                {
                    func.setNewID(curEle, pValue);
                }
                return;
            }
            if (pName.Equals(res.form.GetString("String51")))
            {
                int setvalue = (pValue.Equals(res.form.GetString("String48")) ? 0 : (pValue.Equals(res.form.GetString("String49")) ? 1 : 2));
                func.setNewType(curEle, setvalue);
                return;
            }
            if (pName.Equals(res.form.GetString("String53")))
            {
                func.setNewRule(curEle, pValue);
                return;
            }
        }
    }
    [System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
    public class Radio
    {
        public Radio()
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl));
        }
        public static IHTMLElement curEle = null;
        public static PropertyBag Bag()
        {
            PropertyBag bag = new PropertyBag();
            bag.GetValue += new PropertySpecEventHandler(GetValue);
            bag.SetValue += new PropertySpecEventHandler(SetValue);
            string curcate = res.form.GetString("String47");
            bag.Properties.Add(new PropertySpec(res.form.GetString("String45"), typeof(string), curcate, res.form.GetString("String46"), func.getID(curEle)));
            int eletype = func.getType(curEle);
            bag.Properties.Add(new PropertySpec(res.form.GetString("String51"), typeof(string), curcate, res.form.GetString("String52"), (eletype == 0 ? res.form.GetString("String48") : (eletype == 1 ? res.form.GetString("String49") : res.form.GetString("String50"))), typeof(System.Drawing.Design.UITypeEditor), typeof(FormTextBoxType)));
            bag.Properties.Add(new PropertySpec(res.form.GetString("String53"), typeof(string), curcate, res.form.GetString("String54"), func.getRule(curEle), typeof(FTDPClient.classes.TextEditor), typeof(FTDPClient.classes.TextEditorConverter)));
            bag.Properties.Add(new PropertySpec(res.form.GetString("String60"), typeof(string), curcate, res.form.GetString("String61"), "(Collection)", typeof(FTDPClient.classes.OptionEditor), typeof(FTDPClient.classes.OptionEditorConverter)));
            return bag;
        }
        private static void GetValue(object sender, PropertySpecEventArgs e)
        {
            string pName = e.Property.Name;
            if (pName.Equals(res.form.GetString("String45")))
            {
                e.Value = func.getID(curEle);
                return;
            }
            if (pName.Equals(res.form.GetString("String51")))
            {
                int eletype = func.getType(curEle);
                e.Value = (eletype == 0 ? res.form.GetString("String48") : (eletype == 1 ? res.form.GetString("String49") : res.form.GetString("String50")));
                return;
            }
            if (pName.Equals(res.form.GetString("String53")))
            {
                e.Value = func.getRule(curEle);
                return;
            }
            if (pName.Equals(res.form.GetString("String60")))
            {
                e.Value = func.getOptions(curEle);
                return;
            }
        }
        private static void SetValue(object sender, PropertySpecEventArgs e)
        {
            string pName = e.Property.Name;
            string pValue = e.Value.ToString();
            if (pName.Equals(res.form.GetString("String45")))
            {
                if (!str.IsNatural(pValue))
                {
                    MsgBox.Warning("Invalid ID!");
                }
                else
                {
                    func.setNewID(curEle, pValue);
                }
                return;
            }
            if (pName.Equals(res.form.GetString("String51")))
            {
                int setvalue = (pValue.Equals(res.form.GetString("String48")) ? 0 : (pValue.Equals(res.form.GetString("String49")) ? 1 : 2));
                func.setNewType(curEle, setvalue);
                return;
            }
            if (pName.Equals(res.form.GetString("String53")))
            {
                func.setNewRule(curEle, pValue);
                return;
            }
            if (pName.Equals(res.form.GetString("String60")))
            {
                func.setNewOptions(curEle, (ArrayList)e.Value);
                return;
            }
        }
    }
    [System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
    public class Checkbox
    {
        public Checkbox()
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl));
        }
        public static IHTMLElement curEle = null;
        public static PropertyBag Bag()
        {
            PropertyBag bag = new PropertyBag();
            bag.GetValue += new PropertySpecEventHandler(GetValue);
            bag.SetValue += new PropertySpecEventHandler(SetValue);
            string curcate = res.form.GetString("String47");
            bag.Properties.Add(new PropertySpec(res.form.GetString("String45"), typeof(string), curcate, res.form.GetString("String46"), func.getID(curEle)));
            int eletype = func.getType(curEle);
            bag.Properties.Add(new PropertySpec(res.form.GetString("String51"), typeof(string), curcate, res.form.GetString("String52"), (eletype == 0 ? res.form.GetString("String48") : (eletype == 1 ? res.form.GetString("String49") : res.form.GetString("String50"))), typeof(System.Drawing.Design.UITypeEditor), typeof(FormTextBoxType)));
            bag.Properties.Add(new PropertySpec(res.form.GetString("String53"), typeof(string), curcate, res.form.GetString("String54"), func.getRule(curEle), typeof(FTDPClient.classes.TextEditor), typeof(FTDPClient.classes.TextEditorConverter)));
            bag.Properties.Add(new PropertySpec(res.form.GetString("String60"), typeof(string), curcate, res.form.GetString("String61"), "(Collection)", typeof(FTDPClient.classes.OptionEditor), typeof(FTDPClient.classes.OptionEditorConverter)));
            return bag;
        }
        private static void GetValue(object sender, PropertySpecEventArgs e)
        {
            string pName = e.Property.Name;
            if (pName.Equals(res.form.GetString("String45")))
            {
                e.Value = func.getID(curEle);
                return;
            }
            if (pName.Equals(res.form.GetString("String51")))
            {
                int eletype = func.getType(curEle);
                e.Value = (eletype == 0 ? res.form.GetString("String48") : (eletype == 1 ? res.form.GetString("String49") : res.form.GetString("String50")));
                return;
            }
            if (pName.Equals(res.form.GetString("String53")))
            {
                e.Value = func.getRule(curEle);
                return;
            }
            if (pName.Equals(res.form.GetString("String60")))
            {
                e.Value = func.getOptions(curEle);
                return;
            }
        }
        private static void SetValue(object sender, PropertySpecEventArgs e)
        {
            string pName = e.Property.Name;
            string pValue = e.Value.ToString();
            if (pName.Equals(res.form.GetString("String45")))
            {
                if (!str.IsNatural(pValue))
                {
                    MsgBox.Warning("Invalid ID!");
                }
                else
                {
                    func.setNewID(curEle, pValue);
                }
                return;
            }
            if (pName.Equals(res.form.GetString("String51")))
            {
                int setvalue = (pValue.Equals(res.form.GetString("String48")) ? 0 : (pValue.Equals(res.form.GetString("String49")) ? 1 : 2));
                func.setNewType(curEle, setvalue);
                return;
            }
            if (pName.Equals(res.form.GetString("String53")))
            {
                func.setNewRule(curEle, pValue);
                return;
            }
            if (pName.Equals(res.form.GetString("String60")))
            {
                func.setNewOptions(curEle, (ArrayList)e.Value);
                return;
            }
        }
    }
    [System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
    public class FileBox
    {
        public FileBox()
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl));
        }
        public static IHTMLElement curEle = null;
        public static PropertyBag Bag()
        {
            PropertyBag bag = new PropertyBag();
            bag.GetValue += new PropertySpecEventHandler(GetValue);
            bag.SetValue += new PropertySpecEventHandler(SetValue);
            string curcate = res.form.GetString("String47");
            bag.Properties.Add(new PropertySpec(res.form.GetString("String45"), typeof(string), curcate, res.form.GetString("String46"), func.getID(curEle)));
            int eletype = func.getType(curEle);
            bag.Properties.Add(new PropertySpec(res.form.GetString("String51"), typeof(string), curcate, res.form.GetString("String52"), (eletype == 0 ? res.form.GetString("String48") : (eletype == 1 ? res.form.GetString("String49") : res.form.GetString("String50"))), typeof(System.Drawing.Design.UITypeEditor), typeof(FormTextBoxType)));
            bag.Properties.Add(new PropertySpec(res.form.GetString("String53"), typeof(string), curcate, res.form.GetString("String54"), func.getRule(curEle), typeof(FTDPClient.classes.TextEditor), typeof(FTDPClient.classes.TextEditorConverter)));
            return bag;
        }
        private static void GetValue(object sender, PropertySpecEventArgs e)
        {
            string pName = e.Property.Name;
            if (pName.Equals(res.form.GetString("String45")))
            {
                e.Value = func.getID(curEle);
                return;
            }
            if (pName.Equals(res.form.GetString("String51")))
            {
                int eletype = func.getType(curEle);
                e.Value = (eletype == 0 ? res.form.GetString("String48") : (eletype == 1 ? res.form.GetString("String49") : res.form.GetString("String50")));
                return;
            }
            if (pName.Equals(res.form.GetString("String53")))
            {
                e.Value = func.getRule(curEle);
                return;
            }
        }
        private static void SetValue(object sender, PropertySpecEventArgs e)
        {
            string pName = e.Property.Name;
            string pValue = e.Value.ToString();
            if (pName.Equals(res.form.GetString("String45")))
            {
                if (!str.IsNatural(pValue))
                {
                    MsgBox.Warning("Invalid ID!");
                }
                else
                {
                    func.setNewID(curEle, pValue);
                }
                return;
            }
            if (pName.Equals(res.form.GetString("String51")))
            {
                int setvalue = (pValue.Equals(res.form.GetString("String48")) ? 0 : (pValue.Equals(res.form.GetString("String49")) ? 1 : 2));
                func.setNewType(curEle, setvalue);
                return;
            }
            if (pName.Equals(res.form.GetString("String53")))
            {
                func.setNewRule(curEle, pValue);
                return;
            }
        }
    }
    [System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
    public class Label
    {
        public Label()
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl));
        }
        public static IHTMLElement curEle = null;
        public static PropertyBag Bag()
        {
            PropertyBag bag = new PropertyBag();
            bag.GetValue += new PropertySpecEventHandler(GetValue);
            bag.SetValue += new PropertySpecEventHandler(SetValue);
            string curcate = res.form.GetString("String47");
            bag.Properties.Add(new PropertySpec(res.form.GetString("String45"), typeof(string), curcate, res.form.GetString("String46"), func.getID(curEle)));
            return bag;
        }
        private static void GetValue(object sender, PropertySpecEventArgs e)
        {
            string pName = e.Property.Name;
            if (pName.Equals(res.form.GetString("String45")))
            {
                e.Value = func.getID(curEle);
                return;
            }
        }
        private static void SetValue(object sender, PropertySpecEventArgs e)
        {
            string pName = e.Property.Name;
            string pValue = e.Value.ToString();
            if (pName.Equals(res.form.GetString("String45")))
            {
                if (!str.IsNatural(pValue))
                {
                    MsgBox.Warning("Invalid ID!");
                }
                else
                {
                    func.setNewID(curEle, pValue);
                }
                return;
            }
        }
    }
    [System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
    public class Category
    {
        public Category()
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl));
        }
        public static IHTMLElement curEle = null;
        public static PropertyBag Bag()
        {
            PropertyBag bag = new PropertyBag();
            bag.GetValue += new PropertySpecEventHandler(GetValue);
            bag.SetValue += new PropertySpecEventHandler(SetValue);
            string curcate = res.form.GetString("String47");
            bag.Properties.Add(new PropertySpec(res.form.GetString("String53"), typeof(string), curcate, res.form.GetString("String54"), func.getRule(curEle), typeof(FTDPClient.classes.TextEditor), typeof(FTDPClient.classes.TextEditorConverter)));
            bag.Properties.Add(new PropertySpec(res.form.GetString("String60"), typeof(string), curcate, res.form.GetString("String61"), "(Collection)", typeof(FTDPClient.classes.OptionEditor), typeof(FTDPClient.classes.OptionEditorConverter)));
            return bag;
        }
        private static void GetValue(object sender, PropertySpecEventArgs e)
        {
            string pName = e.Property.Name;
            if (pName.Equals(res.form.GetString("String53")))
            {
                e.Value = func.getRule(curEle);
                return;
            }
            if (pName.Equals(res.form.GetString("String60")))
            {
                e.Value = func.getOptions(curEle);
                return;
            }
        }
        private static void SetValue(object sender, PropertySpecEventArgs e)
        {
            string pName = e.Property.Name;
            string pValue = e.Value.ToString();
            if (pName.Equals(res.form.GetString("String53")))
            {
                func.setNewRule(curEle, pValue);
                return;
            }
            if (pName.Equals(res.form.GetString("String60")))
            {
                func.setNewOptions(curEle, (ArrayList)e.Value);
                return;
            }
        }
    }
    [System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
    public class StatFilter
    {
        public StatFilter()
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl));
        }
        public static IHTMLElement curEle = null;
        public static PropertyBag Bag()
        {
            PropertyBag bag = new PropertyBag();
            bag.GetValue += new PropertySpecEventHandler(GetValue);
            bag.SetValue += new PropertySpecEventHandler(SetValue);
            string curcate = res.form.GetString("String47");
            int eletype = func.getStatFilterType(curEle);
            bag.Properties.Add(new PropertySpec(res.form.GetString("String63"), typeof(string), curcate, res.form.GetString("String64"), (eletype == 0 ? res.form.GetString("String65") : res.form.GetString("String66")), typeof(System.Drawing.Design.UITypeEditor), typeof(FormStatFilterType)));
            return bag;
        }
        private static void GetValue(object sender, PropertySpecEventArgs e)
        {
            string pName = e.Property.Name;
            if (pName.Equals(res.form.GetString("String63")))
            {
                int eletype = func.getStatFilterType(curEle);
                e.Value = (eletype == 0 ? res.form.GetString("String65") : res.form.GetString("String66"));
                return;
            }
        }
        private static void SetValue(object sender, PropertySpecEventArgs e)
        {
            string pName = e.Property.Name;
            string pValue = e.Value.ToString();
            if (pName.Equals(res.form.GetString("String63")))
            {
                int setvalue = pValue.Equals(res.form.GetString("String65")) ? 0 :1;
                func.setNewStatFilter(curEle, setvalue);
                return;
            }
        }
    }
    [System.ComponentModel.LicenseProviderAttribute(typeof(FTDPClient.classes.LicenceProvider))]
    public class DateAll
    {
        public DateAll()
        {
            System.ComponentModel.LicenseManager.Validate(typeof(FTDPClient.forms.NewControl));
        }
        public static IHTMLElement curEle = null;
        public static PropertyBag Bag()
        {
            PropertyBag bag = new PropertyBag();
            bag.GetValue += new PropertySpecEventHandler(GetValue);
            bag.SetValue += new PropertySpecEventHandler(SetValue);
            string curcate = res.form.GetString("String47");
            bag.Properties.Add(new PropertySpec(res.form.GetString("String69"), typeof(string), curcate, res.form.GetString("String70"), func.getRule(curEle), typeof(FTDPClient.classes.TextEditor), typeof(FTDPClient.classes.TextEditorConverter)));
            return bag;
        }
        private static void GetValue(object sender, PropertySpecEventArgs e)
        {
            string pName = e.Property.Name;
            if (pName.Equals(res.form.GetString("String69")))
            {
                e.Value = func.getRule(curEle);
                return;
            }
        }
        private static void SetValue(object sender, PropertySpecEventArgs e)
        {
            string pName = e.Property.Name;
            string pValue = e.Value.ToString();
            if (pName.Equals(res.form.GetString("String69")))
            {
                func.setNewRule(curEle, pValue);
                return;
            }
        }
    }
}
