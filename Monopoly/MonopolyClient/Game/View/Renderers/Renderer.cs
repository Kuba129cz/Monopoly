using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Monopoly.MonopolyGame.Model.Enums;
using Monopoly.MonopolyGame.View.UI;
using Monopoly.Play.View.UI;
using Myra.Graphics2D.UI;
using Microsoft.Xna.Framework.Input;
using Monopoly.MonopolyGame.Model;
using Monopoly.Communication;
using System;
using System.Timers;
using Monopoly.MonopolyGame.Model.Tiles;
using MonopolyServer.Board.Tiles;
using Monopoly.Game.Model.Tiles;
using Monopoly.Controlls;
namespace Monopoly.Play.View
{
    class Renderer : AbstractRenderer
    {
        private bool openTilePropertiesWindow = false;
        public static ContentManager content;
        private SpriteBatch spriteBatch;
        SpriteFont myFont;
        SpriteFont moneyFont;
        private List<Player> playersInLobby = null;

        public Window win;
       // private Timer timer;

        public Dice WhiteDice;
        public Dice BlackDice;

        public Dictionary<Pawn, Vector2> moneyLabels = new Dictionary<Pawn, Vector2>();
        private Dictionary<Pawn, Vector2> nickLabels = new Dictionary<Pawn, Vector2>();
        private Dictionary<Pawn, Template> templates = new Dictionary<Pawn, Template>();
        private Dictionary<Pawn, Figure> figuresInTemplates = new Dictionary<Pawn, Figure>();
        public Dictionary<Pawn, Figure> Figures = new Dictionary<Pawn, Figure>();

        private Figure figureBlue;
        private Figure figureGreen;
        private Figure figureRed;
        private Figure figureYellow;
        private int velocity;
        private Rectangle tileDestination;
        public Pawn pawn = Pawn.blue;

        private Panel panel;
        private Desktop desktop;
        public TextButton ButtonThrow;
        public TextBox textBoxForChat;

        private MyListBox listOfHistory;
        private MyListBox listForChat;

