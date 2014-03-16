using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace KingdomRush
{
    class TestCase
    {
        readonly IReadOnlyList<Level> levels;        

        public TestCase(IReadOnlyList<Level> levels)
        {
            this.levels = levels;
        }

        public object Solve()
        {
            var levelsToComplete = new HashSet<Level>(levels);
            var levelsPartialyCompleted = new HashSet<Level>();
            int earnedStars = 0;
            int completionCount = 0;

            while (levelsToComplete.Any() || levelsPartialyCompleted.Any())
            {
                // 2-star in one shot
                {
                    var level = levelsToComplete.FirstOrDefault(x => x.RequiredFor2 <= earnedStars);

                    if (level != null)
                    {
                        levelsToComplete.Remove(level);
                        earnedStars += 2;
                        completionCount++;
                        continue;
                    }
                }

                // second star when the first is already earned
                {
                    var level = levelsPartialyCompleted.FirstOrDefault(x => x.RequiredFor2 <= earnedStars);

                    if (level != null)
                    {
                        levelsPartialyCompleted.Remove(level);
                        earnedStars++;
                        completionCount++;
                        continue;
                    }
                }

                // first star
                {
                    var level = levelsToComplete.Where(x => x.RequiredFor1 <= earnedStars).OrderByDescending(x=>x.RequiredFor2).FirstOrDefault();

                    if (level != null)
                    {
                        levelsToComplete.Remove(level);
                        levelsPartialyCompleted.Add(level);
                        earnedStars++;
                        completionCount++;
                        continue;
                    }
                }

                return "Too Bad";
            }

            return completionCount;
        }      
    }

    class Level
    {
        public Level(int a, int b)
        {
            RequiredFor1 = a;
            RequiredFor2 = b;
        }

        public int RequiredFor1 { get; private set; }
        public int RequiredFor2 { get; private set; }
    }
}
