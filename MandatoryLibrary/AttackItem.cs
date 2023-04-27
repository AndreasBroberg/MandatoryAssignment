using MandatoryLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MandatoryLibrary
{
    public abstract class AttackItem : IAttackItem
    {
        public int Damage { get; set; }
        public string Name { get; set; }

        protected AttackItem(int damage, string name)
        {
            Damage = damage;
            Name = name;
        }

        protected AttackItem()
        {
        }

        public override string ToString()
        {
            return $"{{{nameof(Damage)}={Damage.ToString()}, {nameof(Name)}={Name}}}";
        }
    }
}
