using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Treasure
{
    class Strategy
    {
        readonly int[] keysInHand;
        readonly Chest[] openedChests;
        readonly Chest[] closedChests;

        public Strategy(IEnumerable<int> keysInHand, IEnumerable<Chest> openedChests, IEnumerable<Chest> closedChests)
        {            
            this.keysInHand = keysInHand.ToArray();
            this.openedChests = openedChests.ToArray();
            this.closedChests = closedChests.ToArray();
        }

        public IEnumerable<Chest> OpenedChests
        {
            get { return openedChests; }
        }

        public IEnumerable<int> KeysInHand
        {
            get { return keysInHand; }
        }

        public IEnumerable<Chest> ClosedChests
        {
            get { return closedChests; }
        }

        public Strategy OpenChest(Chest chest)
        {            
            var keyIndex = Array.IndexOf(keysInHand, chest.RequiredKey);
            Debug.Assert(keyIndex >= 0);

            var newKeysInHand = 
                keysInHand
                .Where((key,idx) => idx != keyIndex)
                .Concat(chest.ContainedKeys);
            Debug.Assert(newKeysInHand.Count() == keysInHand.Count() - 1 + chest.ContainedKeys.Count());

            var newOpenedChests = 
                openedChests.Concat(Enumerable.Repeat(chest, 1));
            Debug.Assert(newOpenedChests.Count() == openedChests.Count() + 1);

            var newClosedChests =
                closedChests.Where(x=>x != chest);
            Debug.Assert(newClosedChests.Count() == closedChests.Count() - 1);         
            
            return new Strategy(newKeysInHand, newOpenedChests, newClosedChests);
        }

        public bool ContainsEnoughKeys
        {
            get
            {
                var keyCountByType = new Dictionary<int, int>();

                // available keys
                foreach (var keyType in ClosedChests.SelectMany(x => x.ContainedKeys).Concat(keysInHand))
                {
                    if (keyCountByType.ContainsKey(keyType))
                        keyCountByType[keyType]++;
                    else
                        keyCountByType.Add(keyType, 1);
                }

                // required keys
                foreach (var keyType in ClosedChests.Select(x => x.RequiredKey))
                {
                    if (keyCountByType.ContainsKey(keyType))
                        keyCountByType[keyType]--;
                    else
                        keyCountByType.Add(keyType, -1);
                }

                return keyCountByType.Values.All(x => x >= 0);
            }
        }

        public bool CanReachAnyChest
        {
            get
            {
                var scannedChests = new HashSet<Chest>();
                foreach( var chest in ClosedChests.Where(x=>keysInHand.Contains(x.RequiredKey)))
                    RecursivelyOpenChests(scannedChests, chest);

                return scannedChests.Count == closedChests.Length;;
            }
        }

        private void RecursivelyOpenChests(HashSet<Chest> scannedChests, Chest chest)
        {
            if (scannedChests.Contains(chest)) return;

            scannedChests.Add(chest);

            var openableChests = ClosedChests.Where(x=>chest.ContainedKeys.Contains(x.RequiredKey));

            foreach (var nextChest in openableChests)
                RecursivelyOpenChests(scannedChests, nextChest);
        }
    }
}
