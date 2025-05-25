using System.IO;
using System;
namespace MonopolyServer.Database
{
    class MySQLData
    {
        public string Server = "";
        public uint Port;
        public string Database = "";
        public string UserID = "";
        public string Password = "";
        private const string pathDatabaseConnection = @"DatabaseConnection.config";
        public MySQLData()
        {
            ReadDatabaseData();
        }
        private void ReadDatabaseData()
        {
            try
            {
                if(!File.Exists(pathDatabaseConnection))
                {
                    createDatabaseFile();
                }

                StreamReader sr = new StreamReader(pathDatabaseConnection);             
                if (sr != null)
                {
                    Server = sr.ReadLine().Remove(0, 7);
                    Port = Convert.ToUInt32(sr.ReadLine().Remove(0, 5));
                    Database = sr.ReadLine().Remove(0, 9);
                    UserID = sr.ReadLine().Remove(0, 7);
                    Password = sr.ReadLine().Remove(0, 9);
                }
            }catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        private void createDatabaseFile()
        {
            using (StreamWriter sw = new StreamWriter(pathDatabaseConnection))
            {
                sw.WriteLine("Server=monopolydatabase.cvi0v46s8nha.eu-central-1.rds.amazonaws.com");
                sw.WriteLine("Port=3306");
                sw.WriteLine("Database=mydb");
                sw.WriteLine("UserID=admin");
                sw.WriteLine("Password=Bonifac123");
                sw.Flush();
            }
        }
    }
}
