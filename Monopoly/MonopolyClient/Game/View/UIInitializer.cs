using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Monopoly.Controlls;
using Monopoly.MonopolyGame.Model.Enums;
using Monopoly.MonopolyGame.View.UI;
using Monopoly.Play.View.UI;
using Myra;
using Myra.Graphics2D.Brushes;
using Myra.Graphics2D.UI;
using System;

namespace Monopoly.Play.View
{
    static class UIInitializer
    {
        public static  Dice CreateWhiteDice(ContentManager content)
        {
            Texture2D[] diceImages = new Texture2D[6];
            for(int i=0;i<6;i++)
            {
                diceImages[i] = content.Load<Texture2D>("WhiteDice\\dice_" + (i + 1).ToString());
            }
            Rectangle diceRectangle = adjustResolution(new Rectangle(1510, 200, 200, 200)); //doladit souradnice
            Sprite diceSprite = new Sprite(diceRectangle, diceImages[0]);
            Dice dice = new Dice(diceSprite, diceImages);
            return dice;
        }
        public static Dice CreateBlackDice(ContentManager content)
        {
            Texture2D[] diceImages = new Texture2D[6];
            for (int i = 0; i < 6; i++)
            {
                diceImages[i] = content.Load<Texture2D>("BlackDice\\dice_" + (i + 1).ToString());
            }
            Rectangle diceRectangle = adjustResolution(new Rectangle(1710, 200, 200, 200)); //doladit souradnice
            Sprite diceSprite = new Sprite(diceRectangle, diceImages[0]);
            Dice dice = new Dice(diceSprite, diceImages);
            return dice;
        }
        public static Background CreateBackground(ContentManager content)
        {
            Texture2D backgroundImage = content.Load<Texture2D>("Board\\monopolyBoard");
            Rectangle backgroundRectangle = adjustResolution(new Rectangle(420, 0, 1080, 1080));
            Sprite backgroundSprite = new Sprite(backgroundRectangle, backgroundImage);
            Background background = new Background(backgroundSprite);
            return background;
        }
        public static TextButton CreateThrowButton()
        {
            TextButton button1 = new TextButton
            {
                Text = "Hodit",
                Padding = new Myra.Graphics2D.Thickness(10, 10),
                Left = 1510,
                Width = 380,
                Height = 100,
                Top = 405,
                OverTextColor = Color.Yellow,
                PressedTextColor = Color.Black,
                Font = MyraEnvironment.DefaultAssetManager.Load<SpriteFontBase>("Content/fonts/arial64.fnt"),
                Background = new SolidBrush("#010000"),
                FocusedBackground = new SolidBrush("#808000FF"),
                PressedBackground = new SolidBrush("#FFFF00")
            };
            Rectangle location = adjustResolution(new Rectangle(button1.Left, button1.Top, (int)button1.Width, (int)button1.Height));
            button1.Left = location.X;
            button1.Top = location.Y;
            button1.Width = location.Width;
            button1.Height = location.Height;
            return button1;
        }
        public static Figure CreateFigureInTemplate(ContentManager content, Pawn pawn)
        {
            Texture2D figureImage = content.Load<Texture2D>(string.Format("Figures\\{0}", pawn));
            Rectangle figureRectangle = new Rectangle(0, 0, 0, 0);
            switch (pawn)
            {
                case Pawn.blue:
                    figureRectangle = adjustResolution(new Rectangle(-20, 0, 200, 200));
                    break;
                case Pawn.green:
                    figureRectangle = adjustResolution(new Rectangle(-20, 880, 200, 200));
                    break;
                case Pawn.red:
                    figureRectangle = adjustResolution(new Rectangle(1480, 0, 200, 200));
                    break;
                case Pawn.yellow:
                    figureRectangle = adjustResolution(new Rectangle(1480, 880, 200, 200));
                    break;
            }

            Sprite figureSprite = new Sprite(figureRectangle, figureImage);
            Figure figure = new Figure(figureSprite, (int)pawn);//asi bude potreba upravit
            return figure;
        }
        public static MyListBox CreateListOfHistory()
        {
            MyListBox listBox1 = new MyListBox
            {
                Width = 420,
                Height = 340,
                Top = 540,
                Left = 1500,
                GridColumn = 1,
                GridRow = 1,
                GridColumnSpan = 2
            };
            Rectangle location = adjustResolution(new Rectangle(listBox1.Left, listBox1.Top, (int)listBox1.Width, (int)listBox1.Height));
            listBox1.Left = location.X;
            listBox1.Top = location.Y;
            listBox1.Width = location.Width;
            listBox1.Height = location.Height;
            return listBox1;
        }
        public static MyListBox CreateListChat()
        {
            MyListBox listBox1 = new MyListBox
            {
                Width = 420,
                Height = 620,
                Top = 210,
                Left = 0,
                GridColumn = 1,
                GridRow = 1,
                GridColumnSpan = 2
            };
            Rectangle location = adjustResolution(new Rectangle(listBox1.Left, listBox1.Top, (int)listBox1.Width, (int)listBox1.Height));
            listBox1.Left = location.X;
            listBox1.Top = location.Y;
            listBox1.Width = location.Width;
            listBox1.Height = location.Height;
            return listBox1;
        }

