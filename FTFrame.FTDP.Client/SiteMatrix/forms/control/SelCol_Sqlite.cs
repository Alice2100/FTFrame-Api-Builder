using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using FTDPClient.consts;
using FTDPClient.database;

namespace FTDPClient.forms.control
{
    public partial class SelCol_Sqlite : Form
    {
        public string tablename=null;
        public string connstr = null;
        public string colname = null;
        public string coldesc = null;
        public string nameval = null;
        public SelCol_Sqlite()
        {
            InitializeComponent();
        }

        private void SelCol_SqlServer_Load(object sender, EventArgs e)
        {
            this.Text = tablename + "     " + this.Text;
            DB conn = null; 
            try
            {
                conn = new DB(connstr);
                conn.Open();
                //获取sqlite注释
                var zhuShiDic = DBFunc.GetColumnNoteForSqlite(conn, tablename);
                var sql = "";
                //用存储过程
                /*
            Create function TableStructure
(@tablename varchar(100))
returns table
as
return
(select (select value from sys.extended_properties where sys.extended_properties.major_id = sys.columns.object_id and sys.extended_properties.minor_id = sys.columns.column_id) as description
,sys.columns.name,sys.types.name as typename,sys.columns.max_length,sys.columns.precision,sys.columns.scale, sys.columns.is_nullable,
(select count(*) from sys.identity_columns where sys.identity_columns.object_id = sys.columns.object_id and sys.columns.column_id = sys.identity_columns.column_id) as is_identity ,
sys.columns.column_id
from sys.columns, sys.tables, sys.types where sys.columns.object_id = sys.tables.object_id and sys.columns.user_type_id=sys.types.user_type_id and sys.tables.name=@tablename)

                 */
                /*
                 (select (select value from sys.extended_properties where sys.extended_properties.major_id = sys.columns.object_id and sys.extended_properties.minor_id = sys.columns.column_id) as description  
         ,sys.columns.name,sys.types.name as typename,sys.columns.max_length,sys.columns.precision,sys.columns.scale, sys.columns.is_nullable,  
         (select count(*) from sys.identity_columns where sys.identity_columns.object_id = sys.columns.object_id and sys.columns.column_id = sys.identity_columns.column_id) as is_identity ,  
         sys.columns.column_id  
         from sys.columns, sys.tables, sys.types where sys.columns.object_id = sys.tables.object_id and sys.columns.user_type_id=sys.types.user_type_id and sys.tables.name=@tablename) 
                 */
                sql = "PRAGMA TABLE_INFO("+tablename+")";
                var dr = conn.OpenRecord(sql);
                BindingSource bs = new BindingSource();
                bs.DataSource = dr;
                dataGridView2.DataSource = bs;
                dr.Close();
                DataGridViewColumn col0 = new DataGridViewColumn();
                col0.CellTemplate = new DataGridViewTextBoxCell(); 
                col0.Name = "Note";
                col0.Width = 180;
                dataGridView1.Columns.Add(col0);
                for (int i =1;i<= dataGridView2.Columns.Count - 1; i++)
                {
                    DataGridViewTextBoxColumn colx = new DataGridViewTextBoxColumn();
                    colx.Name = dataGridView2.Columns[i].Name;
                    if (i == 1) { colx.DefaultCellStyle.BackColor = Color.AliceBlue; colx.Width = 180; }
                    dataGridView1.Columns.Add(colx);
                }
                DataGridViewTextBoxColumn colx0 = new DataGridViewTextBoxColumn();
                colx0.Name = dataGridView2.Columns[0].Name;
                dataGridView1.Columns.Add(colx0);
                foreach (DataGridViewRow row in dataGridView2.Rows)
                {
                    object[] item = new object[row.Cells.Count+1];
                    var key = row.Cells[1].Value.ToString();
                    var comment = "";
                    if (zhuShiDic.ContainsKey(key.ToLower())) comment = zhuShiDic[key.ToLower()];
                    item[0] = comment;
                    for(var i=1;i<= row.Cells.Count - 1;i++)
                    {
                        item[i] = row.Cells[i].Value;
                    }
                    item[row.Cells.Count] = row.Cells[0].Value;
                    bool IsRed = false;
                    if(nameval!=null && nameval.ToLower().IndexOf(item[1].ToString().ToLower())>=0)
                    {
                        IsRed = true;
                    }
                    int index=dataGridView1.Rows.Add(item);
                    if(IsRed)
                    {
                        dataGridView1.Rows[index].DefaultCellStyle.ForeColor = Color.Blue;
                        dataGridView1.Rows[index].DefaultCellStyle.Font = new Font(Font.FontFamily,Font.Size, FontStyle.Bold); 
                    }
                }
            }
            catch (Exception ex)
            {
                functions.MsgBox.Error(ex.Message);
            }
            finally
            {
                if (conn != null && conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                    conn = null;
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            colname = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            coldesc = (dataGridView1.Rows[e.RowIndex].Cells[0].Value?.ToString()) ?? "";
            this.Close();
        }

        private void SelCol_MySql_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) this.Close();
        }
    }
}
