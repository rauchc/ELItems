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
            txtResult.AppendText("Item " + cboItems.Text.Substring(0, cboItems.Text.IndexOf("(") - 1) + " is used in the following recipes:" + Environment.NewLine + Environment.NewLine);
            foreach (ELItem _item in _manu.AllItems)
            {
                if (_item.Ingreds != null)
                {
                    foreach (ingred _i in _item.Ingreds)
                    {
                        if (_i.Name.Equals(cboItems.Text.Substring(0, cboItems.Text.IndexOf("(") - 1)))
                        {
                            txtResult.AppendText(_item.Name + Environment.NewLine);
                        }
                    }
                }
            }
        }

    }
}
