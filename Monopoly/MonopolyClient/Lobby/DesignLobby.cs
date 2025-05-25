using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Myra;
using Myra.Graphics2D.UI;
using System.Timers;
using System.Threading;
using Monopoly.Communication;
using Monopoly.Controlls;
using Microsoft.Xna.Framework.Input;

namespace Monopoly.Lobby
{
    class DesignLobby
    {
        private static Desktop _desktop = null;
        private static Label label1 = null;
        private static ListBox listBox1 = null;
        private static TextButton button1 = null;
        private static TextButton button2 = null;
        private static MyListBox chatList;
        private static List<Player> playersInTheLobby = null;
        private static Lobby actualLobby = null;
        private static Player thisPlayer = null;
        private TextBox textBox1 = null;
        Panel panel = null;
        private const int MINIMUM_PLAYERS = 2; 
        public DesignLobby()
        {          
        }
        public void LoadContent()
        {
            panel = new Panel();
            int windowWith = Program.Game.Window.ClientBounds.Width;
            int windowHeight = Program.Game.Window.ClientBounds.Height;
            var grid = new Grid
            {
                RowSpacing = 30,
                ColumnSpacing = 16
            };

            grid.ColumnsProportions.Add(new Proportion(ProportionType.Auto));
            grid.ColumnsProportions.Add(new Proportion(ProportionType.Auto));
            grid.ColumnsProportions.Add(new Proportion(ProportionType.Auto));
            grid.RowsProportions.Add(new Proportion(ProportionType.Auto));
            grid.RowsProportions.Add(new Proportion(ProportionType.Auto));
            grid.RowsProportions.Add(new Proportion(ProportionType.Auto));

            label1 = new Label
            {
                Id = "label1",
                Text = String.Format("Nazev mistnosti: {0}", Communication.Query.GetLobbyName()),
                Top = 10,
                GridColumn = 1,
                GridRow = 0
            };
            grid.Widgets.Add(label1);

            listBox1 = new ListBox
            {
                Width = 200,
                GridColumn = 1,
                GridRow = 1,
                GridColumnSpan = 2
            };
            grid.Widgets.Add(listBox1);

            button1 = new TextButton
            {
                GridColumn = 0,
                GridRow = 2,
                Text = "Odejít",
                Padding = new Myra.Graphics2D.Thickness(10, 10),
                Left = 20,
                Width = 100,
                Height = 50,
                Top = 100
            };
            button1.Click += (s, a) =>
            {
                GameState.ChangeGameState(GameStates.Room);
                Communication.Query.RemovePlayerFromLobby();
                Rooms.DesignRoom.UpdateListOfRooms();
            };
            button2 = new TextButton
            {
                Text = "Spustit hru",
                GridColumn = 2,
                GridRow = 2,
                Width = 100,
                Height = 50,
                Padding = new Myra.Graphics2D.Thickness(10, 10),
                Top = 100,
                Visible = false
            };
            button2.Click += (s, a) =>
            {
                actualLobby = Communication.Query.GetActualLobby();
                Player thisPlayer = Communication.Query.GetThisPlayer();
                if (actualLobby.NumberOfPlayers() >= MINIMUM_PLAYERS)
                {
                    button2.Enabled = false;
                    Communication.Query.LaunchGame(actualLobby);
                    //Thread.Sleep(100); // synchoronizace
                    GameState.ChangeGameState(GameStates.Game);
                }
                else
                {
                    var messageBox = Dialog.CreateMessageBox("Upozornění", "Je potřeba alespoň dvou hráčů.");
                    messageBox.ShowModal(_desktop);
                }
              };

            var label2 = new Label
            {
                Text = "Chat",
                Top = 115,
                HorizontalAlignment = Myra.Graphics2D.UI.HorizontalAlignment.Right,
                Margin = new Myra.Graphics2D.Thickness(0, 0, 23 * windowWith / 100, 0)
            };
            chatList = new MyListBox()
            {
                Width = 220,
                Height = (windowHeight / 100) * 50,
                Top = 150,
                HorizontalAlignment = Myra.Graphics2D.UI.HorizontalAlignment.Right,
                Margin = new Myra.Graphics2D.Thickness(0, 0, 10 * windowWith / 100, 0)
            };
            textBox1 = new TextBox()
            {
                Width = 220,
                Top = 170 + (int)chatList.Height,
                HorizontalAlignment = Myra.Graphics2D.UI.HorizontalAlignment.Right,
                Margin = new Myra.Graphics2D.Thickness(0, 0, 10 * windowWith / 100, 0)
            };

            grid.Widgets.Add(button1);
            grid.Widgets.Add(button2);

            panel.Widgets.Add(label2);
            panel.Widgets.Add(textBox1);
            panel.Widgets.Add(chatList);
            panel.Widgets.Add(grid);
            _desktop = new Desktop();
            _desktop.Root = panel;
            //_desktop.Root = grid;
            UpdateListOfPlayersInLobby(Communication.Query.GetActualLobby());
        }   

