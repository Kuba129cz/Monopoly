using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Monopoly.MonopolyGame.Model.Enums;
using Monopoly.Play.View.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Monopoly.MonopolyGame.View.UI
{
    class TileOwnerNotification
    {
        public Sprite Sprite;
        //public bool IsActive { get; set; } = true;
        public int BoardIndex { get; private set; }
        private Dictionary<Pawn, Color> collors;
        public TileOwnerNotification(int index, Sprite sprite)
        {
            this.BoardIndex = index;
            this.Sprite = sprite;

            collors = new Dictionary<Pawn, Color>();
            collors.Add(Pawn.blue, Color.Blue);
            collors.Add(Pawn.green, Color.Green);
            collors.Add(Pawn.red, Color.Red);
            collors.Add(Pawn.yellow, Color.Yellow);
        }
        public void SetOwner(Pawn pawn)
        {
            //this.IsActive = true;
            //  this.Sprite.Image = zde nastavit texturu v zavislosti jakemu hraci policko patri
            Texture2D rect = new Texture2D(GameState.graphics.GraphicsDevice, 10, 30);
            Color[] data = new Color[10 * 30];
            for (int i = 0; i < data.Length; ++i) data[i] = collors[pawn];

            for (int i = 0; i < 30; ++i) data[i] = Color.Black;
            for (int i = data.Length - 1; i > data.Length - 30; --i) data[i] = Color.Black;
            for (int i = 9; i < data.Length; i += 10) data[i] = Color.Black;
            for (int i = 0; i < data.Length; i += 10) data[i] = Color.Black;

            rect.SetData(data);
            Sprite.Image = rect;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            //if (IsActive)
                spriteBatch.Draw(this.Sprite.Image, this.Sprite.Rectangle, Color.White);
        }
    }
}
