using Myra.Graphics2D;
using Myra.Graphics2D.TextureAtlases;
using Myra.Graphics2D.UI;
using Myra.Graphics2D.Brushes;
using FontStashSharp;
using Myra;
using Microsoft.Xna.Framework;
using Monopoly.Communication;

namespace Monopoly.Menu
{
	partial class Menu:Panel
    {
		private MenuItem _menuItemStartNewGame;
		private MenuItem _menuItemStatistics;
		private MenuItem _menuItemQuit;
		private VerticalMenu _mainMenu;
		private Label label3;
		private void buildUI()
		{
			var label1 = new Label();
			label1.Text = "Monopoly";
			label1.Font = MyraEnvironment.DefaultAssetManager.Load<SpriteFontBase>("fonts/arial64.fnt");
			label1.TextColor = Color.LightBlue;
			label1.HorizontalAlignment = Myra.Graphics2D.UI.HorizontalAlignment.Center;

			_menuItemStartNewGame = new MenuItem();
			_menuItemStartNewGame.Text = "Hrát";
			_menuItemStartNewGame.Id = "_menuItemStartNewGame";

			_menuItemStatistics = new MenuItem();
			_menuItemStatistics.Text = "Statistiky";
			_menuItemStatistics.Id = "_menuItemStatistics";

			_menuItemQuit = new MenuItem();
			_menuItemQuit.Text = "Odejít";
			_menuItemQuit.Id = "_menuItemQuit";

			_mainMenu = new VerticalMenu();
			_mainMenu.HorizontalAlignment = Myra.Graphics2D.UI.HorizontalAlignment.Center;
			_mainMenu.VerticalAlignment = Myra.Graphics2D.UI.VerticalAlignment.Center;
			_mainMenu.LabelFont = MyraEnvironment.DefaultAssetManager.Load<SpriteFontBase>("fonts/comicSans48.fnt");
			_mainMenu.LabelColor = Color.Indigo;
			_mainMenu.SelectionHoverBackground = new SolidBrush("#808000FF");
			_mainMenu.SelectionBackground = new SolidBrush("#FFA500FF");
			_mainMenu.LabelHorizontalAlignment = Myra.Graphics2D.UI.HorizontalAlignment.Center;
			_mainMenu.HoverIndexCanBeNull = false;
			_mainMenu.Background = new SolidBrush("#00000000");
			_mainMenu.Border = new SolidBrush("#00000000");
			_mainMenu.Id = "_mainMenu";
			_mainMenu.Items.Add(_menuItemStartNewGame);
			_mainMenu.Items.Add(_menuItemStatistics);
			_mainMenu.Items.Add(_menuItemQuit);

			var image1 = new Image();
			image1.Renderable = MyraEnvironment.DefaultAssetManager.Load<TextureRegion>("images/LogoOnly_64px.png");
			image1.Left = 10;
			image1.Top = -10;
			image1.VerticalAlignment = Myra.Graphics2D.UI.VerticalAlignment.Bottom;
			image1.Height = 64;
			image1.Width = 64;

			var label2 = new Label();
			label2.Text = "Verze 1.0";
			label2.Font = MyraEnvironment.DefaultAssetManager.Load<SpriteFontBase>("fonts/calibri32.fnt");
			label2.Left = -10;
			label2.Top = -10;
			label2.HorizontalAlignment = Myra.Graphics2D.UI.HorizontalAlignment.Right;
			label2.VerticalAlignment = Myra.Graphics2D.UI.VerticalAlignment.Bottom;

			label3 = new Label();
			label3.Font = MyraEnvironment.DefaultAssetManager.Load<SpriteFontBase>("fonts/calibri32.fnt");
			label3.Left = -20;
			//label3.Top = -50;
			label3.HorizontalAlignment = Myra.Graphics2D.UI.HorizontalAlignment.Right;
			label3.VerticalAlignment = Myra.Graphics2D.UI.VerticalAlignment.Top;

			Background = new SolidBrush("#C78100FF");
			Widgets.Add(label1);
			Widgets.Add(_mainMenu);
			Widgets.Add(image1);
			Widgets.Add(label2);
			Widgets.Add(label3);

			this._menuItemQuit.Selected += (s, a) => Program.Game.Exit();
			this._menuItemStartNewGame.Selected += (s, a) => {
				Communication.Query.UpdateLobbies();
				GameState.ChangeGameState(GameStates.Room); };
			this._menuItemStatistics.Selected += (s, a) => { GameState.ChangeGameState(GameStates.MatchHistory); };
		}
	}
}
