using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Text;
using MonopolyServer.Server.Data;
using MySql.Data;
using MySql.Data.MySqlClient;
namespace MonopolyServer.Database
{
    class Connection
    {
        private MySqlConnection mySqlConnection = null;
        public Connection()
        {
            mySqlConnection = new MySqlConnection();
            try {
                MySqlConnectionStringBuilder mysqlConfiguration = new MySqlConnectionStringBuilder();
               // mysqlConfiguration.Database = "monopoly";

                mysqlConfiguration.SslMode = MySqlSslMode.None;

                //mysqlConfiguration.Server = Dns.GetHostName();
                // mysqlConfiguration.Server = "192.168.1.102";
                //mysqlConfiguration.Server = "127.0.0.1";

                //For amazon
                MySQLData mySQLData = new MySQLData();
                mysqlConfiguration.Server = mySQLData.Server;
                mysqlConfiguration.Port = mySQLData.Port;
                mysqlConfiguration.Database = mySQLData.Database; //MonopolyDat
                mysqlConfiguration.UserID = mySQLData.UserID;
                mysqlConfiguration.Password = mySQLData.Password; //Lokomotiva1@


                //mysqlConfiguration.Port = 3306;
                //mysqlConfiguration.Database = "monopoly";
                //mysqlConfiguration.UserID = "root";//"server"; //pro pc
                ////mysqlConfiguration.UserID = "Server";
                //mysqlConfiguration.Password = "Bonifac";//"Monopoly123";
                ////mysqlConfiguration.Password = "Server123";

                mySqlConnection.ConnectionString = mysqlConfiguration.ConnectionString;

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public bool IsNickFree(string nick)
        {
                if (mySqlConnection.State == ConnectionState.Closed)
                {
                    mySqlConnection.Open();
                }
                MySqlCommand command = new MySqlCommand();
                command.Connection = mySqlConnection;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = "SELECT nick FROM Player WHERE nick = @nick";
                MySqlParameter param = new MySqlParameter("@nick", MySqlDbType.VarChar);
                param.Value = nick;
                command.Parameters.Add(param);

                using (MySqlDataReader mySqlDataReader = command.ExecuteReader())
                {
                    if (mySqlDataReader.Read())
                    {
                        return false;
                    }
                    return true;
                }
        }
        public void RegisterNewPlayer(User user)
        {
            if (mySqlConnection.State == ConnectionState.Closed)
            {
                mySqlConnection.Open();
            }
            MySqlCommand command = new MySqlCommand();
            command.Connection = mySqlConnection;
            command.CommandType = System.Data.CommandType.Text;
            command.CommandText = "INSERT INTO Player (Nick, Password) VALUES (@nick, @password)";
            MySqlParameter param = new MySqlParameter("@nick", MySqlDbType.VarChar);
            param.Value = user.Nick;
            command.Parameters.Add(param);
            param = new MySqlParameter("@Password", MySqlDbType.VarChar);
            param.Value = user.HashPass(user.Password);
            command.Parameters.Add(param);
            command.ExecuteNonQuery();
        }
        public bool LoginPlayer(User user)
        {
            if (mySqlConnection.State == ConnectionState.Closed)
            {
                mySqlConnection.Open();
            }
            MySqlCommand command = new MySqlCommand();
            command.Connection = mySqlConnection;
            command.CommandType = System.Data.CommandType.Text;
            command.CommandText = "SELECT nick, password FROM Player WHERE nick = @nick";
            MySqlParameter param = new MySqlParameter("@nick", MySqlDbType.VarChar);
            param.Value = user.Nick;
            command.Parameters.Add(param);
            string nick, password = string.Empty;
            using (MySqlDataReader mySqlDataReader = command.ExecuteReader())
            {
                while (mySqlDataReader.Read())
                {
                    nick = mySqlDataReader.GetString(0);
                    password = mySqlDataReader.GetString(1);
                }
            }
            if (user.HashPass(user.Password) == password)
            {
                return true;
            }
            else
                return false;
        }
        public void CreateLobby(Lobby lobby)
        {
            if (mySqlConnection.State == ConnectionState.Closed)
            {
                mySqlConnection.Open();
            }
            MySqlCommand command = new MySqlCommand();
            command.Connection = mySqlConnection;
            command.CommandType = System.Data.CommandType.Text;
            command.CommandText = "INSERT INTO Lobby (Date, Name) VALUES (@Date, @Name)";
            MySqlParameter param = new MySqlParameter("@Date", MySqlDbType.Date);
            DateTime myDateTime = DateTime.Now;
            param.Value = myDateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
            command.Parameters.Add(param);
            param = new MySqlParameter("@Name", MySqlDbType.VarChar);
            param.Value = lobby.Name;
            command.Parameters.Add(param);
            command.ExecuteNonQuery();
        }
        private int giveIDLobby(string lobbyName)
        {
            if (mySqlConnection.State == ConnectionState.Closed)
            {
                mySqlConnection.Open();
            }
            MySqlCommand command = new MySqlCommand();
            command.Connection = mySqlConnection;
            command.CommandType = CommandType.Text;
            command.CommandText = "SELECT IDLobby from Lobby WHERE name = @name";
            MySqlParameter param = new MySqlParameter("@name", MySqlDbType.VarChar);
            param.Value = lobbyName;
            command.Parameters.Add(param);
            using (MySqlDataReader mySqlDataReader = command.ExecuteReader())
            {
                while (mySqlDataReader.Read())
                {
                    return mySqlDataReader.GetInt32(0);
                }
            }
            return -1;
        }
        private int giveIDPlayer(string nick)
        {
            if (mySqlConnection.State == ConnectionState.Closed)
            {
                mySqlConnection.Open();
            }
            MySqlCommand command = new MySqlCommand();
            command.Connection = mySqlConnection;
            command.CommandType = CommandType.Text;
            command.CommandText = "SELECT IDPlayer from Player WHERE nick = @nick";
            MySqlParameter param = new MySqlParameter("@nick", MySqlDbType.VarChar);
            param.Value = nick;
            command.Parameters.Add(param);
            using (MySqlDataReader mySqlDataReader = command.ExecuteReader())
            {
                while (mySqlDataReader.Read())
                {
                    return mySqlDataReader.GetInt32(0);
                }
            }
            return -1;
        }
        public void WriteStatistics(Player player, Lobby lobby)
        {
            try
            {
                int idLobby = giveIDLobby(lobby.Name);
                int idPlayer = giveIDPlayer(player.Nick);
                if (mySqlConnection.State == ConnectionState.Closed)
                {
                    mySqlConnection.Open();
                }
                    MySqlCommand command = new MySqlCommand();
                    command.Connection = mySqlConnection;
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = "INSERT INTO PlayersInLobby (IDPlayer, IDLobby, SpendTime, Round, Money, Status) VALUES (@IDPlayer, @IDLobby, @SpendTime, @Round,@Money,@Status);";
                    MySqlParameter param = new MySqlParameter("@SpendTime", MySqlDbType.Date);
                    param.Value = player.TimeInLobby.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    command.Parameters.Add(param);
                    param = new MySqlParameter("@Round", MySqlDbType.Int32);
                    param.Value = lobby.Round;
                    command.Parameters.Add(param);
                    param = new MySqlParameter("@Money", MySqlDbType.Int32);
                    param.Value = player.Money;
                    command.Parameters.Add(param);
                param = new MySqlParameter("@IDLobby", MySqlDbType.Int32);
                param.Value = idLobby;
                command.Parameters.Add(param);
                param = new MySqlParameter("@IDPlayer", MySqlDbType.Int32);
                param.Value = idPlayer;
                command.Parameters.Add(param);
                param = new MySqlParameter("@Status", MySqlDbType.VarChar);
                if (player.Leave)
                {
                    param.Value = "Opustil";
                }else
                 if (player.Money > 0)
                {
                    param.Value = "Výhra";
                    MySqlCommand command2 = new MySqlCommand();
                    command2.Connection = mySqlConnection;
                    command2.CommandType = System.Data.CommandType.Text;
                    command2.CommandText = "INSERT INTO Lobby (Winner) VALUES (@Winner)";
                    MySqlParameter param2 = new MySqlParameter("@Date", MySqlDbType.Date);
                    param2 = new MySqlParameter("@Winner", MySqlDbType.VarChar);
                    param2.Value = player.Nick;
                    command2.Parameters.Add(param2);
                    command2.ExecuteNonQuery();
                }else
                {
                    param.Value = "Prohra";
                }
                command.Parameters.Add(param);
                command.ExecuteNonQuery();
            }
            catch(Exception ex) {
                Console.WriteLine(ex);
            }
        }
        public void statistics(string nick)
        {
            if (mySqlConnection.State == ConnectionState.Closed)
            {
                mySqlConnection.Open();
            }
            MySqlCommand command = new MySqlCommand();
            command.Connection = mySqlConnection;
            command.CommandType = System.Data.CommandType.Text;
            command.CommandText = "SELECT Date, Winner, Name FROM player WHERE nick = @nick";
            MySqlParameter param = new MySqlParameter("@nick", MySqlDbType.VarChar);
            param.Value = nick;
            command.Parameters.Add(param);

            using (MySqlDataReader mySqlDataReader = command.ExecuteReader())
            {
                if (mySqlDataReader.Read())
                {
                  //  return false;
                }
                //return true todo
            }
        }
        public List<BasicStatistics> ReadBasicStatistics(Guid IDPlayer)
        {
            List<BasicStatistics> statistics = new List<BasicStatistics>();
            int i = 0;
            if (mySqlConnection.State == ConnectionState.Closed)
            {
                mySqlConnection.Open();
            }
            MySqlCommand command = new MySqlCommand();
            command.Connection = mySqlConnection;
            command.CommandType = System.Data.CommandType.Text;
            command.CommandText = "SELECT PlayersInLobby.Status, PlayersInLobby.SpendTime, " +
                "Lobby.Date, Count(PlayersInLobby.IDLobby) as numPlayers, PlayersInLobby.Round, Lobby.name, Lobby.IDLobby FROM " +
                "PlayersInLobby join Lobby on Lobby.IDLobby = PlayersInLobby.IDLobby join Player on " +
                "Player.IDPlayer = PlayersInLobby.IDPlayer ORDER BY Lobby.Date; ";
            using (MySqlDataReader mySqlDataReader = command.ExecuteReader())
            {
                if (mySqlDataReader.Read())
                {
                    if (!mySqlDataReader.IsDBNull(0))
                    {
                        BasicStatistics basicStatistics = new BasicStatistics(i);
                        statistics.Add(basicStatistics);
                        statistics[i].Record = fillString(mySqlDataReader.GetString(0)); //status
                        statistics[i].Record += fillString(mySqlDataReader.GetValue(1).ToString()); //spendTime
                        DateTime dateTime = mySqlDataReader.GetDateTime(2);
                        statistics[i].Record += (dateTime.Day + "." + dateTime.Month + "." + dateTime.Year).ToString();
                        statistics[i].Record += fillString(mySqlDataReader.GetInt32(3).ToString()); //pocet hracu
                        statistics[i].Record += fillString(mySqlDataReader.GetInt32(4).ToString()); //pocet kol
                        statistics[i].Record += fillString(mySqlDataReader.GetString(5).ToString()); //name
                        basicStatistics.IDLobby = mySqlDataReader.GetInt32(6);
                        i++;
                    }
                    else
                        return statistics;
                }
                return statistics;
            }
        }
        private string fillString(string str)
        {
            int i = 30 - str.Length;
            while(i>0)
            {
                str += " ";
                i--;
            }
            return str;
        }
        internal List<BasicStatistics> ReadDetailedStatistics(Guid idPlayer, int idLobbyInDatabase)
        {
            List<BasicStatistics> statistics = new List<BasicStatistics>();
            int i = 0;
            if (mySqlConnection.State == ConnectionState.Closed)
            {
                mySqlConnection.Open();
            }
            MySqlCommand command = new MySqlCommand();
            command.Connection = mySqlConnection;
            command.CommandType = System.Data.CommandType.Text;
            command.CommandText = "SELECT PlayersInLobby.Status, Player.Nick, PlayersInLobby.SpendTime, " +
                "PlayersInLobby.Round, PlayersInLobby.Money FROM " +
                "PlayersInLobby join Lobby on Lobby.IDLobby = PlayersInLobby.IDLobby join Player on " +
                "Player.IDPlayer = PlayersInLobby.IDPlayer WHERE PlayersInLobby.IDLobby = @idLobby ORDER BY Lobby.Date; ";
            MySqlParameter param = new MySqlParameter("@idLobby", MySqlDbType.Int32);
            param.Value = idLobbyInDatabase;
            command.Parameters.Add(param);
            using (MySqlDataReader mySqlDataReader = command.ExecuteReader())
            {
                if (mySqlDataReader.Read())
                {
                    statistics.Add(new BasicStatistics(i));
                    statistics[i].Record = fillString(mySqlDataReader.GetString(0)); //status
                    statistics[i].Record += fillString(mySqlDataReader.GetString(1).ToString()); //nick
                    statistics[i].Record += fillString(mySqlDataReader.GetValue(2).ToString()); //spendTime
                    statistics[i].Record += fillString(mySqlDataReader.GetInt32(3).ToString()); //pocet kol
                    statistics[i].Record += fillString(mySqlDataReader.GetInt32(4).ToString()); //money
                    i++;
                }
                return statistics;
            }
        }
        ~Connection()
        {
            if (mySqlConnection.State == ConnectionState.Open)
            {
                mySqlConnection.Close();
            }
        }
    }
}
