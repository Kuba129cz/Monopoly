using Myra.Graphics2D;
using Myra.Graphics2D.TextureAtlases;
using Myra.Graphics2D.UI;
using Myra.Graphics2D.Brushes;
using FontStashSharp;
using Myra;
using Microsoft.Xna.Framework;
using System.Collections.ObjectModel;
using Monopoly.Communication;

namespace Monopoly.Menu
{
   partial class Menu
    {
        private Desktop desktop;
		public Menu(Microsoft.Xna.Framework.Game game)
        {
            MyraEnvironment.Game = game;
            buildUI();
        }
        public void InitializeComponent()
        {
            desktop = new Desktop();
        }
        public void LoadContent()
        {
            desktop.Root = this;
            label3.Text = Data.user.Nick;
        }
        public void Update()
        {
        }
        public void Draw()
        {
            desktop.Render();
        }
        public void HideMenu()
        {
        }

	}
}
