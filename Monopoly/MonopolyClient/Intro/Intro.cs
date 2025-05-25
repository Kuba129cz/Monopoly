using System;
using System.Collections.Generic;
using System.Text;
using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Monopoly.Communication;
using Myra;
using Myra.Graphics2D.Brushes;
using Myra.Graphics2D.UI;

namespace Monopoly.Intro
{
    partial class Intro
    {
        private Desktop desktop = null;
        private Label label1 = null;
        private Label label2 = null;
        private TextBox textBox1 = null;
        private TextBox textBox2 = null;
        private TextButton button1 = null;
        private TextButton button2 = null;
        private TextButton button3 = null;
        private Panel panel;
        private bool changeToMenu = false;
        private Dialog messageBox1 = null;
        private KeyboardState previousKeyboardEnter = Keyboard.GetState();
        private List<ServerBuild> servers = null;
        private Window win = null;
        public Intro(Microsoft.Xna.Framework.Game game)
        {
            MyraEnvironment.Game = game;
            panel = new Panel();
            desktop = new Desktop();
        }
        public void InitializeComponent()
        {         
        }
        public void LoadContent()
        {
            buildUI();          
            desktop.Root = panel;
            findServers();
        }
        private void findServers()
        {
            new System.Threading.Tasks.Task(serverCom).Start();
            messageBox1 = Dialog.CreateMessageBox("Upozorneni.", "Hledám servery..");
            messageBox1.ButtonCancel.Enabled = false;
            messageBox1.ButtonOk.Enabled = false;
            messageBox1.CloseButton.Enabled = false;
            messageBox1.ShowModal(desktop);
        }
        private void button3Clicked(object sender, EventArgs e)
        {
            findServers();
        }
        private void serverCom()
        {
            new Communication.Query();

            servers = Query.GetListOfSevers(); //list
            for (int i = 0; i < servers.Count; i++)
            {
                if (servers[i].Join)
                {
                    button3.Text = servers[i].Name;
                    button3.Background = button1.Background;
                }
            }

            messageBox1.Close();
            choiceServer();
        }
        private void choiceServer()
        {
            win = new Window();
            win.Height = 310;
            win.Width = 250;
            Panel dialogPanel = new Panel();
            win.Title = "Volba serveru";

            var listBox1 = new ListBox
            {
                Top = 20,
                Left = 20,
                Width = win.Width - 40,
            };
            var button1 = new TextButton()
            {
                Width = 100,
                Height = 30,
                Top = 240,
                Left = 250 / 2 - 90 / 2,
                Text = "Připojit"
            };
            dialogPanel.Widgets.Add(button1);
            dialogPanel.Widgets.Add(listBox1);
            win.Content = dialogPanel;
            win.ShowModal(desktop);
            win.DragDirection = DragDirection.None;
           // List<ServerBuild> serverBuilds =Query.GetListOfSevers(); //list
            for(int i=0;i<servers.Count;i++)
            {
                ListItem listItem1 = new ListItem();
                if(servers[i].Online)
                    listItem1.Color = Color.LightGreen;
                else
                    listItem1.Color = Color.Red;
                listItem1.Text = servers[i].Name;
                listBox1.Items.Add(listItem1);
                if (servers[i].Join)
                    listBox1.SelectedIndex = i;
            }
            button1.Click += (s, a) => {
                if (!servers[(int)listBox1.SelectedIndex].Online)
                {
                    var messageBox = Dialog.CreateMessageBox("Upozorneni.", "Na offline server se nelze připojit.");
                    messageBox.ShowModal(desktop);
                }
                else
                     if (servers[(int)listBox1.SelectedIndex].Join)
                {
                    var messageBox = Dialog.CreateMessageBox("Upozorneni.", "Na tomto serveru se již nacházíš.");
                    messageBox.ShowModal(desktop);
                }
                else
                {

                    new System.Threading.Tasks.Task(()=>connectingToServer(servers[(int)listBox1.SelectedIndex])).Start();
                    messageBox1 = Dialog.CreateMessageBox("Upozorneni.", "Připojování na server..");
                    messageBox1.ButtonCancel.Enabled = false;
                    messageBox1.ButtonOk.Enabled = false;
                    messageBox1.CloseButton.Enabled = false;
                    messageBox1.ShowModal(desktop);
                }

            };
        }
        private void connectingToServer(ServerBuild server)
        {
            //osetrit jestli se uspesne pripojilo, mozna napravo label s online/offline informaci, potreba zjistit toto pripojeni, proto aby
            //se hodila chyba v loginu a registraci kdyz klient neni pripojen k zadnemu serveru
            if (Query.JoinToServer(server))
            {
                button3.Text = server.Name;
                win.Close();
            }
            else
            {
                button3.Text = "Server";
                button3.Background = new SolidBrush("#FF0000");
                var messageBox = Dialog.CreateMessageBox("Chyba.", "Nepodařilo se připojit na server.");
                messageBox.ShowModal(desktop);
            }
            messageBox1.Close();

        }
        private void login(object sender, EventArgs e)
        {
            if (Query.IsConnectToServer())
            {
                if (textBox1.Text != null && textBox2.Text != null && textBox1.Text != string.Empty && textBox2.Text != string.Empty)
                {
                    Data.user = new User(textBox1.Text, textBox2.Text);
                    Data.user.IDPlayer = Data.ThisPlayer.IDPlayer;
                    messageBox1 = Dialog.CreateMessageBox("Přihlášení.", "Prihlasovani..");
                    messageBox1.ButtonCancel.Enabled = false;
                    messageBox1.ButtonOk.Enabled = false;
                    messageBox1.CloseButton.Enabled = false;
                    messageBox1.ShowModal(desktop);
                    new System.Threading.Tasks.Task(loginCom).Start();
                    //    Communication.Query.PlayerLogin(Data.user);
                    //    messageBox2.Close();
                    //    if (Data.user.success)
                    //    {
                    //        var messageBox = Dialog.CreateMessageBox("Přihlášení.", Data.user.info);
                    //        messageBox.ShowModal(desktop);
                    //        changeToMenu = true;
                    //        //GameState.ChangeGameState(GameStates.Menu);
                    //    }
                    //    else
                    //    {
                    //        var messageBox = Dialog.CreateMessageBox("Přihlášení se nepodařilo.", Data.user.info);
                    //        messageBox.ShowModal(desktop);
                    //    }
                }
                else
                {
                    var messageBox = Dialog.CreateMessageBox("Upozornění", "Nick, nebo heslo nejsou vyplněný!");
                    messageBox.ShowModal(desktop);
                }
            }else
            {
                var messageBox = Dialog.CreateMessageBox("Upozornění", "Nejsi připojen k žádnému serveru!");
                messageBox.ShowModal(desktop);
            }
        }

