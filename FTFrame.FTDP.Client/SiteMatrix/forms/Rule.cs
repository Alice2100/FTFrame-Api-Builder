using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FTDPClient.Obj;
using FTDPClient.Style;
using FTDPClient.functions;
using FTDPClient.consts;
using FTDPClient.database;
using FTDPClient.forms;
using FTDPClient.SiteClass;
using mshtml;
using htmleditocx;
using FTDPClient.Page;
using FTDPClient.Common;
using System.Collections;
using Microsoft.Data.Sqlite;
using System.Reflection;

namespace FTDPClient.forms
{
    public partial class Rule : Form
    {
        public Rule()
        {
            InitializeComponent();
            #region Language
            label_caption.Text = res.anew.str("Rule.label_caption");
            label_apipath.Text = res.anew.str("Rule.label_apipath");
            btn_apipath.Text = res.anew.str("Rule.btn_bindall");
            btn_table.Text = res.anew.str("Rule.btn_bindall");
            btn_column.Text = res.anew.str("Rule.btn_bindall");
            btn_apipath_remove.Text = res.anew.str("Rule.btn_removeall");
            btn_table_remove.Text = res.anew.str("Rule.btn_removeall");
            btn_column_remove.Text = res.anew.str("Rule.btn_removeall");
            label_rule.Text = res.anew.str("Rule.label_rule");
            label_table.Text = res.anew.str("Rule.label_table");
            label_column.Text = res.anew.str("Rule.label_column");
            Menu_NewRootDir.Text = res.anew.str("Rule.Menu_NewRootDir");
            Menu_NewDir.Text = res.anew.str("Rule.Menu_NewDir");
            Menu_NewDir_Name.Text = res.anew.str("Rule.Menu_NewDir_Name");
            Menu_NewDir_Bind.Text = res.anew.str("Rule.Menu_NewDir_Bind");
            Menu_NewTableRule.Text = res.anew.str("Rule.Menu_NewTableRule");
            Menu_NewTableRule_Name.Text = res.anew.str("Rule.Menu_NewTableRule_Name");
            Menu_NewTableRule_Bind.Text = res.anew.str("Rule.Menu_NewTableRule_Bind");
            Menu_NewColumnRule.Text = res.anew.str("Rule.Menu_NewColumnRule");
            Menu_NewColumnRule_Name.Text = res.anew.str("Rule.Menu_NewColumnRule_Name");
            Menu_NewColumnRule_Bind.Text = res.anew.str("Rule.Menu_NewColumnRule_Bind");
            Menu_Clone.Text = res.anew.str("Rule.Menu_Clone");
            Menu_Dell.Text = res.anew.str("Rule.Menu_Dell");
            Menu_Refresh.Text = res.anew.str("Rule.Menu_Refresh");
            #endregion
        }
        private List<RuleItem> rules = new List<RuleItem>();
        private void Rule_Load(object sender, EventArgs e)
        {

            new FTDP.Util.ICSharpTextEditor().Init(this, box_rule_text, false, null);
            EditControls = new Control[] {
            label_caption,box_caption,
            label_apipath,box_apipath,btn_apipath,btn_apipath_remove,
            label_table,box_table,btn_table,btn_table_remove,
            label_column,box_column,btn_column,btn_column_remove,
            label_rule,box_rule_sel1,box_rule_sel2,box_rule_text,label_rule_desc,
            };
            HiddenControls();
            label_table.Location = label_apipath.Location;
            box_table.Location = box_apipath.Location;
            btn_table.Location = btn_apipath.Location;
            btn_table_remove.Location = btn_apipath_remove.Location;
            label_column.Location = label_apipath.Location;
            box_column.Location = box_apipath.Location;
            btn_column.Location = btn_apipath.Location;
            btn_column_remove.Location = btn_apipath_remove.Location;
            label_rule_desc.Location = new Point(box_rule_sel1.Location.X, box_rule_sel1.Location.Y + 7);
            RuleBuild();
            TreeBuild();
        }
        private void RuleBuild()
        {
            rules = new List<RuleItem>();
            var dbtype = Options.GetSystemDBSetType_Plat();
            string connstr = Options.GetSystemDBSetConnStr_Plat();
            string sql = "select * from ft_rule_dir where pid='' order by caption";
            var dt = Adv.RemoteSqlQuery(sql, dbtype, connstr);
            foreach (DataRow row in dt.Rows)
            {
                var itemsTable = build_dir_table(row["id"].ToString());
                var rule = new RuleItem()
                {
                    Id = row["id"].ToString(),
                    Text = row["caption"].ToString(),
                    Type = RuleItemType.DIR,
                    Bind = itemsTable.Count() == 0 || row["apipath"].ToString() != "",
                    Items = build_dir(row["id"].ToString()).Union(itemsTable).ToList()
                };
                rules.Add(rule);
            }
            List<RuleItem> build_dir(string id)
            {
                var items = new List<RuleItem>();
                sql = "select * from ft_rule_dir where pid='" + id + "' order by caption";
                var dt0 = Adv.RemoteSqlQuery(sql, dbtype, connstr);
                foreach (DataRow row in dt0.Rows)
                {
                    var itemsTable = build_dir_table(row["id"].ToString());
                    var rule = new RuleItem()
                    {
                        Id = row["id"].ToString(),
                        Text = row["caption"].ToString(),
                        Type = RuleItemType.DIR,
                        Bind = itemsTable.Count() == 0 || row["apipath"].ToString() != "",
                        Items = build_dir(row["id"].ToString()).Union(itemsTable).ToList()
                    };
                    items.Add(rule);
                }
                return items;
            }
            List<RuleItem> build_dir_table(string id)
            {
                var items = new List<RuleItem>();
                sql = "select * from ft_rule_item where pid='" + id + "' order by caption";
                var dt0 = Adv.RemoteSqlQuery(sql, dbtype, connstr);
                foreach (DataRow row in dt0.Rows)
                {
                    var rule = new RuleItem()
                    {
                        Id = row["id"].ToString(),
                        Text = row["caption"].ToString(),
                        Type = RuleItemType.TABLE,
                        Bind = row["bindtable"].ToString() != "",
                        Items = build_column(row["id"].ToString())
                    };
                    items.Add(rule);
                }
                return items;
            }
            List<RuleItem> build_column(string id)
            {
                var items = new List<RuleItem>();
                sql = "select * from ft_rule_item where pid='" + id + "' order by caption";
                var dt0 = Adv.RemoteSqlQuery(sql, dbtype, connstr);
                foreach (DataRow row in dt0.Rows)
                {
                    var rule = new RuleItem()
                    {
                        Id = row["id"].ToString(),
                        Text = row["caption"].ToString(),
                        Type = RuleItemType.COLUMN,
                        Bind = row["bindcolumn"].ToString() != "",
                        Items = new List<RuleItem>()
                    };
                    items.Add(rule);
                }
                return items;
            }
        }
        private void TreeBuild()
        {
            tv.Nodes.Clear();
            rules.ForEach(rule =>
            {
                childs(tv_NodeAdd(null, rule), rule.Items);
            });
            void childs(TreeNode treeNode, List<RuleItem> ruleItems)
            {
                if (ruleItems != null)
                {
                    ruleItems.ForEach(rule =>
                    {
                        childs(tv_NodeAdd(treeNode, rule), rule.Items);
                    });
                }
            }
        }
        TreeNode tv_NodeAdd(TreeNode ParentNode, RuleItem rule)
        {
            int imageIndex = 17;
            if (rule.Type == RuleItemType.TABLE) imageIndex = 25;
            else if (rule.Type == RuleItemType.COLUMN) imageIndex = 32;
            TreeNode treeNode = new TreeNode(rule.Text, imageIndex, imageIndex);
            treeNode.Tag = rule;
            treeNode.ForeColor = rule.Bind ? Color.Black : Color.Red;
            if (ParentNode == null)
            {
                tv.Nodes.Add(treeNode);
            }
            else
            {
                ParentNode.Nodes.Add(treeNode);
                ParentNode.Expand();
            }
            return treeNode;
        }
        private void tv_MouseDown(object sender, MouseEventArgs e)
        {
            Point p = new Point(e.X, e.Y);
            TreeNode nd = tv.GetNodeAt(p);
            if (nd != null)
            {
                tv.SelectedNode = nd;
            }
        }

