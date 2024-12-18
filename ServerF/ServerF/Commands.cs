using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerF
{
    class Commands

    {
        
        Dal d = new Dal();

        
        public string SerialGenerate()
        {//Generating A Code With 4 Chars.
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";//Define The Chars Appers In Code
            var stringChars = new char[4];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            string finalString = new String(stringChars);
            return finalString;
        }
        public bool Isconnected(string user , List<string> l)
        {
            foreach(string s in l)
            {
                if (s == user)
                    return true;
            }
            return false;
        }
    }
}
