using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace SpeakingInTongues
{
    class TestCase
    {
        public string Sentence { get; set; }

        static Dictionary<char, char> codeMap;

        static TestCase()
        {
            codeMap = new Dictionary<char,char>();
            
            // Learn("a zoo", "y qee"); <-- wrong example ?
            Learn("ejp mysljylc kd kxveddknmc re jsicpdrysi", "our language is impossible to understand");
            Learn("rbcpc ypc rtcsra dkh wyfrepkym veddknkmkrkcd", "there are twenty six factorial possibilities");
            Learn("de kr kd eoya kw aej tysr re ujdr lkgc jv", "so it is okay if you want to just give up");
            
            //var missingGooglereseChars = Enumerable.Range(0, 26).Select(x => (char)('a' + x)).Where(x => !codeMap.ContainsKey(x)).ToArray();
            //var missingEnglishChars = Enumerable.Range(0, 26).Select(x => (char)('a' + x)).Where(x => !codeMap.ContainsKey(x)).ToArray();
            // missings are q and z in both
            Learn('q', 'z');
            Learn('z', 'q');
            
            Debug.Assert(codeMap.Count == 27);
        }

        static void Learn(string googlerese, string english)
        {
            Debug.Assert(googlerese.Length==english.Length);
            for (int i = 0; i < googlerese.Length; i++)
                Learn(googlerese[i], english[i]);
        }

        static void Learn(char googlerese, char english)
        {
            if (codeMap.ContainsKey(googlerese))
            {
                Debug.Assert(codeMap[googlerese]==english);
            }
            else
            {
                codeMap.Add(googlerese, english);
            }
        }

        public object Solve()
        {
            return new String(Sentence.Select(x => codeMap[x]).ToArray());
        }

        public override string ToString()
        {
            return Sentence;
        }
    }
}
