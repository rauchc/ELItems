using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace ELItems
{
    class Manufacture
    {
        private const string _URL = "http://eternal-lands.com/manufacture.htm";

        private const string _FILENAME = "manufacture.htm";

        private List<ELItem> _allItems = null;

        internal List<ELItem> AllItems
        {
            get { return _allItems; }
            set { _allItems = value; }
        }

        private Dictionary<string, ingred> _summary = null;

        public Dictionary<string, ingred> Summary
        {
            get { return _summary; }
            set { }
        }

        private int food = 0;

        public int Food
        {
            get { return food; }
            set { }
        }

        private ComboBox _iCbo;

        public Manufacture(ref ComboBox cbo)
        {
            _allItems = new List<ELItem>();
            _summary = new Dictionary<string,ingred>();
            food = 0;
            _iCbo = cbo;
        }

        public Manufacture()
        {
            _allItems = new List<ELItem>();
            _summary = new Dictionary<string, ingred>();
            food = 0;
        }


        public string getIngreds(string name, int amount, int iteration)
        {
            if (iteration == 1)
            {
                _summary.Clear();
                food = 0;
            }

            string result = null;
            string intendation = null;
            for (int i = 0; i < iteration; i++)
            {
                intendation = intendation + "\t";
            }
            try
            {
                for (int i = 0; i < _allItems.Count; i++)
                {
                    if (_allItems[i].Name == name)
                    {
                        food += _allItems[i].Food * amount;
                        // found the right one
                        if (_allItems[i].Ingreds != null && _allItems[i].Ingreds.Count > 0)
                        {
                            // item does have ingreds?
                            foreach (ingred ing in _allItems[i].Ingreds)
                            {
                                result = result + intendation + (ing.Amount * amount).ToString() + " " + ing.Name + Environment.NewLine;
                                // try finding ingred in main list
                                for (int j = 0; j < _allItems.Count; j++)
                                {
                                    if (_allItems[j].Name == ing.Name)
                                    {
                                        if (_allItems[j].ByLuckOnly == true)
                                        {
                                            result = result + intendation + "\tMade by luck only." + Environment.NewLine;
                                        }
                                        break;
                                    }
                                } 
                                string _subIngs = getIngreds(ing.Name, amount * ing.Amount, iteration + 1);
                                if (_subIngs == null)
                                {
                                    ingred ing2 = ing;
                                    if (_summary.ContainsKey(ing2.Name))
                                    {
                                        ing2.Amount = ing2.Amount * amount;
                                        ing2.Amount += _summary[ing.Name].Amount;
                                        _summary[ing2.Name] = ing2;
                                    }
                                    else
                                    {
                                        ing2.Amount = ing2.Amount * amount;
                                        _summary.Add(ing2.Name, ing2);
                                    }
                                }
                                result = result + _subIngs;
                            }
                            return result;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
            return null;
        }

        public string getUses(string name)
        {
            string result = null;
            ELItem _Uses = null;
            int _amounts = 0;

            foreach (ELItem _item in this.AllItems)
            {
                if (_item.Name == name)
                {
                    _Uses = _item;
                }
                if (_item.Ingreds != null)
                {
                    foreach (ingred _i in _item.Ingreds)
                    {
                        if (_i.Name.Equals(name.Substring(0, name.IndexOf("(") - 1)))
                        {
                            _amounts++;
                            result = result + _item.Name + Environment.NewLine;
                        }
                    }
                }
            }
            if (_Uses != null)
            {
                if (_Uses.ByLuckOnly == true)
                {
                    result = result + Environment.NewLine +  "\tMade by luck only." + Environment.NewLine;
                }
            }

            if (_amounts == 0)
            {
                result = "Item " + name.Substring(0, name.IndexOf("(") - 1) + " is not used in any recipe." + Environment.NewLine + Environment.NewLine + result;
            }
            else if (_amounts == 1)
            {
                result = "Item " + name.Substring(0, name.IndexOf("(") - 1) + " is used in the following recipe:" + Environment.NewLine + Environment.NewLine + result;
            }
            else
            {
                result = "Item " + name.Substring(0, name.IndexOf("(") - 1) + " is used in the following " + _amounts.ToString() + " recipes:" + Environment.NewLine + Environment.NewLine + result;
            }

            return result;
        }


        public void readManufacture()
        {
            if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + Path.DirectorySeparatorChar + _FILENAME))
            {
                FileInfo _fi = new FileInfo(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + Path.DirectorySeparatorChar + _FILENAME);
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(_URL);
                req.Method = "HEAD";
                HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                DateTime _remoteDate = res.LastModified;
                if (_remoteDate > _fi.LastWriteTime)
                {
                    if (MessageBox.Show("Your " + _FILENAME + " is outdated, do you want to update it?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                    {
                        downloadManufacture();
                    }
                    else
                    {
                        parseManufacture();
                    }
                }
                else
                {
                    parseManufacture();
                }
            }
            else
            {
                downloadManufacture();
            }
        }

        public void downloadManufacture()
        {
            WebClient webClient = new WebClient();
            webClient.DownloadFile(_URL, Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + Path.DirectorySeparatorChar + _FILENAME);
            parseManufacture();
        }

        /// <summary>
        /// parses the manufacture.htm and adds the items to the cbo
        /// </summary>
        public void parseManufacture()
        {
            // clear cbo
            if(_iCbo != null)
                _iCbo.Items.Clear();
            string _manFile;
            string[] _split1 = new string[1];
            _split1[0] = "<br><br><br>";
            string[] _split2 = new string[1];
            _split2[0] = "<br>";
            _allItems = new List<ELItem>();
            try
            {
                if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + Path.DirectorySeparatorChar + _FILENAME))
                {
                    StreamReader sr = new StreamReader(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + Path.DirectorySeparatorChar + _FILENAME);
                    _manFile = sr.ReadToEnd();

                    string[] _items = _manFile.Split(_split1, StringSplitOptions.RemoveEmptyEntries);

                    int i = 0;
                    foreach (string _i in _items)
                    {
                        ELItem item = new ELItem();
                        ingred ing;
                        // extract name
                        item.Name = (_i.Substring(_i.IndexOf("<b>") + 3, _i.IndexOf("</b>") - 3)).Trim();
                        // extract amount
                        item.Amount = Convert.ToInt32((item.Name.Substring(item.Name.IndexOf('(') + 1, item.Name.IndexOf(')') - item.Name.IndexOf('(') - 1).Trim()));
                        // remove amount from name
                        item.Name = item.Name.Substring(0, item.Name.LastIndexOf('(') - 1);

                        // get food amount
                        try
                        {
                            string _tmpFood = _i.Substring(_i.IndexOf("<b>Food subtracted:</b>"));

                            _tmpFood = _tmpFood.Replace("<b>Food subtracted:</b>", "");
                            //txtDebug.AppendText(_tmpFood + Environment.NewLine);
                            item.Food = Int32.Parse(_tmpFood);
                        }
                        catch (System.ArgumentOutOfRangeException)
                        {
                            item.Food = 0;
                        }
                        // get ingreds
                        // <b>Items needed:</b><br>
                        string _tmpIngs = _i.Substring(_i.IndexOf("<b>Items needed:</b><br>") + 24);
                        _tmpIngs = _tmpIngs.Substring(0, _tmpIngs.IndexOf("<b>"));
                        string[] _ings = _tmpIngs.Split(_split2, StringSplitOptions.RemoveEmptyEntries);

                        item.Ingreds = new List<ingred>();

                        foreach (string _ing in _ings)
                        {
                            ing = new ingred();
                            ing.Amount = Convert.ToInt32(_ing.Substring(0, _ing.IndexOf(' ')).Trim());
                            ing.Name = _ing.Substring(_ing.IndexOf(' ')).Trim();
                            if (ing.Amount > 1)
                                ing.Name = ing.Name.Substring(0, ing.Name.Length - 1);
                            item.Ingreds.Add(ing);
                        }
                        // add action point requirements for the big books
                        switch (item.Name)
                        {
                            case "Big Book of Manufacturing":
                                ing = new ingred();
                                ing.Amount = 250;
                                ing.Name = "Action Points";
                                item.Ingreds.Add(ing);
                                break;
                            case "Big Book of Tailoring":
                                ing = new ingred();
                                ing.Amount = 450;
                                ing.Name = "Action Points";
                                item.Ingreds.Add(ing);
                                break;
                            case "Big Book of Crafting":
                                ing = new ingred();
                                ing.Amount = 200;
                                ing.Name = "Action Points";
                                item.Ingreds.Add(ing);
                                break;
                            default:
                                break;
                        }
                        _allItems.Add(item);
                        i++;
                    }

                    // create a few hard coded items not in the list
                    ingred _manualIng;
                    List<ingred> _ManualIngreds;
                    // hydrogenium ore
                    _ManualIngreds = new List<ingred>();
                    _manualIng = new ingred();
                    _manualIng.Amount = 1;
                    _manualIng.Name = "Steel Two Edged Sword";
                    _ManualIngreds.Add(_manualIng);
                    _allItems.Add(new ELItem("Hydrogenium Ore", 1, _ManualIngreds, false));
                    // seridium ore
                    _ManualIngreds = new List<ingred>();
                    _manualIng = new ingred();
                    _manualIng.Amount = 1;
                    _manualIng.Name = "Matter Conglomerate";
                    _ManualIngreds.Add(_manualIng);
                    _allItems.Add(new ELItem("Seridium Ore", 1, _ManualIngreds, false));
                    // wolfram ore
                    _ManualIngreds = new List<ingred>();
                    _manualIng = new ingred();
                    _manualIng.Amount = 1;
                    _manualIng.Name = "Steel Long Sword";
                    _ManualIngreds.Add(_manualIng);
                    _allItems.Add(new ELItem("Wolframite", 1, _ManualIngreds, false));
                    // tin ore
                    _ManualIngreds = new List<ingred>();
                    _manualIng = new ingred();
                    _manualIng.Amount = 1;
                    _manualIng.Name = "Iron Sword";
                    _ManualIngreds.Add(_manualIng);
                    _allItems.Add(new ELItem("Tin Ore", 1, _ManualIngreds, false));
                    // copper ore
                    _ManualIngreds = new List<ingred>();
                    _manualIng = new ingred();
                    _manualIng.Amount = 1;
                    _manualIng.Name = "Matter Conglomerate";
                    _ManualIngreds.Add(_manualIng);
                    _allItems.Add(new ELItem("Copper Ore", 1, _ManualIngreds, false));
                    // dvarium ore
                    _ManualIngreds = new List<ingred>();
                    _manualIng = new ingred();
                    _manualIng.Amount = 1;
                    _manualIng.Name = "Iron Broad Sword";
                    _ManualIngreds.Add(_manualIng);
                    _allItems.Add(new ELItem("Dvarium Ore", 1, _ManualIngreds, false));
                    // amber
                    _ManualIngreds = new List<ingred>();
                    _manualIng = new ingred();
                    _manualIng.Amount = 30;
                    _manualIng.Name = "Action Points";
                    _ManualIngreds.Add(_manualIng);
                    _allItems.Add(new ELItem("Amber",1,_ManualIngreds,false));
                    // enriched water essence
                    _allItems.Add(new ELItem("Enriched Water Essence", 1, true));
                    // enriched energy essence
                    _allItems.Add(new ELItem("Enriched Energy Essence", 1, true));
                    // enriched death essence
                    _allItems.Add(new ELItem("Enriched Death Essence", 1, true));
                    // modable swords
                    // Modable Iron Sword
                    _allItems.Add(new ELItem("Modable Iron Sword", 1, true));
                    // Modable Steel Long Sword
                    _allItems.Add(new ELItem("Modable Steel Long Sword", 1, true));
                    // Modable Steel Two Edged Sword
                    _allItems.Add(new ELItem("Modable Steel Two Edged Sword", 1, true));
                    // Modable Titanium/Steel Alloy Short Sword
                    _allItems.Add(new ELItem("Modable Titanium/Steel Alloy Short Sword", 1, true));
                    // add items to GUI
                    if (_iCbo != null)
                    {
                        for (int ii = 0; ii < _allItems.Count; ii++)
                        {
                            _iCbo.Items.Add(_allItems[ii].Name + " (" + _allItems[ii].Amount + ")");
                        }
                    }
                }
                else
                {
                    downloadManufacture();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

    }
}
