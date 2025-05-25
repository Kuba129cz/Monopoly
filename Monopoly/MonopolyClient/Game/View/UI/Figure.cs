namespace Monopoly.Play.View.UI
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using System;

    class Figure
    {
        public int ID { get; set; }
        public Sprite Sprite { get; set; }

        public Figure(Sprite sprite, int ID)
        {
            this.Sprite = sprite;
            this.ID = ID;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.Sprite.Image, this.Sprite.Rectangle, Color.White);
        }
    }
}
