using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace TestProject
{
    class TestCase
    {
        public TestCase(IList<string> existing, IList<string> toCreate)
        {
            ExistingDirectories = existing;
            DirectoriesToCreate = toCreate;
        }

        public readonly IList<string> ExistingDirectories;
        public readonly IList<string> DirectoriesToCreate;

        public override string ToString()
        {
            return string.Format("{0} existing directories, {1} to be created",
                ExistingDirectories.Count, DirectoriesToCreate.Count);
        }
    }
}
