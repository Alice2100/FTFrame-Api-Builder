using System;
using SiteMatrix.consts;
using SiteMatrix.functions;
using System.Windows.Forms;
using SiteMatrix.Compression;
using System.IO;
using System.Reflection;
using System.Xml;
namespace SiteMatrix.controls
{
	/// <summary>
	/// Controls 的摘要说明。
	/// </summary>
	[System.ComponentModel.LicenseProviderAttribute(typeof(SiteMatrix.classes.LicenceProvider))]
	public class controls
	{
		public controls()
		{
			System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
		}
		public static void DeleteControl(string controlname)
		{
			System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
			try
			{
				string sql="delete from controls where name='" + controlname + "'";
				globalConst.ConfigConn.execSql(sql);
				sql="delete from parts where controlname='" + controlname + "'";
				globalConst.ConfigConn.execSql(sql);
			}
			catch(Exception ex)
			{
			new error(ex);
			}
		}
		public static bool AddControl()
		{
			System.ComponentModel.LicenseManager.Validate(typeof(SiteMatrix.forms.NewControl));
			try
			{
			OpenFileDialog ofd=new OpenFileDialog();
            ofd.Filter = "FTDP Control(*.control)|*.control";
			ofd.ShowDialog();
			string zipfile=ofd.FileName.Trim();
			Cursor.Current=Cursors.WaitCursor;
			if(zipfile.Equals(""))return false;
			FileInfo fi=new FileInfo(zipfile);
			string controlname=fi.Name.ToLower().Replace(".control","");
			string sql="select count(name) from controls where name='" + controlname + "'";
			if(globalConst.ConfigConn.GetInt32(sql)>0)
			{
				MsgBox.Warning(res._controls.GetString("m1"));
				return false;
			}
				try
				{
					if(File.Exists(globalConst.LibPath + "\\ft_" + controlname + ".dll"))
					{
					File.Move(globalConst.LibPath + "\\ft_" + controlname + ".dll",globalConst.LibPath + "\\temp\\" + controlname + rdm.getDataSourceID() + DateTime.Now.Millisecond +".dll");
					}
					new ZipClass().UnZip(zipfile,globalConst.LibPath + "\\");
				}
				catch
				{
					log.Error("UnZip Error File Can't OverWrite:" + zipfile);
				}
			//利用反射取得控件信息
			Application.DoEvents();
			Assembly assem=Assembly.LoadFile(globalConst.LibPath + @"\ft_" + controlname + ".dll");
			Type MyAppType=assem.GetType("ft_" + controlname + ".client");
			object obj = MyAppType.InvokeMember(null,BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.CreateInstance, null, null,null);
			string WareXML = (string)MyAppType.InvokeMember("ControlXML",BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Instance,null,obj,null);
			//build XmlDocument
			XmlDocument WareDom=new XmlDocument();
			WareDom.LoadXml(WareXML);
			string control_name=WareDom.SelectSingleNode("//config/control_name").InnerText.ToLower();
			string control_caption=WareDom.SelectSingleNode("//config/control_caption").InnerText.Replace("'","''");
			string control_company=WareDom.SelectSingleNode("//config/control_company").InnerText.Replace("'","''");
			string control_copyright=WareDom.SelectSingleNode("//config/control_copyright").InnerText.Replace("'","''");
			string control_category=WareDom.SelectSingleNode("//config/control_category").InnerText.Replace("'","''");
			string control_version=WareDom.SelectSingleNode("//config/control_version").InnerText.Replace("'","''");
			string control_description=WareDom.SelectSingleNode("//config/control_description").InnerText.Replace("'","''");
			string control_icon=WareDom.SelectSingleNode("//config/control_icon").InnerText.Replace("'","''");
			string canpublish=WareDom.SelectSingleNode("//config/canpublish").InnerText;
			string cancheck=WareDom.SelectSingleNode("//config/cancheck").InnerText;
			string fore_parts=WareDom.SelectSingleNode("//config/fore_parts").OuterXml.Replace("'","''");
			string back_parts=WareDom.SelectSingleNode("//config/back_parts").OuterXml.Replace("'","''");

            string control_paras = "";
            if (WareDom.SelectSingleNode("//config/paras") == null)
            {
                control_paras = "<paras></paras>";
            }
            else
            {
                control_paras = WareDom.SelectSingleNode("//config/paras").OuterXml.Replace("'", "''");
            }


				sql="select count(name) from controls where name='" + control_name + "'";
				if(globalConst.ConfigConn.GetInt32(sql)>0)
				{
                    MsgBox.Warning(res._controls.GetString("m2") + "[" + control_name + "]");
					return false;
				}
				MyAppType=assem.GetType("ft_" + controlname + ".client");
				obj = MyAppType.InvokeMember(null,BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.CreateInstance, null, null,null);
				string DeployTime = (string)MyAppType.InvokeMember("DeployTime",BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Instance,null,obj,null);
			sql="insert into controls(name,caption,company,copyright,category,version,description,icon,canpublish,cancheck,fore_parts,back_parts,deploytime,paras)";
			sql+="values('" + control_name + "','" + control_caption + "','" + control_company + "','" + control_copyright + "','" + control_category + "','" + control_version + "','" + control_description + "','" + control_icon + "','" + canpublish + "','" + cancheck + "','" + fore_parts + "','" + back_parts + "','" + DeployTime + "','"+control_paras+"')";
			globalConst.ConfigConn.execSql(sql);
			//处理part
				foreach(XmlNode nd in WareDom.SelectNodes("//config/fore_parts/part"))
				{
					
					MyAppType=assem.GetType("ft_" + controlname + "." + nd.Attributes.GetNamedItem("name").Value);
                    if (MyAppType != null)
                    {
                        obj = MyAppType.InvokeMember(null, BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.CreateInstance, null, null, null);
                        string partXML = (string)MyAppType.InvokeMember("getPartXml", BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Instance, null, obj, null);
                        sql = "insert into parts(name,caption,controlname,partxml)values('" + nd.Attributes.GetNamedItem("name").Value + "','" + nd.InnerText.Trim() + "','" + controlname + "','" + partXML.Replace("'", "''") + "')";
                        globalConst.ConfigConn.execSql(sql);
                    }
                    
				}
				MyAppType=null;
				obj=null;
				assem=null;
                return true;
			}
			catch(Exception ex)
			{
				new error(ex);
                return false;
			}
		}
        public static System.Drawing.Image GetIconByPartID(string partid)
        {
            try
            {
                string sql = "select c.name as cname from controls c,parts p where c.id=p.controlid and p.id='" + partid + "'";
                return GetIconByControlID(globalConst.CurSite.SiteConn.GetString(sql));
            }
            catch 
            {
                return globalConst.DefaultControlIcon;
            }
        }
        public static System.Drawing.Image GetIconByControlID(string controlid)
        {
            try
            {
                if(controlid==null)return globalConst.DefaultControlIcon;
                foreach (ControlIcons ci in globalConst.ControlIcon)
                {
                    if (ci.ControlID.Equals(controlid)) return ci.Icon;
                }
                return globalConst.DefaultControlIcon;
            }
            catch
            {
                return globalConst.DefaultControlIcon;
            }
        }
        public static void InitControlIcon()
        {
            try
            {
                string sql = "select count(*) as countall from controls";
                int countall=globalConst.ConfigConn.GetInt32(sql)+73;
                globalConst.ControlIcon = new ControlIcons[countall];
                sql = "select name,icon,caption,company,description from controls order by caption";
                database.DR dr= new database.DR(globalConst.ConfigConn.OpenRecord(sql));
                int i = 0;
                globalConst.ControlsImages = new ImageList();
                globalConst.ControlsImages.TransparentColor = System.Drawing.Color.Transparent;
                //Static Icons
                for (i = 0; file.Exists(globalConst.ImgsPath + "\\toolbox\\" + i + ".gif"); i++)
                {
                    globalConst.ControlIcon[i] = new ControlIcons();
                    globalConst.ControlIcon[i].ControlID = "";
                    globalConst.ControlIcon[i].Caption = "";
                    globalConst.ControlIcon[i].Company = "";
                    globalConst.ControlIcon[i].Description = "";
                    try
                    {
                        globalConst.ControlIcon[i].Icon = System.Drawing.Image.FromFile(globalConst.ImgsPath + "\\toolbox\\" + i + ".gif");
                    }
                    catch
                    {
                        globalConst.ControlIcon[i].Icon = globalConst.DefaultControlIcon;
                    }
                    globalConst.ControlsImages.Images.Add(globalConst.ControlIcon[i].Icon);
                }
                //End
                while (dr.Read())
                {
                    globalConst.ControlIcon[i] = new ControlIcons();
                    globalConst.ControlIcon[i].ControlID = dr.getString("name");
                    globalConst.ControlIcon[i].Caption = dr.getString("caption");
                    globalConst.ControlIcon[i].Company = dr.getString("company");
                    globalConst.ControlIcon[i].Description = dr.getString("description");
                    string iconfile = globalConst.LibPath + "\\" + dr.getString("name") + ".res\\" + dr.getString("icon");
                    if (!File.Exists(iconfile))
                    {
                        globalConst.ControlIcon[i].Icon = globalConst.DefaultControlIcon;
                    }
                    else
                    {
                        try
                        {
                            globalConst.ControlIcon[i].Icon = System.Drawing.Image.FromFile(iconfile);
                        }
                        catch 
                        {
                            globalConst.ControlIcon[i].Icon = globalConst.DefaultControlIcon;
                        }
                    }
                    globalConst.ControlsImages.Images.Add(globalConst.ControlIcon[i].Icon);
                    i++;
                }
                globalConst.ControlsImages.Images.Add(System.Drawing.Image.FromFile(globalConst.ImgsPath + "\\control.gif"));
            }
            catch(Exception ex)
            {
                new error(ex);
            }
        }
	}
    //add by maobb 20071031
    [System.ComponentModel.LicenseProviderAttribute(typeof(SiteMatrix.classes.LicenceProvider))]
    public class AccessObj
    {
        private int a_ip_s;
        private string a_ip_c;
        private int a_ip_o;
        private int a_se_s;
        private string a_se_c;
        private int a_se_o;
        private int a_jp_s;
        private string a_jp_u;
        private int a_tp_s;
        private string a_tp_c;
        public string getA_ip_c()
        {
            return a_ip_c;
        }
        public void setA_ip_c(string a_ip_c)
        {
            this.a_ip_c = a_ip_c;
        }
        public int getA_ip_o()
        {
            return a_ip_o;
        }
        public void setA_ip_o(int a_ip_o)
        {
            this.a_ip_o = a_ip_o;
        }
        public int getA_ip_s()
        {
            return a_ip_s;
        }
        public void setA_ip_s(int a_ip_s)
        {
            this.a_ip_s = a_ip_s;
        }
        public int getA_jp_s()
        {
            return a_jp_s;
        }
        public void setA_jp_s(int a_jp_s)
        {
            this.a_jp_s = a_jp_s;
        }
        public string getA_jp_u()
        {
            return a_jp_u;
        }
        public void setA_jp_u(string a_jp_u)
        {
            this.a_jp_u = a_jp_u;
        }
        public string getA_se_c()
        {
            return a_se_c;
        }
        public void setA_se_c(string a_se_c)
        {
            this.a_se_c = a_se_c;
        }
        public int getA_se_o()
        {
            return a_se_o;
        }
        public void setA_se_o(int a_se_o)
        {
            this.a_se_o = a_se_o;
        }
        public int getA_se_s()
        {
            return a_se_s;
        }
        public void setA_se_s(int a_se_s)
        {
            this.a_se_s = a_se_s;
        }
        public string getA_tp_c()
        {
            return a_tp_c;
        }
        public void setA_tp_c(string a_tp_c)
        {
            this.a_tp_c = a_tp_c;
        }
        public int getA_tp_s()
        {
            return a_tp_s;
        }
        public void setA_tp_s(int a_tp_s)
        {
            this.a_tp_s = a_tp_s;
        }
    }
}
