using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MandatoryLibrary
{
    public class World
    {
        private readonly String horizontalLine = "";
        private static World _world;
        public int MaxX { get; set; }
        public int MaxY { get; set; }
        public Creature Hero { get; set; }
        public List<Creature> Creatures { get; set; }
        public List<Position> DeadCreaturePositions { get; set; } = new List<Position>();
        public List<Position> PositionsList { get; set; } = new List<Position>();
        private Log _log = Log.GetLogger();

        private World InitializeWorld()
        {
            XmlDocument configDoc = new XmlDocument();
            string _path = Environment.GetEnvironmentVariable("FrameWorkConfig");
            configDoc.Load(_path);
            var world = configDoc.DocumentElement.SelectSingleNode("Playground");
            int maxX = Convert.ToInt32(world.SelectSingleNode("MaxX").InnerText);
            int maxY = Convert.ToInt32(world.SelectSingleNode("MaxY").InnerText);
            int healthMod = Convert.ToInt32(world.SelectSingleNode("HeroHealthModifier").InnerText);
            int amountOfCreatures = Convert.ToInt32(world.SelectSingleNode("AmountOfCreatures").InnerText);
            this.MaxX = maxX;
            this.MaxY = maxY;
            Creatures = new List<Creature>();
            for (int i = 0; i < amountOfCreatures; i++)
            {
                Creature theCreature = new Creature(maxX, maxY) { Name = "Creature" + i };
                while (PositionsList.Contains(theCreature.Position))
                {
                    theCreature.ReRollPosition(maxX, maxY);
                }
                Creatures.Add(theCreature);
                PositionsList.Add(theCreature.Position);
            }
            var rnd = new Random();
            Hero = Creatures[rnd.Next(0, Creatures.Count)];
            Hero.HitPoints *= healthMod;
            Hero.Name = "Hero";
            Creatures.Remove(Hero);
            return this;
        }

        public static World Instance()
        {
            return _world ?? new World();
        }

        private World()
        {
            _world = InitializeWorld();
            for (int i = 0; i < _world.MaxX+2; i++)
            {
                horizontalLine += "-";
            }
        }

        public void PrintWorld()
        {
            Console.Clear();
            Console.WriteLine("Fight game: ");
            Console.WriteLine(horizontalLine);
            for (int i = 0; i < _world.MaxY; i++)
            {
                Console.Write("|");
                PrintRowString(i);
                Console.WriteLine($"|");
            }
            Console.WriteLine(horizontalLine);
            Fight();
        }

        private void PrintRowString(int r)
        {
            StringBuilder sb = new StringBuilder();
            for (int c = 0; c < _world.MaxX; c++)
            {
                PrintColRowChar(r, c);
            }
        }

        private void PrintColRowChar(int row, int col)
        {
            Position p = new Position(col, row);

            var positions = Creatures.Select(it => it.Position).ToList();
            positions.Add(Hero.Position);

            bool isMatch = false;
            bool isDeathMatch = false;

            foreach (var position in positions)
            {
                if (position.Equals(p))
                {
                    isMatch = true;
                    break;
                }
            }
            foreach (var position in DeadCreaturePositions)
            {
                if (position.Equals(p))
                {
                    isDeathMatch = true;
                    break;
                }
            }
            if (isMatch && Hero.Position.Equals(p))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write('X');
                Console.ForegroundColor = ConsoleColor.White;
            }
            else if (isMatch)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write('X');
                Console.ForegroundColor = ConsoleColor.White;
            }
            else if (isDeathMatch)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write('D');
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write('X');
            }
        }

        public void MoveHero(int input)
        {
            switch (input)
            {
                case 0:
                    if (Hero.Position.PositionY != 0)
                        Hero.Position.PositionY--;
                    break;
                case 1:
                    if (Hero.Position.PositionX != MaxX - 1)
                        Hero.Position.PositionX++;
                    break;
                case 2:
                    if (Hero.Position.PositionY != MaxY - 1)
                        Hero.Position.PositionY++;
                    break;
                case 3:
                    if (Hero.Position.PositionX != 0)
                        Hero.Position.PositionX--;
                    break;
            }
        }

        public int CharToNumber(ConsoleKey c)
        {
            switch (c)
            {
                case ConsoleKey.W: return 0;
                case ConsoleKey.UpArrow: return 0;
                case ConsoleKey.D: return 1;
                case ConsoleKey.RightArrow: return 1;
                case ConsoleKey.S: return 2;
                case ConsoleKey.DownArrow: return 2;
                case ConsoleKey.A: return 3;
                case ConsoleKey.LeftArrow: return 3;
                default: return 4;
            }
        }

        public void Fight()
        {
            var creatureToFight = Creatures.FirstOrDefault(creature => creature.Position.Equals(Hero.Position));
            if (creatureToFight != null)
            {
                while (creatureToFight.IsAlive && Hero.IsAlive)
                {
                    creatureToFight.RecieveHit(Hero.Hit());
                    if (!creatureToFight.IsAlive)
                    {
                        Creatures.Remove(creatureToFight);
                        DeadCreaturePositions.Add(creatureToFight.Position);
                    }
                    else
                    {
                        Hero.RecieveHit(creatureToFight.Hit());
                    }
                }
                string winner = Hero.IsAlive ? "Hero" : "Creature";
                _log.LogInfo(winner + " won!");
            }
        }

    }
}
