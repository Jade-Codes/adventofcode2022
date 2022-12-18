namespace Challenges
{
    public class Challenge18
    {
        public static void Part1(IEnumerable<string> lines)
        {
            var lavaPieces = new HashSet<Lava>();

            foreach (var line in lines)
            {
                var coordinates = line.Split(',').Select(Int32.Parse).ToArray();
                lavaPieces.Add(new Lava(coordinates[0], coordinates[1], coordinates[2]));
            }

            var lavaSurfaceArea = lavaPieces.Count() * 6 ;
            foreach (var lavaPiece in lavaPieces)
            {
                foreach(var sideLava in lavaPiece.GetSides()) 
                {
                    if (lavaPieces.Any(_ => _.Coordinates == (sideLava.x, sideLava.y, sideLava.z))) {
                        lavaSurfaceArea --;
                    }
                }
            }

            Console.WriteLine(lavaSurfaceArea);
        }

        public static void Part2(IEnumerable<string> lines)
        {
        }
    }

    public class Lava
    {
        private readonly int _x;
        private readonly int _y;
        private readonly int _z;

        public Lava(int x, int y, int z)
        {
            _x = x;
            _y = y;
            _z = z;
        }

        public (int x, int y, int z) Coordinates => (_x, _y, _z);

        public HashSet<(int x, int y, int z)> GetSides()
        {
            return new HashSet<(int x, int y, int z)>
            {
                (_x+1, _y, _z),
                (_x-1, _y, _z),
                (_x, _y+1, _z),
                (_x, _y-1, _z),
                (_x, _y, _z+1),
                (_x, _y, _z-1),
            };
        }

    }
}