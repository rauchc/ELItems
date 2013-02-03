using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ELItems
{
    class ELItem
    {
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        private int _amount = 1;

        public int Amount
        {
            get { return _amount; }
            set { _amount = value; }
        }


        private int _food = 0;

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
            _name = name;
            _amount = amount;
        }
    }

    public struct ingred
    {
        public int Amount;
        public string Name;
    }

}
