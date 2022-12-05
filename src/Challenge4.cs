namespace Challenges
{
    public class Challenge4
    {
        public static void Part1(IEnumerable<string> lines)
        {            
            var count = 0;

            foreach (var line in lines)
            {
                var array = line.Split(',');
                
                var numbers1 = array[0].Split('-').Select(Int32.Parse).ToList().ToArray();
                var numbers2 = array[1].Split('-').Select(Int32.Parse).ToList().ToArray();

                if(numbers1[0] <= numbers2[0] && numbers1[1] >= numbers2[1])
                {
                    count ++;
                }
                else if (numbers2[0] <= numbers1[0] && numbers2[1] >= numbers1[1])
                {
                    count ++;
                }

            }

            Console.WriteLine(count);
        }

        public static void Part2(IEnumerable<string> lines)
        {            
            var count = 0;

            foreach (var line in lines)
            {
                var array = line.Split(',');
                
                var numbers1 = array[0].Split('-').Select(Int32.Parse).ToList().ToArray();
                var numbers2 = array[1].Split('-').Select(Int32.Parse).ToList().ToArray();

                var sequence1 = Enumerable.Range(numbers1[0], numbers1[1]-numbers1[0]+1).ToArray();
                var sequence2 = Enumerable.Range(numbers2[0], numbers2[1]-numbers2[0]+1).ToArray(); 
                
                var intersect = sequence1.Intersect(sequence2);  

                if( intersect.Count() > 0){
                    count ++;
                }

            }

            Console.WriteLine(count);
        }

    }
}