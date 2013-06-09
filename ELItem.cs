using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ELItems
{
    class ELItem
    {
        private bool _byLuckOnly = false;
        /// <summary>
        /// indicates if this item can be made by luck only
        /// </summary>
        public bool ByLuckOnly
        {
            get { return _byLuckOnly; }
            set { _byLuckOnly = value; }
        }

        private string _name;
        /// <summary>
        /// The name as used ingame
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        private int _amount = 1;
        /// <summary>
        /// the amount of items created by the recipe
        /// </summary>
        public int Amount
        {
            get { return _amount; }
            set { _amount = value; }
        }

        private int _food = 0;
        /// <summary>
        /// 
        /// </summary>
        public int Food
        {
            get { return _food; }
            set { _food = value; }
        }


        private List<ingred> _ingreds = null;

        internal List<ingred> Ingreds
        {
            get { return _ingreds; }
            set { _ingreds = value; }
        }

        public ELItem()
        {

        }

        public ELItem(string name, int amount)
        {
            this._name = name;
            this._amount = amount;
        }

        public ELItem(string name, int amount, bool byLuckOnly)
        {
            this._name = name;
            this._amount = amount;
            this._byLuckOnly = byLuckOnly;
        }

        public ELItem(string name, int amount, List<ingred> ingreds, bool byLuckOnly)
        {
            this._name = name;
            this._amount = amount;
            this._byLuckOnly = byLuckOnly;
            this._ingreds = ingreds;
        }
        

    }

    public struct ingred
    {
        public int Amount;
        public string Name;
    }

}
