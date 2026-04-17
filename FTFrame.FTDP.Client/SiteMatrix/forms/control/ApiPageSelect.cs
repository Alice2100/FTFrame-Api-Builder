using FTDPClient.functions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FTDPClient.forms.control
{
    public partial class ApiPageSelect : Form
    {
        public ApiPageSelect()
        {
            InitializeComponent();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null) return;
            var curitem =(Obj.ComboItem) listBox1.SelectedItem;
            var tagArray = (string[])curitem.Tag;
            tree.PagePositonToPart(tagArray[0], tagArray[1], tagArray[2], tagArray[3]);
            this.Close();
        }
    }
}
