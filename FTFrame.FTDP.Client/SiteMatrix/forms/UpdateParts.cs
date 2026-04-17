using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using FTDPClient.database;
using FTDPClient.consts;
using FTDPClient.functions;
using System.Data.OleDb;
using System.Xml;
using Microsoft.Data.Sqlite;

namespace FTDPClient.forms
{
    public partial class UpdateParts : Form
    {
        public UpdateParts()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void UpdateParts_Load(object sender, EventArgs e)
        {
            if (globalConst.CurSite.ID != null)
            {
                textBox2.Text = globalConst.CurSite.ID;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DB sitedb = new DB();
            try
            {
                button1.Enabled = false;
                button2.Enabled = false;
                textBox1.Enabled = false;
                textBox2.Enabled = false;

                label3.Text = "Initialize database...";
                Application.DoEvents();

                string siteid = textBox2.Text.Trim();
                string controlname = textBox1.Text.Trim();
                sitedb.Open(functions.db.ConnStr_Site(siteid));

                string sql = null;
                sql = "select * from parts where controlname='" + functions.str.Dot2DotDot(controlname) + "'";
                SqliteDataReader parts_rdr = globalConst.ConfigConn.OpenRecord(sql);
                while (parts_rdr.Read())
                {
                    string partname = parts_rdr["name"].ToString();
                    string PartXml = parts_rdr["partxml"].ToString();

                    label3.Text = "Part " + partname + "...";
                    Application.DoEvents();

                    string partXml = Page.PageWare.ConvertPartXML(PartXml);
                    XmlDocument newdoc = new XmlDocument();
                    //newdoc.LoadXml(partXml);

                    sql = "select a.id,a.partxml from parts a,controls b where a.controlid=b.id and b.name='" + functions.str.Dot2DotDot(controlname) + "' and a.name='" + functions.str.Dot2DotDot(partname) + "'";
                    SqliteDataReader rdr = sitedb.OpenRecord(sql);
                    while (rdr.Read())
                    {
                        newdoc.LoadXml(partXml);

                        string partid = rdr["id"].ToString();
                        string oldpartxml = rdr["partxml"].ToString();

                        label3.Text = "Update " + partid + "...";
                        Application.DoEvents();

                        XmlDocument olddoc= new XmlDocument();
                        olddoc.LoadXml(oldpartxml);

                        XmlNodeList nodes=newdoc.SelectNodes("//partxml/param");
                        foreach (XmlNode node in nodes)
                        {
                            string newparaname = node.SelectSingleNode("name").InnerText;
                            string newparatype = node.SelectSingleNode("type").InnerText;
                            string newvalue = GetOldParaValue(olddoc,newparaname,newparatype);
                            if (newvalue != null)
                            {
                                node.SelectSingleNode("value").InnerText = newvalue;
                            }
                        }
                        nodes = null;

                        nodes = newdoc.SelectNodes("//partxml/styles/style");
                        foreach (XmlNode node in nodes)
                        {
                            string newstylename = node.Attributes["name"].Value;
                            string[] newvalue = GetOldStyleValue(olddoc, newstylename);
                            if (newvalue != null)
                            {
                                node.Attributes["class"].Value = newvalue[0];
                                node.Attributes["csstext"].Value = newvalue[1];
                            }
                        }
                        nodes = null;

                        olddoc = null;

                        sql = "update parts set partxml='" + functions.str.Dot2DotDot(newdoc.OuterXml) + "' where id='"+partid+"'";
                        sitedb.execSql(sql);

                    }
                    rdr.Close();

                    
                    newdoc = null;
                }
                parts_rdr.Close();
            }
            catch (Exception ex)
            {
                new functions.error(ex);
            }
            finally
            {
                button1.Enabled = true;
                button2.Enabled = true;
                textBox1.Enabled = true;
                textBox2.Enabled = true;
                label3.Text = "Done.";
                sitedb.Close();
                sitedb = null;
            }
        }
        private string GetOldParaValue(XmlDocument olddoc, string paraname, string paratype)
        {
            string returnstr = null;
            XmlNodeList nodes = olddoc.SelectNodes("//partxml/param");
            foreach (XmlNode node in nodes)
            {
                string oldparaname = node.SelectSingleNode("name").InnerText;
                string oldparatype = node.SelectSingleNode("type").InnerText;
                if (paraname.Equals(oldparaname) && paratype.Equals(oldparatype))
                {
                    returnstr = node.SelectSingleNode("value").InnerText;
                    break;
                }
            }
            nodes = null;
            return returnstr;
        }
        private string[] GetOldStyleValue(XmlDocument olddoc, string stylename)
        {
            string[] returnstr = null;
            XmlNode node = olddoc.SelectSingleNode("//partxml/styles/style[@name=\"" + stylename + "\"]");
            if (node != null)
            {
                returnstr = new string[] { node.Attributes["class"].Value, node.Attributes["csstext"].Value };
            }
            node = null;
            return returnstr;
        }
    }
}