        void HiddenControls()
        {
            foreach (var control in EditControls) control.Visible = false;
        }
        RuleItem EditingRule;//正在编辑的Item
        private void tv_AfterSelect(object sender, TreeViewEventArgs e)
        {
            HiddenControls();
            if (tv.SelectedNode != null)
            {
                EditingRule = null;
                var rule = tv.SelectedNode.Tag as RuleItem;
                DataTable dt = null;
                if (rule.Type == RuleItemType.DIR)
                {
                    dt = Adv.RemoteSqlQuery("select * from ft_rule_dir where id='" + rule.Id + "'");
                    foreach (var control in EditControls)
                    {
                        switch (control.Name)
                        {
                            case "label_caption":
                            case "box_caption":
                            case "label_apipath":
                            case "box_apipath":
                            case "btn_apipath":
                            case "btn_apipath_remove":
                                control.Visible = true; break;
                        }
                    }
                    if (dt.Rows.Count == 0)
                    {
                        MsgBox.Error(res.anew.str("Rule.shujubucunzai"));
                    }
                    else
                    {
                        box_caption.Text = dt.Rows[0]["caption"].ToString();
                        string sql = "select ApiPath,Mimo,ApiType from ft_ftdp_apidoc where ApiPath='" + str.D2DD(dt.Rows[0]["apipath"].ToString()) + "'";
                        var dtapi = Adv.RemoteSqlQuery(sql);
                        box_apipath.Text = res.anew.str("Rule.weibangding"); 
                        NodeAction(GetTreeNode(rule.Id), n => { n.ForeColor = tv.SelectedNode.Nodes.Cast<TreeNode>().Where(r => ((RuleItem)r.Tag).Type == RuleItemType.TABLE).Count() > 0 ? Color.Red : Color.Black; });
                        if (dtapi.Rows.Count > 0)
                        {
                            box_apipath.Text = dtapi.Rows[0]["ApiPath"].ToString() + "\r\n\r\n" + dtapi.Rows[0]["Mimo"].ToString() + "\r\n\r\n" + res.anew.str("Rule.leixing") + functions.control.ApiCaption(dtapi.Rows[0]["ApiType"].ToString());
                            NodeAction(GetTreeNode(rule.Id), n => { n.ForeColor = Color.Black; });
                        }
                        EditingRule = new RuleItem() { Id = rule.Id, Type = rule.Type };
                    }
                }
                else if (rule.Type == RuleItemType.TABLE)
                {
                    dt = Adv.RemoteSqlQuery("select * from ft_rule_item where id='" + rule.Id + "'");
                    foreach (var control in EditControls)
                    {
                        switch (control.Name)
                        {
                            case "label_caption":
                            case "box_caption":
                            case "label_table":
                            case "box_table":
                            case "btn_table":
                            case "btn_table_remove":
                            case "label_rule":
                                control.Visible = true; break;
                        }
                    }
                    if (dt.Rows.Count == 0)
                    {
                        MsgBox.Error(res.anew.str("Rule.shujubucunzai"));
                    }
                    else
                    {
                        box_caption.Text = dt.Rows[0]["caption"].ToString();
                        box_table.Text = res.anew.str("Rule.weibangding");
                        NodeAction(GetTreeNode(rule.Id), n => { n.ForeColor = Color.Red; });
                        if (dt.Rows[0]["bindtable"].ToString() != "")
                        {
                            var desc = Adv.TableDesc(dt.Rows[0]["bindtable"].ToString());
                            if (desc != null)
                            {
                                box_table.Text = dt.Rows[0]["bindtable"].ToString() + "\r\n\r\n" + desc;
                                NodeAction(GetTreeNode(rule.Id), n => { n.ForeColor = Color.Black; });
                            }
                        }
                        EditingRule = new RuleItem() { Id = rule.Id, Type = rule.Type };
                        RuleSet_Initing = true;
                        RuleInit_Table(rule);
                        RuleSet_Init();
                        RuleSet_Initing = false;
                    }
                }
                else if (rule.Type == RuleItemType.COLUMN)
                {
                    dt = Adv.RemoteSqlQuery("select * from ft_rule_item where id='" + rule.Id + "'");
                    foreach (var control in EditControls)
                    {
                        switch (control.Name)
                        {
                            case "label_caption":
                            case "box_caption":
                            case "label_column":
                            case "box_column":
                            case "btn_column":
                            case "btn_column_remove":
                            case "label_rule":
                                control.Visible = true; break;
                        }
                    }
                    if (dt.Rows.Count == 0)
                    {
                        MsgBox.Error(res.anew.str("Rule.shujubucunzai"));
                    }
                    else
                    {
                        box_caption.Text = dt.Rows[0]["caption"].ToString();
                        box_column.Text = res.anew.str("Rule.weibangding");
                        NodeAction(GetTreeNode(rule.Id), n => { n.ForeColor = Color.Red; });
                        if (dt.Rows[0]["bindcolumn"].ToString() != "")
                        {
                            string pid = (tv.SelectedNode.Parent.Tag as RuleItem).Id;
                            string tablename = null;
                            if (pid != null)
                            {
                                string sql = "select bindtable from ft_rule_item where id='" + pid + "'";
                                var dtrt = Adv.RemoteSqlQuery(sql);
                                if (dtrt.Rows.Count > 0) tablename = dtrt.Rows[0][0].ToString();
                            }
                            if (tablename != null)
                            {
                                var desc = Adv.ColumnDesc(tablename, dt.Rows[0]["bindcolumn"].ToString());
                                if (desc != null)
                                {
                                    box_column.Text = tablename + "." + dt.Rows[0]["bindcolumn"].ToString() + "\r\n\r\n" + desc;
                                    NodeAction(GetTreeNode(rule.Id), n => { n.ForeColor = Color.Black; });
                                }
                            }
                        }
                        EditingRule = new RuleItem() { Id = rule.Id, Type = rule.Type };
                        RuleSet_Initing = true;
                        RuleInit_Column(rule);
                        RuleSet_Init();
                        RuleSet_Initing = false;
                    }
                }
            }
        }
        static List<RuleInformation> RuleInformation = null;
        List<RuleInformation> RuleInformationBuild()
        {
            if (RuleInformation != null) return RuleInformation;
            RuleInformation = new List<RuleInformation>();
            var types = Assembly.GetExecutingAssembly().GetTypes().Where(r => r.Namespace == "FTDPClient.RuleClass" && r.IsClass);
            List<RuleInformation> items = new List<RuleInformation>();
            foreach (var type in types)
            {
                if (!type.Name.StartsWith("<>"))
                {
                    object obj = Activator.CreateInstance(type);
                    MethodInfo mi = type.GetMethod("Information");
                    var ret = mi.Invoke(obj, null) as RuleInformation;
                    ret.RuleId = type.Name;
                    RuleInformation.Add(ret);
                }
            }
            return RuleInformation;
        }
        void RuleInit_Table(RuleItem rule)
        {
            string pid = (GetTreeNode(rule.Id).Parent.Tag as RuleItem).Id;
            string sql = "select b.ApiType from ft_rule_dir a left join ft_ftdp_apidoc b on a.apipath=b.ApiPath where a.id='" + pid + "'";
            var dt = Adv.RemoteSqlQuery(sql);
            string _ApiType = null;
            if (dt.Rows.Count > 0)
            {
                _ApiType = dt.Rows[0]["ApiType"]?.ToString();
            }
            if (string.IsNullOrEmpty(_ApiType))
            {
                label_rule_desc.Visible = true;
                label_rule_desc.Text = res.anew.str("Rule.weibangding2");
                return;
            }
            box_rule_sel1.Items.Clear();
            box_rule_sel1.Items.Add(new ComboItem() { Name = res.anew.str("Rule.wu"), Id = "" });
            ApiType apiType = ApiType.LIST;
            switch (_ApiType)
            {
                case "List": apiType = ApiType.LIST; break;
                case "DyValue": apiType = ApiType.DYVALUE; break;
                case "DataOP": apiType = ApiType.DATAOP; break;
            }
            List<RuleInformation> items = new List<RuleInformation>();
            foreach (var item in RuleInformationBuild())
            {
                if (item.Type == "Table" && item.ApiType == apiType)
                {
                    items.Add(item);
                }
            }
            foreach (var item in items.OrderBy(r => r.Rank))
            {
                var obj = new ComboItem() { Name = item.Caption, Id = item.RuleId };
                obj.Tag = item;
                box_rule_sel1.Items.Add(obj);
            }
            box_rule_sel1.SelectedIndex = 0;
            box_rule_sel1.Visible = true;
        }
        void RuleInit_Column(RuleItem rule)
        {
            string pid = (GetTreeNode(rule.Id).Parent.Parent.Tag as RuleItem).Id;
            string sql = "select b.ApiType from ft_rule_dir a left join ft_ftdp_apidoc b on a.apipath=b.ApiPath where a.id='" + pid + "'";
            var dt = Adv.RemoteSqlQuery(sql);
            string _ApiType = null;
            if (dt.Rows.Count > 0)
            {
                _ApiType = dt.Rows[0]["ApiType"]?.ToString();
            }
            if (string.IsNullOrEmpty(_ApiType))
            {
                label_rule_desc.Visible = true;
                label_rule_desc.Text = res.anew.str("Rule.weibangding2");
                return;
            }
            box_rule_sel1.Items.Clear();
            box_rule_sel1.Items.Add(new ComboItem() { Name = res.anew.str("Rule.wu"), Id = "" });
            ApiType apiType = ApiType.LIST;
            switch (_ApiType)
            {
                case "List": apiType = ApiType.LIST; break;
                case "DyValue": apiType = ApiType.DYVALUE; break;
                case "DataOP": apiType = ApiType.DATAOP; break;
            }
            List<RuleInformation> items = new List<RuleInformation>();
            foreach (var item in RuleInformationBuild())
            {
                if (item.Type == "Column" && item.ApiType == apiType)
                {
                    items.Add(item);
                }
            }
            foreach (var item in items.OrderBy(r => r.Rank))
            {
                var obj = new ComboItem() { Name = item.Caption, Id = item.RuleId };
                obj.Tag = item;
                box_rule_sel1.Items.Add(obj);
            }
            box_rule_sel1.SelectedIndex = 0;
            box_rule_sel1.Visible = true;
        }
        void RuleSet_Init()
        {
            string sql = "select * from ft_rule_atom where pid='" + EditingRule.Id + "'";
            var dt = Adv.RemoteSqlQuery(sql);
            if (dt.Rows.Count > 0)
            {
                string ruleSet = dt.Rows[0]["rule"].ToString();
                string[] ruleItem = ruleSet.Split(new string[] { "[##]" }, StringSplitOptions.None);
                string rule0 = ruleItem[0];
                string rule1 = ruleItem[1];
                string rule2 = ruleItem[2];
                string rule3 = ruleItem[3];
                for (int i = 0; i < box_rule_sel1.Items.Count; i++)
                {
                    if ((box_rule_sel1.Items[i] as ComboItem).Name == rule1) box_rule_sel1.SelectedIndex = i;
                }
                if (box_rule_sel2.Visible)
                {
                    for (int i = 0; i < box_rule_sel2.Items.Count; i++)
                    {
                        if ((box_rule_sel2.Items[i] as ComboItem).Id == rule2) box_rule_sel2.SelectedIndex = i;
                    }
                }
                if (box_rule_text.Visible)
                {
                    box_rule_text.Text = rule3;
                }
            }
        }
        void RuleSetOtherInit()
        {
            box_rule_sel2.Visible = false;
            box_rule_text.Visible = false;
            box_rule_sel2.Items.Clear();
            box_rule_text.Text = "";
            var obj = box_rule_sel1.SelectedItem as ComboItem;
            if (obj.Id != "")
            {
                var ruleInfo = obj.Tag as RuleInformation;
                if (ruleInfo.IsText) box_rule_text.Visible = true;
                if (ruleInfo.SubRules != null && ruleInfo.SubRules.Count > 0)
                {
                    box_rule_sel2.Visible = true;
                    ruleInfo.SubRules.ForEach(r => { box_rule_sel2.Items.Add(new ComboItem() { Name = r[1], Id = r[0] }); });
                    box_rule_sel2.SelectedIndex = 0;
                }
            }
        }
        private void edit_leave(object sender, EventArgs e)
        {
            if (EditingRule != null)
            {
                string sql;
                if (EditingRule.Type == RuleItemType.DIR)
                {
                    sql = "update ft_rule_dir set caption='" + str.D2DD(box_caption.Text) + "' where id='" + EditingRule.Id + "'";
                    Adv.RemoteSqlExec(sql);
                    NodeAction(GetTreeNode(), n => { n.Text = box_caption.Text; });
                }
                else if (EditingRule.Type == RuleItemType.TABLE)
                {
                    sql = "update ft_rule_item set caption='" + str.D2DD(box_caption.Text) + "' where id='" + EditingRule.Id + "'";
                    Adv.RemoteSqlExec(sql);
                    NodeAction(GetTreeNode(), n => { n.Text = box_caption.Text; });
                }
                else if (EditingRule.Type == RuleItemType.COLUMN)
                {
                    sql = "update ft_rule_item set caption='" + str.D2DD(box_caption.Text) + "' where id='" + EditingRule.Id + "'";
                    Adv.RemoteSqlExec(sql);
                    NodeAction(GetTreeNode(), n => { n.Text = box_caption.Text; });
                }
            }
        }

