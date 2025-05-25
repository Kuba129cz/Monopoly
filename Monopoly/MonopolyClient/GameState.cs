using Microsoft.Xna.Framework;
using Monopoly.MonopolyGame.Controller;
using Monopoly.Play.View;
using Myra.Graphics2D.UI;
using System;

namespace Monopoly
{
    public enum GameStates { Intro, Menu, Room, Lobby, Game, MatchHistory}
    static class  GameState
    {
        public static GraphicsDeviceManager graphics { set; get; }

        private static Intro.Intro myIntro;
        private static Menu.Menu myMenu;
        private static Rooms.DesignRoom myRoom;
        private static Lobby.DesignLobby myLobby;
        private static Renderer myRenderer;
        private static Desktop myDesktop;
        private static MatchHistory.MatchHistory matchHistory;
        // private static StateMachine stateMachine;
        private const int GAME_HEIGHT = 720;
        private const int GAME_WIDTH = 1280;
        private static GameStates currentState;
        public static void InitializeIntro(Intro.Intro intro)
        {
            myIntro = intro;
            graphics.HardwareModeSwitch = false;
        }
        public static void InitializeRoom(Rooms.DesignRoom room)
        {
            myRoom = room;
        }
        public static void InitializeMenu(Menu.Menu menu)
        {
            myMenu = menu;
        }
        public static void InitializeLobby(Lobby.DesignLobby lobby)
        {
            myLobby = lobby;
        }
        public static void InitializeMatchHistory(MatchHistory.MatchHistory match)
        {
            matchHistory = match;
        }
        public static GameStates GetCurrentState()
        {
            return currentState;
        }
        public static Renderer GetRenderer()
        {
            return myRenderer;
        }
        public static Rooms.DesignRoom GetRoom()
        {
            return myRoom;
        }
        public static Lobby.DesignLobby GetLobby()
        {
            return myLobby;
        }
        public static Desktop GetDesktop()
        {
            return myDesktop;
        }
        public static void ChangeGameState(GameStates state)
        {
            try
            {
                switch (state)
                {
                    case GameStates.Intro:
                        {
                            currentState = GameStates.Intro;
                            ChangeResolutionIntro();
                            myIntro.LoadContent();
                            hideAll();
                            myIntro.ShowIntro();
                            break;
                        }
                    case GameStates.Menu:
                        {
                            currentState = GameStates.Menu;
                            ChangeResolutionMenu();
                            myMenu.LoadContent();
                            hideAll();
                            break;
                        }
                    case GameStates.Room:
                        {
                            currentState = GameStates.Room;
                            ChangeResolutionRoom();
                            myRoom.LoadContent();
                            myRoom.ShowWidgets();
                            hideAll();
                            break;
                        }
                    case GameStates.Lobby:
                        {
                            currentState = GameStates.Lobby;
                            ChangeResolutionLobby();
                            myLobby.LoadContent();
                            hideAll();
                            break;
                        }
                    case GameStates.MatchHistory:
                        {
                            currentState = GameStates.MatchHistory;
                            ChangeResolutionMatchHistory();
                            matchHistory.LoadContent();
                            hideAll();
                            break;
                        }
                    case GameStates.Game:
                        {
                            currentState = GameStates.Game;
                            StateMachine.Initialize();
                            StateMachine.CurrentState = StateMachine.States["InitialState"];
                            StateMachine.CurrentState.Execute();
                            StateMachine.ChangeState();
                            ChangeResolutionGame();
                            //    DesignLobby.timer.Enabled = false;
                            //hideAll();
                            break;
                        }
                }
            }catch(Exception ex)
            {
                Error.HandleError(ex);
            }
        }

        internal static void InitializeRenderer(Renderer renderer)
        {
            myRenderer = renderer;
        }
        internal static void InitializeDesktop(Desktop desktop)
        {
            myDesktop = desktop;
        }
        private static void hideAll()
        {
            if(currentState != GameStates.Intro && myIntro!=null)
            myIntro.HideIntro();
            if (currentState != GameStates.Room && myRoom!=null)
                myRoom.HideWidgets();
            if (currentState != GameStates.Lobby && myRoom != null)
                myLobby.HideDesignLobby();
            if (currentState != GameStates.MatchHistory && matchHistory != null)
                matchHistory.Hide();
        }
        private static void ChangeResolutionGame()
        {
                //graphics.PreferredBackBufferHeight = 690;
                //graphics.PreferredBackBufferWidth = 1280;
                //graphics.IsFullScreen = false;
                graphics.PreferredBackBufferWidth = Program.Game.GraphicsDevice.DisplayMode.Width;
                graphics.PreferredBackBufferHeight = Program.Game.GraphicsDevice.DisplayMode.Height;
                graphics.IsFullScreen = true;

                graphics.ApplyChanges();
        }
        private static void ChangeResolutionLobby()
        {
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 690;
            graphics.ApplyChanges();
        }
        private static void ChangeResolutionMatchHistory()
        {
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 690;
            graphics.ApplyChanges();
        }
        private static void ChangeResolutionRoom()
        {
            // Rooms.Widgets.SetFragmets();
            //graphics.PreferredBackBufferHeight = 600;
            //graphics.PreferredBackBufferWidth = 500;
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 690;
            graphics.ApplyChanges();
        }

        private static void ChangeResolutionMenu()
        {
            // Rooms.Widgets.SetFragmets();
            //graphics.PreferredBackBufferWidth = 1200;
            //graphics.PreferredBackBufferHeight = 800;
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 690;
            graphics.ApplyChanges();
        }

        private static void ChangeResolutionIntro()
        {
            //graphics.PreferredBackBufferHeight = 200;
            //graphics.PreferredBackBufferWidth = 300;
            
            Program.Game.Window.AllowUserResizing = true;
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 690;
            graphics.ApplyChanges();

        }
        public static void ShowMessageBox(string str)//vypise messageBox v prislusnym okne
        {
            switch (currentState)
            {
                case GameStates.Intro:
                    {
                        myIntro.ShowMessageBox(str);
                        break;
                    }
                case GameStates.Menu:
                    {
                        break;
                    }
                case GameStates.Room:
                    {
                        break;
                    }
                case GameStates.Lobby:
                    {
                        break;
                    }
                case GameStates.MatchHistory:
                    {
                        break;
                    }
                case GameStates.Game:
                    {
                        break;
                    }
            }
        }
    }
}
