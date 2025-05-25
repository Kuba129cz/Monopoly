using System;
using System.Collections.Generic;
using System.Text;

namespace MonopolyServer
{
    
    public static class ExtensionString
    {
        public static string MySplit(this String str, char c)
        {
            string code = null;
            for(int i=0;i<str.Length;i++)
            {
                if (str[i] == c)
                    break;
                else
                    code += str[i];
            }
            return code;
        }
    }
}
