using Microsoft.Xna.Framework;
using Monopoly.Communication;
using Monopoly.Controlls;
using Myra;
using Myra.Graphics2D.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Monopoly.MatchHistory
{
    partial class MatchHistory
    {
        private Desktop desktop;
        private Panel panel;
        private MyListBox listBox1;
        private TextButton button1;
        private TextButton button2;
        private Label label4;
        private bool basicStatis = true;
        bool detailStat = false;
        public MatchHistory()
        {
            panel = new Panel();
            desktop = new Desktop();
        }
        public void InitializeComponent()
        {  
            buildUI();
        }
        public void LoadContent()
        {
            buildUI();
            desktop.Root = panel;
            label3.Text = Data.user.Nick;
            Show();
            basicStatistics(null, null);
        }
        public void Update()
        {
        }
        public void Draw()
        {
            desktop.Render();
        }
        public void Hide()
        {
            panel.Visible = false;
        }
        public void Show()
        { 
            panel.Visible = true;
        }
        private void fillList()
        {
            listBox1.Items.Clear();
            if (Data.Statistics != null && Data.Statistics.Count != 0)
            {
                for (int i = 0; i < Data.Statistics.Count; i++)
                    addItemToList(Data.Statistics[i].Record);
            }
            else
            {
                var messageBox = Dialog.CreateMessageBox("Upozornění", "Žádné data k dispozici.");
                messageBox.ShowModal(desktop);
            }
        }
        private void clientSizeChanged(object sender, EventArgs e)
        {
            buildUI();
        }
        private void addItemToList(string str)
        {
            ListItem listItem1 = new ListItem();
            listItem1.Color = Color.White;
            listItem1.Text = str;
            listBox1.Items.Add(listItem1);
        }
        private void basicStatistics(object sender, EventArgs e)
        {
                basicStatis = true;
                button2.Visible = true;
                listBox1.TouchDoubleClick += detailedStatistics;
                Query.GetListOfStatistics();
                label4.Text = "status/ doba v zápase/ Datum/ frekventanti/ kol/ název lobby";
                fillList();
            detailStat = false;
        }
        private void detailedStatistics(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != null)
            {
                if (!detailStat)
                {
                    detailStat = true;
                    basicStatis = false;
                    button2.Visible = false;
                    listBox1.TouchDoubleClick -= detailedStatistics;
                    Data.Statistics[(int)listBox1.SelectedIndex].IDPlayer = Data.ThisPlayer.IDPlayer;
                    Query.GetListOfDetailedStatistics(Data.Statistics[(int)listBox1.SelectedIndex]);
                    label4.Text = "status/ nick / doba v zápase/ kola/ peníze";
                    fillList();
                }else
                {
                    var messageBox = Dialog.CreateMessageBox("Upozornění", "Již se nacházíš v podrobnostech.");
                    messageBox.ShowModal(desktop);
                }
            }else
            {
                var messageBox = Dialog.CreateMessageBox("Upozornění", "Nevybral si záznam.");
                messageBox.ShowModal(desktop);
            }
        }

    }
}
