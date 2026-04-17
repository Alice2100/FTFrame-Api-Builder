using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace SiteMatrix.forms.control
{
    public partial class SelCol_SqlServer : Form
    {
        public string tablename=null;
        public string connstr = null;
        public string colname = null;
        public string nameval = null;
        public SelCol_SqlServer()
        {
            InitializeComponent();
        }

        private void SelCol_SqlServer_Load(object sender, EventArgs e)
        {
            this.Text = tablename + "     " + this.Text;
            SqlConnection conn = null; 
            try
            {
                conn = new SqlConnection(connstr);
                conn.Open();
                string sql = "select name from sysobjects where name<>'dtproperties' and xtype='U' and name='"+tablename+"'";
                SqlDataReader dr = new SqlCommand(sql, conn).ExecuteReader();
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
                sql = "select * from TableStructure('" + tablename + "') order by column_id";
                dr = new SqlCommand(sql, conn).ExecuteReader();
                BindingSource bs = new BindingSource();
                bs.DataSource = dr;
                dataGridView1.DataSource = bs;
                dr.Close();
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

        private void SelCol_SqlServer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) this.Close();
        }
    }
}
