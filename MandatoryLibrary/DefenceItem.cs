using MandatoryLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MandatoryLibrary
{
    public class DefenceItem : Item, IDefenceItem
    {
        public int DefenceValue { get; set; }

        public DefenceItem(string name, int defenceValue) : base(name)
        {
            DefenceValue = defenceValue;
        }

        public override string ToString()
        {
            return $"{{{nameof(DefenceValue)}={DefenceValue.ToString()}, {nameof(Name)}={Name}}}";
        }
    }
}
