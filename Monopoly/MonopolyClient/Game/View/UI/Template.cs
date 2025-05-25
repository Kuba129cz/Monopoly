using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monopoly.Play.View.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Monopoly.MonopolyGame.View.UI
{
    class Template
    {
        private Sprite sprite;
        public Template(Sprite sprite)
        {
            this.sprite = sprite;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.sprite.Image, this.sprite.Rectangle, Color.White);
        }
    }
}