        Control[] EditControls;
        public static void NodeAction<TreeNode>(TreeNode node, Action<TreeNode> action)
        {
            if (node != null) action(node);
        }
        TreeNode GetTreeNode(string id = null)
        {
            if (id == null) id = EditingRule.Id;
            foreach (TreeNode node in tv.Nodes)
            {
                if (((RuleItem)(node.Tag)).Id == id)
                {
                    return node;
                }
                var _node = GetTreeNodeChilds(node);
                if (_node != null) return _node;
            }
            TreeNode GetTreeNodeChilds(TreeNode treeNode)
            {
                foreach (TreeNode node in treeNode.Nodes)
                {
                    if (((RuleItem)(node.Tag)).Id == id)
                    {
                        return node;
                    }
                    var _node = GetTreeNodeChilds(node);
                    if (_node != null) return _node;
                }
                return null;
            }
            return null;
        }
        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            Menu_Dell.Enabled = false;
            Menu_NewColumnRule.Enabled = false;
            Menu_NewDir.Enabled = false;
            Menu_NewTableRule.Enabled = false;
            Menu_Clone.Enabled = false;
            if (tv.SelectedNode != null)
            {
                var rule = tv.SelectedNode.Tag as RuleItem;
                Menu_Dell.Enabled = true;
                Menu_Clone.Enabled = true;
                if (rule.Type == RuleItemType.DIR)
                {
                    Menu_NewDir.Enabled = true;
                    Menu_NewTableRule.Enabled = true;
                }
                else if (rule.Type == RuleItemType.TABLE)
                {
                    Menu_NewColumnRule.Enabled = true;
                }
            }
            edit_leave(null, null);
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            InputName inputName = new InputName();
            inputName.TopMost = true;
            inputName.ShowDialog();
            if (inputName.NameValue.Text != "")
            {
                RuleItem rule = new RuleItem()
                {
                    Id = rdm.getCombID(),
                    Text = inputName.NameValue.Text.Trim(),
                    Type = RuleItemType.DIR,
                    Bind = true
                };
                string sql = "insert into ft_rule_dir(id,pid,caption,apipath,updatetime)";
                sql += "values('" + rule.Id + "','','" + str.D2DD(rule.Text) + "','','" + str.GetDateTime(DateTime.Now) + "')";
                Adv.RemoteSqlExec(sql);
                tv_NodeAdd(null, rule);
            }
        }
        private void Menu_NewDir_Click(object sender, EventArgs e)
        {

        }

