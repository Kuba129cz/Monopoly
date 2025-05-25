namespace Monopoly.Play.View.UI
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    class Sprite
    {
        public Rectangle Rectangle;
        public Texture2D Image { get; set; }

        public Sprite(Rectangle rectangle, Texture2D image)
        {
            this.Rectangle = rectangle;
            this.Image = image;
        }
    }
}
