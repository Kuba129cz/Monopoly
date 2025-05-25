using Myra.Graphics2D.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Monopoly.Controlls
{
    public class MyListBox : ListBox
    {
        protected override void OnItemCollectionChanged()
        {
            //base.OnItemCollectionChanged();
            //InternalChild.ScrollPosition = InternalChild.ScrollMaximum;
            try
            {
                lock (this)
                {
                    base.OnItemCollectionChanged();
                    if (Desktop != null)
                    {
                        UpdateLayout();
                    }
                    InternalChild.ScrollPosition = InternalChild.ScrollMaximum;
                }
            }
            catch(Exception ex)
            {
                Error.HandleError(ex);
            }
        }
    }
}
