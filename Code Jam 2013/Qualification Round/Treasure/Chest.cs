using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Treasure
{
    class Chest
    {
        readonly int id;
        readonly int requiredKey;
        readonly int[] containedKeys;

        public Chest(int id, int requiredKey, IEnumerable<int> containedKeys)
        {
            Debug.Assert(id >= 1);
            Debug.Assert(requiredKey >= 1);
            Debug.Assert(containedKeys != null);

            this.id = id;
            this.requiredKey = requiredKey;
            this.containedKeys = containedKeys.ToArray();
        }

        public int Id
        {
            get { return id; }
        }

        public int RequiredKey
        {
            get { return requiredKey; }
        }

        public IEnumerable<int> ContainedKeys 
        {
            get { return containedKeys; }
        }
    }
}