        private Background background;
        public Rectangle[] TileColliders;
        public TileOwnerNotification[] TileOwnerNotifications;
        public TileOwnerNotification[] FirstHouses;
        public TileOwnerNotification[] SecondHouses;
        public TileOwnerNotification[] ThirdHouses;
        public TileOwnerNotification[] FourthHouses;
        public TileOwnerNotification[] Hotels;
        public bool ShouldPlayerMove = false;
        private KeyboardState previousKeyboardEscape = Keyboard.GetState();
        public Renderer()
        {
            desktop = new Desktop();
            panel = new Panel();

            this.background = UIInitializer.CreateBackground(content);
            this.WhiteDice = UIInitializer.CreateWhiteDice(content);
            this.BlackDice = UIInitializer.CreateBlackDice(content);


            this.figureBlue = UIInitializer.CreateFigure(content, Pawn.blue);
            this.figureGreen = UIInitializer.CreateFigure(content, Pawn.green);
            this.figureRed = UIInitializer.CreateFigure(content, Pawn.red);
            this.figureYellow = UIInitializer.CreateFigure(content, Pawn.yellow);

            this.templates.Add(Pawn.blue, UIInitializer.CreateTemplate(content, Pawn.blue));
            this.templates.Add(Pawn.green, UIInitializer.CreateTemplate(content, Pawn.green));
            this.templates.Add(Pawn.red, UIInitializer.CreateTemplate(content, Pawn.red));           
            this.templates.Add(Pawn.yellow, UIInitializer.CreateTemplate(content, Pawn.yellow));

            this.figuresInTemplates.Add(Pawn.blue,UIInitializer.CreateFigureInTemplate(content, Pawn.blue));
            this.figuresInTemplates.Add(Pawn.green, UIInitializer.CreateFigureInTemplate(content, Pawn.green));
            this.figuresInTemplates.Add(Pawn.red, UIInitializer.CreateFigureInTemplate(content, Pawn.red));
            this.figuresInTemplates.Add(Pawn.yellow, UIInitializer.CreateFigureInTemplate(content, Pawn.yellow));
            //TODO
            this.Figures.Add(Pawn.blue,figureBlue);
            this.Figures.Add(Pawn.green,figureGreen);
            this.Figures.Add(Pawn.red,figureRed);
            this.Figures.Add(Pawn.yellow,figureYellow);

            this.ButtonThrow = UIInitializer.CreateThrowButton();

            this.TileColliders = UIInitializer.CreateTileColliders();
            this.TileOwnerNotifications = UIInitializer.CreateTileFlags(content);
            this.FirstHouses = UIInitializer.CreateFirstHouses(content);
            this.SecondHouses = UIInitializer.CreateSecondHouses(content);
            this.ThirdHouses = UIInitializer.CreateThirdHouses(content);
            this.FourthHouses = UIInitializer.CreateFourthHouses(content);
            this.Hotels = UIInitializer.CreateHotels(content);
            this.velocity = 400;

            this.listOfHistory = UIInitializer.CreateListOfHistory();
            this.textBoxForChat = UIInitializer.CreateTextBoxForChat();
            this.listForChat = UIInitializer.CreateListChat();

            nickLabels.Add(Pawn.blue, adjustResolutionXY(new Vector2(175, 30)));
            nickLabels.Add(Pawn.green, adjustResolutionXY(new Vector2(175, 910)));
            nickLabels.Add(Pawn.red, adjustResolutionXY(new Vector2(1680, 30)));
            nickLabels.Add(Pawn.yellow, adjustResolutionXY(new Vector2(1680, 910)));

            moneyLabels.Add(Pawn.blue, adjustResolutionXY(new Vector2(175, 130)));
            moneyLabels.Add(Pawn.green, adjustResolutionXY(new Vector2(175, 1010)));
            moneyLabels.Add(Pawn.red, adjustResolutionXY(new Vector2(1680, 130)));
            moneyLabels.Add(Pawn.yellow, adjustResolutionXY(new Vector2(1680, 1010)));

            myFont = content.Load<SpriteFont>("myFont");
            moneyFont = content.Load<SpriteFont>("moneyFont");

            panel.Widgets.Add(textBoxForChat);
            panel.Widgets.Add(ButtonThrow);
            panel.Widgets.Add(listOfHistory);
            panel.Widgets.Add(listForChat);

            previousKeyboardEscape = Keyboard.GetState();

            desktop.Root = panel;
        }
        internal void DialogOfEvent(string msg, bool buyHouse = false)
        {
            win = new Window();
            win.Height = 310;
            win.Width = 250;
            Panel dialogPanel = new Panel();
            win.Title = "";

            var label1 = new Myra.Graphics2D.UI.Label
            {
                Top = 20,
                Left = 20,
                MaxWidth = 210,
                Wrap = true
            };
            var Button1 = new TextButton()
            {
                Width = 100,
                Height = 30,
                Top = 240,
                Left = 250 / 2 - 90 / 2,
                Text= "Koupit"
                //Visible = false
            };
            dialogPanel.Widgets.Add(Button1);
            dialogPanel.Widgets.Add(label1);
            win.Content = dialogPanel;
            win.ShowModal(desktop);
            win.DragDirection = DragDirection.None;
            Button1.Click += (s, a) => { 
                if(!buyHouse)
                Communication.Query.BuyStreet(Query.GetThisPlayer());
                else
                    Communication.Query.BuyHouse(Query.GetThisPlayer());
                win.Close();
                
            };
            win.Closed += (s, a) =>
                {
                    Communication.Query.EndOfBuying();
                };
            label1.Text = msg;
        }

        public void InitializeComponent()
        {
        }
        
