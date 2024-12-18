using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerF
{
    class GameS
    {
        private int [,] P1board = new int[6, 6];
        private int [,] P2board = new int[6, 6];
        private string p1Ip;
        private string p2Ip;
        private string p1n;
        private string p2n;
        private Hashtable PL;
        private int countA;
        private int countB;
        private string serialN;

        public GameS(string p1 , string p2 , string n1 , string n2 , string serial )
        {
            p1Ip = p1;
            p2Ip = p2;
            p1n = n1;
            p2n = n2;
            PL.Add(p1n, p1Ip);
            PL.Add(p2n, p2Ip);
            serialN = serial;
        }

        public void Setp1 (string s)
        {
            p1Ip = s;
        }

        public void Setp2(string s)
        {
            p2Ip = s;
        }

        public string Getp1()
        {
            return p1Ip;
        }

        public string Getp2()
        {
            return p2Ip;
        }

        public int[,] Get_B1()
        {
            return P1board;
        }

        public int[,] Get_B2()
        {
            return P2board;
        }

        public void Update_p1B(int r , int c , int n)
        {
            for (int i = 0; i < 6; i++)
                for (int j = 0; j < 6; j++)
                {
                    if((i == r)&&(j == c))
                            {
                            if(n == 1)
                                P1board[i, j] = 1;
                            if(n == 2)
                                P1board[i, j] = 2;
                            if(n == 3)
                                P1board[i, j] = 3;
                        }
                  
                    
                }
        }

        public void Update_p2B(int r, int c, int n)
        {
            for (int i = 0; i < 6; i++)
                for (int j = 0; j < 6; j++)
                {
                    if ((i == r) && (j == c))
                    {
                        if (n == 1)
                            P2board[i, j] = 1;
                        if (n == 2)
                            P2board[i, j] = 2;
                        if (n == 3)
                            P2board[i, j] = 3;
                    }


                }

        }


    }
}
