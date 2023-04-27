using MandatoryLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MandatoryLibrary.Decorator
{
    public class DamageDecorator
    {
        private IAttackItem _attackItem;
        public int Damage { get; set; }
        public string Name { get; set; }

        public DamageDecorator(IAttackItem item)
        {
            _attackItem = item;
            Damage = _attackItem.Damage;
            Name = _attackItem.Name;
        }

        public void Buff()
        {
            _attackItem.Damage *= 2;
        }
    }
}
