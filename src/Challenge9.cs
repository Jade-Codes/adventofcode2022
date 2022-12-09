namespace Challenges
{
    public class Challenge9
    {
        public static void Part1(IEnumerable<string> lines)
        {
            var dictCoords = new Dictionary<int, (int, int)>{
                {0, (0, 0)},
                {9, (0, 0)}
            };
            var tCoordsHistory = new List<(int, int)>();

            foreach (var line in lines)
            {
                var splitLine = line.Split(' ');
                var isNumber = Int32.TryParse(splitLine[1], out var number);

                GetCoords(splitLine[0], number, 0, 9, ref dictCoords, ref tCoordsHistory);
            }

            Console.WriteLine(tCoordsHistory.Distinct().Count());
        }

        public static void Part2(IEnumerable<string> lines)
        {
            var dictCoords = new Dictionary<int, (int, int)>{
                {0, (0, 0)},
                {1, (0, 0)},
                {2, (0, 0)},
                {3, (0, 0)},
                {4, (0, 0)},
                {5, (0, 0)},
                {6, (0, 0)},
                {7, (0, 0)},
                {8, (0, 0)},
                {9, (0, 0)}
            };
            var tCoordsHistory = new List<(int, int)>();

            foreach (var line in lines)
            {
                var splitLine = line.Split(' ');
                var isNumber = Int32.TryParse(splitLine[1], out var number);
                
                var tempCoords = new Dictionary<int, (int,int)>( dictCoords);
                
                for(var i=0; i<dictCoords.Count-1; i++){
                    GetCoords(splitLine[0], number, i, i + 1, ref dictCoords, ref tCoordsHistory);

                    var tempCoord = tempCoords[i+1];
                    var dictCoord = dictCoords[i+1];
                    if(tempCoord == dictCoord) {
                       break;
                    }
                    tempCoords = new Dictionary<int, (int,int)>( dictCoords);
                }
            }

            Console.WriteLine(tCoordsHistory.Distinct().Count());
        }

        private static void GetCoords(string direction, int number, int hKey, int tKey, ref Dictionary<int, (int, int)> dictCoords, ref List<(int,int)> tCoordsHistory)
        { 
            for (var i = 0; i < number; i++)
            {
                var hCoords = dictCoords[hKey];
                var tCoords = dictCoords[tKey];
                switch (direction)
                {
                    case "L":
                        if (hCoords.Item2 >= tCoords.Item2)
                        {
                            hCoords.Item2--;
                        }
                        else if (hCoords.Item1 != tCoords.Item1)
                        {
                            tCoords.Item1 = hCoords.Item1;
                            hCoords.Item2--;
                            tCoords.Item2--;
                        }
                        else
                        {
                            hCoords.Item2--;
                            tCoords.Item2--;
                        }
                        break;
                    case "R":
                        if (hCoords.Item2 <= tCoords.Item2)
                        {
                            hCoords.Item2++;
                        }
                        else if (hCoords.Item1 != tCoords.Item1)
                        {
                            tCoords.Item1 = hCoords.Item1;
                            hCoords.Item2++;
                            tCoords.Item2++;
                        }
                        else
                        {
                            hCoords.Item2++;
                            tCoords.Item2++;
                        }
                        break;
                    case "U":
                        if (hCoords.Item1 >= tCoords.Item1)
                        {
                            hCoords.Item1--;
                        }
                        else if (hCoords.Item2 != tCoords.Item2)
                        {
                            tCoords.Item2 = hCoords.Item2;
                            hCoords.Item1--;
                            tCoords.Item1--;
                        }
                        else
                        {
                            hCoords.Item1--;
                            tCoords.Item1--;
                        }
                        break;
                    case "D":
                        if (hCoords.Item1 <= tCoords.Item1)
                        {
                            hCoords.Item1++;
                        }
                        else if (hCoords.Item2 != tCoords.Item2)
                        {
                            tCoords.Item2 = hCoords.Item2;
                            hCoords.Item1++;
                            tCoords.Item1++;
                        }
                        else
                        {
                            hCoords.Item1++;
                            tCoords.Item1++;
                        }
                        break;
                    default:
                        break;
                }

                dictCoords[hKey] = hCoords;
                dictCoords[tKey] = tCoords;
                
                if(tKey == 9) {
                    tCoordsHistory.Add(tCoords);
                }
            }

        }
    }
}