        public static TextBox CreateTextBoxForChat()
        {
            TextBox textBox1 = new TextBox
            {
                Width = 420,
                Height = 25,
                Top = 850,
                Left = 0,
                GridColumn = 1,
                GridRow = 1,
                GridColumnSpan = 2
            };
            Rectangle location = adjustResolution(new Rectangle(textBox1.Left, textBox1.Top, (int)textBox1.Width, (int)textBox1.Height));
            textBox1.Left = location.X;
            textBox1.Top = location.Y;
            textBox1.Width = location.Width;
            textBox1.Height = location.Height;
            return textBox1;
        }
        public static Figure CreateFigure(ContentManager content, Pawn pawn)
        {
            Texture2D figureImage = content.Load<Texture2D>(string.Format("Figures\\{0}", pawn));
            Rectangle figureRectangle = new Rectangle(0, 0, 0, 0);
            switch (pawn)
            {
                case Pawn.blue:
                     figureRectangle = adjustResolution(new Rectangle(1370, 975, 35, 35));
                    break;
                case Pawn.green:
                    figureRectangle = adjustResolution(new Rectangle(1370, 1015, 35, 35));
                    break;
                case Pawn.red:
                    figureRectangle = adjustResolution(new Rectangle(1410, 975, 35, 35));
                    break;
                case Pawn.yellow:
                    figureRectangle = adjustResolution(new Rectangle(1410, 1015, 35, 35));
                    break;
            }
        
            Sprite figureSprite = new Sprite(figureRectangle, figureImage);
            Figure figure = new Figure(figureSprite, (int)pawn);//asi bude potreba upravit
            return figure;
        }
        public static Template CreateTemplate(ContentManager content, Pawn pawn)
        {
            Texture2D templateImage = content.Load<Texture2D>(string.Format("Players\\template"));
            Rectangle templateRectangle = new Rectangle(0, 0, 0, 0);
            switch (pawn)
            {
                case Pawn.blue:
                    templateRectangle = adjustResolution(new Rectangle(0, 0, 420, 200));
                    break;
                case Pawn.green:
                    templateRectangle = adjustResolution(new Rectangle(0, 880, 420, 200));
                    break;
                case Pawn.red:
                    templateRectangle = adjustResolution(new Rectangle(1500, 0, 420, 200));
                    break;
                case Pawn.yellow:
                    templateRectangle = adjustResolution(new Rectangle(1500, 880, 420, 200));
                    break;
            }
            Sprite templateSprite = new Sprite(templateRectangle, templateImage);
            Template template = new Template(templateSprite);
            return template;
        }
        
