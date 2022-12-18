namespace Challenges
{
    public static class Challenge18
    {
        public static void Part1(IEnumerable<string> lines)
        {
            var lavaPieces = new HashSet<Lava>();
            var lavaSurfaceArea = lavaPieces.Count() * 6;

            foreach (var line in lines)
            {
                var coordinates = line.Split(',').Select(Int32.Parse).ToArray();
                lavaPieces.Add(new Lava(coordinates[0], coordinates[1], coordinates[2]));
            }

            foreach (var lavaPiece in lavaPieces)
            {
                foreach (var sideLava in lavaPiece.AdjacentLavaPieces)
                {
                    var sideLavaPiece = new Lava(sideLava.x, sideLava.y, sideLava.z);
                    if (lavaPieces.Any(_ => _.Coordinates == sideLavaPiece.Coordinates))
                    {
                        lavaSurfaceArea--;
                    }
                }
            }

            Console.WriteLine(lavaSurfaceArea);
        }

        public static void Part2(IEnumerable<string> lines)
        {
            var lavaPieces = new HashSet<Lava>();
            var searchedLava = new HashSet<Lava>();
            var unsearchedLava = new Queue<Lava>();
            var lavaSurfaceArea = 0;

            foreach (var line in lines)
            {
                var coordinates = line.Split(',').Select(Int32.Parse).ToArray();
                lavaPieces.Add(new Lava(coordinates[0], coordinates[1], coordinates[2]));
            }

            // Get minimum and maximum possible lava points
            var minLava = new Lava(lavaPieces.Min(_ => _.Coordinates.x) - 1, lavaPieces.Min(_ => _.Coordinates.y) - 1, lavaPieces.Min(_ => _.Coordinates.z) - 1);
            var maxLava = new Lava(lavaPieces.Max(_ => _.Coordinates.x) + 1, lavaPieces.Max(_ => _.Coordinates.y) + 1, lavaPieces.Max(_ => _.Coordinates.z) + 1);
            unsearchedLava.Enqueue(minLava);

            // While unsearched lava points, determine if lava can move above, to the side or below lava levels, repeat until all lava pieces have been checked
            while (unsearchedLava.TryDequeue(out var current))
            {
                if (!current.IsLessThan(minLava) &&
                    !current.IsGreaterThan(maxLava) &&
                    !searchedLava.Any(_ => _.Coordinates == current.Coordinates))
                {
                    searchedLava.Add(current);

                    foreach (var sideLava in current.AdjacentLavaPieces)
                    {
                        var sideLavaPiece = new Lava(sideLava.x, sideLava.y, sideLava.z);
                        if (lavaPieces.Any(_ => _.Coordinates == sideLavaPiece.Coordinates))
                        {
                            lavaSurfaceArea++;
                        }
                        else
                        {
                            unsearchedLava.Enqueue(sideLavaPiece);
                        }
                    }
                }
            }

            Console.WriteLine(lavaSurfaceArea);
        }

        public static bool IsGreaterThan(this Lava firstLava, Lava secondLava)
        {
            return firstLava.Coordinates.x > secondLava.Coordinates.x ||
                firstLava.Coordinates.y > secondLava.Coordinates.y ||
                firstLava.Coordinates.z > secondLava.Coordinates.z;
        }

        public static bool IsLessThan(this Lava firstLava, Lava secondLava)
        {
            return firstLava.Coordinates.x < secondLava.Coordinates.x ||
                firstLava.Coordinates.y < secondLava.Coordinates.y ||
                firstLava.Coordinates.z < secondLava.Coordinates.z;
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

        public HashSet<(int x, int y, int z)> AdjacentLavaPieces =>
            new HashSet<(int x, int y, int z)>
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