        public static void UpdateListOfPlayersInLobby(Lobby lobby)
        {
            try
            {
                if (GameState.GetCurrentState() == GameStates.Lobby && label1 != null && button2!= null)
                {
                    label1.Text = String.Format("Nazev mistnosti: {0}", lobby.Name);
                    thisPlayer = Communication.Query.GetThisPlayer();
                    ListItem listItem1 = null;
                    playersInTheLobby = lobby.GetListOfPlayers();

                    if (thisPlayer.IDPlayer == lobby.Master.IDPlayer)
                        button2.Visible = true;

                    if (listBox1.Items.Count != playersInTheLobby.Count)
                    {
                        listBox1.Items.Clear();
                        for (int i = 0; i < playersInTheLobby.Count; i++)
                        {
                            if (playersInTheLobby[i] != null && playersInTheLobby[i].IDPlayer != lobby.Master.IDPlayer)
                            {
                                listItem1 = new ListItem();
                                listItem1.Color = Color.White;
                                listItem1.Text = string.Format(playersInTheLobby[i].Nick);
                                listBox1.Items.Add(listItem1);
                            }
                            else if (playersInTheLobby[i] != null && playersInTheLobby[i].IDPlayer == lobby.Master.IDPlayer)
                            {
                                listItem1 = new ListItem();
                                listItem1.Color = Color.Gold;
                                listItem1.Text = string.Format(playersInTheLobby[i].Nick);
                                listBox1.Items.Add(listItem1);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var messageBox = Dialog.CreateMessageBox("Upozornění", ex.ToString());
                messageBox.ShowModal(_desktop);
            }
        }
        public void Update()
        {
            if (Communication.Query.GetActualLobby(false).isInGame)
            {
                //  Thread.Sleep(100); // synchoronizace
                GameState.ChangeGameState(GameStates.Game);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Enter) && textBox1.Text != string.Empty && textBox1.Text != null)
            {
                if (textBox1.Text.Length > 2 && textBox1.Text[0] == '/' && textBox1.Text[1] == 'w' && textBox1.Text[2] == ' ')
                {
                    string nick = "";
                    string msg = "";
                    int index = 3;
                    for (int i = 3; i < textBox1.Text.Length; i++)
                    {
                        if (textBox1.Text[i] != ' ')
                        {
                            nick += textBox1.Text[i];
                            index++;
                        }
                        else
                        {
                            index++;
                            break;
                        }
                    }
                    Player player = Query.FindPlayerByNick(nick);
                    if (player == null)
                    {
                        var messageBox = Dialog.CreateMessageBox("Chyba", "Zadal si nesprávný nick hráče!");
                        messageBox.ShowModal(_desktop);
                    }
                    else
                    if (player.IDPlayer == Data.ThisPlayer.IDPlayer)
                    {
                        var messageBox = Dialog.CreateMessageBox("Chyba", "Nemůžeš poslat zprávu sám sobě!");
                        messageBox.ShowModal(_desktop);
                    }
                    else
                    {
                        for (; index < textBox1.Text.Length; index++)
                            msg += textBox1.Text[index];
                        Chat.Chat chat = new Chat.Chat(Communication.Query.GetThisPlayer().Nick, 'P', player.IDPlayer);
                        chat.SendMessage(msg);
                    }
                }
                else
                {
                    Chat.Chat chat = new Chat.Chat(Communication.Query.GetThisPlayer().Nick, 'L', Communication.Query.GetActualLobby(false).IDLobby);
                    chat.SendMessage(textBox1.Text);
                    textBox1.Text = string.Empty;
                }
            }
        }

        public void Draw()
        {
            _desktop.Render();
        }
        public void HideDesignLobby()
        {
            if (label1 != null)
            {
                label1.Visible = false;
                listBox1.Visible = false;
                button1.Visible = false;
                button2.Visible = false;
            }
        }
        public void ShowDesignLobby()
        {
            label1.Visible = true;
            listBox1.Visible = true;
            button1.Visible = true;
            button2.Visible = true;
        }
        internal void addMessageToChatList(Chat.Chat chat)
        {
            ListItem listItem1 = new ListItem();
            if (chat.marker == 'P')
                listItem1.Color = Color.Yellow;
            else
                listItem1.Color = Color.White;
            listItem1.Text = chat.ToString();
            chatList.Items.Add(listItem1);
            chatList.SelectedIndex = chatList.Items.Count - 1;
        }
    }
}
