using FTDPClient.database;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;
using ICSharpCode.TextEditor.Gui.CompletionWindow;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FTDP.Util
{
    public class ICSharpTextEditor
    {
        static char[] chars = new char[] { '\n', '\t', ' ', '\"', '\'', ',', '|', '{', '[', '(', '}', ']', ')', ';', ':', '/', '*', '=', '!', '>', '<' };
        static List<char> labels = new List<char>() {
        '\n', '\t', ' ', '\"', '\'', ',', '|', '{', '[', '(', '}', ']', ')', ';', ':', '/', '*', '=', '!', '>', '<'
        };
        static ImageList imageList1 = null;
        string ptnstr = "";
        CodeCompletionWindow completionWindow;
        TextEditorControl textEditorControl;
        Form ParentForm;
        string SourceType;
        object[] Paras;
        TextArea textArea;
        private static ImageList GetImageList()
        {
            if (imageList1 == null)
            {
                imageList1 = new ImageList();
                imageList1.Images.AddRange(
                    new Image[] {
                    Image.FromFile(Tool.AppPath + @"\img\18.gif"),//SQL
                    Image.FromFile(Tool.AppPath + @"\img\20.gif"),//Table
                    Image.FromFile(Tool.AppPath + @"\img\21.gif"),//Field（根据最近选过的Table自动出来Field）
                    Image.FromFile(Tool.AppPath + @"\img\22.gif"),//Rule
                    Image.FromFile(Tool.AppPath + @"\img\23.gif"),//Assembly
                    Image.FromFile(Tool.AppPath + @"\img\24.gif"),//JS（包括页面内置组件的Js）
                    Image.FromFile(Tool.AppPath + @"\img\25.gif"),//页面Id
                    Image.FromFile(Tool.AppPath + @"\img\26.gif"),//Page Url
                    Image.FromFile(Tool.AppPath + @"\img\32.gif"),//精确字段    8
                    Image.FromFile(Tool.AppPath + @"\img\14.gif"),//站点参数    9
                    }
                    );
            }
            return imageList1;
        }
        public void Init(Form _ParentForm, TextEditorControl _textEditorControl, bool useStrictField, params object[] paras)
        {
            ParentForm = _ParentForm;
            textEditorControl = _textEditorControl;
            SourceType = ParentForm.Name;
            Paras = paras;
            textArea = textEditorControl.ActiveTextAreaControl.TextArea;
            textArea.KeyEventHandler += (ch) =>
            {
                if (labels.Contains(ch)) return false;
                string leftStr = textArea.Document.GetText(textArea.Document.GetLineSegment(textArea.Caret.Position.Line)).Substring(0, textArea.Caret.Position.X);
                string[] steItems = leftStr.Split(chars);
                ptnstr = "";
                if (steItems.Length > 0) ptnstr = steItems[steItems.Length - 1];
                ptnstr += ch;
                if (ch == '.' || ptnstr.IndexOf('.') > 0)
                {
                    string[] steItems2 = leftStr.Split('@');
                    if (steItems2.Length > 0)
                    {
                        string commonPareStr = steItems2[steItems2.Length - 1];
                        if (commonPareStr.StartsWith("dic("))
                        {
                            Dic_Type = commonPareStr.Split(new char[] { '(', '.' })[1];
                            ptnstr = "";
                        }
                        else if (commonPareStr.StartsWith("enum("))
                        {
                            Enum_Type = commonPareStr.Split(new char[] { '(', '.' })[1];
                            ptnstr = "";
                            //string enumkey = commonPareStr.Split(new char[] { '(', '.' })[1];
                            //string otherval = null;
                            //if (!string.IsNullOrEmpty(DBType_Plat) && !string.IsNullOrEmpty(DBConnStr_Plat))
                            //{
                            //    if (DBType_Plat.ToLower() == "mysql")
                            //    {
                            //        using (var conn = new MySqlConnection(DBConnStr_Plat))
                            //        {
                            //            conn.Open();
                            //            string sql = "SELECT otherval from ft_ftdp_codestatic where codetype=1 and codekey='" + enumkey + "'";
                            //            using (MySqlDataReader dr = new MySqlCommand(sql, conn).ExecuteReader())
                            //            {
                            //                if (dr.Read())
                            //                {
                            //                    otherval = dr.IsDBNull(0) ? "" : dr.GetString(0);
                            //                }
                            //            }
                            //        }
                            //    }
                            //    else if (DBType_Plat.ToLower() == "sqlserver")
                            //    {
                            //        using (var conn = new SqlConnection(DBConnStr_Plat))
                            //        {
                            //            conn.Open();
                            //            string sql = "SELECT otherval from ft_ftdp_codestatic where codetype=1 and codekey='" + enumkey + "'";
                            //            using (SqlDataReader dr = new SqlCommand(sql, conn).ExecuteReader())
                            //            {
                            //                if (dr.Read())
                            //                {
                            //                    otherval = dr.IsDBNull(0) ? "" : dr.GetString(0);
                            //                }
                            //            }
                            //        }
                            //    }
                            //}
                            //if (!string.IsNullOrEmpty(otherval))
                            //{
                            //    if (ch == '.') ptnstr = "";
                            //    else ptnstr = ptnstr.Split('.')[1];
                            //    EnumItems = new List<(string, string)>();
                            //    var _items = otherval.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                            //    foreach (string _item in _items)
                            //    {
                            //        string _key = _item;
                            //        string _desc = "";
                            //        int _index = _item.IndexOf(':');
                            //        if (_index >= 0)
                            //        {
                            //            _key = _item.Substring(0, _index);
                            //            _desc = _item.Substring(_index + 1);
                            //        }
                            //        EnumItems.Add((_key, string.IsNullOrEmpty(_desc)?"  ": _desc));
                            //    }
                            //}
                        }
                    }
                }
                if (useStrictField && Enum_Type == null && Dic_Type==null)
                {
                    if (ch == '.' || ptnstr.IndexOf('.') > 0)
                    {
                        string aliasName = ptnstr.Split('.')[0];
                        if (aliasName != "")
                        {
                            if (ch == '.') ptnstr = "";
                            else ptnstr = ptnstr.Split('.')[1];
                            var dic = TableAlias(textEditorControl.Text);
                            if (dic.ContainsKey(aliasName)) StrictFieldTable = dic[aliasName];
                        }
                    }
                }
                ShowCompletionWindow();
                return false;
            };
        }
        public static Dictionary<string, string> TableAlias(string sqlText)
        {
            var dic = new Dictionary<string, string>();
            var items = sqlText.Split(new char[] { '\n', '\t', ' ', '\r',',' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i].Equals("from", StringComparison.CurrentCultureIgnoreCase) || items[i].Equals("join", StringComparison.CurrentCultureIgnoreCase))
                {
                    if (i < items.Length - 2)
                    {
                        string table = items[i + 1];
                        string alias = items[i + 2];
                        if (alias.Equals("as", StringComparison.CurrentCultureIgnoreCase))
                        {
                            if (i < items.Length - 3)
                            {
                                alias = items[i + 3];
                                if (!dic.ContainsKey(alias)) dic.Add(alias, table);
                            }
                        }
                        else
                        {
                            if (!dic.ContainsKey(alias)) dic.Add(alias, table);
                        }
                    }
                }
            }
            return dic;
        }
        public bool Find(string text)
        {
            int startIndex = 0;
            if (textArea.Caret != null && textArea.Caret.Position != null)
            {
                startIndex = textArea.Document.PositionToOffset(textArea.Caret.Position);
            }
            var offset = textEditorControl.Text.IndexOf(text, startIndex, StringComparison.CurrentCultureIgnoreCase);
            if (offset >= 0)
            {
                //findIndex = offset + text.Length;//为查找下一个做准备
                var start = textEditorControl.Document.OffsetToPosition(offset);
                var end = textEditorControl.Document.OffsetToPosition(offset + text.Length);
                textEditorControl.ActiveTextAreaControl.SelectionManager.SetSelection(new DefaultSelection(textEditorControl.Document, start, end));

                //滚动到选择的位置。
                textEditorControl.ActiveTextAreaControl.Caret.Position = end;
                textEditorControl.ActiveTextAreaControl.TextArea.ScrollToCaret();
                return true;
            }
            else
            {
                //findIndex = 0;//循环查找
                return false;
            }
        }
        public bool Replace(string text, string newText)
        {
            try
            {
                new ICSharpCode.TextEditor.Actions.Copy().Execute(textArea);
                if (Clipboard.GetText() != null && Clipboard.GetText().ToLower() == text.ToLower())
                {
                    Clipboard.SetText(newText);
                    new ICSharpCode.TextEditor.Actions.Paste().Execute(textArea);
                }
                int startIndex = 0;
                if (textArea.Caret != null && textArea.Caret.Position != null)
                {
                    startIndex = textArea.Document.PositionToOffset(textArea.Caret.Position);
                }
                var offset = textEditorControl.Text.IndexOf(text, startIndex, StringComparison.CurrentCultureIgnoreCase);
                if (offset >= 0)
                {
                    //findIndex = offset + text.Length;//为查找下一个做准备
                    var start = textEditorControl.Document.OffsetToPosition(offset);
                    var end = textEditorControl.Document.OffsetToPosition(offset + text.Length);
                    textEditorControl.ActiveTextAreaControl.SelectionManager.SetSelection(new DefaultSelection(textEditorControl.Document, start, end));

                    //滚动到选择的位置。
                    textEditorControl.ActiveTextAreaControl.Caret.Position = end;
                    textEditorControl.ActiveTextAreaControl.TextArea.ScrollToCaret();
                    return true;
                }
                else
                {
                    //findIndex = 0;//循环查找
                    return false;
                }
            }
            catch { return false; }
        }
        private static List<object[]> completionData_SQL =
        new List<object[]>()
            {
                new object[]{"SELECT","SQL",0 },
                new object[]{"FROM", "SQL", 0 },
                new object[]{"WHERE", "SQL", 0 },
                new object[]{"AND", "SQL", 0 },
                new object[]{"OR", "SQL", 0 },
                new object[]{"LEFT JOIN", "SQL", 0 },
                new object[]{"RIGHT JOIN", "SQL", 0 },
                new object[]{"INNER JOIN", "SQL", 0 },
                new object[]{"UPDATE", "SQL", 0 },
                new object[]{"INSERT", "SQL", 0 },
                new object[]{"DELETE", "SQL", 0 },
                new object[]{"COUNT", "SQL", 0 },
                new object[]{"GROUP BY", "SQL", 0 },
                new object[]{"DISTINCT", "SQL", 0 },
                new object[]{"VALUES", "SQL", 0 },
                new object[]{"UNION", "SQL", 0 },
            };
        private static List<object[]> completionData_Other_Old =
        new List<object[]>()
            {
                //new object[]{"showDialog(id,name)","弹出对话框",5 },
                new object[]{"@code(define,para)","代码返回字符" ,3},
                //new object[]{ "@code:", "代码模式的定义" ,4},
                //new object[]{"@string{}","代码模式中代码块中的内嵌字符" ,4},
                //new object[]{ "@{}", "代码模式的代码块" ,4},
                //new object[]{ "@:", "代码模式的代码行" ,4},
                //new object[]{ "@()", "代码模式中字符的内嵌变量" ,4},
                //new object[]{"@code(define,para)@","代码返回字符，作为内嵌配置" ,4},
                new object[]{"sql@code(define,para)", "代码返回Sql，(顶格配置)", 3},
                new object[]{"str@code(define,@val@)", "代码传当前值再返回，(顶格配置)", 3},
                new object[]{"all@code(define,para)", "代码返回脚本并执行，(顶格配置)", 3},
                new object[]{"@FromList:@code(define,para)","代码返回List结果集，(顶格配置)" ,3},
                new object[]{"@FromDic:@code(define,para)", "代码返回字典结果集，(顶格配置)", 3},
                new object[]{ "@keyValue(key)", "数据操作获取根级Key的原始值" ,3},
                new object[]{ "@val@", "数据获取的当前配置的原始值" ,3},
                new object[]{ "@newfid@", "数据操作的第一个新增的新主键值" ,3},
                new object[]{ "@rowindex@", "数据操作多行时的行索引" ,3},
                new object[]{ "@sql:", "数据操作时根据sql获取值，(顶格配置)", 3},
                new object[]{ "@from(key)", "数据操作和获取根据根级TableTag和Id获取最终值" ,3},
                new object[]{ "@key(key)", "数据操作和获取根据根级Name和Id获取原始值和最终值" ,3},
            };
        public static List<object[]> completionData_Other()
        {
            if (System.Threading.Thread.CurrentThread.CurrentUICulture.Name == "en")
            {
                return new List<object[]>()
            {
                //new object[]{"showDialog(id,name)","弹出对话框",5 },
                new object[]{"@code(define,para)", "Code return string", 3},
                //new object[]{ "@code:", "代码模式的定义" ,4},
                //new object[]{"@string{}","代码模式中代码块中的内嵌字符" ,4},
                //new object[]{ "@{}", "代码模式的代码块" ,4},
                //new object[]{ "@:", "代码模式的代码行" ,4},
                //new object[]{ "@()", "代码模式中字符的内嵌变量" ,4},
                //new object[]{"@code(define,para)@","代码返回字符，作为内嵌配置" ,4},
                new object[]{"sql@code(define,para)", "Code returns Sql, (top config)", 3},
                new object[]{"str@code(define,@val@)", "Code passes the current value and then return, (top config)", 3},
                new object[]{"all@code(define,para)", "Code returns the script and executes it, (top config)", 3},
                new object[]{"@FromList:@code(define,para)", "Code returns the List result, (top config)", 3},
                new object[]{"@FromDic:@code(define,para)", "Code returns the Dictionary result, (top config)", 3},
                new object[]{ "@keyValue(key)", "Data operation to obtain the original value of the root level Key", 3},
                new object[]{ "@val@", "The original value of the current configuration", 3},
                new object[]{ "@newfid@", "The first newly added primary key value for data operations", 3},
                new object[]{ "@rowindex@", "Row index for data operations with multiple rows", 3},
                new object[]{ "@sql:", "Obtain values based on SQL during data operations, (top config)", 3},
                new object[]{ "@from(key)", "Data operation and retrieval obtain the final value based on the root level TableTag and Key", 3},
                new object[]{ "@key(key)", "Data operation and retrieval retrieve raw and final values based on root level Key", 3},
            };
            }
            else {
                return new List<object[]>()
            {
                //new object[]{"showDialog(id,name)","弹出对话框",5 },
                new object[]{"@code(define,para)","代码返回字符" ,3},
                //new object[]{ "@code:", "代码模式的定义" ,4},
                //new object[]{"@string{}","代码模式中代码块中的内嵌字符" ,4},
                //new object[]{ "@{}", "代码模式的代码块" ,4},
                //new object[]{ "@:", "代码模式的代码行" ,4},
                //new object[]{ "@()", "代码模式中字符的内嵌变量" ,4},
                //new object[]{"@code(define,para)@","代码返回字符，作为内嵌配置" ,4},
                new object[]{"sql@code(define,para)", "代码返回Sql，(顶格配置)", 3},
                new object[]{"str@code(define,@val@)", "代码传当前值再返回，(顶格配置)", 3},
                new object[]{"all@code(define,para)", "代码返回脚本并执行，(顶格配置)", 3},
                new object[]{"@FromList:@code(define,para)","代码返回List结果集，(顶格配置)" ,3},
                new object[]{"@FromDic:@code(define,para)", "代码返回字典结果集，(顶格配置)", 3},
                new object[]{ "@keyValue(key)", "数据操作获取根级Key的原始值" ,3},
                new object[]{ "@val@", "数据获取的当前配置的原始值" ,3},
                new object[]{ "@newfid@", "数据操作的第一个新增的新主键值" ,3},
                new object[]{ "@rowindex@", "数据操作多行时的行索引" ,3},
                new object[]{ "@sql:", "数据操作时根据sql获取值，(顶格配置)", 3},
                new object[]{ "@from(key)", "数据操作和获取根据根级TableTag和Id获取最终值" ,3},
                new object[]{ "@key(key)", "数据操作和获取根据根级Name和Id获取原始值和最终值" ,3},
            };
            }
        }
        public static string DBType = null;
        public static string DBConnStr = null;
        public static string DBType_Plat = null;
        public static string DBConnStr_Plat = null;
        //public static List<(string, string)> EnumItems = null;
        public static string StrictFieldTable = null;
        public static List<string> LastTables = new List<string>();
        public static List<object[]> completionData_Assembly = new List<object[]>();
        public static List<object[]> completionData_Table = new List<object[]>();
        public static List<object[]> completionData_Field = new List<object[]>();
        public static List<object[]> completionData_Para = new List<object[]>();
        public static List<object[]> completionData_Dic = new List<object[]>();
        public static Dictionary<string, List<(string val, string desc)>> completionData_Dic_Data = new Dictionary<string, List<(string val, string desc)>>();
        public static string Dic_Type = null;
        public static List<object[]> completionData_Enum = new List<object[]>();
        public static Dictionary<string, List<(string val, string desc)>> completionData_Enum_Data = new Dictionary<string, List<(string val, string desc)>>();
        public static string Enum_Type = null;
        public List<object[]> completionData_StrictField = new List<object[]>();
        List<DefaultCompletionData> completionData()
        {
            List<DefaultCompletionData> item = new List<DefaultCompletionData>();
            if (StrictFieldTable != null)
            {
                GenerateCompletionData_StrictField();
                foreach (object[] s in completionData_StrictField)
                {
                    if (((string)s[0]).ToLower().Contains(ptnstr.ToLower()))
                    {
                        item.Add(new DefaultCompletionData(((string)s[0]), ((string)s[1]) + " [" + StrictFieldTable + "]", 8));
                    }
                }
            }
            else if (Dic_Type != null)
            {
                item.AddRange(completionData_Dic_Data[Dic_Type].Select(r=> new DefaultCompletionData(r.val, r.desc, 8)));
            }
            else if (Enum_Type != null)
            {
                item.AddRange(completionData_Enum_Data[Enum_Type].Select(r => new DefaultCompletionData(r.val, r.desc, 8)));
                //foreach (var _item in EnumItems)
                //{
                //    if (_item.Item1.ToLower().Contains(ptnstr.ToLower()))
                //        item.Add(new DefaultCompletionData(_item.Item1, _item.Item2, 8));
                //}
            }
            else
            {
                IList<object[]> items = new List<object[]>();
                items = items.Concat(completionData_SQL).Concat(completionData_Other()).Concat(completionData_Assembly).Concat(completionData_Table).Concat(completionData_Field).Concat(completionData_Para).Concat(completionData_Dic).Concat(completionData_Enum).ToList();
                foreach (object[] s in items)
                {
                    if (((string)s[0]).ToLower().Contains(ptnstr.ToLower()))
                    {
                        item.Add(new DefaultCompletionData(((string)s[0]), ((string)s[1])==""?" ": ((string)s[1]), ((int)s[2])));
                    }
                }
            }
            StrictFieldTable = null;
            Dic_Type = null;
            Enum_Type = null;
            return item;
        }
        public static void GenerateCompletionData_Field()
        {
            return;//用Strict Field代替
            completionData_Field.Clear();
            if (!string.IsNullOrEmpty(DBType) && !string.IsNullOrEmpty(DBConnStr))
            {
                if (DBType.ToLower() == "mysql")
                {
                    using (var conn = new MySqlConnection(DBConnStr))
                    {
                        conn.Open();
                        foreach (string tablename in LastTables)
                        {
                            string sql = "show full fields from " + tablename + "";
                            using (MySqlDataReader dr = new MySqlCommand(sql, conn).ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    FTDP.Util.ICSharpTextEditor.completionData_Field.Add(new object[] { dr.GetString(0), dr.IsDBNull(dr.GetOrdinal("Comment")) ? "" : dr.GetValue(dr.GetOrdinal("Comment")).ToString(), 2 });
                                }
                            }
                        }
                    }
                }
                else if (DBType.ToLower() == "sqlserver")
                {
                    using (var conn = new SqlConnection(DBConnStr))
                    {
                        conn.Open();
                        foreach (string tablename in LastTables)
                        {
                            bool isView = false;
                            string sql = "select name,xtype from sysobjects where name<>'dtproperties' and (xtype='U' or xtype='V') and name='" + tablename + "'";
                            SqlDataReader dr = new SqlCommand(sql, conn).ExecuteReader();
                            if (!dr.HasRows)
                            {
                                dr.Close();
                                continue;
                            }
                            else
                            {
                                dr.Read();
                                isView = dr.GetString(1).Trim().ToLower() == "v";
                            }
                            dr.Close();
                            if (isView)
                            {
                                sql = @"(select sys.columns.name,sys.types.name as typename,sys.columns.max_length,sys.columns.precision,sys.columns.scale, sys.columns.is_nullable,
 (select count(*) from sys.identity_columns where sys.identity_columns.object_id = sys.columns.object_id and sys.columns.column_id = sys.identity_columns.column_id) as is_identity ,
 sys.columns.column_id,(select value from sys.extended_properties where sys.extended_properties.major_id = sys.columns.object_id and sys.extended_properties.minor_id = sys.columns.column_id) as description 
 from sys.columns, sys.views, sys.types where sys.columns.object_id = sys.views.object_id and sys.columns.user_type_id=sys.types.user_type_id and sys.views.name='" + tablename + "')";
                            }
                            else
                            {
                                sql = @"(select sys.columns.name,sys.types.name as typename,sys.columns.max_length,sys.columns.precision,sys.columns.scale, sys.columns.is_nullable,
 (select count(*) from sys.identity_columns where sys.identity_columns.object_id = sys.columns.object_id and sys.columns.column_id = sys.identity_columns.column_id) as is_identity ,
 sys.columns.column_id,(select value from sys.extended_properties where sys.extended_properties.major_id = sys.columns.object_id and sys.extended_properties.minor_id = sys.columns.column_id) as description 
 from sys.columns, sys.tables, sys.types where sys.columns.object_id = sys.tables.object_id and sys.columns.user_type_id=sys.types.user_type_id and sys.tables.name='" + tablename + "')";
                            }
                            using (SqlDataReader dr2 = new SqlCommand(sql, conn).ExecuteReader())
                            {
                                while (dr2.Read())
                                {
                                    FTDP.Util.ICSharpTextEditor.completionData_Field.Add(new object[] { dr2.GetString(0), dr2.IsDBNull(8) ? "" : dr2.GetValue(8).ToString(), 2 });
                                }
                            }
                        }
                    }
                }
            }
        }
        private Dictionary<string, List<object[]>> StrictFieldDic = new Dictionary<string, List<object[]>>();
        public static List<object[]> GetStrictFields(string DBType, string DBConnStr, string tablename)
        {
            var _StrictField = new List<object[]>();

            if (!string.IsNullOrEmpty(DBType) && !string.IsNullOrEmpty(DBConnStr))
            {
                if (DBType.ToLower() == "mysql")
                {
                    using (var conn = new MySqlConnection(DBConnStr))
                    {
                        conn.Open();
                        string sql = "show full fields from " + tablename + "";
                        using (MySqlDataReader dr = new MySqlCommand(sql, conn).ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                _StrictField.Add(new object[] { dr.GetString(0), dr.IsDBNull(dr.GetOrdinal("Comment")) ? "" : dr.GetValue(dr.GetOrdinal("Comment")).ToString(), 2 });
                            }
                        }
                    }
                }
                else if (DBType.ToLower() == "sqlserver")
                {
                    using (var conn = new SqlConnection(DBConnStr))
                    {
                        conn.Open();
                        bool isView = false;
                        string sql = "select name,xtype from sysobjects where name<>'dtproperties' and (xtype='U' or xtype='V') and name='" + tablename + "'";
                        SqlDataReader dr = new SqlCommand(sql, conn).ExecuteReader();
                        if (!dr.HasRows)
                        {
                            dr.Close();
                            return _StrictField;
                        }
                        else
                        {
                            dr.Read();
                            isView = dr.GetString(1).Trim().ToLower() == "v";
                        }
                        dr.Close();
                        if (isView)
                        {
                            sql = @"(select sys.columns.name,sys.types.name as typename,sys.columns.max_length,sys.columns.precision,sys.columns.scale, sys.columns.is_nullable,
 (select count(*) from sys.identity_columns where sys.identity_columns.object_id = sys.columns.object_id and sys.columns.column_id = sys.identity_columns.column_id) as is_identity ,
 sys.columns.column_id,(select value from sys.extended_properties where sys.extended_properties.major_id = sys.columns.object_id and sys.extended_properties.minor_id = sys.columns.column_id) as description 
 from sys.columns, sys.views, sys.types where sys.columns.object_id = sys.views.object_id and sys.columns.user_type_id=sys.types.user_type_id and sys.views.name='" + tablename + "')";
                        }
                        else
                        {
                            sql = @"(select sys.columns.name,sys.types.name as typename,sys.columns.max_length,sys.columns.precision,sys.columns.scale, sys.columns.is_nullable,
 (select count(*) from sys.identity_columns where sys.identity_columns.object_id = sys.columns.object_id and sys.columns.column_id = sys.identity_columns.column_id) as is_identity ,
 sys.columns.column_id,(select value from sys.extended_properties where sys.extended_properties.major_id = sys.columns.object_id and sys.extended_properties.minor_id = sys.columns.column_id) as description 
 from sys.columns, sys.tables, sys.types where sys.columns.object_id = sys.tables.object_id and sys.columns.user_type_id=sys.types.user_type_id and sys.tables.name='" + tablename + "')";
                        }
                        using (SqlDataReader dr2 = new SqlCommand(sql, conn).ExecuteReader())
                        {
                            while (dr2.Read())
                            {
                                _StrictField.Add(new object[] { dr2.GetString(0), dr2.IsDBNull(8) ? "" : dr2.GetValue(8).ToString(), 2 });
                            }
                        }
                    }
                }
                else if (DBType.ToLower() == "sqlite")
                {
                    using (var conn = new DB(DBConnStr))
                    {
                        conn.Open();
                        //获取sqlite注释
                        var zhuShiDic = new Dictionary<string, string>();
                        var DDL = "";
                        string sql = "SELECT sql FROM sqlite_master WHERE type='table' AND name = '" + tablename + "'";
                        var dr = conn.OpenRecord(sql);
                        if (dr.Read())
                        {
                            DDL = dr.GetString(0);
                            dr.Close();
                        }
                        else
                        {
                            dr.Close();
                        }
                        var rows = DDL.Split(new string[] { Environment.NewLine, "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries).Select(r => r.Trim());
                        foreach (var row in rows)
                        {
                            var index = row.IndexOf("--");
                            if (index > 0)
                            {
                                var zhushi = row.Substring(index + 2).Trim();
                                if (row.StartsWith("\""))
                                {
                                    var rs = row.Split(new string[] { "\"" }, StringSplitOptions.RemoveEmptyEntries);
                                    if (rs.Length >= 2)
                                    {
                                        var columnname = rs[0].Trim().ToLower();
                                        if (!zhuShiDic.ContainsKey(columnname)) zhuShiDic.Add(columnname, zhushi);
                                    }
                                }
                            }
                        }
                        sql = "PRAGMA TABLE_INFO(" + tablename + ")";
                        using (dr = conn.OpenRecord(sql))
                        {
                            while (dr.Read())
                            {
                                _StrictField.Add(new object[] { dr.GetString(1), zhuShiDic.ContainsKey(dr.GetString(1).ToLower())? zhuShiDic[dr.GetString(1).ToLower()]:"", 2 });
                            }
                        }
                    }
                }
            }
            return _StrictField;
        }
        public void GenerateCompletionData_StrictField()
        {
            if (StrictFieldDic.ContainsKey(StrictFieldTable))
            {
                completionData_StrictField = StrictFieldDic[StrictFieldTable];
                return;
            }
            var _StrictField = GetStrictFields(DBType, DBConnStr, StrictFieldTable);
            StrictFieldDic.Add(StrictFieldTable, _StrictField);
            completionData_StrictField = _StrictField;
        }
        private void ShowCompletionWindow()
        {
            if (completionWindow != null) completionWindow.Close();
            var items = completionData();
            if (items.Count == 0)
            {
                return;
            }
            CompletionDataProvider completionDataProvider = new CompletionDataProvider(items);
            completionDataProvider.ImageList = GetImageList();
            completionDataProvider.PatternStr = ptnstr;
            completionDataProvider.ListViewFont = textEditorControl.Font;
            completionWindow = CodeCompletionWindow.ShowCompletionWindow(ParentForm, textEditorControl, String.Empty, completionDataProvider, '.', true, false);

            if (completionWindow != null)
            {
                completionWindow.Closed += CompletionWindowClosed;
            };
        }
        private void CompletionWindowClosed(object source, EventArgs e)
        {
            if (completionWindow != null)
            {
                completionWindow.Closed -= CompletionWindowClosed;
                completionWindow.Dispose();
                completionWindow = null;
            }
        }
    }
    public class CompletionDataProvider : ICompletionDataProvider
    {
        ImageList imageList = new ImageList();

        List<DefaultCompletionData> completionData = new List<DefaultCompletionData>();
        public string PatternStr = "";
        public CompletionDataProvider()
        {
            completionData.Add(new DefaultCompletionData("Item1", 0));
            completionData.Add(new DefaultCompletionData("Item2", 0));
            completionData.Add(new DefaultCompletionData("Item3", 0));
            completionData.Add(new DefaultCompletionData("Another item", 0));
        }
        public CompletionDataProvider(List<DefaultCompletionData> items)
        {
            completionData = items;
        }
        public Font ListViewFont { get; set; }

        public ImageList ImageList
        {
            get { return imageList; }
            set { imageList = value; }
        }

        public string PreSelection
        {
            get { return null; }
        }

        public int DefaultIndex
        {
            get { return 0; }
        }

        public CompletionDataProviderKeyResult ProcessKey(char key)
        {
            if (char.IsLetterOrDigit(key))
            {
                return CompletionDataProviderKeyResult.NormalKey;
            }
            return CompletionDataProviderKeyResult.InsertionKey;
        }

        public bool InsertAction(ICompletionData data, TextArea textArea, int insertionOffset, char key)
        {
            textArea.Caret.Position = textArea.Document.OffsetToPosition(insertionOffset);
            TextLocation startPos = new TextLocation
            {
                Line = textArea.Caret.Position.Line,
                X = textArea.Caret.Position.X - PatternStr.Length,
                Y = textArea.Caret.Position.Y,
            };
            textArea.SelectionManager.SetSelection(startPos, textArea.Caret.Position);
            new ICSharpCode.TextEditor.Actions.Delete().Execute(textArea);
            bool b = data.InsertAction(textArea, key);
            if (data.ImageIndex == 1)
            {
                if (!ICSharpTextEditor.LastTables.Contains(data.Text))
                {
                    if (ICSharpTextEditor.LastTables.Count > 3) ICSharpTextEditor.LastTables.RemoveAt(0);
                    ICSharpTextEditor.LastTables.Add(data.Text);
                    System.Threading.Tasks.Task.Run(() => ICSharpTextEditor.GenerateCompletionData_Field());
                }
            }
            return b;
        }

        public ICompletionData[] GenerateCompletionData(string fileName, TextArea textArea, char charTyped)
        {
            return completionData.ToArray();
        }
    }
}
