/*FTDP Control,This Code Created By FTDP Deploy Tool
Build By Maobb 2007-6-10
Code Deploy Time is:2020-06-05 09:54:53*/
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;
using System.Collections;
using FTDP;
using FTDP.Tool;
using FTDP.DBClient;
using FTDP.Page;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace FT.List.Fore
{
    public class client
    {
        public static string thisControlID = "";
        public static object[] thisStyleObject;
    }
    public class List : Control, INamingContainer
    {
        public string SiteID;
        public string ControlName;
        public string ControlID;
        public string PartID;
        public string DataSource;
        public string SetStyle;
        public string ControlPara;
        public string CurPage;
        public int SelectType;
        public string LoadingImg;
        public int RateNumType;
        public string SelectOnClick;
        public int NumsPerPage;
        public string FirstPage;
        public string PrePage;
        public string NextPage;
        public string LastPage;
        public string TurnPage;
        public string CountZero;
        public int Cellspacing;
        public int Cellpadding;
        public string ColorHover;
        public string ColorDefault;
        public string ColorSelect;
        public int HeadIsShow;
        public int TurnIsShow;
        public string OrderBy;
        public string NewSiteID;
        public string FormStartTime;
        public string FormEndTime;
        public string FormPID;
        public string MainTable;
        public string RowAll;
        public string APISet;
        public int IsAutoShow;
        public string CusCondition;
        public string RateEle;
        public string CusSQL;
        public string CusSQLHalf;
        public string RerationTree;
        public string BlockDataDefine;
        public string AppendTitle;
        public string LoadEndJS;
        public string CustomConnection;
        public string CustomTurnPageBottom;
        public string CustomTurnPageTop;
        public string CacuRowData;
        public int IsSearchShow;
        public int IsCusCdnShow;
        public string SchAreaApdHtml;
        public string AdvSearch;
        public string AdvSearchCaption;
        public string SearchDefine;
        public string SearchDefineTip;
        public string StrictSearch;
        public int IsRefreshShow;
        public int IsColDefineShow;
        public string CusCdnCols;
        public string Const1;
        public string Const2;
        public string Const3;
        public string Const4;
        public string Const5;
        public string Const6;
        public string Const7;
        public string Const8;
        public string Const9;
        public string Const10;
        public string Const11;
        public string Const12;
        public string Const13;
        public string Const14;
        public string Const15;
        public string Const16;
        public string Const17;
        public string Const18;
        public string Const19;
        public string Const20;
        public string Const21;
        public string Const22;
        public string Const23;
        public string Const24;
        public string List_OPID;
        public string List_Code;
        public string MemBind;
        public string EleCondition;
        public string RoleBindData;
        public string RoleBindSession;
        public string AuthRule;
        public string FlowStat;
        public string Del_Code;
        public string Del_MemBind;
        public string Del_EleCondition;
        public string Del_RoleBindData;
        public string Del_RoleBindSession;
        public string Del_AuthRule;
        public string Del_FlowStat;
        public string Del_OPID;
        public string Flow_Code;
        public string Flow_MemBind;
        public string Flow_EleCondition;
        public string Flow_RoleBindData;
        public string Flow_RoleBindSession;
        public string Flow_AuthRule;
        public string Flow_FlowStat;
        public string Flow_OPID;
        public string Copy_Code;
        public string Copy_MemBind;
        public string Copy_EleCondition;
        public string Copy_RoleBindData;
        public string Copy_RoleBindSession;
        public string Copy_AuthRule;
        public string Copy_FlowStat;
        public string Copy_OPID;
        public int ExportMax;
        public int MenuIsShow;
        public string MenuButtonSet;
        //public string MenuOprole;
        //public string MenuOpname;
        //public string MenuOpcheck;
        //public string MenuOpendurer;
        //public string MenuOppic;
        protected override void Render(HtmlTextWriter output)
        {
            ControlPara = ControlPara.Replace("{dsqt}", "\"");
            CurPage = CurPage.Replace("{dsqt}", "\"");
            LoadingImg = LoadingImg.Replace("{dsqt}", "\"");
            SelectOnClick = SelectOnClick.Replace("{dsqt}", "\"");
            FirstPage = FirstPage.Replace("{dsqt}", "\"");
            PrePage = PrePage.Replace("{dsqt}", "\"");
            NextPage = NextPage.Replace("{dsqt}", "\"");
            LastPage = LastPage.Replace("{dsqt}", "\"");
            TurnPage = TurnPage.Replace("{dsqt}", "\"");
            CountZero = CountZero.Replace("{dsqt}", "\"");
            ColorHover = ColorHover.Replace("{dsqt}", "\"");
            ColorDefault = ColorDefault.Replace("{dsqt}", "\"");
            ColorSelect = ColorSelect.Replace("{dsqt}", "\"");
            OrderBy = OrderBy.Replace("{dsqt}", "\"");
            NewSiteID = NewSiteID.Replace("{dsqt}", "\"");
            FormStartTime = FormStartTime.Replace("{dsqt}", "\"");
            FormEndTime = FormEndTime.Replace("{dsqt}", "\"");
            FormPID = FormPID.Replace("{dsqt}", "\"");
            MainTable = MainTable.Replace("{dsqt}", "\"");
            RowAll = RowAll.Replace("{dsqt}", "\"");
            CusCondition = CusCondition.Replace("{dsqt}", "\"");
            RateEle = RateEle.Replace("{dsqt}", "\"");
            CusSQL = CusSQL.Replace("{dsqt}", "\"");
            CusSQLHalf = CusSQLHalf.Replace("{dsqt}", "\"");
            RerationTree = RerationTree.Replace("{dsqt}", "\"");
            BlockDataDefine = BlockDataDefine.Replace("{dsqt}", "\"");
            AppendTitle = AppendTitle.Replace("{dsqt}", "\"");
            LoadEndJS = LoadEndJS.Replace("{dsqt}", "\"");
            CustomConnection = CustomConnection.Replace("{dsqt}", "\"");
            CustomTurnPageBottom = CustomTurnPageBottom.Replace("{dsqt}", "\"");
            CustomTurnPageTop = CustomTurnPageTop.Replace("{dsqt}", "\"");
            CacuRowData = CacuRowData.Replace("{dsqt}", "\"");
            SchAreaApdHtml = SchAreaApdHtml.Replace("{dsqt}", "\"");
            AdvSearch = AdvSearch.Replace("{dsqt}", "\"");
            AdvSearchCaption = AdvSearchCaption.Replace("{dsqt}", "\"");
            SearchDefine = SearchDefine.Replace("{dsqt}", "\"");
            SearchDefineTip = SearchDefineTip.Replace("{dsqt}", "\"");
            StrictSearch = StrictSearch.Replace("{dsqt}", "\"");
            CusCdnCols = CusCdnCols.Replace("{dsqt}", "\"");
            Const1 = Const1.Replace("{dsqt}", "\"");
            Const2 = Const2.Replace("{dsqt}", "\"");
            Const3 = Const3.Replace("{dsqt}", "\"");
            Const4 = Const4.Replace("{dsqt}", "\"");
            Const5 = Const5.Replace("{dsqt}", "\"");
            Const6 = Const6.Replace("{dsqt}", "\"");
            Const7 = Const7.Replace("{dsqt}", "\"");
            Const8 = Const8.Replace("{dsqt}", "\"");
            Const9 = Const9.Replace("{dsqt}", "\"");
            Const10 = Const10.Replace("{dsqt}", "\"");
            Const11 = Const11.Replace("{dsqt}", "\"");
            Const12 = Const12.Replace("{dsqt}", "\"");
            Const13 = Const13.Replace("{dsqt}", "\"");
            Const14 = Const14.Replace("{dsqt}", "\"");
            Const15 = Const15.Replace("{dsqt}", "\"");
            Const16 = Const16.Replace("{dsqt}", "\"");
            Const17 = Const17.Replace("{dsqt}", "\"");
            Const18 = Const18.Replace("{dsqt}", "\"");
            Const19 = Const19.Replace("{dsqt}", "\"");
            Const20 = Const20.Replace("{dsqt}", "\"");
            Const21 = Const21.Replace("{dsqt}", "\"");
            Const22 = Const22.Replace("{dsqt}", "\"");
            Const23 = Const23.Replace("{dsqt}", "\"");
            Const24 = Const24.Replace("{dsqt}", "\"");
            List_OPID = List_OPID.Replace("{dsqt}", "\"");
            List_Code = List_Code.Replace("{dsqt}", "\"");
            MemBind = MemBind.Replace("{dsqt}", "\"");
            EleCondition = EleCondition.Replace("{dsqt}", "\"");
            RoleBindData = RoleBindData.Replace("{dsqt}", "\"");
            RoleBindSession = RoleBindSession.Replace("{dsqt}", "\"");
            AuthRule = AuthRule.Replace("{dsqt}", "\"");
            FlowStat = FlowStat.Replace("{dsqt}", "\"");
            Del_Code = Del_Code.Replace("{dsqt}", "\"");
            Del_MemBind = Del_MemBind.Replace("{dsqt}", "\"");
            Del_EleCondition = Del_EleCondition.Replace("{dsqt}", "\"");
            Del_RoleBindData = Del_RoleBindData.Replace("{dsqt}", "\"");
            Del_RoleBindSession = Del_RoleBindSession.Replace("{dsqt}", "\"");
            Del_AuthRule = Del_AuthRule.Replace("{dsqt}", "\"");
            Del_FlowStat = Del_FlowStat.Replace("{dsqt}", "\"");
            Del_OPID = Del_OPID.Replace("{dsqt}", "\"");
            Flow_Code = Flow_Code.Replace("{dsqt}", "\"");
            Flow_MemBind = Flow_MemBind.Replace("{dsqt}", "\"");
            Flow_EleCondition = Flow_EleCondition.Replace("{dsqt}", "\"");
            Flow_RoleBindData = Flow_RoleBindData.Replace("{dsqt}", "\"");
            Flow_RoleBindSession = Flow_RoleBindSession.Replace("{dsqt}", "\"");
            Flow_AuthRule = Flow_AuthRule.Replace("{dsqt}", "\"");
            Flow_FlowStat = Flow_FlowStat.Replace("{dsqt}", "\"");
            Flow_OPID = Flow_OPID.Replace("{dsqt}", "\"");
            Copy_Code = Copy_Code.Replace("{dsqt}", "\"");
            Copy_MemBind = Copy_MemBind.Replace("{dsqt}", "\"");
            Copy_EleCondition = Copy_EleCondition.Replace("{dsqt}", "\"");
            Copy_RoleBindData = Copy_RoleBindData.Replace("{dsqt}", "\"");
            Copy_RoleBindSession = Copy_RoleBindSession.Replace("{dsqt}", "\"");
            Copy_AuthRule = Copy_AuthRule.Replace("{dsqt}", "\"");
            Copy_FlowStat = Copy_FlowStat.Replace("{dsqt}", "\"");
            Copy_OPID = Copy_OPID.Replace("{dsqt}", "\"");
            MenuButtonSet= MenuButtonSet.Replace("{dsqt}", "\"");
            //MenuOprole = MenuOprole.Replace("{dsqt}", "\"");
            //MenuOpname = MenuOpname.Replace("{dsqt}", "\"");
            //MenuOpcheck = MenuOpcheck.Replace("{dsqt}", "\"");
            //MenuOpendurer = MenuOpendurer.Replace("{dsqt}", "\"");
            //MenuOppic = MenuOppic.Replace("{dsqt}", "\"");
            client.thisControlID = ControlID;
            string[] StyleStringArray = SetStyle.Split('{');
            int StyleStringArrayi;
            int StyleStringArrayLength = StyleStringArray.Length;
            client.thisStyleObject = new object[StyleStringArrayLength];
            for (StyleStringArrayi = 0; StyleStringArrayi < StyleStringArrayLength; StyleStringArrayi++)
            {
                string[] StyleStringArrayOne = StyleStringArray[StyleStringArrayi].Split('}');
                client.thisStyleObject[StyleStringArrayi] = StyleStringArrayOne;
            }

            try
            {
                //aa
                ForeFrameType ForeFrame = (ForeFrameType)Enum.Parse(typeof(ForeFrameType), ControlPara);
                string s = "";
                NewSiteID = NewSiteID.Trim();
                if (!NewSiteID.Equals("")) SiteID = NewSiteID;
                string[] Consts = new string[24];
                Consts[0] = Const1;
                Consts[1] = Const2;
                Consts[2] = Const3;
                Consts[3] = Const4;
                Consts[4] = Const5;
                Consts[5] = Const6;
                Consts[6] = Const7;
                Consts[7] = Const8;
                Consts[8] = Const9;
                Consts[9] = Const10;
                Consts[10] = Const11;
                Consts[11] = Const12;
                Consts[12] = Const13;
                Consts[13] = Const14;
                Consts[14] = Const15;
                Consts[15] = Const16;
                Consts[16] = Const17;
                Consts[17] = Const18;
                Consts[18] = Const19;
                Consts[19] = Const20;
                Consts[20] = Const21;
                Consts[21] = Const22;
                Consts[22] = Const23;
                Consts[23] = Const24;
                if (MenuIsShow == 1)
                {
                    //string[] colname = MenuOpname.Split(';');
                    //string[] colendurer = adv.GetSpecialBase(this.Context, MenuOpendurer, SiteID).Split(new string[] { "##" }, StringSplitOptions.None);
                    //string[] colrole = MenuOprole.Split(';');
                    //string[] colcheck = MenuOpcheck.Split(';');
                    //string[] MenuOppics = MenuOppic.Split(';');
                    //System.Collections.Generic.List<string> colname = new System.Collections.Generic.List<string>();
                    //System.Collections.Generic.List<string> colendurer = new System.Collections.Generic.List<string>();
                    //System.Collections.Generic.List<string> colrole = new System.Collections.Generic.List<string>();
                    //System.Collections.Generic.List<string> colcheck = new System.Collections.Generic.List<string>();
                    //System.Collections.Generic.List<string> MenuOppics = new System.Collections.Generic.List<string>();
                    string[] MenuItems = MenuButtonSet.Split(new string[] { "{$$}" },StringSplitOptions.RemoveEmptyEntries);
                    string ButtonStyleJs = "";
                    string btns = "";
                    int loop = 0;
                    foreach (string MenuItem in MenuItems)
                    {
                        loop++;
                        string[] Items = MenuItem.Split(new string[] { "[##]" }, StringSplitOptions.None);
                        string colname = Items[0].ToString().Trim();
                        string colendurer = Items[1].ToString().Trim();
                        string colrole = Items[2].ToString().Trim();
                        string colpic = Items[3].ToString().Trim();
                        string colcheck = Items[4].ToString().Trim();
                        if (colpic.StartsWith("@c"))
                        {
                            colpic = Consts[Convert.ToInt32(colpic.Substring(2)) - 1];
                        }
                        if (colname.Equals("")) continue;
                        string clickstr = "";

                        if(colrole.Equals("del"))
                        {
                            clickstr = "onclick=\"" + "dl_delete(this,'" + SiteID + "','" + PartID + "')\"";
                        }
                        else if (colrole.Equals("refresh"))
                        {
                            clickstr = "onclick=\"" + "dl_refresh('" + PartID + "')\"";
                        }
                        else if(colrole.Equals("copy"))
                        {
                            clickstr = "onclick=\"" + "dl_copy(this,'" + SiteID + "','" + PartID + "')\"";
                        }
                        else if(colrole.Equals("export"))
                        {
                            clickstr = "onclick=\"" + "dl_export_all('" + PartID + "')\"";
                        }
                        else if(colrole.Equals("export_direct"))
                        {
                            clickstr = "onclick=\"" + "load_" + PartID + "(1,null,null,true)\"";
                        }
                        else if (colrole.StartsWith("flow"))
                        {
                            if (colrole.IndexOf(":") > 0)
                            {
                                clickstr = "onclick=\"" + "if(confirm('" + colrole.Split(':')[1] + "'))dl_flow(this,'" + SiteID + "','" + PartID + "','" + colrole.Split(':')[0].Substring(4) + "')\"";
                            }
                            else
                                clickstr = "onclick=\"" + "dl_flow(this,'" + SiteID + "','" + PartID + "','" + colrole.Substring(4) + "')\"";
                        }
                        else if (colrole.StartsWith("saveto("))
                        {
                            clickstr = "onclick=\"" + "dl_saveto(this,'" + colrole.Replace("saveto(", "").Replace(")", "") + "','" + SiteID + "','" + PartID + "')\"";
                        }
                        else
                        {
                            if (colcheck.Equals("one"))
                            {
                                clickstr = "onclick=\"if(dl_SelectOne(this)!=null){" + colendurer.Replace("[id]", "dl_SelectOne(this)") + "}\"";
                            }
                            else if (colcheck.Equals("more"))
                            {
                                clickstr = "onclick=\"if(dl_SelectMore(this,true)!=null){" + colendurer.Replace("[ids]", "dl_SelectMore(this,true)") + "}\"";
                            }
                            else
                            {
                                clickstr = "onclick=\"" + colendurer + "\"";
                            }
                        }
                        //aa
                        if (ForeFrame == ForeFrameType.JQueryUI)
                        {
                            btns += ftdpcontroltemp.TextLinkDemoStyle2("btn" + loop + "_" + PartID, clickstr, colname);
                            ButtonStyleJs += ftdpcontroltemp.ButtonStyleInitJs("btn" + loop + "_" + PartID, colpic);
                        }
                        else if (ForeFrame == ForeFrameType.LayUI)
                        {
                            btns += ftdpcontroltemp.TextLinkDemoStyle2_LayUI("btn" + loop + "_" + PartID, clickstr, colname);
                        }
                    }
                    //string schs = "";
                    List<string> schsList = new List<string>();
                    if (IsSearchShow == 1)
                    {
                        if (!SearchDefine.Equals(""))
                        {
                            //aa
                            if (ForeFrame == ForeFrameType.JQueryUI)
                            {
                                if (SearchDefineTip.Equals(""))
                                {
                                    schsList.Add(ftdpcontroltemp.SeachDiv(ftdpcontroltemp.SearchText(PartID, PartID, PartID, SearchDefine),4));
                                }
                                else
                                {
                                    schsList.Add(ftdpcontroltemp.SeachDiv(ftdpcontroltemp.SearchTextTip(PartID, SearchDefineTip, PartID, PartID, SearchDefine, PartID), 4));
                                }
                            }
                            else if (ForeFrame == ForeFrameType.LayUI)
                            {
                                if (SearchDefineTip.Equals(""))
                                {
                                    schsList.Add(ftdpcontroltemp.SeachDiv(ftdpcontroltemp.SearchText_LayUI(PartID, PartID, PartID, SearchDefine), 4));
                                }
                                else
                                {
                                    schsList.Add(ftdpcontroltemp.SeachDiv(ftdpcontroltemp.SearchTextTip_LayUI(PartID, PartID, PartID, SearchDefine, SearchDefineTip), 4));
                                }
                            }
                        }
                        if (StrictSearch.StartsWith("@code("))
                        {
                            StrictSearch = FTDP.Interface.Code.Get(StrictSearch, Context);
                        }
                        string[] strictsearch = StrictSearch.Split(new string[] { "#" }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string strictsearch1 in strictsearch)
                        {
                            string options = "";
                            options += "<option value=''>" + strictsearch1.Split('|')[0].Split(';')[1] + "</option>";
                            //string schcoldefine=strictsearch1.Split('|')[1];
                            string schcoldefine = strictsearch1.Substring(strictsearch1.IndexOf('|') + 1);
                            string SqlOpType = null;
                            if (schcoldefine.StartsWith("@SQL{")) SqlOpType = "";
                            else if (schcoldefine.StartsWith(">@SQL{")) SqlOpType = ">";
                            else if (schcoldefine.StartsWith("<@SQL{")) SqlOpType = "<";
                            else if (schcoldefine.StartsWith(">=@SQL{")) SqlOpType = ">=";
                            else if (schcoldefine.StartsWith("<=@SQL{")) SqlOpType = "<=";
                            else if (schcoldefine.StartsWith("!@SQL{")) SqlOpType = "!";
                            if (SqlOpType != null)
                            {
                                DB db = new DB();
                                db.Open();
                                try
                                {
                                    string schcoldefinesql = adv.GetSpecialBase(Context, schcoldefine.Replace(SqlOpType + "@SQL{", "").Replace("}", ""), SiteID, true);
                                    DR dr = db.OpenRecord(schcoldefinesql);
                                    while (dr.Read())
                                    {
                                        options += "<option value=\"" + SqlOpType + dr.GetString(0) + "\">" + dr.GetString(1) + "</option>";
                                    }
                                    dr.Close();
                                }
                                catch (Exception ex)
                                {
                                    log.Exception(ex);
                                }
                                finally
                                {
                                    db.Close();
                                }
                            }
                            else if (schcoldefine.StartsWith("@code("))
                            {
                                options += FTDP.Interface.Code.Get(schcoldefine, Context);
                            }
                            else
                            {
                                string[] items = schcoldefine.Split(';');
                                foreach (string _item in items)
                                {
                                    options += "<option value=\"" + _item.Split(':')[1] + "\">" + _item.Split(':')[0] + "</option>";
                                }
                            }
                            schsList.Add(ftdpcontroltemp.SeachDiv(ftdpcontroltemp.SearchSelectStart(strictsearch1.Split('|')[0].Split(';')[0], PartID) + options + ftdpcontroltemp.SearchSelectEnd(), 4));
                        }
                        //list_adv_s_"+PartID+"='';
                        //aa
                        if (ForeFrame == ForeFrameType.JQueryUI)
                        {
                            schsList.Add(ftdpcontroltemp.SeachDiv(ftdpcontroltemp.TextLinkDemoStyle2("btns_" + PartID, "onclick=\"dl_search('" + PartID + "')\"", "查询"), 4));
                            ButtonStyleJs += ftdpcontroltemp.ButtonStyleInitJs("btns_" + PartID, "ui-icon-search");
                            if (IsCusCdnShow == 1)
                            {
                                schsList.Add(ftdpcontroltemp.SeachDiv(ftdpcontroltemp.TextLinkDemoStyle2("btnc_" + PartID, "onclick=\"dl_cuscdn('" + PartID + "','" + CusCdnCols + "')\"", "自定义"), 4));
                                ButtonStyleJs += ftdpcontroltemp.ButtonStyleInitJs("btnc_" + PartID, "ui-icon-search");
                            }
                            if (!AdvSearch.Equals(""))
                            {
                                schsList.Add(ftdpcontroltemp.SeachDiv(ftdpcontroltemp.TextLinkDemoStyle2("btna_" + PartID, "onclick=\"" + AdvSearch + "\"", AdvSearchCaption == "" ? "高级" : AdvSearchCaption), 4));
                                ButtonStyleJs += ftdpcontroltemp.ButtonStyleInitJs("btna_" + PartID, "ui-icon-search");
                            }
                        }
                        else if (ForeFrame == ForeFrameType.LayUI)
                        {
                            schsList.Add(ftdpcontroltemp.SeachDiv(ftdpcontroltemp.TextLinkDemoStyle2_LayUI("btns_" + PartID, "onclick=\"dl_search('" + PartID + "')\"", "查询"), 4));
                            if (IsCusCdnShow == 1)
                            {
                                schsList.Add(ftdpcontroltemp.SeachDiv(ftdpcontroltemp.TextLinkDemoStyle2_LayUI("btnc_" + PartID, "onclick=\"dl_cuscdn('" + PartID + "','" + CusCdnCols + "')\"", "自定义"), 4));
                            }
                            if (!AdvSearch.Equals(""))
                            {
                                schsList.Add(ftdpcontroltemp.SeachDiv(ftdpcontroltemp.TextLinkDemoStyle2_LayUI("btna_" + PartID, "onclick=\"" + AdvSearch + "\"", AdvSearchCaption == "" ? "高级" : AdvSearchCaption), 4));
                            }
                        }
                    }
                    //aa
                    if (ForeFrame == ForeFrameType.JQueryUI)
                    {
                        if (IsColDefineShow == 1)
                        {
                            schsList.Add(ftdpcontroltemp.SeachDiv(ftdpcontroltemp.TextLinkDemoStyle2("btncoldefine_" + PartID, "onclick=\"dl_coldefine('" + PartID + "',l_cd_" + PartID + ")\"", "列定义"), 4));
                            ButtonStyleJs += ftdpcontroltemp.ButtonStyleInitJs("btncoldefine_" + PartID, "ui-icon-gear");
                        }
                        if (IsRefreshShow == 1)
                        {
                            schsList.Add(ftdpcontroltemp.SeachDiv(ftdpcontroltemp.TextLinkDemoStyle2("btnf_" + PartID, "onclick=\"dl_refresh('" + PartID + "')\"", "刷新"), 4));
                            ButtonStyleJs += ftdpcontroltemp.ButtonStyleInitJs("btnf_" + PartID, "ui-icon-refresh");
                        }
                        btns += ftdpcontroltemp.ButtonStyleInitJs_Main(ButtonStyleJs);
                    }
                    else if (ForeFrame == ForeFrameType.LayUI)
                    {
                        if (IsColDefineShow == 1)
                        {
                            schsList.Add(ftdpcontroltemp.SeachDiv(ftdpcontroltemp.TextLinkDemoStyle2_LayUI("btncoldefine_" + PartID, "onclick=\"dl_coldefine('" + PartID + "',l_cd_" + PartID + ")\"", "列定义"), 4));
                        }
                        if (IsRefreshShow == 1)
                        {
                            schsList.Add(ftdpcontroltemp.SeachDiv( ftdpcontroltemp.TextLinkDemoStyle2_LayUI("btnf_" + PartID, "onclick=\"dl_refresh('" + PartID + "')\"", "刷新"),4));
                        }
                    }
                    string schs = "";
                    for(int i=schsList.Count-1;i>=0;i--)
                    {
                        schs += schsList[i];
                    }
                    s = ftdpcontroltemp.Tableframe(btns, SchAreaApdHtml + "<span style='padding:0px;margin:0px'"+(ForeFrame == ForeFrameType.LayUI? " class='layui-form'":"") +">" + schs + "</span>");
                }
                MainTable = MainTable.Trim();
                if (MainTable.Equals(""))
                {
                    MainTable = "@NotSetMainTable";
                    //output.Write("Main Table not Defined!");
                    //return;
                }
                if (MainTable.StartsWith("@")) MainTable = MainTable.Substring(1);
                else MainTable = "ft_" + SiteID + "_f_" + MainTable;

                string sql = null;
                if (CusSQL == null || CusSQL.Trim().Equals(""))
                {
                    string FlowStat_sql = "";
                    if (FlowStat != null && !FlowStat.Trim().Equals(""))
                    {
                        FlowStat = adv.GetSpecialBase(Context, FlowStat.Trim(), SiteID, true);
                        if (!FlowStat.Trim().Equals(""))
                        {
                            FlowStat_sql = " and a.flow in (" + str.D2DD(FlowStat) + ")";
                        }
                    }
                    string StartTime_sql = "";
                    if (FormStartTime != null && !FormStartTime.Trim().Equals(""))
                    {
                        FormStartTime = adv.GetSpecialBase(Context, FormStartTime.Trim(), SiteID, true);
                        if (!FormStartTime.Trim().Equals(""))
                        {
                            StartTime_sql = " and a.addtime>='" + str.D2DD(FormStartTime) + "'";
                        }
                    }
                    string EndTime_sql = "";
                    if (FormEndTime != null && !FormEndTime.Trim().Equals(""))
                    {
                        FormEndTime = adv.GetSpecialBase(Context, FormEndTime.Trim(), SiteID, true);
                        if (!FormEndTime.Trim().Equals(""))
                        {
                            EndTime_sql = " and a.addtime<='" + str.D2DD(FormEndTime) + "'";
                        }
                    }
                    string FormPID_sql = "";
                    if (FormPID != null && !FormPID.Trim().Equals(""))
                    {
                        FormPID = adv.GetSpecialBase(Context, FormPID.Trim(), SiteID, true);
                        if (!FormPID.Trim().Equals(""))
                        {
                            FormPID_sql = " and a.pid='" + str.D2DD(FormPID) + "'";
                        }
                    }
                    string MemBind_sql = "1=1";
                    string EleCondition_sql = "1=1";
                    string RoleBindFid_sql = "1=1";
                    string RoleBindStat_sql = "1=1";
                    if (AuthRule.IndexOf("%m%") >= 0)
                    {
                        if (MemBind != null && !MemBind.Trim().Equals(""))
                        {
                            MemBind = adv.GetSpecialBase(Context, MemBind.Trim(), SiteID, true);
                            if (!MemBind.Trim().Equals(""))
                            {
                                MemBind_sql = "a.fmem='" + str.D2DD(MemBind) + "'";
                            }
                        }
                    }
                    if (AuthRule.IndexOf("%e%") >= 0)
                    {
                        if (EleCondition != null && !EleCondition.Trim().Equals(""))
                        {
                            EleCondition = adv.GetSpecialBase(Context, EleCondition.Trim(), SiteID, true);
                            EleCondition_sql = EleCondition;
                        }
                    }
                    if (RoleBindData != null && !RoleBindData.Trim().Equals(""))
                    {
                        if (AuthRule.IndexOf("%f%") >= 0)
                        {
                            if (RoleBindSession != null && !RoleBindSession.Trim().Equals(""))
                            {
                                object _sessionval = HttpContext.Current.Session[RoleBindSession.Trim()];
                                string sessionval = _sessionval == null ? "" : _sessionval.ToString();
                                RoleBindFid_sql = "a.fid in (select distinct(ra.fid) from ds_" + SiteID + "_role_" + RoleBindData + "_fid ra,ds_" + SiteID + "_role_" + RoleBindData + "_user rb where ra.roleid=rb.roleid and rb.userid='" + str.D2DD(sessionval) + "')";
                            }
                        }
                        if (AuthRule.IndexOf("%s%") >= 0)
                        {
                            if (RoleBindSession != null && !RoleBindSession.Trim().Equals(""))
                            {
                                object _sessionval = HttpContext.Current.Session[RoleBindSession.Trim()];
                                string sessionval = _sessionval == null ? "" : _sessionval.ToString();
                                RoleBindStat_sql = "a.flow in (select distinct(ra.stat) from ds_" + SiteID + "_role_" + RoleBindData + "_stat ra,ds_" + SiteID + "_role_" + RoleBindData + "_user rb where ra.roleid=rb.roleid and rb.userid='" + str.D2DD(sessionval) + "')";
                            }
                        }
                    }
                    string AuthRule_sql = "";
                    if (!AuthRule.Trim().Equals(""))
                        AuthRule_sql = " and (" + AuthRule.Replace("%m%", "(" + MemBind_sql + ")").Replace("%f%", "(" + RoleBindFid_sql + ")").Replace("%s%", "(" + RoleBindStat_sql + ")").Replace("%e%", "(" + EleCondition_sql + ")") + ")";
                    string CusCondition_sql = " " + FTDP.Interface.Code.Get(adv.GetSpecialBase(Context, CusCondition, SiteID));
                    //string OrderBy_sql = " " + adv.GetSpecialBase(Context, OrderBy, SiteID);
                    if (CusSQLHalf == null || CusSQLHalf.Trim().Equals(""))
                    {
                        if (RateEle == null || RateEle.Trim().Equals(""))
                        {
                            sql = "select a.* from " + MainTable + " a where a.stat=1 ";
                            sql += FlowStat_sql + StartTime_sql + EndTime_sql + FormPID_sql + AuthRule_sql + CusCondition_sql;
                        }
                        else
                        {
                            sql = "select * from ( select a.fid,a.pid,a.fmem,a.addtime,a.flow,a.updatetime,a.modfmem," + str.D2DD(RateEle.Trim()) + ",0 as erate from " + MainTable + " a where a.stat=1 " + FlowStat_sql + StartTime_sql + EndTime_sql + FormPID_sql + AuthRule_sql + CusCondition_sql + " union all select a.fid,a.pid,a.fmem,a.addtime,a.flow,a.updatetime,a.modfmem,dytb.evalue as " + str.D2DD(RateEle.Trim()) + ",dytb.erate from " + MainTable + " a," + MainTable + "_dy dytb where a.stat=1 and a.fid=dytb.fid and dytb.eid='" + str.D2DD(RateEle.Trim()) + "' " + FlowStat_sql + StartTime_sql + EndTime_sql + FormPID_sql + AuthRule_sql + CusCondition_sql + ") a ";

                        }

                    }
                    else
                    {
                        sql = FTDP.Interface.Code.Get(adv.GetSpecialBase(Context, CusSQLHalf, SiteID)) + " " + FlowStat_sql + StartTime_sql + EndTime_sql + FormPID_sql + AuthRule_sql + CusCondition_sql;
                    }
                }
                else
                {
                    //sql = FTDP.Interface.Code.Get(adv.GetSpecialBase(Context, CusSQL, SiteID));
                    sql = adv.GetSpecialBase(Context, CusSQL, SiteID);
                }
                RerationTree = RerationTree.Trim();
                bool IsTree = !RerationTree.Equals("");
                string[] TreeParas = new string[3];
                if (IsTree)
                {
                    RerationTree = adv.GetSpecialBase(Context, RerationTree, SiteID, true);
                    TreeParas = RerationTree.Split(';');
                }
                if (NumsPerPage == 0)
                {
                    TurnIsShow = 0;
                }
                s += ftdpcontroltemp.FormStart(PartID, PartID);
                string JSTableHead = ftdpcontroltemp.ListHead(Cellspacing, Cellpadding);
                //ArrayList ColumnTitle = new ArrayList();
                //ArrayList ColumnWidth = new ArrayList();
                //ArrayList ColumnAlign = new ArrayList();
                //ArrayList ColumnOpen = new ArrayList();
                //ArrayList ColumnOrder=new ArrayList();
                string ColDefine_Default = "";
                string[] rows = RowAll.Split(new string[] { "|||" }, StringSplitOptions.RemoveEmptyEntries);
                for (int rowi = 0; rowi < rows.Length; rowi++)
                {
                    bool IsColOpen = FTDP.Ajax.Control.List_IsColumnOpen(Context, rows[rowi].Substring(rows[rowi].IndexOf("&&&") + 3).Trim());
                    //ColumnOpen.Add(IsColOpen.ToString());
                    string row = rows[rowi].Substring(0, rows[rowi].IndexOf("&&&")).Trim();
                    string[] rowcols = row.Split('#');
                    //ColumnTitle.Add(rowcols[0]);
                    string __wd = rowcols[2].Split(';')[0];
                    string __al = rowcols[2].Split(';')[1];
                    //ColumnWidth.Add(__wd);
                    //ColumnAlign.Add(__al);
                    ColDefine_Default += "|||" + rowcols[0] + "##" + ((rowcols.Length > 7 && rowcols[7].Equals("1")) ? 1 : 0) + "##" + __wd + "##" + __al + "##" + (IsColOpen ? 1 : 0) + "##" + ((rowcols.Length > 3) ? rowcols[3] : "");
                    //if (rowcols.Length > 3 && !rowcols[3].Equals(""))
                    //{
                    //    ColumnOrder.Add(rowcols[3]);
                    //}
                    //else
                    //{
                    //    ColumnOrder.Add(null);
                    //}
                }
                if (!ColDefine_Default.Equals("")) ColDefine_Default = ColDefine_Default.Substring(3);
                /*
                string JSTableTitle = "";
                if (HeadIsShow == 1)
                {
                    string rowtitle = "";
                    if (SelectType == 0)
                    {
                        rowtitle += ftdpcontroltemp.ListTitleTd0(ftdpcontroltemp.ListTitleMSelect(ColorSelect, ColorDefault));
                    }
                    else if (SelectType == 1)
                    {
                        rowtitle += ftdpcontroltemp.ListTitleTd0("");
                    }
                    if (RateNumType == 1 || RateNumType == 2)
                    {
                        rowtitle += ftdpcontroltemp.ListTitleTd0("No.");
                    }
                    for (int i = 0; i < ColumnTitle.Count; i++)
                    {
                        if (bool.Parse(ColumnOpen[i].ToString()))
                        {
                            string DyColumnTitle = null;
                            if (ColumnOrder[i] == null)
                            {
                                DyColumnTitle = ColumnTitle[i].ToString();
                            }
                            else
                            {
                                DyColumnTitle="<a href=\"javascript:void(0)\" title=\"点击排序\" onclick=\"load_"+PartID+"(1,'"+ColumnOrder[i].ToString()+"')\">" + ColumnTitle[i].ToString() + "</a>";
                            }
                            switch (i + 1)
                            {
                                case 1:
                                    rowtitle += ftdpcontroltemp.ListTitleTd1(ColumnWidth[i].ToString(), ColumnWidth[i].ToString(), ColumnAlign[i].ToString(), DyColumnTitle);
                                    break;
                                case 2:
                                    rowtitle += ftdpcontroltemp.ListTitleTd2(ColumnWidth[i].ToString(), ColumnWidth[i].ToString(), ColumnAlign[i].ToString(), DyColumnTitle);
                                    break;
                                case 3:
                                    rowtitle += ftdpcontroltemp.ListTitleTd3(ColumnWidth[i].ToString(), ColumnWidth[i].ToString(), ColumnAlign[i].ToString(), DyColumnTitle);
                                    break;
                                case 4:
                                    rowtitle += ftdpcontroltemp.ListTitleTd4(ColumnWidth[i].ToString(), ColumnWidth[i].ToString(), ColumnAlign[i].ToString(), DyColumnTitle);
                                    break;
                                case 5:
                                    rowtitle += ftdpcontroltemp.ListTitleTd5(ColumnWidth[i].ToString(), ColumnWidth[i].ToString(), ColumnAlign[i].ToString(), DyColumnTitle);
                                    break;
                                case 6:
                                    rowtitle += ftdpcontroltemp.ListTitleTd6(ColumnWidth[i].ToString(), ColumnWidth[i].ToString(), ColumnAlign[i].ToString(), DyColumnTitle);
                                    break;
                                case 7:
                                    rowtitle += ftdpcontroltemp.ListTitleTd7(ColumnWidth[i].ToString(), ColumnWidth[i].ToString(), ColumnAlign[i].ToString(), DyColumnTitle);
                                    break;
                                case 8:
                                    rowtitle += ftdpcontroltemp.ListTitleTd8(ColumnWidth[i].ToString(), ColumnWidth[i].ToString(), ColumnAlign[i].ToString(), DyColumnTitle);
                                    break;
                                case 9:
                                    rowtitle += ftdpcontroltemp.ListTitleTd9(ColumnWidth[i].ToString(), ColumnWidth[i].ToString(), ColumnAlign[i].ToString(), DyColumnTitle);
                                    break;
                                case 10:
                                    rowtitle += ftdpcontroltemp.ListTitleTd10(ColumnWidth[i].ToString(), ColumnWidth[i].ToString(), ColumnAlign[i].ToString(), DyColumnTitle);
                                    break;
                                case 11:
                                    rowtitle += ftdpcontroltemp.ListTitleTd11(ColumnWidth[i].ToString(), ColumnWidth[i].ToString(), ColumnAlign[i].ToString(), DyColumnTitle);
                                    break;
                                case 12:
                                    rowtitle += ftdpcontroltemp.ListTitleTd12(ColumnWidth[i].ToString(), ColumnWidth[i].ToString(), ColumnAlign[i].ToString(), DyColumnTitle);
                                    break;
                                default:
                                    rowtitle += ftdpcontroltemp.ListTitleTdOther(ColumnWidth[i].ToString(), ColumnWidth[i].ToString(), ColumnAlign[i].ToString(), DyColumnTitle);
                                    break;
                            }
                        }
                    }
                    JSTableTitle = ftdpcontroltemp.ListTitleTr(rowtitle);
                } 
                */
                //ColumnTitle.Clear();
                //ColumnWidth.Clear();
                //ColumnAlign.Clear();
                //ColumnOpen.Clear();
                //ColumnOrder.Clear();
                //ColumnTitle = null;
                //ColumnWidth = null;
                //ColumnAlign = null;
                //ColumnOpen = null;
                //ColumnOrder=null;

                string JSTableTail = ftdpcontroltemp.ListTail();
                s += ftdpcontroltemp.FormEnd();
                s += "<style>.dialog_coldefine_holder { border:1px solid #f9dd34;background:#ffef8f;height: 24px;margin: 5px;}</style>";
                //aa
                if (ForeFrame == ForeFrameType.JQueryUI)
                {
                    s += "<script language=\"javascript\" src=\""+ FTDP.ConstStr.SubPath+ "/lib/list.res/func1712271.js\"></script>";
                }
                else if (ForeFrame == ForeFrameType.LayUI)
                {
                    s += "<script language=\"javascript\" src=\"" + FTDP.ConstStr.SubPath + "/lib/list.res/func200605_layui.js\"></script>";

                }
                s += "<script language=\"javascript\">";
                s += "var l_cd_" + PartID + "=\"" + ColDefine_Default + "\";";
                if (RateNumType == 1 || RateNumType == 2)
                {
                    s += ftdpcontroltemp.ListTd0num_JS_Num(PartID);
                }
                else
                {
                    s += ftdpcontroltemp.ListTd0num_JS_None(PartID);
                }
                if (SelectType == 0)
                {
                    //aa
                    if (ForeFrame == ForeFrameType.JQueryUI)
                    {
                        s += ftdpcontroltemp.ListTd0_JS_Check_Inner(PartID, SelectOnClick.Replace("@color1", ColorSelect.StartsWith("@") ? ColorSelect.Substring(1) : ColorSelect).Replace("@color2", ColorDefault.StartsWith("@") ? ColorDefault.Substring(1) : ColorDefault).Replace("'", "\\'"));
                        s += ftdpcontroltemp.ListTd0_JS_Check(PartID, SelectOnClick.Replace("@color1", ColorSelect.StartsWith("@") ? ColorSelect.Substring(1) : ColorSelect).Replace("@color2", ColorDefault.StartsWith("@") ? ColorDefault.Substring(1) : ColorDefault).Replace("'", "\\'"));
                    }
                    else if (ForeFrame == ForeFrameType.LayUI)
                    {
                        s += ftdpcontroltemp.ListTd0_JS_Check_Inner_LayUI(PartID, SelectOnClick.Replace("@color1", ColorSelect.StartsWith("@") ? ColorSelect.Substring(1) : ColorSelect).Replace("@color2", ColorDefault.StartsWith("@") ? ColorDefault.Substring(1) : ColorDefault).Replace("'", "\\'"));
                        s += ftdpcontroltemp.ListTd0_JS_Check_LayUI(PartID, SelectOnClick.Replace("@color1", ColorSelect.StartsWith("@") ? ColorSelect.Substring(1) : ColorSelect).Replace("@color2", ColorDefault.StartsWith("@") ? ColorDefault.Substring(1) : ColorDefault).Replace("'", "\\'"));
                    }
                }
                else if (SelectType == 1)
                {
                    s += ftdpcontroltemp.ListTd0_JS_Radio_Inner(PartID);
                    s += ftdpcontroltemp.ListTd0_JS_Radio(PartID);
                }
                else
                {
                    s += ftdpcontroltemp.ListTd0_JS_None_Inner(PartID);
                    s += ftdpcontroltemp.ListTd0_JS_None(PartID);
                }
                s += ftdpcontroltemp.ListTr0_JS(PartID, ColorHover.StartsWith("@") ? ColorHover.Substring(1) : ("\\'" + ColorHover + "\\'"), ColorDefault.StartsWith("@") ? ColorDefault.Substring(1) : ("\\'" + (ColorDefault.IndexOf(',') > 0 ? ColorDefault.Split(',')[1] : ColorDefault) + "\\'"));
                s += ftdpcontroltemp.ListTr1_JS(PartID, ColorHover.StartsWith("@") ? ColorHover.Substring(1) : ("\\'" + ColorHover + "\\'"), ColorDefault.StartsWith("@") ? ColorDefault.Substring(1) : ("\\'" + (ColorDefault.IndexOf(',') > 0 ? ColorDefault.Split(',')[0] : ColorDefault) + "\\'"));
                s += ftdpcontroltemp.ListTrTdDataBlock_JS(PartID, ColorHover.StartsWith("@") ? ColorHover.Substring(1) : ("\\'" + ColorHover + "\\'"), ColorDefault.StartsWith("@") ? ColorDefault.Substring(1) : ("\\'" + ColorDefault + "\\'"));
                s += ftdpcontroltemp.ListTrCacu_JS(PartID);
                s += ftdpcontroltemp.ListTd1_JS(PartID);
                s += ftdpcontroltemp.ListTd2_JS(PartID);
                s += ftdpcontroltemp.ListTd3_JS(PartID);
                s += ftdpcontroltemp.ListTd4_JS(PartID);
                s += ftdpcontroltemp.ListTd5_JS(PartID);
                s += ftdpcontroltemp.ListTd6_JS(PartID);
                s += ftdpcontroltemp.ListTd7_JS(PartID);
                s += ftdpcontroltemp.ListTd8_JS(PartID);
                s += ftdpcontroltemp.ListTd9_JS(PartID);
                s += ftdpcontroltemp.ListTd10_JS(PartID);
                s += ftdpcontroltemp.ListTd11_JS(PartID);
                s += ftdpcontroltemp.ListTd12_JS(PartID);
                s += ftdpcontroltemp.ListTdother_JS(PartID);
                s += ftdpcontroltemp.TurnPageTb_JS(PartID, Cellspacing, Cellpadding);
                s += ftdpcontroltemp.TurnPageTbTop_JS(PartID, Cellspacing, Cellpadding);
                s += ftdpcontroltemp.TurnPageInput_JS(PartID, PartID);
                s += ftdpcontroltemp.TurnPageSelect_JS(PartID, PartID, PartID);
                s += ftdpcontroltemp.SaveColInput_JS(PartID);
                s += ftdpcontroltemp.ListTitleTr_JS(PartID);
                //aa
                if (ForeFrame == ForeFrameType.JQueryUI)
                {
                    s += ftdpcontroltemp.ListTitleMSelect_JS(PartID);
                }
                else if (ForeFrame == ForeFrameType.LayUI)
                {
                    s += ftdpcontroltemp.ListTitleMSelect__LayUI_JS(PartID);
                }
                s += ftdpcontroltemp.ListTitleTd0_JS(PartID);
                s += ftdpcontroltemp.ListTitleTd1_JS(PartID);
                s += ftdpcontroltemp.ListTitleTd2_JS(PartID);
                s += ftdpcontroltemp.ListTitleTd3_JS(PartID);
                s += ftdpcontroltemp.ListTitleTd4_JS(PartID);
                s += ftdpcontroltemp.ListTitleTd5_JS(PartID);
                s += ftdpcontroltemp.ListTitleTd6_JS(PartID);
                s += ftdpcontroltemp.ListTitleTd7_JS(PartID);
                s += ftdpcontroltemp.ListTitleTd8_JS(PartID);
                s += ftdpcontroltemp.ListTitleTd9_JS(PartID);
                s += ftdpcontroltemp.ListTitleTd10_JS(PartID);
                s += ftdpcontroltemp.ListTitleTd11_JS(PartID);
                s += ftdpcontroltemp.ListTitleTd12_JS(PartID);
                s += ftdpcontroltemp.ListTitleTdOther_JS(PartID);
                string _Conststr = "";
                foreach (string _s in Consts)
                {
                    _Conststr += _s + "##";
                }

                string RerationTreeevalvals = "";
                Regex r = new Regex(@"{[^}]*}");
                MatchCollection mc = r.Matches(RerationTree);
                foreach (Match m in mc)
                {
                    RerationTreeevalvals += m.Value.Replace("{", "").Replace("}", "") + "+\"##\"+";
                }
                string sqlevalvals = "";
                r = new Regex(@"{[^}]*}");
                mc = r.Matches(sql);
                foreach (Match m in mc)
                {
                    sqlevalvals += m.Value.Replace("{", "").Replace("}", "") + "+\"##\"+";
                }

                string BlockDataDefineReal = BlockDataDefine.Replace("\r\n", "");
                r = new Regex(@"{[^}]*}");
                mc = r.Matches(BlockDataDefine);
                foreach (Match m in mc)
                {
                    if (m.Value.Equals("{check}")) continue;
                    BlockDataDefineReal = BlockDataDefineReal.Replace(m.Value, "@ftquoat_r@+" + m.Value.Replace("{", "").Replace("}", "").Replace("\"", "@ftquoat_r@") + "+@ftquoat_r@");
                }
                BlockDataDefineReal = BlockDataDefineReal.Replace("\"", "@ftquoat@");
                BlockDataDefineReal = BlockDataDefineReal.Replace("@ftquoat_r@", "\"");
                BlockDataDefineReal = "\"" + BlockDataDefineReal + "\"";
                string CacuRowDataReal = CacuRowData.Replace("\r\n", "");
                r = new Regex(@"{[^}]*}");
                mc = r.Matches(CacuRowData);
                foreach (Match m in mc)
                {
                    if (m.Value.IndexOf("(") >= 0) continue;
                    CacuRowDataReal = CacuRowDataReal.Replace(m.Value, "@ftquoat_r@+" + m.Value.Replace("{", "").Replace("}", "").Replace("\"", "@ftquoat_r@") + "+@ftquoat_r@");
                }
                CacuRowDataReal = CacuRowDataReal.Replace("\"", "@ftquoat@");
                CacuRowDataReal = CacuRowDataReal.Replace("@ftquoat_r@", "\"");
                CacuRowDataReal = "\"" + CacuRowDataReal + "\"";

                string _AppendTitle = "";
                if (AppendTitle != null)
                {
                    _AppendTitle = adv.GetSpecialBase(Context, AppendTitle.Trim(), SiteID).Replace("\"", "");
                    if (_AppendTitle.StartsWith("@code(")) _AppendTitle = FTDP.Interface.Code.Get(_AppendTitle, Context);
                }

                s += ftdpcontroltemp.ajax(PartID, _AppendTitle, PartID, PartID, PartID, PartID, PartID, str.GetEncode(OrderBy), str.GetEncode(List_Code), str.GetEncode(List_OPID), RerationTreeevalvals, sqlevalvals, IsTree.ToString(), str.GetEncode(RerationTree), SiteID, str.GetEncode(sql), RowAll.Replace("\"", "@ftquoat@"), _Conststr.Replace("\"", "@ftquoat@"), str.GetEncode(MainTable), ExportMax.ToString(), RateNumType.ToString(), NumsPerPage.ToString(), JSTableHead.Replace("\"", "\\\""), HeadIsShow, SelectType, ColorSelect, ColorDefault, RateNumType, JSTableTail.Replace("\"", "\\\""), TurnPage, FirstPage, PrePage, NextPage, LastPage, CountZero, TurnIsShow, BlockDataDefineReal, CacuRowDataReal, LoadEndJS, CustomConnection, LoadingImg, CustomTurnPageBottom, CustomTurnPageTop, IsAutoShow, PartID);
                s += "</script>";
                s += ftdpcontroltemp.menujsframe(PartID, PartID, str.GetEncode(Del_MemBind), str.GetEncode(Del_EleCondition), str.GetEncode(Del_RoleBindData), str.GetEncode(Del_RoleBindSession), str.GetEncode(Del_AuthRule), str.GetEncode(Del_FlowStat), str.GetEncode(Del_OPID), str.GetEncode(Copy_MemBind), str.GetEncode(Copy_EleCondition), str.GetEncode(Copy_RoleBindData), str.GetEncode(Copy_RoleBindSession), str.GetEncode(Copy_AuthRule), str.GetEncode(Copy_FlowStat), str.GetEncode(Copy_OPID), str.GetEncode(Flow_MemBind), str.GetEncode(Flow_EleCondition), str.GetEncode(Flow_RoleBindData), str.GetEncode(Flow_RoleBindSession), str.GetEncode(Flow_AuthRule), str.GetEncode(Flow_FlowStat), str.GetEncode(Flow_OPID), str.GetEncode(MainTable), str.GetEncode(Del_Code), str.GetEncode(Copy_Code), str.GetEncode(Flow_Code));
                output.Write(s);
            }
            catch (Exception ex)
            {
                output.Write(ex.Message);
                log.Exception(ex);
            }
            finally
            {
            }

        }
    }
}
