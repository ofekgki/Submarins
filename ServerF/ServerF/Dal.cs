using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerF
{
    class Dal
    {
        string connectionString;//נתיב
        SqlConnection connection;//מחלקת תקשרות
        SqlCommand cmd;//מחלקת ביצוע הוראות
        SqlDataReader rdr;//עותק טבלה

        public Dal()
        {
            connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\ofekg\Documents\Project.mdf;Integrated Security=True;Connect Timeout=30";
            connection = new SqlConnection(connectionString);
            cmd = new SqlCommand();
            
        }

        public void AddUser(string user, string pass, string email, string first, string last, string seq , string answer)
        {//uder pass email first last seq answer
            string comm = "INSERT INTO Users (Username , Password , eMail , First , Last , Security , Answer) VALUES ('" + user + "','" + pass + "','" + email + "','" + first +"','" + last + "','"+ seq + "','" + answer + "')";
            cmd.CommandText = comm;
            cmd.Connection = connection;
            connection.Open();
            cmd.ExecuteScalar();
            connection.Close();

        }
        public bool CheckLogin (string user, string pass)
        {
            string comm = "SELECT COUNT(Username) FROM Users WHERE Username = '" + user + "'AND Password = '" + pass + "'";
            cmd.CommandText = comm;
            cmd.Connection = connection;
            connection.Open();
            int x = (int)cmd.ExecuteScalar();
            connection.Close();
            if (x == 1)
                return true;
            return false;
        }
        public bool CheckMail(string email, string user)
        {
            string comm = "SELECT COUNT(Username) FROM Users WHERE Username = '" + user + "'AND eMail = '" + email + "'";
            cmd.CommandText = comm;
            cmd.Connection = connection;
            connection.Open();
            int x = (int)cmd.ExecuteScalar();
            connection.Close();
            if (x == 1)
                return true;
            return false;
        }
        public void UpdatePass(string user , string pass)
        {
            string comm = "UPDATE Users SET Password=" + "'" + pass + "'" + "WHERE Username=" + "'" + user + "'" +  ";";
            cmd.CommandText = comm;
            cmd.Connection = connection;
            connection.Open();
            cmd.ExecuteScalar();
            connection.Close();
        }
        public void UpdateEmail(string user, string mail)
        {
            string comm = "UPDATE Users SET eMail=" + "'" + mail + "'" + "WHERE Username=" + "'" + user + "'" + ";";
            cmd.CommandText = comm;
            cmd.Connection = connection;
            connection.Open();
            cmd.ExecuteScalar();
            connection.Close();
        }
        public bool IsUser (string user)
        {
            string comm = "SELECT COUNT(Username) FROM Users WHERE Username='" + user + "'";
            cmd.CommandText = comm;
            cmd.Connection = connection;
            connection.Open();
            int x = (int)cmd.ExecuteScalar();
            connection.Close();
            if (x == 1)
                return true;
            return false;
        }
    }
}

