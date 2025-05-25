using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Monopoly.Intro
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
    }
}
