using FontStashSharp;
using Microsoft.Xna.Framework;
using Monopoly.Controlls;
using Myra;
using Myra.Graphics2D.Brushes;
using Myra.Graphics2D.TextureAtlases;
using Myra.Graphics2D.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Monopoly.MatchHistory
{
    partial class MatchHistory
    {
		private Label label3;
		private void buildUI()
		{
			int windowWith = Program.Game.Window.ClientBounds.Width;
			int windowHeight = Program.Game.Window.ClientBounds.Height;

			var label1 = new Label();
			label1.Text = "Monopoly - STATISTIKY";
			label1.Font = MyraEnvironment.DefaultAssetManager.Load<SpriteFontBase>("fonts/arial64.fnt");
			label1.TextColor = Color.LightBlue;
			label1.HorizontalAlignment = Myra.Graphics2D.UI.HorizontalAlignment.Center;

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
			label3.HorizontalAlignment = Myra.Graphics2D.UI.HorizontalAlignment.Right;
			label3.VerticalAlignment = Myra.Graphics2D.UI.VerticalAlignment.Top;

			label4 = new Label();
			label4.Font = MyraEnvironment.DefaultAssetManager.Load<SpriteFontBase>("fonts/calibri32.fnt");
			//label4.Text = "status/ doba v zápase/ Datum/ frekventanti/ kol/ název lobby";
			label4.HorizontalAlignment = Myra.Graphics2D.UI.HorizontalAlignment.Center;
			label4.Top = 10 * windowHeight / 100;

			button1 = new TextButton()
			{
				Font = MyraEnvironment.DefaultAssetManager.Load<SpriteFontBase>("fonts/calibri32.fnt"),				
				Text = "Vrátit se",
				Left = 300,
				Top = 80 * windowHeight / 100,
				Width = 150,
				Height = 60
			};
			if(windowWith!=0)
			button1.Left = 20 * windowWith / 100 - (windowWith / 100) * 150 * 100 / windowWith;

			button2 = new TextButton()
			{
				Font = MyraEnvironment.DefaultAssetManager.Load<SpriteFontBase>("fonts/calibri32.fnt"),
				Text = "Podrobnosti",
				Left = 80 * windowWith / 100,
				Top = 80 * windowHeight / 100,
				Width = 150,
				Height = 60
			};
			listBox1 = new MyListBox
			{
				Width = 50 * windowWith / 100,
				Height = 60 * windowHeight / 100,
				HorizontalAlignment = Myra.Graphics2D.UI.HorizontalAlignment.Center,
			    VerticalAlignment = Myra.Graphics2D.UI.VerticalAlignment.Center
		};
			panel.Background = new SolidBrush("#C78100FF");
			panel.Widgets.Add(label1);
			panel.Widgets.Add(image1);
			panel.Widgets.Add(label2);
			panel.Widgets.Add(label3);
			panel.Widgets.Add(label4);
			panel.Widgets.Add(button1);
			panel.Widgets.Add(button2);
			panel.Widgets.Add(listBox1);

			Program.Game.Window.ClientSizeChanged += clientSizeChanged;
			button1.Click += button1Click;
			button2.Click += detailedStatistics;

            if (basicStatis)
            {
                button2.Visible = true;
                button2.Enabled = true;
            }
            else
            {
                button2.Visible = false;
                button2.Enabled = false;
            }
        }

        private void button1Click(object sender, EventArgs e)
        {
            if(basicStatis)
            {
				GameState.ChangeGameState(GameStates.Menu);

			}else
            {
				basicStatistics(sender, e);
			}
        }
    }
	}
