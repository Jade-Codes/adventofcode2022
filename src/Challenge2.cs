namespace Challenges
{
    public class Challenge2
    {
        public static void Part1(IEnumerable<string> lines)
        {
            var scoreList = new List<int>();

            // Wins
            scoreList.Add(lines.Where(_ => _.Contains("A Y")).Count() * 8);
            scoreList.Add(lines.Where(_ => _.Contains("C X")).Count() * 7);
            scoreList.Add(lines.Where(_ => _.Contains("B Z")).Count() * 9);

            // Draws
            scoreList.Add(lines.Where(_ => _.Contains("B Y")).Count() * 5);
            scoreList.Add(lines.Where(_ => _.Contains("A X")).Count() * 4);
            scoreList.Add(lines.Where(_ => _.Contains("C Z")).Count() * 6);

            // Losses
            scoreList.Add(lines.Where(_ => _.Contains("C Y")).Count() * 2);
            scoreList.Add(lines.Where(_ => _.Contains("B X")).Count() * 1);
            scoreList.Add(lines.Where(_ => _.Contains("A Z")).Count() * 3);

            Console.WriteLine(scoreList.Sum());
        }


        public static void Part2(IEnumerable<string> lines) 
        {
            var scoreList = new List<int>();

            // Rock opponent
            scoreList.Add(lines.Where(_ => _.Contains("A Z")).Count() * 8);
            scoreList.Add(lines.Where(_ => _.Contains("A X")).Count() * 3);
            scoreList.Add(lines.Where(_ => _.Contains("A Y")).Count() * 4);

            // Paper opponent
            scoreList.Add(lines.Where(_ => _.Contains("B Z")).Count() * 9);
            scoreList.Add(lines.Where(_ => _.Contains("B X")).Count() * 1);
            scoreList.Add(lines.Where(_ => _.Contains("B Y")).Count() * 5);
            
            // Scissors opponent
            scoreList.Add(lines.Where(_ => _.Contains("C Z")).Count() * 7);
            scoreList.Add(lines.Where(_ => _.Contains("C X")).Count() * 2);
            scoreList.Add(lines.Where(_ => _.Contains("C Y")).Count() * 6);
            
            Console.WriteLine(scoreList.Sum());
        }
    }
}