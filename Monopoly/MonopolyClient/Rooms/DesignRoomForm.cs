using FontStashSharp;
using Microsoft.Xna.Framework;
using Monopoly.Controlls;
using Myra;
using Myra.Graphics2D.Brushes;
using Myra.Graphics2D.TextureAtlases;
using Myra.Graphics2D.UI;
using System;

namespace Monopoly.Rooms
{
    partial class DesignRoom
    {
        private MenuItem _menuItemJoinGame;
        private MenuItem _menuItemCreateGame;
        private MenuItem _menuItemQuit;
        private HorizontalMenu _mainMenu;
		private static MyListBox chatList;
		private static ListBox listBox1;
		private static ListBox playersList;
		private TextBox textBox1;
		private Panel panel;
		private void buildUI()
		{
			var label1 = new Label();
			label1.Text = "Monopoly";
			label1.Font = MyraEnvironment.DefaultAssetManager.Load<SpriteFontBase>("fonts/arial64.fnt");
			label1.TextColor = Color.LightBlue;
			label1.HorizontalAlignment = Myra.Graphics2D.UI.HorizontalAlignment.Center;

			int windowWith = Program.Game.Window.ClientBounds.Width;
			int windowHeight = Program.Game.Window.ClientBounds.Height;

			_menuItemQuit = new MenuItem();
			_menuItemQuit.Text = "Opustit";
			_menuItemQuit.Id = "_menuItemQuit";
			_menuItemQuit.ShortcutColor = Color.Red;

			_menuItemCreateGame = new MenuItem();
			_menuItemCreateGame.Text = "Založit";
			_menuItemCreateGame.Id = "_menuItemCreateGame";

			_menuItemJoinGame = new MenuItem();
			_menuItemJoinGame.Text = "Připojit";
			_menuItemJoinGame.Id = "_menuItemJoinGame";

			_mainMenu = new HorizontalMenu();
			_mainMenu.HorizontalAlignment = Myra.Graphics2D.UI.HorizontalAlignment.Left;
			_mainMenu.VerticalAlignment = Myra.Graphics2D.UI.VerticalAlignment.Top;
			_mainMenu.Top = 45;
			_mainMenu.Left = 20;
			_mainMenu.Margin = new Myra.Graphics2D.Thickness(0, 10, 0, 0);
			_mainMenu.LabelFont = MyraEnvironment.DefaultAssetManager.Load<SpriteFontBase>("fonts/ComicSans.fnt");
			_mainMenu.LabelColor = Color.Indigo;
			_mainMenu.SelectionHoverBackground = new SolidBrush("#808000FF");
			_mainMenu.SelectionBackground = new SolidBrush("#FFA500FF");
			_mainMenu.LabelHorizontalAlignment = Myra.Graphics2D.UI.HorizontalAlignment.Center;
			_mainMenu.HoverIndexCanBeNull = false;
			_mainMenu.Background = new SolidBrush("#00000000");
			_mainMenu.Border = new SolidBrush("#00000000");
			_mainMenu.Id = "_mainMenu";


			_mainMenu.Items.Add(_menuItemQuit);
			_mainMenu.Items.Add(_menuItemCreateGame);
			_mainMenu.Items.Add(_menuItemJoinGame);

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

			listBox1 = new ListBox();
			listBox1.Width = 200;
			listBox1.MaxHeight = 400;
			listBox1.Top = 150;
			listBox1.Left = 180;
			listBox1.TouchDoubleClick += btnEnter_Clicked;

			playersList = new ListBox()
			{
				Width = 180,
				Height = 200,
				VerticalAlignment = Myra.Graphics2D.UI.VerticalAlignment.Center,
				HorizontalAlignment = Myra.Graphics2D.UI.HorizontalAlignment.Center,
				Margin = new Myra.Graphics2D.Thickness(100, 100, 100, 100),
			};
			playersList.TouchDoubleClick += (a, b) => { textBox1.Text = "/w " + playersList.SelectedItem.Text; };
			var label4 = new Label
			{
				Text = "Hráči na serveru",
				HorizontalAlignment = Myra.Graphics2D.UI.HorizontalAlignment.Center,
				Top = windowHeight/2 -130,
			};
			var label3 = new Label
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
				Margin = new Myra.Graphics2D.Thickness(0, 0, 10*windowWith/100, 0)
			};
			textBox1 = new TextBox()
			{
				Width = 220,
				Top = 170 + (int)chatList.Height,
                HorizontalAlignment = Myra.Graphics2D.UI.HorizontalAlignment.Right,
                Margin = new Myra.Graphics2D.Thickness(0, 0, 10 * windowWith / 100, 0)
            };

			panel.Background = new SolidBrush("#C78100FF");
			panel.Widgets.Add(_mainMenu);
			panel.Widgets.Add(image1);
			panel.Widgets.Add(label1);
			panel.Widgets.Add(label2);
			panel.Widgets.Add(label3);
			panel.Widgets.Add(label4);
			panel.Widgets.Add(listBox1); 
			panel.Widgets.Add(textBox1);
			panel.Widgets.Add(chatList);
			panel.Widgets.Add(playersList);

			this._menuItemJoinGame.Selected += btnEnter_Clicked;
			this._menuItemCreateGame.Selected += btnAddRoom_clicked;
			this._menuItemQuit.Selected += btnQuit_clicked;
		}
    }
}
