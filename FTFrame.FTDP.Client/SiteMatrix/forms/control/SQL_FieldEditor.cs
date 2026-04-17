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
using FTDPClient.functions;
using System.Collections;
using FTDPClient.consts;
using FTDPClient.database;

namespace FTDPClient.forms.control
{
    public partial class SQL_FieldEditor : Form
    {
        public List<(string prefix, string field, string alias)> Fields =null;
        public Dictionary<string, string> TableAlias = null;
        public string FieldsStr = "";
        public bool IsOk=false;
        List<(string TableName, string TableAlias, string Field, string Type, string Comment)> TableFields = new List<(string TableName, string TableAlias, string Field, string Type, string Comment)>();
        public SQL_FieldEditor()
        {
            InitializeComponent();
        }
        private bool IsAutoAlias( ){ return checkBox2.Checked; }
        private void SelCol_SqlServer_Load(object sender, EventArgs e)
        {
            try
            {
                this.Text = "SQL Field Editor";         //快速定义列
                button1.Text = "&OK";           //确定(&O)
                checkBox1.Text = "Select All";
                checkBox2.Text ="Auto Alias Name";
                string connstr = Options.GetSystemDBSetConnStr();
                var dbtype = Options.GetSystemDBSetType();
                if (connstr == null || connstr.Trim().Equals(""))
                {
                    MsgBox.Warning(res.ctl.str("RawSelCol.1"));
                    this.Close();
                    return;
                }
                DataGridViewCheckBoxColumn colXuanZe = new DataGridViewCheckBoxColumn();
                //colXuanZe.CellTemplate = new DataGridViewCheckBoxCell();
                colXuanZe.Name = "";
                dataGridView1.Columns.Add(colXuanZe);
                colXuanZe.Width = 40;

                DataGridViewColumn colField = new DataGridViewColumn();
                colField.CellTemplate = new DataGridViewTextBoxCell();
                colField.Name = "_Field";
                colField.HeaderText = "Field";
                dataGridView1.Columns.Add(colField);
                colField.Width = 120;

                DataGridViewColumn colAlias = new DataGridViewColumn();
                colAlias.CellTemplate = new DataGridViewTextBoxCell();
                colAlias.Name = "_Alias";
                colAlias.HeaderText = "Alias";
                dataGridView1.Columns.Add(colAlias);
                colAlias.Width = 120;

                DataGridViewColumn colzs = new DataGridViewColumn();
                colzs.CellTemplate = new DataGridViewTextBoxCell();
                colzs.Name = "_Description";
                colzs.HeaderText = "Description";
                colzs.DefaultCellStyle.ForeColor = Color.Blue;
                dataGridView1.Columns.Add(colzs);
                colzs.Width = 180;

                DataGridViewColumn colType = new DataGridViewColumn();
                colType.CellTemplate = new DataGridViewTextBoxCell();
                colType.Name = "_Type";
                colType.HeaderText = "Type";
                dataGridView1.Columns.Add(colType);
                colType.Width = 120;

                DataGridViewButtonColumn colShang = new DataGridViewButtonColumn();
                //colShang.CellTemplate = new DataGridViewButtonCell();
                colShang.Name = "";
                colShang.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.Columns.Add(colShang);
                colShang.Width = 50;

                DataGridViewButtonColumn colXia = new DataGridViewButtonColumn();
                //colXia.CellTemplate = new DataGridViewButtonCell();
                colXia.Name = "";
                colXia.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.Columns.Add(colXia);
                colXia.Width = 50;

                DataGridViewButtonColumn colDing = new DataGridViewButtonColumn();
                //colXia.CellTemplate = new DataGridViewButtonCell();
                colDing.Name = "";
                colDing.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.Columns.Add(colDing);
                colDing.Width = 50;

                DataGridViewButtonColumn colDi = new DataGridViewButtonColumn();
                //colXia.CellTemplate = new DataGridViewButtonCell();
                colDi.Name = "";
                colDi.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.Columns.Add(colDi);
                colDi.Width = 50;


                DataGridViewColumn col0 = new DataGridViewColumn();
                col0.CellTemplate = new DataGridViewTextBoxCell();
                col0.Name = "_Table";
                col0.HeaderText = "Table";
                col0.DefaultCellStyle.BackColor = Color.AliceBlue;
                col0.DefaultCellStyle.ForeColor = Color.Blue;
                dataGridView1.Columns.Add(col0);
                col0.Width = 160;

                DataGridViewColumn col2 = new DataGridViewColumn();
                col2.CellTemplate = new DataGridViewTextBoxCell();
                col2.Name = "_TableAlias";
                col2.HeaderText = "Table Alias";
                col2.DefaultCellStyle.BackColor = Color.AliceBlue;
                col2.DefaultCellStyle.ForeColor = Color.Blue;
                dataGridView1.Columns.Add(col2);
                col2.Width = 110;


                if (dbtype==globalConst.DBType.SqlServer)
                {
                    using (SqlConnection conn = new SqlConnection(connstr))
                    {
                        conn.Open();
                        foreach (var dicItem in TableAlias)
                        {
                            string sql = @"(select sys.columns.name,sys.types.name as typename,sys.columns.max_length,sys.columns.precision,sys.columns.scale, sys.columns.is_nullable,
 (select count(*) from sys.identity_columns where sys.identity_columns.object_id = sys.columns.object_id and sys.columns.column_id = sys.identity_columns.column_id) as is_identity ,
 sys.columns.column_id,(select value from sys.extended_properties where sys.extended_properties.major_id = sys.columns.object_id and sys.extended_properties.minor_id = sys.columns.column_id) as description 
 from sys.columns, sys.tables, sys.types where sys.columns.object_id = sys.tables.object_id and sys.columns.user_type_id=sys.types.user_type_id and sys.tables.name='" + dicItem.Value.Replace("'","") + "')";

                            using (SqlDataReader dr = new SqlCommand(sql, conn).ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    TableFields.Add((dicItem.Value.Replace("'", ""),dicItem.Key,dr.GetString(0), dr.GetString(1), dr.IsDBNull(dr.GetOrdinal("description")) ? null : dr.GetString(dr.GetOrdinal("description"))));
                                }
                            }
                        }
                    }
                }
                else if (dbtype == globalConst.DBType.MySql)
                {
                    using (MySqlConnection conn = new MySqlConnection(connstr))
                    {
                        conn.Open();
                        foreach (var dicItem in TableAlias)
                        {
                            string sql = "show full fields from " + dicItem.Value.Replace("'", "") + "";

                            using (MySqlDataReader dr = new MySqlCommand(sql, conn).ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    TableFields.Add((dicItem.Value.Replace("'", ""), dicItem.Key, dr.GetString(dr.GetOrdinal("Field")), dr.GetString(dr.GetOrdinal("Type")), dr.IsDBNull(dr.GetOrdinal("Comment")) ? null : dr.GetString(dr.GetOrdinal("Comment"))));
                                }
                            }
                        }
                    }
                }
                else if (dbtype == globalConst.DBType.Sqlite)
                {
                    using (var conn = new DB(connstr))
                    {
                        conn.Open();
                        foreach (var dicItem in TableAlias)
                        {
                            string sql = "PRAGMA TABLE_INFO(" + dicItem.Value.Replace("'", "") + ")";
                            var zhuShiDic = DBFunc.GetColumnNoteForSqlite(conn, dicItem.Value.Replace("'", ""));


                            using (var dr = conn.OpenRecord(sql))
                            {
                                while (dr.Read())
                                {
                                    TableFields.Add((dicItem.Value.Replace("'", ""), dicItem.Key, dr.GetString(dr.GetOrdinal("name")), dr.GetString(dr.GetOrdinal("type")), zhuShiDic.ContainsKey(dr.GetString(dr.GetOrdinal("name")).ToLower()) ? zhuShiDic[dr.GetString(dr.GetOrdinal("name")).ToLower()] : ""));
                                }
                            }
                        }
                            
                    }
                }
                else
                {
                    MsgBox.Warning(dbtype + " " + res.ctl.str("RawSelCol.6"));
                    this.Close();
                    return;
                }
                var fieldseld = new List<string>();
                foreach(var item in Fields)
                {
                    if (item.field == "*")
                    {
                        var _aaa = TableFields.Where(r => r.TableAlias.ToLower() == item.prefix.ToLower());
                        if (_aaa.Count() > 0)
                        {
                            foreach(var tf in _aaa)
                            {
                                fieldseld.Add(tf.TableName + "." + tf.Field);
                                var i = dataGridView1.Rows.Add(true, tf.Field, "", tf.Comment, tf.Type, res.ctl.str("RawSelCols.9"), res.ctl.str("RawSelCols.10"), res.ctl.str("RawSelCols.11"), res.ctl.str("RawSelCols.12"), tf.TableName,tf.TableAlias);
                                dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.LightBlue;
                            }
                        }
                    }
                    else
                    {
                        var _aaa = TableFields.Where(r => r.TableAlias.ToLower() == item.prefix.ToLower() && r.Field.ToLower() == item.field.ToLower());
                        if (_aaa.Count() <= 0)
                        {
                            MsgBox.Error(item.prefix + "." + item.field + " 在指定表字段中不存在");
                            this.Close();
                        }
                        var tableField = _aaa.First();
                        fieldseld.Add(tableField.TableName + "." + tableField.Field);
                        var i = dataGridView1.Rows.Add(true, item.field, item.alias, tableField.Comment, tableField.Type, res.ctl.str("RawSelCols.9"), res.ctl.str("RawSelCols.10"), res.ctl.str("RawSelCols.11"), res.ctl.str("RawSelCols.12"), tableField.TableName, tableField.TableAlias);
                        dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.LightBlue;
                    }
                }
                foreach(var item in TableFields)
                {
                    if(!fieldseld.Contains(item.TableName + "." + item.Field))
                    {
                        dataGridView1.Rows.Add(false, item.Field, "", item.Comment, item.Type, res.ctl.str("RawSelCols.9"), res.ctl.str("RawSelCols.10"), res.ctl.str("RawSelCols.11"), res.ctl.str("RawSelCols.12"), item.TableName,item.TableAlias);
                    }
                }
            }
            catch(Exception ex)
            {
                new error(ex);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex==0)//复选框
            {
                if((bool)dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].EditedFormattedValue)
                {
                    dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightBlue;
                    var row = dataGridView1.Rows[e.RowIndex];
                    if (IsAutoAlias() && string.IsNullOrWhiteSpace(row.Cells[2].Value.ToString()))
                    {
                        var field = row.Cells[1].Value.ToString();
                        foreach (DataGridViewRow row1 in dataGridView1.Rows)
                        {
                            if(row1.Index!=row.Index)
                            {
                                //var otherName = string.IsNullOrWhiteSpace(row1.Cells[2].Value.ToString())? row1.Cells[1].Value.ToString():row1.Cells[2].Value.ToString();
                                var otherName = row1.Cells[1].Value.ToString();
                                if(otherName.ToLower()==field.ToLower())
                                {
                                    if(row.Cells[9].Value.ToString().IndexOf('_')>0)
                                    {
                                        row.Cells[2].Value = row.Cells[9].Value.ToString().Substring(row.Cells[9].Value.ToString().IndexOf('_')+1)+"_"+field;
                                    }
                                    else
                                    {
                                        row.Cells[2].Value = row.Cells[9].Value.ToString()+"_"+field;
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                    dataGridView1.Rows[e.RowIndex].Cells[4].Style.BackColor = Color.AliceBlue;
                }
            }
            else if (e.ColumnIndex == 5)//上
            {
                UpDataGridView(dataGridView1, dataGridView1.Rows[e.RowIndex]);
            }
            else if (e.ColumnIndex == 6)//下
            {
                DnDataGridView(dataGridView1, dataGridView1.Rows[e.RowIndex]);
            }
            else if (e.ColumnIndex == 7)//顶
            {
                TopDataGridView(dataGridView1, dataGridView1.Rows[e.RowIndex]);
            }
            else if (e.ColumnIndex == 8)//底
            {
                BottomDataGridView(dataGridView1, dataGridView1.Rows[e.RowIndex]);
            }
        }
        private  void UpDataGridView(DataGridView dataGridView,DataGridViewRow row)
        {
            try
            {
                int index = row.Index;
                if (index > 0)//如果该行不是第一行
                {
                    DataGridViewRow dgvr = dataGridView.Rows[index - 1];//获取选中行的上一行
                    dataGridView.Rows.RemoveAt(index - 1);//删除原选中行的上一行
                    dataGridView.Rows.Insert((index), dgvr);//将选中行的上一行插入到选中行的后面
                    dataGridView.Rows[index - 1].Selected = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void DnDataGridView(DataGridView dataGridView, DataGridViewRow row)
        {
            try
            {
                int index = row.Index;
                if (index>=0 && index < dataGridView.Rows.Count-1)
                {
                    DataGridViewRow dgvr = dataGridView.Rows[index + 1];
                    dataGridView.Rows.RemoveAt(index + 1);
                    dataGridView.Rows.Insert((index), dgvr);
                    dataGridView.Rows[index + 1].Selected = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private  void TopDataGridView(DataGridView dataGridView, DataGridViewRow row)
        {
            try
            {
                int index = row.Index;
                if (index > 0)//如果该行不是第一行
                {
                    DataGridViewRow dgvr = dataGridView.Rows[index];//获取选中行的上一行
                    dataGridView.Rows.RemoveAt(index);//删除原选中行的上一行
                    dataGridView.Rows.Insert(0, dgvr);//将选中行的上一行插入到选中行的后面                       
                    for (int i = 0; i < dataGridView.Rows.Count; i++)//选中移动后的行
                    {
                        if (i != 0)
                        {
                            dataGridView.Rows[i].Selected = false;
                        }
                        else
                        {
                            dataGridView.Rows[i].Selected = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private  void BottomDataGridView(DataGridView dataGridView, DataGridViewRow row)
        {
            try
            {
                int index = row.Index;
                if (index < dataGridView.Rows.Count - 1)
                {
                    DataGridViewRow dgvr = dataGridView.Rows[index];//获取选中行的上一行
                    dataGridView.Rows.RemoveAt(index);//删除原选中行的上一行
                    int nCount = dataGridView.Rows.Count;
                    dataGridView.Rows.Insert(nCount, dgvr);//将选中行的上一行插入到选中行的后面
                    for (int i = 0; i < dataGridView.Rows.Count; i++)//选中移动后的行
                    {
                        if (i != dataGridView.Rows.Count - 1)
                        {
                            dataGridView.Rows[i].Selected = false;
                        }
                        else
                        {
                            dataGridView.Rows[i].Selected = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void SelCol_SqlServer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) this.Close();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            var sb = new StringBuilder();
            var dic=new Dictionary<string, int>();
            foreach(DataGridViewRow row in dataGridView1.Rows)
            {
                if ((bool)row.Cells[0].EditedFormattedValue && string.IsNullOrWhiteSpace(row.Cells[2].Value.ToString()))
                {
                    if(!dic.ContainsKey(row.Cells[10].Value.ToString()))
                    {
                        dic.Add(row.Cells[10].Value.ToString(), 1);
                    }
                    else dic[row.Cells[10].Value.ToString()]++;
                }
            }
            var list=new List<string>();
            foreach(var key in dic.Keys)
            {
                if (dic[key]>= TableFields.Where(r=>r.TableAlias==key).Count())
                {
                    sb.Append($",{key}.*");
                    list.Add(key);  
                }
            }
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if ((bool)row.Cells[0].EditedFormattedValue && !list.Contains(row.Cells[10].Value.ToString()))
                {
                    sb.Append(string.IsNullOrWhiteSpace(row.Cells[2].Value.ToString()) ? ("," + row.Cells[10].Value.ToString() +"."+ row.Cells[1].Value.ToString()) : ("," + row.Cells[10].Value.ToString() + "." + row.Cells[1].Value.ToString() + " " + row.Cells[2].Value.ToString()));
                }
            }
            FieldsStr =sb.ToString();
            if (FieldsStr != "") FieldsStr = FieldsStr.Substring(1);
            IsOk = true;
            this.Close();
        }

        private void checkBox1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0) return;
            var tablename = comboBox1.Items[comboBox1.SelectedIndex].ToString().Split(' ')[0];
            var tablealias = comboBox1.Items[comboBox1.SelectedIndex].ToString().Split(' ')[1];
            var check = checkBox1.Checked;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells[9].Value.ToString().ToLower()==tablename.ToLower() && row.Cells[10].Value.ToString().ToLower()== tablealias.ToLower())
                {
                    row.Cells[0].Value = check;
                    if (check)
                    {
                        row.DefaultCellStyle.BackColor = Color.LightBlue;
                    }
                    else
                    {
                        row.DefaultCellStyle.BackColor = Color.White;
                    }
                }
            }
        }

        #region DataGridView1 Drag Drop
        int selectionIdx = -1;
        private void dataGridView1_DragEnter(object sender, DragEventArgs e)
        {
            dataGridView1.EndEdit();
            e.Effect = DragDropEffects.Move;
        }

        private void dataGridView1_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if ((e.Clicks < 2) && (e.Button == MouseButtons.Left))

            {
                if ((e.ColumnIndex == -1) && (e.RowIndex > -1))

                    dataGridView1.DoDragDrop(dataGridView1.Rows[e.RowIndex], DragDropEffects.Move);

            }
        }
        private int GetRowFromPoint(int x, int y)

        {
            for (int i = 0; i < dataGridView1.RowCount; i++)

            {
                Rectangle rec = dataGridView1.GetRowDisplayRectangle(i, false);

                if (dataGridView1.RectangleToScreen(rec).Contains(x, y))

                    return i;

            }
            return -1;

        }
        private void dataGridView1_DragDrop(object sender, DragEventArgs e)
        {
            int idx = GetRowFromPoint(e.X, e.Y);

            if (idx < 0) return;

            if (e.Data.GetDataPresent(typeof(DataGridViewRow)))

            {
                DataGridViewRow row = (DataGridViewRow)e.Data.GetData(typeof(DataGridViewRow));

                dataGridView1.Rows.Remove(row);

                selectionIdx = idx;

                dataGridView1.Rows.Insert(idx, row);

            }
        }

        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (selectionIdx > -1)

            {
                dataGridView1.Rows[selectionIdx].Selected = true;

                dataGridView1.CurrentCell = dataGridView1.Rows[selectionIdx].Cells[0];
                selectionIdx = -1;

            }
        }

        #endregion

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox2_Click(object sender, EventArgs e)
        {
            
        }
    }
}