        public void Update()
        {
            if(Data.ThisPlayer.IDLobby == Guid.Empty)
            {
                var messageBox = Dialog.CreateMessageBox("Upozorneni", "Konec hry");
                messageBox.ButtonCancel.Enabled = false;
                messageBox.CloseButton.Enabled = false;
                messageBox.ButtonOk.Click += (a, b) => { GameState.ChangeGameState(GameStates.Menu); };
                messageBox.ShowModal(desktop);
            }
            if (previousKeyboardEscape.IsKeyUp(Keys.Escape) && Keyboard.GetState().IsKeyDown(Keys.Escape)
    && (GameState.GetCurrentState() != GameStates.Intro || GameState.GetCurrentState() != GameStates.Menu))
            {
                previousKeyboardEscape = Keyboard.GetState();
                var messageBox = Dialog.CreateMessageBox("Upozornění", "Chceš se vrátit do menu?");
                messageBox.ButtonOk.Click += (a, b) => { Data.ThisPlayer.Leave = true; GameState.graphics.IsFullScreen = false; Query.LeaveFromGame(Data.ThisPlayer); GameState.ChangeGameState(GameStates.Menu); };
                messageBox.ButtonCancel.Click += (a, b) => { previousKeyboardEscape = Keyboard.GetState(); };
                messageBox.CloseButton.Enabled = false;
                messageBox.ShowModal(desktop);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Enter) && textBoxForChat.Text != string.Empty && textBoxForChat.Text!=null)
            {
                if (textBoxForChat.Text.Length > 2 && textBoxForChat.Text[0] == '/' && textBoxForChat.Text[1] == 'w' && textBoxForChat.Text[2] == ' ')
                {
                    string nick = "";
                    string msg = "";
                    int index = 3;
                    for (int i = 3; i < textBoxForChat.Text.Length; i++)
                    {
                        if (textBoxForChat.Text[i] != ' ')
                        {
                            nick += textBoxForChat.Text[i];
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
                        messageBox.ShowModal(desktop);
                    }else
                    if (player.IDPlayer == Data.ThisPlayer.IDPlayer)
                    {
                        var messageBox = Dialog.CreateMessageBox("Chyba", "Nemůžeš poslat zprávu sám sobě!");
                        messageBox.ShowModal(desktop);
                    }
                    else
                    {
                        for (; index < textBoxForChat.Text.Length; index++)
                            msg += textBoxForChat.Text[index];
                        Chat.Chat chat = new Chat.Chat(Communication.Query.GetThisPlayer().Nick, 'P', player.IDPlayer);
                        chat.SendMessage(msg);
                    }
                }
                else
                {
                    Chat.Chat chat = new Chat.Chat(Communication.Query.GetThisPlayer().Nick, 'L', Communication.Query.GetActualLobby(false).IDLobby);
                    chat.SendMessage(textBoxForChat.Text);
                    textBoxForChat.Text = string.Empty;
                }
            }
            MouseState mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed && !openTilePropertiesWindow)
            {              
                var mousePoint = new Point(mouseState.X, mouseState.Y);
                for(int i=0; i< TileColliders.Length; i++)
                {
                    var grid = new Grid
                    {
                        RowSpacing = 30,
                        ColumnSpacing = 16
                    };
                    grid.ColumnsProportions.Add(new Proportion(ProportionType.Auto));
                    grid.RowsProportions.Add(new Proportion(ProportionType.Auto));

                    if (TileColliders[i].Contains(mousePoint))
                    {
                        Tile currentTile = Board.allTiles[i];
                        openTilePropertiesWindow = true;
                        win = new Window();
                        win.Height = 310;
                        win.Width = 250;
                        Panel dialogPanel = new Panel();
                        win.Title = "";

                        var label1 = new Myra.Graphics2D.UI.Label
                        {
                            Top = 20,
                            Left = 30,
                            GridColumn = 0,
                            HorizontalAlignment = HorizontalAlignment.Center,
                            GridRow = 0,
                            MaxWidth = 210,
                            Wrap = true,
                            TextAlign = TextAlign.Center
                        };
                        var label2 = new Myra.Graphics2D.UI.Label
                        {
                            //Top = 20,
                            //Left = 30,
                            GridColumn = 0,
                            GridRow = 1,
                            MaxWidth = 210,
                            Wrap = true,
                            Left = 15
                            //   PressedBackground = new SolidBrush("#FFFF00")
                        };
                        var listBox1 = new ListBox
                        {
                            Width = 200,
                            GridColumn = 0,
                            GridRow = 1,
                            Left = 25,
                         //   GridColumnSpan = 2                         
                        };
                        win.Closed += (s, a) =>
                        {
                            openTilePropertiesWindow = false;
                        };
                        // dialogPanel.Widgets.Add(label1);
                        grid.Widgets.Add(label1);
                      //  win.Content = dialogPanel;
                      win.Content = grid;
                        win.ShowModal(desktop);
                        label1.Text = currentTile.Name;
                        if(currentTile is Street)
                        {
                            Street street = (Street)currentTile;
                            //  dialogPanel.Widgets.Add(listBox1);
                            Player player = Query.FindPlayerByID(street.Owner);
                            string owner = "-";
                            if (player != null)
                                owner = player.Nick;
                            listBox1.Items.Add(addListItem(String.Format("vlastník: {0}", owner)));
                            listBox1.Items.Add(addListItem(String.Format("nájemné: {0}$", street.Rent)));
                            listBox1.Items.Add(addListItem(String.Format("s 1 domy: {0}$", street.RentWithHouses[0])));
                            listBox1.Items.Add(addListItem(String.Format("s 2 domy: {0}$", street.RentWithHouses[1])));
                            listBox1.Items.Add(addListItem(String.Format("s 3 domy: {0}$", street.RentWithHouses[2])));
                            listBox1.Items.Add(addListItem(String.Format("s 4 domy: {0}$", street.RentWithHouses[3])));
                            listBox1.Items.Add(addListItem(String.Format("s hotelem: {0}$", street.RentWithHouses[4])));
                            listBox1.Items.Add(addListItem(String.Format("")));
                            listBox1.Items.Add(addListItem(String.Format("cena pozemku: {0}$", street.Price)));
                            listBox1.Items.Add(addListItem(String.Format("cena domu: {0}$", street.PriceHouse)));
                            listBox1.Items.Add(addListItem(String.Format("cena hotelu: {0}$", street.PriceHouse)));

                            grid.Widgets.Add(listBox1);
                        }else
                        if(currentTile is DiceCard)
                        {
                            grid.RowsProportions.Add(new Proportion(ProportionType.Auto));
                            listBox1.GridRow = 2;
                            DiceCard diceCard = (DiceCard)currentTile;
                            Player player = Query.FindPlayerByID(diceCard.Owner);
                            string owner = "-";
                            if (player != null)
                                owner = player.Nick;
                            grid.Widgets.Add(label2);
                            grid.Widgets.Add(listBox1);
                            if (diceCard.Index != 32 && diceCard.Index != 18)
                            {
                                if(diceCard.Index == 28)
                                label2.Text = String.Format("Vlastníš-li vodovod obdržíš 5x tolik kolik je na kostkách. Vlastníš-li i telefon " +
                                    "10x tolik a máš-li i EL. proud 20x tolik.");
                                if (diceCard.Index == 12)
                                    label2.Text = String.Format("Vlastníš-li el. proud obdržíš 5x tolik kolik je na kostkách. Vlastníš-li i vodovod " +
                                        "10x tolik a máš-li i telefon 20x tolik, kolik je na kostkách.");
                                if (diceCard.Index == 8)
                                    label2.Text = String.Format("Vlastníš-li telefon obdržíš 5x tolik kolik je na kostkách. Vlastníš-li i vodovod obdržíš " +
                                        "10x tolik a máš-li i elektr. proud obdržíš 20x tolik.");
                                listBox1.Items.Add(addListItem(String.Format("Vlastník: {0}", owner)));
                                listBox1.Items.Add(addListItem(String.Format("")));
                                listBox1.Items.Add(addListItem(String.Format("kupní cena: {0}$", diceCard.Price)));
                            }
                            else
                            {
                                listBox1.Items.Add(addListItem(String.Format("Vlastník: {0}", owner)));
                                listBox1.Items.Add(addListItem(String.Format("")));
                                listBox1.Items.Add(addListItem(String.Format("kupní cena: {0}$", diceCard.Price)));
                                label2.Text = String.Format("Poplatek za poslech 50$, vlastníš-li druhou koncesi obdržíš 50x tolik, kolik je na kostkách.");
                            }
                        }else
                        if (currentTile is Train)
                        {
                            Train train = (Train)currentTile;
                            //  dialogPanel.Widgets.Add(listBox1);
                            Player player = Query.FindPlayerByID(train.Owner);
                            string owner = "-";
                            if (player != null)
                                owner = player.Nick;
                            listBox1.Items.Add(addListItem(String.Format("vlastník: {0}", owner)));
                            listBox1.Items.Add(addListItem(String.Format("jízdné: {0}$", train.Rent)));
                            listBox1.Items.Add(addListItem(String.Format("2 železnice: {0}$", train.Rent*2)));
                            listBox1.Items.Add(addListItem(String.Format("3 železnice: {0}$", train.Rent * 4)));
                            listBox1.Items.Add(addListItem(String.Format("4 železnice: {0}$", train.Rent * 8)));
                            listBox1.Items.Add(addListItem(String.Format("")));
                            listBox1.Items.Add(addListItem(String.Format("kupní cena: {0}$", train.Price)));

                            grid.Widgets.Add(listBox1);
                        }else
                        if(currentTile is SpecialTile)
                        {
                            SpecialTile specialTile = (SpecialTile)currentTile;
                            if (specialTile.Index == 0)
                                label2.Text = String.Format("Za zdolání GO hráč od banky dostává 200$.");
                            if (specialTile.Index == 10)
                                label2.Text = String.Format("Figurka, která se zastaví na poli Ve vězení je ve vězení pouze Na návštěvě, což znamená, že může v příštím tahu pokračovat ve hře a v postupu bez omezení.");
                            if (specialTile.Index == 20)
                                label2.Text = String.Format("Pole Parkování zdarma je pole, kde se nic neplatí a od nikoho se nic nedostává..");
                            if (specialTile.Index == 30)
                                label2.Text = String.Format("Když figurka zastaví na poli Jdi do vězení, tak se přesouvá na další 3 tahy do vězení a při průchodu cíle nemá právo na dotaci.");
                            grid.Widgets.Add(label2);
                        }else
                        if (currentTile is Tax)
                        {
                            Tax tax = (Tax)currentTile;
                            if (tax.Index == 4)
                                label2.Text = String.Format("Daň z příjmu musí hráč odevzdat 10% z hotových peněz i z ceny pozemků, domů a hotelů, které mu patří.");
                            if (tax.Index == 38)
                                label2.Text = String.Format("Při zastavení na poli Daň z luxusu zaplatí hráč bance 75$.");
                            grid.Widgets.Add(label2);
                        }else
                           if (currentTile is ChanceCard || currentTile is ChestCard)
                        {
                            label2.Text = String.Format("Když se figurka zastaví na polích pokladna, nebo  Náhoda, je hráči vylosována karta z příslušné skupiny karet a provede se její akce.");
                            grid.Widgets.Add(label2);
                        }
                        break;
                    }
                }
            }
        }
        private ListItem addListItem(string text)
        {
            var listItem1 = new ListItem();
            listItem1.Color = Color.Gold;
            listItem1.Text = string.Format(text);
            return listItem1;
        }
        public void addMessageToChatList(Chat.Chat chat)
        {
            ListItem listItem1 = new ListItem();
            if (chat.marker == 'P')
                listItem1.Color = Color.Yellow;
            else
                listItem1.Color = Color.White;
            listItem1.Text = chat.ToString();
            listForChat.Items.Add(listItem1);
            listForChat.SelectedIndex = listForChat.Items.Count - 1;
            textBoxForChat.Text = string.Empty;
        }

