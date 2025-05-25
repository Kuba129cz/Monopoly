using FontStashSharp;
using Microsoft.Xna.Framework;
using Myra;
using Myra.Graphics2D;
using Myra.Graphics2D.Brushes;
using Myra.Graphics2D.TextureAtlases;
using Myra.Graphics2D.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Monopoly.Intro
{
   partial class Intro
    {
		
		private void buildUI()
		{
			 var grid = new Grid
			{
				ShowGridLines = false,
				ColumnSpacing = 8,
				RowSpacing = 8,
				//Background = new SolidBrush("#FFA500FF"),
				//Border = new SolidBrush("#008000")
			};
			
			//label3.Text = "";
			int windowWith = Program.Game.Window.ClientBounds.Width;
			int windowHeight = Program.Game.Window.ClientBounds.Height;

			Program.Game.Window.ClientSizeChanged += clientSizeChanged;
			//panel.Width = windowWith;
			//panel.Height = windowHeight;

			var label4 = new Label();
			label4.Text = "Monopoly";
			label4.Font = MyraEnvironment.DefaultAssetManager.Load<SpriteFontBase>("fonts/arial64.fnt");
			label4.TextColor = Color.LightBlue;
			label4.HorizontalAlignment = Myra.Graphics2D.UI.HorizontalAlignment.Center;

 		    var image1 = new Image();
			image1.Renderable = MyraEnvironment.DefaultAssetManager.Load<TextureRegion>("images/IntroMan.png");
			image1.Left = 10;
			image1.Top = -10;
			image1.VerticalAlignment = Myra.Graphics2D.UI.VerticalAlignment.Bottom;
			//image1.HorizontalAlignment = Myra.Graphics2D.UI.HorizontalAlignment.Right;
			image1.Left = (windowWith / 100) * 30;
			image1.Height = windowHeight - 100;
			image1.Width = (windowWith / 100) * 70;
			//image1.Width = 64;

			var image2 = new Image();
			image2.Renderable = MyraEnvironment.DefaultAssetManager.Load<TextureRegion>("images/LogoOnly_64px.png");
			image2.Left = 10;
			image2.Top = -10;
			image2.VerticalAlignment = Myra.Graphics2D.UI.VerticalAlignment.Bottom;
			image2.Height = 64;
			image2.Width = 64;

			var label3 = new Label();
			label3.Text = "Verze 1.0";
			label3.Font = MyraEnvironment.DefaultAssetManager.Load<SpriteFontBase>("fonts/calibri32.fnt");
			label3.Left = -10;
			label3.Top = -10;
			label3.HorizontalAlignment = Myra.Graphics2D.UI.HorizontalAlignment.Right;
			label3.VerticalAlignment = Myra.Graphics2D.UI.VerticalAlignment.Bottom;

			button3 = new TextButton();
			button3.Text = "Server";
			button3.Left = -20;
			button3.Top = 10;
			button3.Width = 100;
			button3.Height = 30;
			button3.HorizontalAlignment = Myra.Graphics2D.UI.HorizontalAlignment.Right;
			button3.VerticalAlignment = Myra.Graphics2D.UI.VerticalAlignment.Top;
			button3.Click += button3Clicked;
			button3.Background = new SolidBrush("#FF0000");

			//label3.Left = (windowWith / 100) * 5;
			label2 = new Label();
			label2.Id = "label2";
			label2.Text = "Zadej nick:";
			label2.Margin = new Thickness(10,0,0,0);
			//label3.BorderThickness = new Thickness(24, 0);
			//label3.Padding = new Thickness(20, 20, 0, 0);
			label2.GridColumn = 0;
			label2.GridRow = 1;

			//textBox1.Left = (windowWith / 100) * 10 + 30;
			textBox1 = new TextBox();
			textBox1.GridColumn = 1;
            textBox1.GridRow = 1;
			textBox1.Width = 200;
			//         textBox1.BorderThickness = new Thickness(24, 0);
			//textBox1.Padding = new Thickness(20);
			textBox2 = new TextBox
			{
				GridColumn = 1,
				GridRow = 2,
				PasswordField = true
			};
			label1 = new Label
			{
				Text = "Zadej heslo:",
				Margin = new Thickness(10, 0, 0, 0),
				GridColumn = 0,
				GridRow = 2,
			};
			button1 = new TextButton
			{
				Text = "Přihlásit",
				Margin = new Thickness(20,10,10,10),
				Width = 150,
				Height = 60,
				GridColumn = 1,
				GridRow = 3,				
			};
			button1.Click += login;
			button2 = new TextButton
			{
				Text = "Registrovat",
				Margin = new Thickness(20, 30, 10, 10),
				Width = 130,
				Height = 50,
				GridColumn = 0,
				GridRow = 4,
			};
			button2.Click += registration;
			grid.ColumnsProportions.Add(new Proportion());
			grid.ColumnsProportions.Add(new Proportion());

			grid.RowsProportions.Add(new Proportion
			{
				Type = ProportionType.Pixels,
				Value = 64
			});
			grid.RowsProportions.Add(new Proportion(ProportionType.Auto));
			grid.RowsProportions.Add(new Proportion(ProportionType.Auto));
			grid.RowsProportions.Add(new Proportion(ProportionType.Auto));
			grid.RowsProportions.Add(new Proportion(ProportionType.Auto));
			grid.RowsProportions.Add(new Proportion(ProportionType.Auto));

			grid.Widgets.Add(label1);
			grid.Widgets.Add(textBox1);
			grid.Widgets.Add(textBox2);
			grid.Widgets.Add(label2);
			grid.Widgets.Add(button1);
			grid.Widgets.Add(button2);

			grid.Background = new SolidBrush("#C78100FF");

			panel.Widgets.Add(grid);
			panel.Widgets.Add(label3);
			panel.Widgets.Add(image1);
			panel.Widgets.Add(image2);
			panel.Widgets.Add(label4);
			panel.Widgets.Add(button3);
		}

        private void clientSizeChanged(object sender, EventArgs e)
        {
			//int windowWith = Program.Game.Window.ClientBounds.Width;
			//int windowHeight = Program.Game.Window.ClientBounds.Height;
			//if (windowWith < 980 || windowHeight < 420)
			//{
			//	GameState.graphics.PreferredBackBufferWidth = 980;
			//	GameState.graphics.PreferredBackBufferHeight = 420;
			//	GameState.graphics.ApplyChanges();
			//}
			buildUI();
		}

        internal void ConnectionFailed()
        {
			var messageBox = Dialog.CreateMessageBox("Chyba", "Nepodařilo se připojit k serveru.");
			messageBox.ShowModal(desktop);
		}
    }
}
