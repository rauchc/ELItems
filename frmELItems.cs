using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;

namespace ELItems
{
    public partial class frmELItems : Form
    {
        private Manufacture _manu = null;

        public frmELItems()
        {
            InitializeComponent();
        }


        private void btnCalculate_Click(object sender, EventArgs e)
        {
            if (cboItems.SelectedIndex > -1)
            {
                txtResult.Text = numAmount.Value.ToString() + " " + cboItems.Text + Environment.NewLine;
                string result = _manu.getIngreds(cboItems.Text.Substring(0,cboItems.Text.IndexOf("(") -1), Int32.Parse(numAmount.Value.ToString()),1);
                txtResult.Text += result + Environment.NewLine;
                txtResult.Text += "Summary of ingredients:" + Environment.NewLine;
                foreach (ingred ing in _manu.Summary.Values)
                {
                    txtResult.Text += String.Format("{0}\t{1}" + Environment.NewLine, ing.Amount, ing.Name);
                }
                txtResult.Text += Environment.NewLine + "Total food used: " + _manu.Food.ToString() + Environment.NewLine;
            }
        }
        
        private void frmELItems_Load(object sender, EventArgs e)
        {
            _manu = new Manufacture(ref cboItems);
            _manu.parseManufacture();
        }

        private void reloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _manu.readManufacture();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnUses_Click(object sender, EventArgs e)
        {
            txtResult.Clear();
            List<string> _uses = _manu.getUses(cboItems.Text.Substring(0, cboItems.Text.IndexOf("(") - 1));
            if (_uses.Count > 0)
            {
                txtResult.AppendText("Item " + cboItems.Text.Substring(0, cboItems.Text.IndexOf("(") - 1) + " is used in the following " + _uses.Count.ToString() + " recipes:" + Environment.NewLine + Environment.NewLine);
                foreach (string _s in _uses)
                {
                    txtResult.AppendText(_s + Environment.NewLine);
                }
            }
            else
            {
                txtResult.AppendText("Item " + cboItems.Text.Substring(0, cboItems.Text.IndexOf("(") - 1) + " has no known use in a recipe." + Environment.NewLine);
            }
        }

    }
}