        public void Draw()
        {
          
        }

        public override void DrawBoard()
        {
            //  playersInLobby = Communication.Query.GetThisLobby().GetListOfPlayers();
            playersInLobby = Communication.Query.GetActualLobby(false).GetListOfPlayers();
            this.spriteBatch = Monopoly.spriteBatch;
            background.Draw(spriteBatch);
            WhiteDice.Draw(spriteBatch);
            BlackDice.Draw(spriteBatch);
            for (int i = 0; i < playersInLobby.Count; i++) //na figurky
            {
                templates[playersInLobby[i].Pawn].Draw(spriteBatch);
                figuresInTemplates[playersInLobby[i].Pawn].Draw(spriteBatch);
                Figures[playersInLobby[i].Pawn].Draw(spriteBatch);
                spriteBatch.DrawString(myFont, playersInLobby[i].Nick, nickLabels[playersInLobby[i].Pawn], Color.Black);
                spriteBatch.DrawString(moneyFont, (playersInLobby[i].Money).ToString(), moneyLabels[playersInLobby[i].Pawn], Color.White);
            }
            foreach (var notification in TileOwnerNotifications)
            {
                notification.Draw(spriteBatch);
            }
            foreach (var house in FirstHouses)
            {
                house.Draw(spriteBatch);
            }
            foreach (var house in SecondHouses)
            {
                house.Draw(spriteBatch);
            }
            foreach (var house in ThirdHouses)
            {
                house.Draw(spriteBatch);
            }
            foreach (var house in FourthHouses)
            {
                house.Draw(spriteBatch);
            }
            foreach (var hotel in Hotels)
            {
                hotel.Draw(spriteBatch);
            }
            // desktop.Render();
        }
        public void DrawRender()
        {
            try
            {
                desktop.Render();
            }
            catch(System.IndexOutOfRangeException ex)
            {
                Error.HandleError(ex);
            }
            catch (Exception ex)
            {
                Error.HandleError(ex);
            }
        }
        public void AddTextToHistoryList(string text)
        {
            if (listOfHistory.Items.Count == 0)
            {
                writeIntoHistoryList(text);
            }
            else
            {
                string txt = listOfHistory.Items[listOfHistory.Items.Count - 1].ToString();
                if (txt != (text + " "))
                {
                    writeIntoHistoryList(text);
                }
            }
        }