        public static Rectangle[] CreateTileColliders()
        {
            Rectangle[] tileColliders = new Rectangle[40];
            //int xIncrement = 57;
            //int WINDOW_WIDTH = 1080;
            //int WINDOW_HEIGHT = 1080;

            tileColliders[0] = adjustResolution(new Rectangle(1358, 938, 140, 140)); //GO
            tileColliders[1] = adjustResolution(new Rectangle(1272, 938, 83, 140)); //Branik
            tileColliders[2] = adjustResolution(new Rectangle(1183, 938, 83, 140)); //Kasa dole
            tileColliders[3] = adjustResolution(new Rectangle(1094, 938, 85, 140)); //podoli  
            tileColliders[4] = adjustResolution(new Rectangle(1006, 938, 85, 140)); //Dan z majetku
            tileColliders[5] = adjustResolution(new Rectangle(916, 938, 85, 140)); //Praha - Podmokly
            tileColliders[6] = adjustResolution(new Rectangle(830, 938, 85, 140)); //Masarykuv stadion
            tileColliders[7] = adjustResolution(new Rectangle(745, 938, 81, 140)); //Sance
            tileColliders[8] = adjustResolution(new Rectangle(654, 938, 90, 140)); //Telefon
            tileColliders[9] = adjustResolution(new Rectangle(563, 938, 90, 140)); //Petrin
            tileColliders[10] = adjustResolution(new Rectangle(422, 938, 140, 140)); //vezeni
            tileColliders[11] = adjustResolution(new Rectangle(423, 847, 140, 88)); //Pohorelec
            tileColliders[12] = adjustResolution(new Rectangle(423, 759, 140, 87)); //Zarovka
            tileColliders[13] = adjustResolution(new Rectangle(423, 671, 140, 87)); //Primatorska trida
            tileColliders[14] = adjustResolution(new Rectangle(423, 584, 140, 87)); //Kralovska trida
            tileColliders[15] = adjustResolution(new Rectangle(423, 497, 140, 87)); //Praha-Plzen
            tileColliders[16] = adjustResolution(new Rectangle(423, 410, 140, 87)); //Taborska ulice
            tileColliders[17] = adjustResolution(new Rectangle(423, 320, 140, 90)); //Pokladna
            tileColliders[18] = adjustResolution(new Rectangle(423, 230, 140, 90)); //Radiova koncese
            tileColliders[19] = adjustResolution(new Rectangle(423, 140, 140, 90)); //Husova trida
            tileColliders[20] = adjustResolution(new Rectangle(422, 0, 140, 140)); //Parking
            tileColliders[21] = adjustResolution(new Rectangle(563, 0, 90, 140)); //Vitkov
            tileColliders[22] = adjustResolution(new Rectangle(654, 0, 90, 140)); //Sance
            tileColliders[23] = adjustResolution(new Rectangle(745, 0, 81, 140)); //Karlovo Namesti
            tileColliders[24] = adjustResolution(new Rectangle(830, 0, 85, 140)); //Vysehradska trida
            tileColliders[25] = adjustResolution(new Rectangle(916, 0, 85, 140)); //Praha-Hradec Kralove
            tileColliders[26] = adjustResolution(new Rectangle(1006, 0, 85, 140)); //Botanicka zahrada
            tileColliders[27] = adjustResolution(new Rectangle(1094, 0, 85, 140)); //Fochova trida
            tileColliders[28] = adjustResolution(new Rectangle(1183, 0, 83, 140)); //Vodovod
            tileColliders[29] = adjustResolution(new Rectangle(1272, 0, 83, 140)); //Namesti miru
            tileColliders[30] = adjustResolution(new Rectangle(1358, 0, 140, 140)); //Jdi do zalare
            tileColliders[31] = adjustResolution(new Rectangle(1358, 140, 140, 90)); //Ovocny trh
            tileColliders[32] = adjustResolution(new Rectangle(1358, 230, 140, 90)); //Radiova koncese
            tileColliders[33] = adjustResolution(new Rectangle(1358, 320, 140, 90)); //pokladna
            tileColliders[34] = adjustResolution(new Rectangle(1358, 410, 140, 87)); //Staromestske namesti
            tileColliders[35] = adjustResolution(new Rectangle(1358, 497, 140, 87)); //Praha-Brno
            tileColliders[36] = adjustResolution(new Rectangle(1358, 584, 140, 87)); //Sance
            tileColliders[37] = adjustResolution(new Rectangle(1358, 671, 140, 87)); //Narodni trida
            tileColliders[38] = adjustResolution(new Rectangle(1358, 759, 140, 87)); //Luxusni taxa
            tileColliders[39] = adjustResolution(new Rectangle(1358, 847, 140, 88)); //Vaclavske namesti
            return tileColliders;
        }
        public static TileOwnerNotification[] CreateTileFlags(ContentManager content)
        {
            Texture2D rect = new Texture2D(GameState.graphics.GraphicsDevice, 10, 30);
            Color[] data = new Color[10 * 30];
            for (int i = 0; i < data.Length; ++i) data[i] = Color.Transparent;

            rect.SetData(data);

            TileOwnerNotification[] tileOwnerNotifications = new TileOwnerNotification[28];
            tileOwnerNotifications[0] = new TileOwnerNotification(1, new Sprite((adjustResolution(new Rectangle(1271, 1048, 10, 30))),rect)); //Branik
            tileOwnerNotifications[1] = new TileOwnerNotification(3, new Sprite((adjustResolution(new Rectangle(1094, 1048, 10, 30))), rect)); //Podoli
            tileOwnerNotifications[2] = new TileOwnerNotification(5, new Sprite((adjustResolution(new Rectangle(916, 1048, 10, 30))), rect)); //Praha-Podmokly
            tileOwnerNotifications[3] = new TileOwnerNotification(6, new Sprite((adjustResolution(new Rectangle(830, 1048, 10, 30))), rect)); //Masarykuv stadion
            tileOwnerNotifications[4] = new TileOwnerNotification(8, new Sprite((adjustResolution(new Rectangle(654, 1048, 10, 30))), rect)); //Telefon
            tileOwnerNotifications[5] = new TileOwnerNotification(9, new Sprite((adjustResolution(new Rectangle(565, 1048, 10, 30))), rect)); //Petrin
            tileOwnerNotifications[6] = new TileOwnerNotification(11, new Sprite((adjustResolution(new Rectangle(423, 848, 30, 10))), rect)); //Pohorelec
            tileOwnerNotifications[7] = new TileOwnerNotification(12, new Sprite((adjustResolution(new Rectangle(423, 760, 30, 10))), rect)); //Zarovka
            tileOwnerNotifications[8] = new TileOwnerNotification(13, new Sprite((adjustResolution(new Rectangle(423, 672, 30, 10))), rect)); //Primatorska trida
            tileOwnerNotifications[9] = new TileOwnerNotification(14, new Sprite((adjustResolution(new Rectangle(423, 584, 30, 10))), rect)); //Kralovska trida
            tileOwnerNotifications[10] = new TileOwnerNotification(15, new Sprite((adjustResolution(new Rectangle(423, 497, 30, 10))), rect)); //Praha-Plzen
            tileOwnerNotifications[11] = new TileOwnerNotification(16, new Sprite((adjustResolution(new Rectangle(423, 409, 30, 10))), rect)); //Taborska ulice
            tileOwnerNotifications[12] = new TileOwnerNotification(18, new Sprite((adjustResolution(new Rectangle(423, 231, 30, 10))), rect)); //Radiova koncese
            tileOwnerNotifications[13] = new TileOwnerNotification(19, new Sprite((adjustResolution(new Rectangle(423, 144, 30, 10))), rect)); //Husova trida
            tileOwnerNotifications[14] = new TileOwnerNotification(21, new Sprite((adjustResolution(new Rectangle(640, 111, 10, 30))), rect)); //Vitkov
            tileOwnerNotifications[15] = new TileOwnerNotification(23, new Sprite((adjustResolution(new Rectangle(817, 111, 10, 30))), rect)); //Karlovo Namesti
            tileOwnerNotifications[16] = new TileOwnerNotification(24, new Sprite((adjustResolution(new Rectangle(905, 111, 10, 30))), rect)); //Vysehradska trida
            tileOwnerNotifications[17] = new TileOwnerNotification(25, new Sprite((adjustResolution(new Rectangle(993, 111, 10, 30))), rect)); //Praha-Hradec Kralove
            tileOwnerNotifications[18] = new TileOwnerNotification(26, new Sprite((adjustResolution(new Rectangle(1081, 111, 10, 30))), rect)); //Botanicka zahrada
            tileOwnerNotifications[19] = new TileOwnerNotification(27, new Sprite((adjustResolution(new Rectangle(1169, 111, 10, 30))), rect)); //Fochova trida
            tileOwnerNotifications[20] = new TileOwnerNotification(28, new Sprite((adjustResolution(new Rectangle(1257, 111, 10, 30))), rect)); //Vodovod
            tileOwnerNotifications[21] = new TileOwnerNotification(29, new Sprite((adjustResolution(new Rectangle(1345, 111, 10, 30))), rect)); //Namesti miru
            tileOwnerNotifications[22] = new TileOwnerNotification(31, new Sprite((adjustResolution(new Rectangle(1468, 219, 30, 10))), rect)); //Ovocny trh
            tileOwnerNotifications[23] = new TileOwnerNotification(32, new Sprite((adjustResolution(new Rectangle(1468, 308, 30, 10))), rect)); //Radio
            tileOwnerNotifications[24] = new TileOwnerNotification(34, new Sprite((adjustResolution(new Rectangle(1468, 483, 30, 10))), rect)); //Staromestske nam.
            tileOwnerNotifications[25] = new TileOwnerNotification(35, new Sprite((adjustResolution(new Rectangle(1468, 572, 30, 10))), rect)); //Praha-Brno
            tileOwnerNotifications[26] = new TileOwnerNotification(37, new Sprite((adjustResolution(new Rectangle(1468, 748, 30, 10))), rect)); //Narodni trida
            tileOwnerNotifications[27] = new TileOwnerNotification(39, new Sprite((adjustResolution(new Rectangle(1468, 924, 30, 10))), rect)); //Vaclavske nam.
            return tileOwnerNotifications;
        }
        public static TileOwnerNotification[] CreateFirstHouses(ContentManager content)
        {
            Texture2D rect = new Texture2D(GameState.graphics.GraphicsDevice, 10, 30);
            Color[] data = new Color[10 * 30];
            for (int i = 0; i < data.Length; ++i) data[i] = Color.Transparent;

            rect.SetData(data);
            int offset = 9;
            TileOwnerNotification[] tileOwnerNotifications = new TileOwnerNotification[19];
            tileOwnerNotifications[0] = new TileOwnerNotification(1, new Sprite((adjustResolution(new Rectangle(1279 + offset, 943, 8, 16))), rect)); //Branik
            tileOwnerNotifications[1] = new TileOwnerNotification(3, new Sprite((adjustResolution(new Rectangle(1101 + offset, 943, 8, 16))), rect)); //Podoli
            tileOwnerNotifications[2] = new TileOwnerNotification(6, new Sprite((adjustResolution(new Rectangle(837 + offset, 943, 8, 16))), rect)); //Masarykuv stadion
            tileOwnerNotifications[3] = new TileOwnerNotification(9, new Sprite((adjustResolution(new Rectangle(570 + offset, 943, 8, 16))), rect)); //Petrin

            tileOwnerNotifications[4] = new TileOwnerNotification(11, new Sprite((adjustResolution(new Rectangle(541, 854 + offset, 16, 8))), rect)); //Pohorelec
            tileOwnerNotifications[5] = new TileOwnerNotification(13, new Sprite((adjustResolution(new Rectangle(541, 679 + offset, 16, 8))), rect)); //Primatorska trida
            tileOwnerNotifications[6] = new TileOwnerNotification(14, new Sprite((adjustResolution(new Rectangle(541, 591 + offset, 16, 8))), rect)); //Kralovska trida
            tileOwnerNotifications[7] = new TileOwnerNotification(16, new Sprite((adjustResolution(new Rectangle(541, 416 + offset, 16, 8))), rect)); //Taborska ulice
            tileOwnerNotifications[8] = new TileOwnerNotification(19, new Sprite((adjustResolution(new Rectangle(541, 151 + offset, 16, 8))), rect)); //Husova trida

            tileOwnerNotifications[9] = new TileOwnerNotification(21, new Sprite((adjustResolution(new Rectangle(570 + offset, 7, 8, 16))), rect)); //Vitkov 
            tileOwnerNotifications[10] = new TileOwnerNotification(23, new Sprite((adjustResolution(new Rectangle(750 + offset, 7, 8, 16))), rect)); //Karlovo Namesti
            tileOwnerNotifications[11] = new TileOwnerNotification(24, new Sprite((adjustResolution(new Rectangle(836 + offset, 7, 8, 16))), rect)); //Vysehradska trida 
            tileOwnerNotifications[12] = new TileOwnerNotification(26, new Sprite((adjustResolution(new Rectangle(1012 + offset, 7, 8, 16))), rect)); //Botanicka zahrada
            tileOwnerNotifications[13] = new TileOwnerNotification(27, new Sprite((adjustResolution(new Rectangle(1101 + offset, 7, 8, 16))), rect)); //Fochova trida 
            tileOwnerNotifications[14] = new TileOwnerNotification(29, new Sprite((adjustResolution(new Rectangle(1279 + offset, 7, 8, 16))), rect)); //Namesti miru 

            tileOwnerNotifications[15] = new TileOwnerNotification(31, new Sprite((adjustResolution(new Rectangle(1365, 151 + offset, 16, 8))), rect)); //Ovocny trh
            tileOwnerNotifications[16] = new TileOwnerNotification(34, new Sprite((adjustResolution(new Rectangle(1365, 416 + offset, 16, 8))), rect)); //Staromestske nam.
            tileOwnerNotifications[17] = new TileOwnerNotification(37, new Sprite((adjustResolution(new Rectangle(1365, 679 + offset, 16, 8))), rect)); //Narodni trida
            tileOwnerNotifications[18] = new TileOwnerNotification(39, new Sprite((adjustResolution(new Rectangle(1365, 854 + offset, 16, 8))), rect)); //Vaclavske nam.
            return tileOwnerNotifications;
        }
        public static TileOwnerNotification[] CreateSecondHouses(ContentManager content)
        {
            Texture2D rect = new Texture2D(GameState.graphics.GraphicsDevice, 10, 30);
            Color[] data = new Color[10 * 30];
            for (int i = 0; i < data.Length; ++i) data[i] = Color.Transparent;

            rect.SetData(data);
            int offset = 24;
            TileOwnerNotification[] tileOwnerNotifications = new TileOwnerNotification[19];
            tileOwnerNotifications[0] = new TileOwnerNotification(1, new Sprite((adjustResolution(new Rectangle(1279 + offset, 943, 8, 16))), rect)); //Branik
            tileOwnerNotifications[1] = new TileOwnerNotification(3, new Sprite((adjustResolution(new Rectangle(1101 + offset, 943, 8, 16))), rect)); //Podoli
            tileOwnerNotifications[2] = new TileOwnerNotification(6, new Sprite((adjustResolution(new Rectangle(837 + offset, 943, 8, 16))), rect)); //Masarykuv stadion
            tileOwnerNotifications[3] = new TileOwnerNotification(9, new Sprite((adjustResolution(new Rectangle(570 + offset, 943, 8, 16))), rect)); //Petrin

            tileOwnerNotifications[4] = new TileOwnerNotification(11, new Sprite((adjustResolution(new Rectangle(541, 854 + offset, 16, 8))), rect)); //Pohorelec
            tileOwnerNotifications[5] = new TileOwnerNotification(13, new Sprite((adjustResolution(new Rectangle(541, 679 + offset, 16, 8))), rect)); //Primatorska trida
            tileOwnerNotifications[6] = new TileOwnerNotification(14, new Sprite((adjustResolution(new Rectangle(541, 591 + offset, 16, 8))), rect)); //Kralovska trida
            tileOwnerNotifications[7] = new TileOwnerNotification(16, new Sprite((adjustResolution(new Rectangle(541, 416 + offset, 16, 8))), rect)); //Taborska ulice
            tileOwnerNotifications[8] = new TileOwnerNotification(19, new Sprite((adjustResolution(new Rectangle(541, 151 + offset, 16, 8))), rect)); //Husova trida

            tileOwnerNotifications[9] = new TileOwnerNotification(21, new Sprite((adjustResolution(new Rectangle(570 + offset, 7, 8, 16))), rect)); //Vitkov 
            tileOwnerNotifications[10] = new TileOwnerNotification(23, new Sprite((adjustResolution(new Rectangle(750 + offset, 7, 8, 16))), rect)); //Karlovo Namesti
            tileOwnerNotifications[11] = new TileOwnerNotification(24, new Sprite((adjustResolution(new Rectangle(836 + offset, 7, 8, 16))), rect)); //Vysehradska trida 
            tileOwnerNotifications[12] = new TileOwnerNotification(26, new Sprite((adjustResolution(new Rectangle(1012 + offset, 7, 8, 16))), rect)); //Botanicka zahrada
            tileOwnerNotifications[13] = new TileOwnerNotification(27, new Sprite((adjustResolution(new Rectangle(1101 + offset, 7, 8, 16))), rect)); //Fochova trida 
            tileOwnerNotifications[14] = new TileOwnerNotification(29, new Sprite((adjustResolution(new Rectangle(1279 + offset, 7, 8, 16))), rect)); //Namesti miru 

            tileOwnerNotifications[15] = new TileOwnerNotification(31, new Sprite((adjustResolution(new Rectangle(1365, 151 + offset, 16, 8))), rect)); //Ovocny trh
            tileOwnerNotifications[16] = new TileOwnerNotification(34, new Sprite((adjustResolution(new Rectangle(1365, 416 + offset, 16, 8))), rect)); //Staromestske nam.
            tileOwnerNotifications[17] = new TileOwnerNotification(37, new Sprite((adjustResolution(new Rectangle(1365, 679 + offset, 16, 8))), rect)); //Narodni trida
            tileOwnerNotifications[18] = new TileOwnerNotification(39, new Sprite((adjustResolution(new Rectangle(1365, 854 + offset, 16, 8))), rect)); //Vaclavske nam.
            return tileOwnerNotifications;
        }
        public static TileOwnerNotification[] CreateThirdHouses(ContentManager content)
        {
            Texture2D rect = new Texture2D(GameState.graphics.GraphicsDevice, 10, 30);
            Color[] data = new Color[10 * 30];
            for (int i = 0; i < data.Length; ++i) data[i] = Color.Transparent;

            rect.SetData(data);
            int offset = 39;
            TileOwnerNotification[] tileOwnerNotifications = new TileOwnerNotification[19];
            tileOwnerNotifications[0] = new TileOwnerNotification(1, new Sprite((adjustResolution(new Rectangle(1279 + offset, 943, 8, 16))), rect)); //Branik
            tileOwnerNotifications[1] = new TileOwnerNotification(3, new Sprite((adjustResolution(new Rectangle(1101 + offset, 943, 8, 16))), rect)); //Podoli
            tileOwnerNotifications[2] = new TileOwnerNotification(6, new Sprite((adjustResolution(new Rectangle(837 + offset, 943, 8, 16))), rect)); //Masarykuv stadion
            tileOwnerNotifications[3] = new TileOwnerNotification(9, new Sprite((adjustResolution(new Rectangle(570 + offset, 943, 8, 16))), rect)); //Petrin

            tileOwnerNotifications[4] = new TileOwnerNotification(11, new Sprite((adjustResolution(new Rectangle(541, 854 + offset, 16, 8))), rect)); //Pohorelec
            tileOwnerNotifications[5] = new TileOwnerNotification(13, new Sprite((adjustResolution(new Rectangle(541, 679 + offset, 16, 8))), rect)); //Primatorska trida
            tileOwnerNotifications[6] = new TileOwnerNotification(14, new Sprite((adjustResolution(new Rectangle(541, 591 + offset, 16, 8))), rect)); //Kralovska trida
            tileOwnerNotifications[7] = new TileOwnerNotification(16, new Sprite((adjustResolution(new Rectangle(541, 416 + offset, 16, 8))), rect)); //Taborska ulice
            tileOwnerNotifications[8] = new TileOwnerNotification(19, new Sprite((adjustResolution(new Rectangle(541, 151 + offset, 16, 8))), rect)); //Husova trida

            tileOwnerNotifications[9] = new TileOwnerNotification(21, new Sprite((adjustResolution(new Rectangle(570 + offset, 7, 8, 16))), rect)); //Vitkov 
            tileOwnerNotifications[10] = new TileOwnerNotification(23, new Sprite((adjustResolution(new Rectangle(750 + offset, 7, 8, 16))), rect)); //Karlovo Namesti
            tileOwnerNotifications[11] = new TileOwnerNotification(24, new Sprite((adjustResolution(new Rectangle(836 + offset, 7, 8, 16))), rect)); //Vysehradska trida 
            tileOwnerNotifications[12] = new TileOwnerNotification(26, new Sprite((adjustResolution(new Rectangle(1012 + offset, 7, 8, 16))), rect)); //Botanicka zahrada
            tileOwnerNotifications[13] = new TileOwnerNotification(27, new Sprite((adjustResolution(new Rectangle(1101 + offset, 7, 8, 16))), rect)); //Fochova trida 
            tileOwnerNotifications[14] = new TileOwnerNotification(29, new Sprite((adjustResolution(new Rectangle(1279 + offset, 7, 8, 16))), rect)); //Namesti miru 

            tileOwnerNotifications[15] = new TileOwnerNotification(31, new Sprite((adjustResolution(new Rectangle(1365, 151 + offset, 16, 8))), rect)); //Ovocny trh
            tileOwnerNotifications[16] = new TileOwnerNotification(34, new Sprite((adjustResolution(new Rectangle(1365, 416 + offset, 16, 8))), rect)); //Staromestske nam.
            tileOwnerNotifications[17] = new TileOwnerNotification(37, new Sprite((adjustResolution(new Rectangle(1365, 679 + offset, 16, 8))), rect)); //Narodni trida
            tileOwnerNotifications[18] = new TileOwnerNotification(39, new Sprite((adjustResolution(new Rectangle(1365, 854 + offset, 16, 8))), rect)); //Vaclavske nam.
            return tileOwnerNotifications;
        }
        public static TileOwnerNotification[] CreateFourthHouses(ContentManager content)
        {
            Texture2D rect = new Texture2D(GameState.graphics.GraphicsDevice, 10, 30);
            Color[] data = new Color[10 * 30];
            for (int i = 0; i < data.Length; ++i) data[i] = Color.Transparent;

            rect.SetData(data);
            int offset = 54;
            TileOwnerNotification[] tileOwnerNotifications = new TileOwnerNotification[19];
            tileOwnerNotifications[0] = new TileOwnerNotification(1, new Sprite((adjustResolution(new Rectangle(1279 + offset, 943, 8, 16))), rect)); //Branik
            tileOwnerNotifications[1] = new TileOwnerNotification(3, new Sprite((adjustResolution(new Rectangle(1101 + offset, 943, 8, 16))), rect)); //Podoli
            tileOwnerNotifications[2] = new TileOwnerNotification(6, new Sprite((adjustResolution(new Rectangle(837 + offset, 943, 8, 16))), rect)); //Masarykuv stadion
            tileOwnerNotifications[3] = new TileOwnerNotification(9, new Sprite((adjustResolution(new Rectangle(570 + offset, 943, 8, 16))), rect)); //Petrin

            tileOwnerNotifications[4] = new TileOwnerNotification(11, new Sprite((adjustResolution(new Rectangle(541, 854 + offset, 16, 8))), rect)); //Pohorelec
            tileOwnerNotifications[5] = new TileOwnerNotification(13, new Sprite((adjustResolution(new Rectangle(541, 679 + offset, 16, 8))), rect)); //Primatorska trida
            tileOwnerNotifications[6] = new TileOwnerNotification(14, new Sprite((adjustResolution(new Rectangle(541, 591 + offset, 16, 8))), rect)); //Kralovska trida
            tileOwnerNotifications[7] = new TileOwnerNotification(16, new Sprite((adjustResolution(new Rectangle(541, 416 + offset, 16, 8))), rect)); //Taborska ulice
            tileOwnerNotifications[8] = new TileOwnerNotification(19, new Sprite((adjustResolution(new Rectangle(541, 151 + offset, 16, 8))), rect)); //Husova trida

            tileOwnerNotifications[9] = new TileOwnerNotification(21, new Sprite((adjustResolution(new Rectangle(570 + offset, 7, 8, 16))), rect)); //Vitkov 
            tileOwnerNotifications[10] = new TileOwnerNotification(23, new Sprite((adjustResolution(new Rectangle(750 + offset, 7, 8, 16))), rect)); //Karlovo Namesti
            tileOwnerNotifications[11] = new TileOwnerNotification(24, new Sprite((adjustResolution(new Rectangle(836 + offset, 7, 8, 16))), rect)); //Vysehradska trida 
            tileOwnerNotifications[12] = new TileOwnerNotification(26, new Sprite((adjustResolution(new Rectangle(1012 + offset, 7, 8, 16))), rect)); //Botanicka zahrada
            tileOwnerNotifications[13] = new TileOwnerNotification(27, new Sprite((adjustResolution(new Rectangle(1101 + offset, 7, 8, 16))), rect)); //Fochova trida 
            tileOwnerNotifications[14] = new TileOwnerNotification(29, new Sprite((adjustResolution(new Rectangle(1279 + offset, 7, 8, 16))), rect)); //Namesti miru 

            tileOwnerNotifications[15] = new TileOwnerNotification(31, new Sprite((adjustResolution(new Rectangle(1365, 151 + offset, 16, 8))), rect)); //Ovocny trh
            tileOwnerNotifications[16] = new TileOwnerNotification(34, new Sprite((adjustResolution(new Rectangle(1365, 416 + offset, 16, 8))), rect)); //Staromestske nam.
            tileOwnerNotifications[17] = new TileOwnerNotification(37, new Sprite((adjustResolution(new Rectangle(1365, 679 + offset, 16, 8))), rect)); //Narodni trida
            tileOwnerNotifications[18] = new TileOwnerNotification(39, new Sprite((adjustResolution(new Rectangle(1365, 854 + offset, 16, 8))), rect)); //Vaclavske nam.
            return tileOwnerNotifications;
        }
        public static TileOwnerNotification[] CreateHotels(ContentManager content)
        {
            Texture2D rect = new Texture2D(GameState.graphics.GraphicsDevice, 10, 30);
            Color[] data = new Color[10 * 30];
            for (int i = 0; i < data.Length; ++i) data[i] = Color.Transparent;

            rect.SetData(data);
            int offset = 9;
            TileOwnerNotification[] tileOwnerNotifications = new TileOwnerNotification[19];
            tileOwnerNotifications[0] = new TileOwnerNotification(1, new Sprite((adjustResolution(new Rectangle(1279 + offset, 943, 53, 16))), rect)); //Branik
            tileOwnerNotifications[1] = new TileOwnerNotification(3, new Sprite((adjustResolution(new Rectangle(1101 + offset, 943, 53, 16))), rect)); //Podoli
            tileOwnerNotifications[2] = new TileOwnerNotification(6, new Sprite((adjustResolution(new Rectangle(837 + offset, 943, 53, 16))), rect)); //Masarykuv stadion
            tileOwnerNotifications[3] = new TileOwnerNotification(9, new Sprite((adjustResolution(new Rectangle(570 + offset, 943, 53, 16))), rect)); //Petrin

            tileOwnerNotifications[4] = new TileOwnerNotification(11, new Sprite((adjustResolution(new Rectangle(541, 854 + offset, 16, 53))), rect)); //Pohorelec
            tileOwnerNotifications[5] = new TileOwnerNotification(13, new Sprite((adjustResolution(new Rectangle(541, 679 + offset, 16, 53))), rect)); //Primatorska trida
            tileOwnerNotifications[6] = new TileOwnerNotification(14, new Sprite((adjustResolution(new Rectangle(541, 591 + offset, 16, 53))), rect)); //Kralovska trida
            tileOwnerNotifications[7] = new TileOwnerNotification(16, new Sprite((adjustResolution(new Rectangle(541, 416 + offset, 16, 53))), rect)); //Taborska ulice
            tileOwnerNotifications[8] = new TileOwnerNotification(19, new Sprite((adjustResolution(new Rectangle(541, 151 + offset, 16, 53))), rect)); //Husova trida

            tileOwnerNotifications[9] = new TileOwnerNotification(21, new Sprite((adjustResolution(new Rectangle(570 + offset, 7, 53, 16))), rect)); //Vitkov 
            tileOwnerNotifications[10] = new TileOwnerNotification(23, new Sprite((adjustResolution(new Rectangle(750 + offset, 7, 53, 16))), rect)); //Karlovo Namesti
            tileOwnerNotifications[11] = new TileOwnerNotification(24, new Sprite((adjustResolution(new Rectangle(836 + offset, 7, 53, 16))), rect)); //Vysehradska trida 
            tileOwnerNotifications[12] = new TileOwnerNotification(26, new Sprite((adjustResolution(new Rectangle(1012 + offset, 7, 53, 16))), rect)); //Botanicka zahrada
            tileOwnerNotifications[13] = new TileOwnerNotification(27, new Sprite((adjustResolution(new Rectangle(1101 + offset, 7, 53, 16))), rect)); //Fochova trida 
            tileOwnerNotifications[14] = new TileOwnerNotification(29, new Sprite((adjustResolution(new Rectangle(1279 + offset, 7, 53, 16))), rect)); //Namesti miru 

            tileOwnerNotifications[15] = new TileOwnerNotification(31, new Sprite((adjustResolution(new Rectangle(1365, 151 + offset, 16, 53))), rect)); //Ovocny trh
            tileOwnerNotifications[16] = new TileOwnerNotification(34, new Sprite((adjustResolution(new Rectangle(1365, 416 + offset, 16, 53))), rect)); //Staromestske nam.
            tileOwnerNotifications[17] = new TileOwnerNotification(37, new Sprite((adjustResolution(new Rectangle(1365, 679 + offset, 16, 53))), rect)); //Narodni trida
            tileOwnerNotifications[18] = new TileOwnerNotification(39, new Sprite((adjustResolution(new Rectangle(1365, 854 + offset, 16, 53))), rect)); //Vaclavske nam.
            return tileOwnerNotifications;
        }
        private static Rectangle adjustResolution(Rectangle location)
        {
            int x_width = Program.Game.GraphicsDevice.DisplayMode.Width;
            int y_height = Program.Game.GraphicsDevice.DisplayMode.Height;

            //int x_width = 1280;
            //int y_height = 690;

            const int x_width_fullHD = 1920;
            const int y_height_fullHD = 1080;
            location.X = (int)((x_width * location.X) / (double) x_width_fullHD);
            location.Y = (int)((y_height * location.Y)/ (double)y_height_fullHD);
            location.Width = (int)((location.Width / (double)(x_width_fullHD)) * x_width);
            location.Height = (int)((location.Height / (double)(y_height_fullHD)) * y_height);
            return location;
        }
    }
}
