using MandatoryLibrary.Factory;
using MandatoryLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MandatoryLibrary
{
    public class Creature
    {
        private Random _random { get; set; } = new Random();
        private Log _Log = Log.GetLogger();
        public int HitPoints { get; set; }
        public string Name { get; set; }
        public bool IsAlive
        {
            get
            {
                return HitPoints > 0;
            }
        }
        private int _damage
        {
            get { return AttackItems.Sum(dmg => dmg.Damage); }
        }


        public int Hit()
        {
            return _random.Next(_damage - 5, _damage);
        }

        public List<IAttackItem>  AttackItems { get; set; }
        public List<IDefenceItem> DefenceItems { get; set; }
        public Position Position { get; set; }

        public Creature(int maxY, int maxX)
        {
            XmlDocument configDoc = new XmlDocument();
            string _path = Environment.GetEnvironmentVariable("FrameWorkConfig");
            configDoc.Load(_path);
            var world = configDoc.DocumentElement.SelectSingleNode("Creature");

            HitPoints = Convert.ToInt32(world.SelectSingleNode("Hitpoints").InnerText);
            Position = new Position(_random.Next(0, maxX), _random.Next(0, maxY));
            AttackItems = GenerateAttackItems(configDoc);
            DefenceItems = GenerateDefenceItems(configDoc);
        }

        public void Loot(IItem item)
        {
            switch (item.GetType())
            {
                case (IAttackItem):
                    AttackItems.Add((AttackItem)item);
                    break;
                case (IDefenceItem): 
                    DefenceItems.Add((DefenceItem)item);
                    break;
            }
        }

        public int RecieveHit(int damage)
        {
            if(damage > 0 && IsAlive)
            {
                HitPoints -= damage;
            }
            _Log.LogInfo(Name + " was hit for: " + damage + " new health: " + HitPoints);
            return HitPoints;
        }

        public void ReRollPosition(int maxX, int maxY)
        {
            Position = new Position(_random.Next(0, maxX), _random.Next(0, maxY));
        }

        private List<IAttackItem> GenerateAttackItems(XmlDocument configDoc)
        {
            var world = configDoc.DocumentElement.SelectSingleNode("Creature");
            int maxAttackItems = Convert.ToInt32(world.SelectSingleNode("AttackItemLimit").InnerText);

            var attackItems = configDoc.DocumentElement.SelectSingleNode("Weapons").ChildNodes;
            var aItems = new List<IAttackItem>();
            for (int i= 0; i < attackItems.Count; i++)
            {
                var type = attackItems.Item(i).ChildNodes.Item(0).InnerText;
                var dmg = Convert.ToInt32(attackItems.Item(i).ChildNodes.Item(1).InnerText);
                var name = attackItems.Item(i).ChildNodes.Item(2).InnerText;
                aItems.Add(WeaponFactory.Create(type, dmg, name));
            }
            Random random = new Random();
            List<IAttackItem> items = new List<IAttackItem>();
            for (int i= 0; i < maxAttackItems; i++)
            {
                items.Add(aItems[random.Next(0, aItems.Count-1)]);
            }
            return items;
        }

        private List<IDefenceItem> GenerateDefenceItems(XmlDocument configDoc)
        {
            var world = configDoc.DocumentElement.SelectSingleNode("Creature");
            int maxDefenceItems = Convert.ToInt32(world.SelectSingleNode("DefenceItemLimit").InnerText);
            var defenceItems = configDoc.DocumentElement.SelectSingleNode("Armor").ChildNodes;
            var dItems = new List<IDefenceItem>();
            for (int i= 0; i < defenceItems.Count ; i++)
            {
                var name = defenceItems.Item(i).ChildNodes.Item(0).InnerText;
                var value = Convert.ToInt32(defenceItems.Item(i).ChildNodes.Item(1).InnerText);
                dItems.Add(new DefenceItem(name, value));
            }
            Random random = new Random();
            List<IDefenceItem> items = new List<IDefenceItem>();
            for (int i = 0; i < maxDefenceItems; i++)
            {
                items.Add(dItems[random.Next(0, dItems.Count-1)]);
            }
            return items;
        }

        public override string ToString()
        {
            return $"{{{nameof(HitPoints)}={HitPoints.ToString()}, {nameof(Name)}={Name}, {nameof(IsAlive)}={IsAlive.ToString()}, {nameof(AttackItems)}={AttackItems}, {nameof(DefenceItems)}={DefenceItems}, {nameof(Position)}={Position}}}";
        }
    }
}