        private void loginCom()
        {
                Communication.Query.PlayerLogin(Data.user);
                messageBox1.Close();
                if (Data.user.success)
                {
                    var messageBox = Dialog.CreateMessageBox("Přihlášení.", Data.user.info);
                    messageBox.ShowModal(desktop);
                    changeToMenu = true;
                }
                else
                {
                    var messageBox = Dialog.CreateMessageBox("Přihlášení se nepodařilo.", Data.user.info);
                    messageBox.ShowModal(desktop);
                }
            }
        

        private void registration(object sender, EventArgs e)
        {
            if (Query.IsConnectToServer())
            {
                if (textBox1.Text != null && textBox2.Text != null && textBox1.Text != string.Empty && textBox2.Text != string.Empty)
                {
                    if (textBox1.Text.Length < 7)
                    {
                        messageBox1 = Dialog.CreateMessageBox("Registrace.", "registrovávám..");
                        messageBox1.ShowModal(desktop);
                        new System.Threading.Tasks.Task(registrationCom).Start();
                        //Data.user = new User(textBox1.Text, textBox2.Text);
                        //Communication.Query.PlayerRegistration(Data.user);
                        //if(Data.user.success)
                        //{
                        //    var messageBox = Dialog.CreateMessageBox("Registrace.", Data.user.info);
                        //    messageBox.ShowModal(desktop);
                        //    //GameState.ChangeGameState(GameStates.Menu);
                        //}else
                        //{
                        //    var messageBox = Dialog.CreateMessageBox("Registrace se nepodařila.", Data.user.info);
                        //    messageBox.ShowModal(desktop);
                        //}
                    }
                    else
                    {
                        var messageBox = Dialog.CreateMessageBox("Upozornění", "Tvůj nick je dlouhý!\nJe povoleno 7 znaků.");
                        messageBox.ShowModal(desktop);
                    }
                }
                else
                {
                    var messageBox = Dialog.CreateMessageBox("Upozornění", "Není zadána přezdívka, nebo heslo!");
                    messageBox.ShowModal(desktop);
                }
            }else
            {
                var messageBox = Dialog.CreateMessageBox("Upozornění", "Nejsi připojen k žádnému serveru!");
                messageBox.ShowModal(desktop);
            }
        }

        private void registrationCom()
        {
            Data.user = new User(textBox1.Text, textBox2.Text);
            Query.PlayerRegistration(Data.user);
            messageBox1.Close();
            if (Data.user.success)
            {
                var messageBox = Dialog.CreateMessageBox("Registrace.", Data.user.info);
                messageBox.ShowModal(desktop);
            }
            else
            {
                var messageBox = Dialog.CreateMessageBox("Registrace se nepodařila.", Data.user.info);
                messageBox.ShowModal(desktop);
            }
        }

        public void Update()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Enter) && previousKeyboardEnter.IsKeyUp(Keys.Enter))
            {
                login(null, null);
            }
            if (textBox1.Text == string.Empty || textBox2.Text == string.Empty || textBox1.Text == null || textBox2.Text == null)
            {
                button1.Enabled = false;
                button2.Enabled = false;
            }
            else
            {
                button1.Enabled = true;
                button2.Enabled = true;
            }
            if(changeToMenu)
            {
                changeToMenu = false;
                GameState.ChangeGameState(GameStates.Menu);
            }
            previousKeyboardEnter = Keyboard.GetState();
        }
        public void ShowMessageBox(string message)
        {
            var messageBox = Dialog.CreateMessageBox("Chyba", message);
            messageBox.ShowModal(desktop);
        }

        public void Draw()
        {
            desktop.Render();
        }

        public void HideIntro()
        {
            panel.Visible = false;
        }
        public void ShowIntro()
        {
            panel.Visible = true;
        }
    }
}
