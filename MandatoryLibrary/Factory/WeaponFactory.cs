using MandatoryLibrary.Interfaces;
using MandatoryLibrary.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace MandatoryLibrary.Factory
{
    public class WeaponFactory
    {
        public static IAttackItem Create(string type, int damage, string name)
        {
            switch (type)
            {
                case "Sword":
                    return new Sword(damage, name);
                case "Bow":
                    return new Bow(damage, name);
                case "Staff":
                    return new Staff(damage, name);
                default:
                    return null;
            }
        }
    }
}
