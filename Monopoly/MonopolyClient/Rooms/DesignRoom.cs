using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Myra.Graphics2D.UI;
using Myra;
using System.Threading.Tasks;
using System.Threading;
using Monopoly.Communication;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework.Input;

namespace Monopoly.Rooms
{
    partial class DesignRoom
    {
        private Desktop _desktop;
        private Microsoft.Xna.Framework.Game game;
        public DesignRoom(Microsoft.Xna.Framework.Game game)
        {
            MyraEnvironment.Game = game;
            this.game = game;
            panel = new Panel();
        }
       public void LoadContent()
        {
            buildUI();
            _desktop = new Desktop();
            _desktop.Root = panel;
            UpdateListOfRooms();
            ListItem listItem1 = new ListItem();
            listItem1.Color = Color.White;
            listItem1.Text = Data.ThisPlayer.Nick;
            playersList.Items.Add(listItem1);
        }

        public static void UpdateListOfRooms()
        {
            if (GameStates.Room == GameState.GetCurrentState())
            {
                listBox1.Items.Clear();
                for (int i = 0; i < Data.room.Lobbies.Count; i++)
                {
                    ListItem listItem1 = new ListItem();
                    listItem1.Text = string.Format("{0}   {1}/4", Data.room.Lobbies[i].Name, Data.room.Lobbies[i].NumberOfPlayers());
                    listItem1.Color = Color.White;
                    listBox1.Items.Add(listItem1);
                }
            }
        }
        private void btnQuit_clicked(object sender, EventArgs e)
        {
            GameState.ChangeGameState(GameStates.Menu);
        }
        private void btnEnter_Clicked(object sender, EventArgs e)
        {
            if(listBox1.SelectedIndex != null)
            {
                if ((Data.room.Lobbies[(int)listBox1.SelectedIndex].NumberOfPlayers()) < 4)
                {
                   bool joined = Query.JoinLobby(Data.room.Lobbies[(int)listBox1.SelectedIndex]);
                    if (joined)
                    {
                        GameState.ChangeGameState(GameStates.Lobby);
                    }
                    else
                    {
                        var messageBox = Dialog.CreateMessageBox("Upozornění", "Tato místnost je plná!");
                        messageBox.ShowModal(_desktop);
                    }

                }
                else
                {
                    var messageBox = Dialog.CreateMessageBox("Upozornění", "Tato místnost je plná!");
                    messageBox.ShowModal(_desktop);
                }
            }else
            {
                    var messageBox = Dialog.CreateMessageBox("Upozornění", "místnost není vybrána!");
                    messageBox.ShowModal(_desktop);
            }
        }
        private void btnAddRoom_clicked(object sender, EventArgs e)
        {
            panel.Visible = false;
            Window win = new Window();
            win.Height = 150;
            win.Width = 335;
            win.Title = "Založení místnosti";
            var grid1 = new Grid();
            grid1.ColumnSpacing = 16;
            grid1.RowSpacing = 16;
            grid1.DefaultColumnProportion = new Proportion
            {
                Type = Myra.Graphics2D.UI.ProportionType.Auto,
            };
            grid1.DefaultRowProportion = new Proportion
            {
                Type = Myra.Graphics2D.UI.ProportionType.Auto,
            };

            var label1 = new Myra.Graphics2D.UI.Label();
            label1.Text = "Zvol jméno:";

            TextBox txt = new TextBox();
            txt.Width = 188;
            txt.MaxWidth = 188;
            txt.GridRow = 1;
            txt.Text = Communication.Query.GetThisPlayer().Nick + "'s lobby";
          
            var Button1 = new TextButton();
            Button1.Text = "Potvrdit";
            Button1.Width = 90;
            Button1.GridColumn = 1;
            Button1.GridRow = 2;

            grid1.Widgets.Add(label1);
            grid1.Widgets.Add(txt);
            grid1.Widgets.Add(Button1);

            var verticalStackPanel1 = new VerticalStackPanel();
            verticalStackPanel1.Spacing = 8;
            verticalStackPanel1.Widgets.Add(grid1);

            win.Content = grid1;

            win.ShowModal(_desktop);

            win.Closed += (s, a) =>
            {
                panel.Visible = true;
            };
            Button1.Click += (s, a) => {
                Query.CreateLobby(new Lobby.Lobby(txt.Text));
                GameState.ChangeGameState(GameStates.Lobby);
                };
        }
        public void HideWidgets()
        {
            if (listBox1 != null)
            {
                panel.Visible = false;
            }
        }
        public void ShowWidgets()
        {
            if (listBox1 != null)
            {
                panel.Visible = true;
            }
        }
        public void Draw()
        {
            _desktop.Render();
        }
        public void Update()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Enter) && textBox1.Text != string.Empty && textBox1.Text != null)
            {
                if(textBox1.Text.Length >2 && textBox1.Text[0] == '/' && textBox1.Text[1] == 'w' && textBox1.Text[2] == ' ')
                {
                    string nick = "";
                    string msg = "";
                    int index = 3;
                    for(int i=3; i<textBox1.Text.Length;i++)
                    {
                        if(textBox1.Text[i] != ' ')
                        {
                            nick += textBox1.Text[i];
                            index++;
                        }else
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
                    }else
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
                    Chat.Chat chat = new Chat.Chat(Communication.Query.GetThisPlayer().Nick, 'R', Communication.Query.GetThisPlayer().IDPlayer);
                    chat.SendMessage(textBox1.Text);
                }
                textBox1.Text = string.Empty;

            }
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
        internal void updatePlayersInPlayersList()
        {
            playersList.Items.Clear();
            for (int i = 0; i < Data.room.Players.Count; i++)
            {
                ListItem listItem1 = new ListItem();
                if(Data.room.Players[i].IDLobby != Guid.Empty)
                    listItem1.Color = Color.Red;
                else
                    listItem1.Color = Color.White;
                listItem1.Text = Data.room.Players[i].Nick;
                playersList.Items.Add(listItem1);
            }
        }
    }
}
