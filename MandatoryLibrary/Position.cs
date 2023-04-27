using System.Xml;

namespace MandatoryLibrary
{
    public class Position
    {
        public int PositionX { get; set; }
        public int PositionY { get; set; }
        public Random random { get; set; } = new Random();


        public Position(int positionX, int positionY)
        {
            XmlDocument configDoc = new XmlDocument();
            string _path = Environment.GetEnvironmentVariable("FrameWorkConfig");
            configDoc.Load(_path); 
            var world = configDoc.DocumentElement.SelectSingleNode("Playground");
            int maxX = Convert.ToInt32(world.SelectSingleNode("MaxX").InnerText);
            int maxY = Convert.ToInt32(world.SelectSingleNode("MaxY").InnerText);


            PositionX = positionX > maxX ? maxX : positionX;
            PositionY = positionY > maxY ? maxY : positionY;
        }

        public override string ToString()
        {
            return $"{{{nameof(PositionX)}={PositionX.ToString()}, {nameof(PositionY)}={PositionY.ToString()}, {nameof(random)}={random}}}";
        }

        public override bool Equals(object? obj)
        {
            return obj is Position position &&
                   PositionX == position.PositionX &&
                   PositionY == position.PositionY;
        }
    }
}