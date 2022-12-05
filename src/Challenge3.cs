namespace Challenges
{
    public class Challenge3
    {
        public static Dictionary<char, int> NumbersMap = new Dictionary<char, int> {

                {'a', 1},
                {'b', 2},
                {'c', 3},
                {'d', 4},
                {'e', 5},
                {'f', 6},
                {'g', 7},
                {'h', 8},
                {'i', 9},
                {'j', 10},
                {'k', 11},
                {'l', 12},
                {'m', 13},
                {'n', 14},
                {'o', 15},
                {'p', 16},
                {'q', 17},
                {'r', 18},
                {'s', 19},
                {'t', 20},
                {'u', 21},
                {'v', 22},
                {'w', 23},
                {'x', 24},
                {'y', 25},
                {'z', 26},
                {'A', 27},
                {'B', 28},
                {'C', 29},
                {'D', 30},
                {'E', 31},
                {'F', 32},
                {'G', 33},
                {'H', 34},
                {'I', 35},
                {'J', 36},
                {'K', 37},
                {'L', 38},
                {'M', 39},
                {'N', 40},
                {'O', 41},
                {'P', 42},
                {'Q', 43},
                {'R', 44},
                {'S', 45},
                {'T', 46},
                {'U', 47},
                {'V', 48},
                {'W', 49},
                {'X', 50},
                {'Y', 51},
                {'Z', 52},
            };

        public static void Part1(IEnumerable<string> lines) 
        {
            var total = 0;

            foreach(var line in lines){
                
                var first = line.Substring(0, (int)(line.Length / 2));
                var last = line.Substring((int)(line.Length / 2), (int)(line.Length / 2));

                var common = first.Intersect(last);

                foreach(var currentChar in common){
                    total += NumbersMap.GetValueOrDefault(currentChar);
                }
            }

            Console.WriteLine(total);
        }


        public static void Part2(IEnumerable<string> lines) 
        {
            var total = 0;

            var arraySplice = lines.Chunk(3);

            foreach (var arrayList in arraySplice){

                var array = arrayList.ToArray();
                var commonFirstTwo = array[0].Intersect(array[1]);
                var commonAll = commonFirstTwo.Intersect(array[2]);

                foreach(var currentChar in commonAll){
                    total += NumbersMap.GetValueOrDefault(currentChar);
                }
            }

            Console.WriteLine(total);
        }
    }
}