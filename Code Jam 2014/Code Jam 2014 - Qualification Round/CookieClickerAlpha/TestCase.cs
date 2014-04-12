// Copyright © Benoit Blanchon 2014
// All Rights Reserved

namespace CookieClickerAlpha
{
    class TestCase
    {
        public double C { get; set; }
        public double F { get; set; }
        public double X { get; set; }

        public object Solve()
        {
            var now = 0.0;
            var incomes = 2.0;

            while (true)
            {
                if (!ShouldBuyFarm(incomes))
                    return now + X/incomes;

                now += C/incomes;
                incomes += F;
            }
        }
        
        bool ShouldBuyFarm(double incomes)
        {
            var timeWithoutFarm = X / incomes;
            var timeToGetFarm = C / incomes;
            var incomesWithFarm = incomes + F;
            var timeWithOneFarm = timeToGetFarm + X / incomesWithFarm;

            return timeWithOneFarm < timeWithoutFarm;
        }
    }
}
