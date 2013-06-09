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
            // get selected tab and see if the textbox is empty
            TextBox txt = findControl(tabControl1, "txtResult" + (tabControl1.TabPages.Count - 1).ToString());
            if (txt != null)
            {
                if (txt.TextLength > 0)
                {
                    // create a new tab
                    tabControl1.TabPages.Add("I: " + cboItems.Text.Substring(0, cboItems.Text.IndexOf("(") - 1));
                    // add a new textbox to that tab
                    TextBox txtNew = new TextBox();
                    txtNew.Location = new System.Drawing.Point(3, 6);
                    txtNew.Multiline = true;
                    txtNew.Name = "txtResult" + (tabControl1.TabPages.Count - 1).ToString();
                    txtNew.Size = new System.Drawing.Size(733, 487);
                    txtNew.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
                    tabControl1.TabPages[tabControl1.TabPages.Count - 1].Controls.Add(txtNew);
                    txt = txtNew;
                }
                else
                {
                    tabControl1.SelectedTab.Text = "I: " + cboItems.Text.Substring(0, cboItems.Text.IndexOf("(") - 1);
                }
                tabControl1.SelectedIndex = tabControl1.TabPages.Count - 1;
                if (cboItems.SelectedIndex > -1)
                {

                    txt.Text = numAmount.Value.ToString() + " " + cboItems.Text + Environment.NewLine;
                    string result = _manu.getIngreds(cboItems.Text.Substring(0, cboItems.Text.IndexOf("(") - 1), Int32.Parse(numAmount.Value.ToString()), 1);
                    txt.Text += result + Environment.NewLine;
                    txt.Text += "Summary of ingredients:" + Environment.NewLine;
                    foreach (ingred ing in _manu.Summary.Values)
                    {
                        txt.Text += String.Format("{0}\t{1}" + Environment.NewLine, ing.Amount, ing.Name);
                    }
                    txt.Text += Environment.NewLine + "Total food used: " + _manu.Food.ToString() + Environment.NewLine;
                }
            }
            else
            {
                MessageBox.Show("Could not find TextBox!");
            }
        }

        private TextBox findControl(TabControl tab, string name)
        {
            foreach (TabPage c in tab.TabPages)
            {
                if (c.Controls.Count > 0)
                {
                    foreach (Control c1 in c.Controls)
                    {
                        if (c1.Name == name)
                            return (TextBox)c1;
                    }
                }
            }
            return null;
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
            TextBox txt = findControl(tabControl1, "txtResult" + (tabControl1.TabPages.Count - 1).ToString());
            if (txt != null)
            {
                if (txt.TextLength > 0)
                {
                    // create a new tab
                    tabControl1.TabPages.Add("U: " + cboItems.Text.Substring(0, cboItems.Text.IndexOf("(") - 1));
                    // add a new textbox to that tab
                    TextBox txtNew = new TextBox();
                    txtNew.Location = new System.Drawing.Point(3, 6);
                    txtNew.Multiline = true;
                    txtNew.Name = "txtResult" + (tabControl1.TabPages.Count - 1).ToString();
                    txtNew.Size = new System.Drawing.Size(733, 487);
                    txtNew.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
                    tabControl1.TabPages[tabControl1.TabPages.Count - 1].Controls.Add(txtNew);
                    txt = txtNew;
                }
                else
                {
                    tabControl1.SelectedTab.Text = "U: " + cboItems.Text.Substring(0, cboItems.Text.IndexOf("(") - 1);
                }
                tabControl1.SelectedIndex = tabControl1.TabPages.Count - 1;
                txt.Text = _manu.getUses(cboItems.Text);
                
            }
            tabControl1.TabPages[tabControl1.TabPages.Count - 1].Select();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl1.TabPages.Count > 1)
            {
                tabControl1.TabPages.Remove(tabControl1.SelectedTab);
            }
            else
            {
                TextBox txt = findControl(tabControl1.SelectedTab);
                if(txt != null)
                    txt.Clear();
            }
        }

        private TextBox findControl(TabPage tabPage)
        {
            foreach (Control c in tabPage.Controls)
            {
                if (c.GetType() == typeof(TextBox))
                    return (TextBox)c;
            }
            return null;
        }

    }
}