        private void writeIntoHistoryList(string text)
        {
            ListItem listItem1 = new ListItem();
            listItem1.Color = Color.White;
            listItem1.Text = text;
            listOfHistory.Items.Add(listItem1);
        }

        public override void MovePlayer(Pawn pawn, int currentPosition)
        {
            Figure currentFigure = Figures[pawn];
            tileDestination = TileColliders[currentPosition];
            ShouldPlayerMove = true;
                if (tileDestination.Contains(currentFigure.Sprite.Rectangle))
                {
                //Communication.Query.GetThisPlayer().IsOnMove = false;
                ShouldPlayerMove = false;
            }
                else
                {
                    if (currentFigure.Sprite.Rectangle.Y > adjustResolutionY(950) &&
                        currentFigure.Sprite.Rectangle.X > adjustResolutionX(470))
                    {
                        currentFigure.Sprite.Rectangle.X -= (int)(velocity * Program.Game.Elapsed);
                    }
                    else if (currentFigure.Sprite.Rectangle.X <= adjustResolutionX(470) &&
                       currentFigure.Sprite.Rectangle.Y > adjustResolutionY(50))
                    {
                        currentFigure.Sprite.Rectangle.Y -=
                            (int)(velocity * Program.Game.Elapsed);
                    }
                    else if(currentFigure.Sprite.Rectangle.Y <= adjustResolutionY(140)
                        && currentFigure.Sprite.Rectangle.X < adjustResolutionX(1450))
                    {
                        currentFigure.Sprite.Rectangle.X +=
                            (int)(velocity * Program.Game.Elapsed);
                    }else if(currentFigure.Sprite.Rectangle.X >= adjustResolutionX(1360)
                        && currentFigure.Sprite.Rectangle.Y < adjustResolutionY(1080))
                    {
                        currentFigure.Sprite.Rectangle.Y += (int)(velocity * Program.Game.Elapsed);
                    }
                }
            }
        private int adjustResolutionX(int x)
        {
            int x_width = Program.Game.GraphicsDevice.DisplayMode.Width;
            const int x_width_fullHD = 1920;
            x = (int)((x_width * x) / (double)x_width_fullHD);
            return x;
        }
        private int adjustResolutionY(int y)
        {
            int y_height = Program.Game.GraphicsDevice.DisplayMode.Height;
            const int y_height_fullHD = 1080;
            y = (int)((y_height * y) / (double)y_height_fullHD);
            return y;
        }
        //ShowTileOwner
        public override void ShowTileOwner(Pawn pawn, int currentPlayerPosition)
        {
            for(int i=0; i<this.TileOwnerNotifications.Length;i++)
            {
                if(this.TileOwnerNotifications[i].BoardIndex == currentPlayerPosition)
                {
                    this.TileOwnerNotifications[i].SetOwner(pawn);
                    break;
                }
            }
        }
        public void ShowHouses(Pawn pawn, Street street)
        {
            for (int i = 0; i < this.FirstHouses.Length; i++)
            {
                if (street.Index == this.FirstHouses[i].BoardIndex)
                {
                        if(street.Houses[0] == true)
                        {
                            this.FirstHouses[i].SetOwner(pawn);
                        }
                    if (street.Houses[1] == true)
                    {
                        this.SecondHouses[i].SetOwner(pawn);
                    }
                    if (street.Houses[2] == true)
                    {
                        this.ThirdHouses[i].SetOwner(pawn);
                    }
                    if (street.Houses[3] == true)
                    {
                        this.FourthHouses[i].SetOwner(pawn);
                    }
                    if (street.Houses[4] == true)
                    {
                        this.Hotels[i].SetOwner(pawn);
                    }
                    break;
                }
            }
        }
        private static Vector2 adjustResolutionXY(Vector2 location)
        {
            int x_width = Program.Game.GraphicsDevice.DisplayMode.Width;
            int y_height = Program.Game.GraphicsDevice.DisplayMode.Height;
            const int x_width_fullHD = 1920;
            const int y_height_fullHD = 1080;
            location.X = (int)((x_width * location.X) / (double)x_width_fullHD);
            location.Y = (int)((y_height * location.Y) / (double)y_height_fullHD);
            return location;
        }
    }
}
