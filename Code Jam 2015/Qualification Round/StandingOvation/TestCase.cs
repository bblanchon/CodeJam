// Copyright © Benoit Blanchon 2015
// All Rights Reserved

namespace StandingOvation
{
    class TestCase
    {
        public int[] Audience;

        public object Solve()
        {
            var friendCount = 0;
            var standingPersonCount = 0;

            for (int shyness = 0; shyness < Audience.Length; shyness++)
            {
                if (Audience[shyness] == 0) continue;

                if (standingPersonCount < shyness)
                {
                    var additionalFriends = shyness - standingPersonCount;
                    friendCount += additionalFriends;
                    standingPersonCount += additionalFriends;
                }

                standingPersonCount += Audience[shyness];
            }

            return friendCount;
        }
    }
}
