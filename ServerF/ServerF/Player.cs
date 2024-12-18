using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerF
{
    class Player
    {
        private bool ready;
        private string name;

        public Player(string n , bool r)
        {
            this.name = n;
            this.ready = r;
        }
        public string GetName()
        {
            return (name);
        }
        public bool GetReady()
        {
            return ready;
        }
        public void SetReady(bool b)
        {
            ready = b;
        }
    }
}
