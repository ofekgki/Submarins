using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Net;
using System.Windows.Forms;
using System.Media;

namespace ProjectF
{
    class Utilities
    {
        public bool IsValidMail(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);//Define The Mail To Check Format

                return true;
            }
            catch (FormatException)//If Not In Format Return Exception
            {
                return false;
            }
        }
        public bool IsValidPass(string password)
        {
            if ((password.Contains("#")) || (password.Contains(">")) || (password.Contains("/")) || (password.Contains('%')) ||(password.Length <= 6))
                return false;
            else
                return true;
        }
        public void sendMail(string Adress, string Message, string Subject)
        {//Send Mail Function
            
            var fromAddress = new MailAddress("ofekPro21@gmail.com", "Project");//Define The Sender
            var toAddress = new MailAddress( Adress ); // Define The Reciver
            const string fromPassword = "Ofek!Pro1"; //Sender Password
             string subject = Subject; //Define Message Subject
             string body = Message;//Define Message Content

            var smtp = new SmtpClient // Define The Mail Service
            {
                Host = "smtp.gmail.com", // Mail Service Host
                Port = 587,
                EnableSsl = true, // Enable Secure Sending
                DeliveryMethod = SmtpDeliveryMethod.Network, // Define The Delivery Method
                UseDefaultCredentials = false,//Disable Default Setting
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword) // Define The Credentials
            };
            using (var message = new MailMessage(fromAddress, toAddress)//Define The Message Setting
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message); // Sending The Message
                MessageBox.Show("Message send Successfully");
            }
        }
        public string CodeGenerate ()
        {//Generating A Code With 8 Chars.
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";//Define The Chars Appers In Code
            var stringChars = new char[8];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            string finalString = new String(stringChars);
            return finalString;
        }
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
    }
}


