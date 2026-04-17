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
namespace SiteMatrix.forms.control
{
    public partial class SelCol_MySql : Form
    {
        public string tablename=null;
        public string connstr = null;
        public string colname = null;
        public string nameval = null;
        public SelCol_MySql()
        {
            InitializeComponent();
        }

        private void SelCol_SqlServer_Load(object sender, EventArgs e)
        {
            this.Text = tablename + "     " + this.Text;
            MySqlConnection conn = null; 
            try
            {
                conn = new MySqlConnection(connstr);
                conn.Open();
                string sql = "show table status where name='" + tablename + "'";
                MySqlDataReader dr = new MySqlCommand(sql, conn).ExecuteReader();
                if (!dr.HasRows)
                {
                    dr.Close();
                    functions.MsgBox.Warning("Table " + tablename + " 在数据库中不存在！");
                    this.Close();
                    return;
                }
                dr.Close();
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
                sql = "show full fields from " + tablename + "";
                dr = new MySqlCommand(sql, conn).ExecuteReader();
                BindingSource bs = new BindingSource();
                bs.DataSource = dr;
                dataGridView2.DataSource = bs;
                dr.Close();
                DataGridViewColumn col0 = new DataGridViewColumn();
                col0.CellTemplate = new DataGridViewTextBoxCell(); 
                col0.Name = dataGridView2.Columns[dataGridView2.Columns.Count-1].Name;
                dataGridView1.Columns.Add(col0);
                for (int i =0;i< dataGridView2.Columns.Count - 1; i++)
                {
                    DataGridViewTextBoxColumn colx = new DataGridViewTextBoxColumn();
                    colx.Name = dataGridView2.Columns[i].Name;
                    if (i == 0) { colx.DefaultCellStyle.BackColor = Color.AliceBlue; colx.Width = 180; }
                    dataGridView1.Columns.Add(colx);
                }
                foreach (DataGridViewRow row in dataGridView2.Rows)
                {
                    object[] item = new object[row.Cells.Count];
                    object comment = row.Cells[row.Cells.Count - 1].Value;
                    item[0] = comment;
                    for (int i = row.Cells.Count - 1; i > 0; i--)
                    {
                        item[i]=row.Cells[i - 1].Value;
                    }
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
            this.Close();
        }

        private void SelCol_MySql_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) this.Close();
        }
    }
}