        private void Menu_NewTableRule_Click(object sender, EventArgs e)
        {

        }

        private void Menu_NewColumnRule_Click(object sender, EventArgs e)
        {

        }
        
        private void Menu_Dell_Click(object sender, EventArgs e)
        {
            if (tv.SelectedNode.Nodes.Count > 0 && MsgBox.YesNoCancel(res.anew.str("Rule.deleteconfirm")) != DialogResult.Yes) return;
            var prule = tv.SelectedNode.Tag as RuleItem;
            var dbtype = Options.GetSystemDBSetType_Plat();
            string connstr = Options.GetSystemDBSetConnStr_Plat();
            if (prule.Type == RuleItemType.COLUMN)
            {
                del_column(prule.Id);
            }
            else if (prule.Type == RuleItemType.TABLE)
            {
                del_table(prule.Id);
            }
            else if (prule.Type == RuleItemType.DIR)
            {
                del_dir(prule.Id);
            }
            tv.SelectedNode.Remove();
            void del_column(string id)
            {
                string sql = "delete from ft_rule_atom where pid='" + id + "'";
                Adv.RemoteSqlExec(sql, dbtype, connstr);
                sql = "delete from ft_rule_item where id='" + id + "'";
                Adv.RemoteSqlExec(sql, dbtype, connstr);
            }
            void del_table(string id)
            {
                string sql = "select id from ft_rule_item where pid='" + id + "'";
                var dt = Adv.RemoteSqlQuery(sql, dbtype, connstr);
                foreach (DataRow row in dt.Rows)
                {
                    del_column(row[0].ToString());
                }
                sql = "delete from ft_rule_item where id='" + id + "'";
                Adv.RemoteSqlExec(sql, dbtype, connstr);
            }
            void del_dir_table(string id)
            {
                string sql = "select id from ft_rule_item where pid='" + id + "'";
                var dt = Adv.RemoteSqlQuery(sql, dbtype, connstr);
                foreach (DataRow row in dt.Rows)
                {
                    del_table(row[0].ToString());
                }
            }
            void del_dir(string id)
            {
                string sql = "select id from ft_rule_dir where pid='" + id + "'";
                var dt = Adv.RemoteSqlQuery(sql, dbtype, connstr);
                sql = "delete from ft_rule_dir where id='" + id + "'";
                Adv.RemoteSqlExec(sql, dbtype, connstr);
                del_dir_table(id);
                foreach (DataRow row in dt.Rows)
                {
                    del_dir(row[0].ToString());
                }
            }
        }

