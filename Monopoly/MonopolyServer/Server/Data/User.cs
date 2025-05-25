using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace MonopolyServer.Server.Data
{
    class User
    {
        public Guid IDPlayer { get; set; }
        public string Nick { get; set; }
        public string Password { get; set; }
        public string info { get; set; }
        public bool success { get; set; } = false;

        public User(string nick, string pass)
        {
            this.Nick = nick;
            this.Password = pass;
        }
        public string HashPass(string password)
        {
            string SourceText = password;
            Byte[] a = Encoding.UTF8.GetBytes(SourceText);
            Byte[] b;
            SHA512Managed c = new SHA512Managed();
            b = c.ComputeHash(a);
            string HashText = Convert.ToBase64String(b);
            return HashText;
        }
    }
}
