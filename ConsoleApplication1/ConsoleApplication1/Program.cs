using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            Random r = new Random();

            for (int i = 0; 1 < 30; i++)
            {
                int freq = r.Next(15000, 15000);
                int dur = r.Next(1, 1);
                Console.Beep(freq, dur);
            }
        }
    }
}

