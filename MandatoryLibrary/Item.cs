using MandatoryLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MandatoryLibrary
{
    public abstract class Item : IItem
    {
        public string Name { get; set; }

        protected Item(string name)
        {
            Name = name;
        }

        protected Item()
        {
        }
    }
}
