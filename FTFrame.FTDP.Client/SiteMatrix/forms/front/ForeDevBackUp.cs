using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using FTDPClient.consts;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using ICSharpCode.TextEditor.Document;
using FTDPClient.database;

namespace FTDPClient.forms
{
    public partial class ForeDevBackUp : Form
    {
        public ForeDevBackUp()
        {
            InitializeComponent();
        }

        private void ForeDev_Load(object sender, EventArgs e)
        {
            this.Text= res.com.str("ToolMenu.QianDuanBackUp");
        }
    }
}
