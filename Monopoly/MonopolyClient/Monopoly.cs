using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Monopoly.Communication;
using Monopoly.MonopolyGame.Controller;
using Monopoly.Play.View;
using Myra.Graphics2D.UI;
using System;

namespace Monopoly
{
    public class Monopoly : Microsoft.Xna.Framework.Game
    {
        public static SpriteBatch spriteBatch;
        public double Elapsed;
        private Intro.Intro intro;
        private Menu.Menu menu;
        private Rooms.DesignRoom Designroom;
        private Lobby.DesignLobby loby;
        private MatchHistory.MatchHistory matchHistory;
        Renderer renderer;
        public Monopoly()
        {
            GameState.graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
           // Communication.Query qu = new Communication.Query(); 
            Window.AllowUserResizing = true;
        //    Content.Load<SpriteFont>("Fonts\\ComicSansMS");
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            intro = new Intro.Intro(this);
            GameState.InitializeIntro(intro);
            GameState.ChangeGameState(GameStates.Intro);

            matchHistory = new MatchHistory.MatchHistory();
            matchHistory.InitializeComponent();
            GameState.InitializeMatchHistory(matchHistory);

            menu = new Menu.Menu(this);
            GameState.InitializeMenu(menu);
            menu.InitializeComponent(); //zatim zbytecne

            Designroom = new Rooms.DesignRoom(this);
            GameState.InitializeRoom(Designroom);

            //this.Components.Add(Designroom);

            loby = new Lobby.DesignLobby();
            GameState.InitializeLobby(loby);
            intro.InitializeComponent();

            spriteBatch = new SpriteBatch(GraphicsDevice);
            Renderer.content = this.Content;
            renderer = new Renderer();
            GameState.InitializeRenderer(renderer);
            base.Initialize();
        }

        protected override void LoadContent()
        {
           // intro.LoadContent(); docasne
        }

        protected override void Update(GameTime gameTime)
        {
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)
            //    && (GameState.GetCurrentState() != GameStates.Intro || GameState.GetCurrentState() != GameStates.Menu))
            //{
            //    Desktop desktop = new Desktop();
            //    var messageBox = Dialog.CreateMessageBox("Upozornění", "Chceš se vrátit do menu?");
            //    messageBox.ButtonOk.Click += (a, b) => { GameState.ChangeGameState(GameStates.Menu); };
            //    messageBox.ShowModal(desktop);
            //}
            // TODO: Add your update logic here
            Elapsed = (double)gameTime.ElapsedGameTime.TotalSeconds;
            if (GameState.GetCurrentState() == GameStates.Game)
            {
                StateMachine.CurrentState.Execute();
            }
            if (GameState.GetCurrentState() == GameStates.Intro)
                intro.Update();
            if (GameState.GetCurrentState() == GameStates.Menu)
                menu.Update();
            if (GameState.GetCurrentState() == GameStates.Room)
                Designroom.Update();
            if (GameState.GetCurrentState() == GameStates.Lobby)
                loby.Update();
            if (GameState.GetCurrentState() == GameStates.Game)
                renderer.Update();
            if (GameState.GetCurrentState() == GameStates.MatchHistory)
                matchHistory.Update();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            GraphicsDevice.Clear(Color.CornflowerBlue);

            if (Data.LostConnection)
            {
                Data.LostConnection = false;
                GameState.ChangeGameState(GameStates.Intro);
                GameState.ShowMessageBox(Data.LostConnectionMessage);
            }

            if (GameState.GetCurrentState() == GameStates.Intro)
                intro.Draw();
            if (GameState.GetCurrentState() == GameStates.Menu)
                menu.Draw();
            if (GameState.GetCurrentState() == GameStates.Room)
                Designroom.Draw();
            if (GameState.GetCurrentState() == GameStates.Lobby)
                loby.Draw();
            if (GameState.GetCurrentState() == GameStates.Game)
            {
                StateMachine.CurrentState.Draw(renderer);
            }
            if (GameState.GetCurrentState() == GameStates.MatchHistory)
            {
                matchHistory.Draw();
            }
            spriteBatch.End();

            if (GameState.GetCurrentState() == GameStates.Game)
                renderer.DrawRender();

            base.Draw(gameTime);
        }

        internal void ConnectionFailed()
        {
            if (GameState.GetCurrentState() == GameStates.Intro)
                intro.ConnectionFailed();
        }
        protected override void OnExiting(object sender, System.EventArgs args)
        {
            Communication.Query.RemovePlayer();
            base.OnExiting(sender, args);
            // Stop the threads
        }

    }
}
