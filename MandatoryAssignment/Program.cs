using MandatoryLibrary;

World world = World.Instance();
world.PrintWorld();

while (world.Hero.IsAlive && world.Creatures.Count != 0)
{
    ConsoleKey c = Console.ReadKey().Key;
    world.MoveHero(world.CharToNumber(c));
    world.PrintWorld();
    Console.WriteLine(world.Creatures.Count);
    world.PositionsList.ForEach(pos => Console.WriteLine(pos));
}
string victoryString = world.Hero.IsAlive ? "You won!" : "You lost!";
Log.GetLogger().CloseLogger();
Console.WriteLine(victoryString);
