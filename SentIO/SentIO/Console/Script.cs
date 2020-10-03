using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace SentIO.Console
{

    public class TextEnumerable : IEnumerable<string>
    {
        List<string> texts = new List<string>() { "1", "2", "3" };

        IEnumerator<string> enumerator;

        public IEnumerator<string> GetEnumerator()
        {
            return enumerator;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }

    public static class Script
    {
        public static IEnumerable DisplayText(string text)
        {
            // TODO 

            yield return new TextEnumerable();

            Debug.WriteLine("blabla");
        }

        public static string DisplayQuestion(string text)
        {
            return "";
        }
    }
}