        private void Menu_Refresh_Click(object sender, EventArgs e)
        {
            RuleBuild();
            TreeBuild();
        }
        private void Rule_FormClosing(object sender, FormClosingEventArgs e)
        {
            edit_leave(null, null);
        }

        private void btn_apipath_Click(object sender, EventArgs e)
        {
            Rule_Api rule_Api = new Rule_Api();
            NodeAction(GetTreeNode(), n => { rule_Api.PatternStr = n.Text; });
            rule_Api.ShowDialog();
            if (!rule_Api.IsCancel)
            {
                box_apipath.Text = rule_Api.SetVal + "\r\n\r\n" + rule_Api.SetDesc + "\r\n\r\n" + res.anew.str("Rule.leixing") + rule_Api.SetApiType;
                string sql = "update ft_rule_dir set apipath='" + str.D2DD(rule_Api.SetVal) + "' where id='" + EditingRule.Id + "'";
                Adv.RemoteSqlExec(sql);
                NodeAction(GetTreeNode(), n => { n.ForeColor = Color.Black; });
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sql = "update ft_rule_dir set apipath='' where id='" + EditingRule.Id + "'";
            Adv.RemoteSqlExec(sql);
            box_apipath.Text = res.anew.str("Rule.weibangding");
            NodeAction(GetTreeNode(), n => { n.ForeColor = tv.SelectedNode.Nodes.Cast<TreeNode>().Where(r => ((RuleItem)r.Tag).Type == RuleItemType.TABLE).Count() > 0 ? Color.Red : Color.Black; });
        }

        private void btn_table_Click(object sender, EventArgs e)
        {
            var result = Adv.SelectTable();
            if (result.tablename != null)
            {
                box_table.Text = result.tablename + "\r\n\r\n" + result.tabledesc;
                string sql = "update ft_rule_item set bindtable='" + str.D2DD(result.tablename) + "' where id='" + EditingRule.Id + "'";
                Adv.RemoteSqlExec(sql);
                NodeAction(GetTreeNode(), n => { n.ForeColor = Color.Black; });
            }
        }

        private void btn_table_remove_Click(object sender, EventArgs e)
        {
            string sql = "update ft_rule_item set bindtable='' where id='" + EditingRule.Id + "'";
            Adv.RemoteSqlExec(sql);
            box_table.Text = res.anew.str("Rule.weibangding");
            NodeAction(GetTreeNode(), n => { n.ForeColor = Color.Red; });
        }

        private void btn_column_Click(object sender, EventArgs e)
        {
            string pid = null;
            string tablename = null;
            var node = GetTreeNode();
            if (node != null)
            {
                pid = (node.Parent.Tag as RuleItem).Id;
            }
            if (pid != null)
            {
                string sql = "select bindtable from ft_rule_item where id='" + pid + "'";
                var dtrt = Adv.RemoteSqlQuery(sql);
                if (dtrt.Rows.Count > 0) tablename = dtrt.Rows[0][0].ToString();
            }
            if (string.IsNullOrEmpty(tablename))
            {
                MsgBox.Warning(res.anew.str("Rule.BindFirst"));
                return;
            }
            var result = Adv.SelectColumn(tablename);
            if (result.columnname != null)
            {
                box_column.Text = tablename + "." + result.columnname + "\r\n\r\n" + result.columndesc;
                string sql = "update ft_rule_item set bindcolumn='" + str.D2DD(result.columnname) + "' where id='" + EditingRule.Id + "'";
                Adv.RemoteSqlExec(sql);
                node.ForeColor = Color.Black;
            }
        }

        private void btn_column_remove_Click(object sender, EventArgs e)
        {
            string sql = "update ft_rule_item set bindcolumn='' where id='" + EditingRule.Id + "'";
            Adv.RemoteSqlExec(sql);
            box_column.Text = res.anew.str("Rule.weibangding");
            NodeAction(GetTreeNode(), n => { n.ForeColor = Color.Red; });
        }

        private void Menu_Clone_Click(object sender, EventArgs e)
        {
            var rule = tv.SelectedNode.Tag as RuleItem;
            var dbtype = Options.GetSystemDBSetType_Plat();
            string connstr = Options.GetSystemDBSetConnStr_Plat();
            string pid = "";
            string sql = null;
            if (tv.SelectedNode.Parent != null)
            {
                pid = (tv.SelectedNode.Parent.Tag as RuleItem).Id;
            }
            if (rule.Type == RuleItemType.COLUMN)
            {
                clone_column(rule.Id, pid);
            }
            else if (rule.Type == RuleItemType.TABLE)
            {
                clone_table(rule.Id, pid);
            }
            else if (rule.Type == RuleItemType.DIR)
            {
                clone_dir(rule.Id, pid);
            }
            void clone_dir(string id, string parentid, bool nameAppend = true)
            {
                var dtDirSon = Adv.RemoteSqlQuery("select * from ft_rule_dir where pid='" + id + "'", dbtype, connstr);
                var dtDir = Adv.RemoteSqlQuery("select * from ft_rule_dir where id='" + id + "'", dbtype, connstr);
                string newId = rdm.getCombID();
                sql = "insert into ft_rule_dir(id,pid,caption,apipath,updatetime)";
                sql += "values('" + newId + "','" + parentid + "','" + str.D2DD(dtDir.Rows[0]["caption"].ToString() + (nameAppend ? " "+ res.anew.str("Rule.Copy") : "")) + "','" + str.D2DD(dtDir.Rows[0]["apipath"].ToString()) + "','" + str.GetDateTime(DateTime.Now) + "')";
                Adv.RemoteSqlExec(sql, dbtype, connstr);
                foreach (DataRow row in dtDirSon.Rows)
                {
                    clone_dir(row["id"].ToString(), newId, false);
                }
                var dtTable = Adv.RemoteSqlQuery("select * from ft_rule_item where pid='" + id + "'", dbtype, connstr);
                foreach (DataRow row in dtTable.Rows)
                {
                    clone_table(row["id"].ToString(), newId, false);
                }
            }
            void clone_table(string id, string parentid, bool nameAppend = true)
            {
                var dtColumn = Adv.RemoteSqlQuery("select * from ft_rule_item where pid='" + id + "'", dbtype, connstr);
                var dtTable = Adv.RemoteSqlQuery("select * from ft_rule_item where id='" + id + "'", dbtype, connstr);
                string newId = rdm.getCombID();
                sql = "insert into ft_rule_item(id,pid,caption,ruletype,bindtable,bindcolumn,updatetime)";
                sql += "values('" + newId + "','" + parentid + "','" + str.D2DD(dtTable.Rows[0]["caption"].ToString() + (nameAppend ? " "+ res.anew.str("Rule.Copy") : "")) + "','table','" + str.D2DD(dtTable.Rows[0]["bindtable"].ToString()) + "','" + str.D2DD(dtTable.Rows[0]["bindcolumn"].ToString()) + "','" + str.GetDateTime(DateTime.Now) + "')";
                Adv.RemoteSqlExec(sql, dbtype, connstr);
                foreach (DataRow row in dtColumn.Rows)
                {
                    clone_column(row["id"].ToString(), newId, false);
                }
            }
            void clone_column(string id, string parentid, bool nameAppend = true)
            {
                var dtAtom = Adv.RemoteSqlQuery("select * from ft_rule_atom where pid='" + id + "'", dbtype, connstr);
                var dtItem = Adv.RemoteSqlQuery("select * from ft_rule_item where id='" + id + "'", dbtype, connstr);
                string newId = rdm.getCombID();
                sql = "insert into ft_rule_item(id,pid,caption,ruletype,bindtable,bindcolumn,updatetime)";
                sql += "values('" + newId + "','" + parentid + "','" + str.D2DD(dtItem.Rows[0]["caption"].ToString() + (nameAppend ? " "+ res.anew.str("Rule.Copy") : "")) + "','column','" + str.D2DD(dtItem.Rows[0]["bindtable"].ToString()) + "','" + str.D2DD(dtItem.Rows[0]["bindcolumn"].ToString()) + "','" + str.GetDateTime(DateTime.Now) + "')";
                Adv.RemoteSqlExec(sql, dbtype, connstr);
                foreach (DataRow row in dtAtom.Rows)
                {
                    sql = "insert into ft_rule_atom(id,pid,rule)";
                    sql += "values('" + rdm.getCombID() + "','" + newId + "','" + str.D2DD(row["rule"].ToString()) + "')";
                    Adv.RemoteSqlExec(sql, dbtype, connstr);
                }
            }
            RuleBuild();
            TreeBuild();
            tv.SelectedNode = GetTreeNode(rule.Id);
        }
        bool RuleSet_Initing = false;//在初始化赋值过程中，则不保存
        void RuleSet_Save()
        {
            if (!RuleSet_Initing && EditingRule != null)
            {
                string rule0 = null;//规则
                string rule1 = "";//选项1
                string rule2 = "";//选项2
                string rule3 = "";//文本设置
                if ((box_rule_sel1.SelectedItem as ComboItem).Id != "")
                {
                    rule0 = (box_rule_sel1.SelectedItem as ComboItem).Id;
                    rule1 = (box_rule_sel1.SelectedItem as ComboItem).Name;
                }
                if (box_rule_sel2.Visible)
                {
                    rule2 = (box_rule_sel2.SelectedItem as ComboItem).Id;
                }
                if (box_rule_text.Visible)
                {
                    rule3 = box_rule_text.Text;
                }
                string sql = "delete from ft_rule_atom where pid='" + EditingRule.Id + "'";
                Adv.RemoteSqlExec(sql);
                if (rule0 != null)
                {
                    string ruleSet = rule0 + "[##]" + rule1 + "[##]" + rule2 + "[##]" + rule3;
                    sql = "insert into ft_rule_atom(id,pid,rule)values('" + rdm.getCombID() + "','" + EditingRule.Id + "','" + str.D2DD(ruleSet) + "')";
                    Adv.RemoteSqlExec(sql);
                }
            }
        }
        private void box_rule_sel1_SelectedIndexChanged(object sender, EventArgs e)
        {
            RuleSetOtherInit();
            RuleSet_Save();
        }

        private void box_rule_text_Leave(object sender, EventArgs e)
        {
            RuleSet_Save();
        }

        private void box_rule_sel2_SelectedIndexChanged(object sender, EventArgs e)
        {
            RuleSet_Save();
        }

        private void Menu_NewDir_Name_Click(object sender, EventArgs e)
        {
            InputName inputName = new InputName();
            inputName.TopMost = true;
            inputName.ShowDialog();
            if (inputName.NameValue.Text != "")
            {
                var prule = tv.SelectedNode.Tag as RuleItem;
                RuleItem rule = new RuleItem()
                {
                    Id = rdm.getCombID(),
                    Text = inputName.NameValue.Text.Trim(),
                    Type = RuleItemType.DIR,
                    Bind = true
                };
                string sql = "insert into ft_rule_dir(id,pid,caption,apipath,updatetime)";
                sql += "values('" + rule.Id + "','" + prule.Id + "','" + str.D2DD(rule.Text) + "','','" + str.GetDateTime(DateTime.Now) + "')";
                Adv.RemoteSqlExec(sql);
                tv_NodeAdd(tv.SelectedNode, rule);
            }
        }

        private void Menu_NewTableRule_Name_Click(object sender, EventArgs e)
        {
            InputName inputName = new InputName();
            inputName.TopMost = true;
            inputName.ShowDialog();
            if (inputName.NameValue.Text != "")
            {
                var prule = tv.SelectedNode.Tag as RuleItem;
                RuleItem rule = new RuleItem()
                {
                    Id = rdm.getCombID(),
                    Text = inputName.NameValue.Text.Trim(),
                    Type = RuleItemType.TABLE,
                    Bind = false
                };
                string sql = "insert into ft_rule_item(id,pid,caption,ruletype,bindtable,bindcolumn,updatetime)";
                sql += "values('" + rule.Id + "','" + prule.Id + "','" + str.D2DD(rule.Text) + "','table','','','" + str.GetDateTime(DateTime.Now) + "')";
                Adv.RemoteSqlExec(sql);
                tv_NodeAdd(tv.SelectedNode, rule);
            }
        }

        private void Menu_NewColumnRule_Name_Click(object sender, EventArgs e)
        {
            InputName inputName = new InputName();
            inputName.TopMost = true;
            inputName.ShowDialog();
            if (inputName.NameValue.Text != "")
            {
                var prule = tv.SelectedNode.Tag as RuleItem;
                RuleItem rule = new RuleItem()
                {
                    Id = rdm.getCombID(),
                    Text = inputName.NameValue.Text.Trim(),
                    Type = RuleItemType.COLUMN,
                    Bind = false
                };
                string sql = "insert into ft_rule_item(id,pid,caption,ruletype,bindtable,bindcolumn,updatetime)";
                sql += "values('" + rule.Id + "','" + prule.Id + "','" + str.D2DD(rule.Text) + "','column','','','" + str.GetDateTime(DateTime.Now) + "')";
                Adv.RemoteSqlExec(sql);
                tv_NodeAdd(tv.SelectedNode, rule);
            }
        }

        private void Menu_NewDir_Bind_Click(object sender, EventArgs e)
        {
            Rule_Api rule_Api = new Rule_Api();
            rule_Api.ShowDialog();
            if (!rule_Api.IsCancel)
            {
                var prule = tv.SelectedNode.Tag as RuleItem;
                RuleItem rule = new RuleItem()
                {
                    Id = rdm.getCombID(),
                    Text = rule_Api.SetDesc.Trim(),
                    Type = RuleItemType.DIR,
                    Bind = true
                };
                string sql = "insert into ft_rule_dir(id,pid,caption,apipath,updatetime)";
                sql += "values('" + rule.Id + "','" + prule.Id + "','" + str.D2DD(rule.Text) + "','" + str.D2DD(rule_Api.SetVal) + "','" + str.GetDateTime(DateTime.Now) + "')";
                Adv.RemoteSqlExec(sql);
                tv_NodeAdd(tv.SelectedNode, rule);
            }
        }

        private void Menu_NewTableRule_Bind_Click(object sender, EventArgs e)
        {
            var result = Adv.SelectTable();
            if (result.tablename != null)
            {
                var prule = tv.SelectedNode.Tag as RuleItem;
                RuleItem rule = new RuleItem()
                {
                    Id = rdm.getCombID(),
                    Text = result.tabledesc.Trim(),
                    Type = RuleItemType.TABLE,
                    Bind = true
                };
                string sql = "insert into ft_rule_item(id,pid,caption,ruletype,bindtable,bindcolumn,updatetime)";
                sql += "values('" + rule.Id + "','" + prule.Id + "','" + str.D2DD(rule.Text) + "','table','" + str.D2DD(result.tablename) + "','','" + str.GetDateTime(DateTime.Now) + "')";
                Adv.RemoteSqlExec(sql);
                tv_NodeAdd(tv.SelectedNode, rule);
            }
        }

        private void Menu_NewColumnRule_Bind_Click(object sender, EventArgs e)
        {
            string pid = null;
            string tablename = null;
            var node = tv.SelectedNode;
            if (node != null)
            {
                pid = (node.Tag as RuleItem).Id;
            }
            if (pid != null)
            {
                string sql = "select bindtable from ft_rule_item where id='" + pid + "'";
                var dtrt = Adv.RemoteSqlQuery(sql);
                if (dtrt.Rows.Count > 0) tablename = dtrt.Rows[0][0].ToString();
            }
            if (string.IsNullOrEmpty(tablename))
            {
                MsgBox.Warning(res.anew.str("Rule.BindFirst"));
                return;
            }
            var result = Adv.SelectColumn(tablename);
            if (result.columnname != null)
            {
                var prule = tv.SelectedNode.Tag as RuleItem;
                RuleItem rule = new RuleItem()
                {
                    Id = rdm.getCombID(),
                    Text = result.columndesc.Trim(),
                    Type = RuleItemType.COLUMN,
                    Bind = true
                };
                string sql = "insert into ft_rule_item(id,pid,caption,ruletype,bindtable,bindcolumn,updatetime)";
                sql += "values('" + rule.Id + "','" + prule.Id + "','" + str.D2DD(rule.Text) + "','column','','" + str.D2DD(result.columnname) + "','" + str.GetDateTime(DateTime.Now) + "')";
                Adv.RemoteSqlExec(sql);
                tv_NodeAdd(tv.SelectedNode, rule);
            }
        }
    }
}
