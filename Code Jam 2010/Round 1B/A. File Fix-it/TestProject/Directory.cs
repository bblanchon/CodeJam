using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestProject
{
    class Directory
    {
        public int Mkdir(string path)
        {
            var createCount = 0;
            
            var firstSlashIndex = path.IndexOf('/');
            
            var directory = firstSlashIndex>0 ? path.Substring(0, firstSlashIndex) : path;
            var subdirs = firstSlashIndex>0 ?path.Substring(firstSlashIndex+1) : null;

            if (!children.ContainsKey(directory))
            {
                children.Add(directory, new Directory());
                createCount++;
            }

            if( !string.IsNullOrEmpty(subdirs))
            {
                createCount += children[directory].Mkdir(subdirs);
            }

            return createCount;
        }

        private Dictionary<string, Directory> children = new Dictionary<string, Directory>();
    }
}
