using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectF
{
    public partial class Board : Form
    {
        private int[,] myBoard = new int[6, 6];//A Matrix That Represent The Player Board. 1 - Represent Own Ship , 2 - Represent Try, 3 - Represent Hit , 0 - Empty.
        private int[,] enemyBoard = new int[6, 6];//A Matrix That Represent The Enemy Board.1 - Represent Enemy Ship , 2 - Represent Try, 3 - Represent Hit , 0 - Empty.
        private int[] cou = new int[4] { 0, 0, 0, 0 };//Counter Array For The Ship-Type Removal.
        private bool ready = false;//Use To Know If The Enemy Ready To Play.
        public CommunicationHelper ch;
        private string messageR;


        public Board(Lobby l, string opp)
        {
            InitializeComponent();
            this.ch = l.GetCh();
            label32.Text = ch.get_name();
            label34.Text = opp;
            for (int i = 0; i < 6; i++)
                for (int j = 0; j < 6; j++)
                {
                    myBoard[i, j] = 0;
                    enemyBoard[i, j] = 0;
                }
            if(true)
            {
                Random rnd = new Random();
                int x = rnd.Next(0, 3);
                if (x == 1)
                    BoP1();
                if (x == 2)
                    BoP2();

            }
            Att.Visible = false;
            label31.Visible = false;
            comboBox5.Visible = false;
            comboBox6.Visible = false;
        }

        private void send_Click(object sender, EventArgs e)
        {//Sending A Message In Chat.
            ch.SendInfo("BChat:" + textBox2.Text);
            textBox2.Clear();
        }

        private void Set_click(object sender, EventArgs e)
        {//Set The Ship By Player Selction Of Position & Ship Type.
         //Sending The Position To Server & Update The Board.
         //Remove The Ship Type Option If Needed.
            if (myBoard[get_posA(comboBox2.Text) - 1, get_posB(comboBox3.Text) - 1] == 0)
            {
                string s = "MBoard:" + get_posA(comboBox2.Text) + "_" + comboBox3.Text;
                if ((comboBox1.Text == "") || (comboBox2.Text == "") || (comboBox3.Text == "") || (comboBox4.Text == ""))
                    MessageBox.Show("Fill Out All Required Fields");
                else
                {
                    int c = add_to_arr(comboBox4.Text);
                    myBoard[get_posA(comboBox2.Text) - 1, get_posB(comboBox3.Text) - 1] = 1;
                    updateSBoard();
                    rem_op();
                    ch.SendInfo(s);
                }
            }
            else
            {
                MessageBox.Show("Tile Already Used, Pick An Other Tile");
                comboBox2.Items.Remove(comboBox2.SelectedItem);
                comboBox3.Items.Remove(comboBox3.SelectedItem);

            }
        }

        private void BoP1()
        {
            //2 - tile
            myBoard[3, 0] = 1;
            myBoard[4, 0] = 1;
            //2 - tile
            myBoard[1, 4] = 1;
            myBoard[2, 4] = 1;
            //3 - tile
            myBoard[0 , 0] = 1;
            myBoard[0, 1] = 1;
            myBoard[0, 2] = 1;
            //4 - tile
            myBoard[5, 5] = 1;
            myBoard[5, 4] = 1;
            myBoard[5, 3] = 1;
            myBoard[5, 2] = 1;
            //Update
            updateSBoard();
        }

        private void BoP2()
        {
            //2 - tile
            myBoard[3, 0] = 1;
            myBoard[4, 0] = 1;
            //2 - tile
            myBoard[1, 4] = 1;
            myBoard[2, 4] = 1;
            //3 - tile
            myBoard[0, 0] = 1;
            myBoard[0, 1] = 1;
            myBoard[0, 2] = 1;
            //4 - tile
            myBoard[5, 5] = 1;
            myBoard[5, 4] = 1;
            myBoard[5, 3] = 1;
            myBoard[5, 2] = 1;
            //Update
            updateSBoard();
        }
        
        private delegate void delUpdateHistory(string str);
        /// <summary>
        /// Update The Chat.
        /// </summary>
        void UpdateHistory(string str)
        {

            Chatmsg.AppendText(str);


        }
        private int get_posA(string s)
        {//Gets The Letter Position In Integer
            if (s == "A")
                return 1;
            if (s == "B")
                return 2;
            if (s == "C")
                return 3;
            if (s == "D")
                return 4;
            if (s == "E")
                return 5;
            if (s == "F")
                return 6;
            return 0;
        }
        private int get_posB(string s)
        {//Get The Position In Integer.
            if (s == "1")
                return 1;
            if (s == "2")
                return 2;
            if (s == "3")
                return 3;
            if (s == "4")
                return 4;
            if (s == "5")
                return 5;
            if (s == "6")
                return 6;
            return 0;
        }
        private string get_lPos(int i)
        {//Get The Letter In The Position.
            if (i == 1)
                return "A";
            if (i == 2)
                return "B";
            if (i == 3)
                return "C";
            if (i == 4)
                return "D";
            if (i == 5)
                return "E";
            if (i == 6)
                return "F";
            else
                return "";
        }
        private void updateSBoard()
        {//Update The Player Ship Selection And The Moves Of The Enemy.
         //Ships - Green(1) , Enemy Attack - Red(2), Enemy Hit - Black(3).
            int ro;
            int co;
            for (int i = 0; i < 6; i++)
                for (int j = 0; j < 6; j++)
                {
                    if (myBoard[i, j] == 1)
                    {
                        ro = i + 1;
                        co = j + 1;
                        this.Controls.Find(get_lPos(ro) + "_" + co.ToString(), true)[0].BackColor = Color.Green;
                        ch.SendInfo(ro + "," + co + ":" + "1");
                    }

                }
        }
        private int add_to_arr(string s)
        {//Add To The Counter Array The Corrent Move In The Set Ship Section.
            if (s.StartsWith("A"))
            {
                this.cou[0]++;
                return 0;
            }
            if (s.StartsWith("B"))
            {
                this.cou[1]++;
                return 1;
            }
            if (s.StartsWith("C"))
            {
                this.cou[2]++;
                return 2;
            }
            else
            {
                this.cou[3]++;
                return 3;
            }

        }
        private void rem_op()
        {//Remove Option From The Ship Type Combo-Box , Base On The Ship Type.
         //Will Remove If The Counter Array In Ship Position Will Achive The Ship Size.
            if (cou[0] == 2)
            {
                comboBox4.Items.Remove("A. 2 Tile Ship");
                cou[0] = 9;
            }
            if (cou[1] == 2)
            {
                comboBox4.Items.Remove("B. 2 Tile Ship");
                cou[1] = 9;
            }
            if (cou[2] == 3)
            {
                comboBox4.Items.Remove("C. 3 Tile Ship");
                cou[2] = 9;
            }
            if (cou[3] == 4)
            {
                comboBox4.Items.Remove("D. 4 Tile Ship");
                cou[3] = 9;
            }

        }
        private void Att_Click(object sender, EventArgs e)
        {//Clicking The Attack Button Will Send Message To Server With The Position Of The Attack.
         //& Will Update The Board On Screen & In Matrix.
            if (ready)
            {
                string s = "EBoard:" + get_posA(comboBox5.Text) + "_" + comboBox6.Text;
                if ((comboBox5.Text == "") || (comboBox6.Text == ""))
                    MessageBox.Show("Fill Out All Required Fields");
                else
                {
                    updateEBoard(comboBox5.Text, comboBox6.Text);
                    ch.SendInfo(s);
                }
            }
        }
        private void updateEBoard(string c, string r)
        {//Updating The Board Of The Enemy By Choosen Position. 
         //Hit - Red(2), Miss - Black(3) , Enemy Ship (1).
            int ro;
            int co;
            for (int i = 0; i < 6; i++)
                for (int j = 0; j < 6; j++)
                {
                    ro = i + 1;
                    co = j + 1;
                    if (enemyBoard[i, j] == 1)
                    {
                        this.Controls.Find("E" + get_lPos(ro) + "_" + co.ToString(), true)[0].BackColor = Color.Red;
                        ch.SendInfo(ro + "," + co + ":" + "2");
                    }
                    else
                    {
                        this.Controls.Find("E" + get_lPos(ro) + "_" + co.ToString(), true)[0].BackColor = Color.Black;
                        ch.SendInfo(ro + "," + co + ":" + "3");
                    }
                }
        }
        public CommunicationHelper GetCh()
        {
            return this.ch;
        }
        public void UpdateMss(string str)
        {
            if (str.Contains(">"))
            {
                this.Invoke(new delUpdateHistory(UpdateHistory), str);
            }
            if (str.StartsWith("Ready"))
            {
                ready = true;
                Att.Visible = true;
                label31.Visible = true;
                comboBox5.Visible = true;
                comboBox6.Visible = true;
                label33.Visible = false;
            }
            this.messageR = str;
        }

        private void Board_Load(object sender, EventArgs e)
        {
            //Random rnd = new Random();
            //int x = rnd.Next(0, 3);
            //if (x == 1)
            //    BoP1();
            //if (x == 2) ;
        }
    }
}
