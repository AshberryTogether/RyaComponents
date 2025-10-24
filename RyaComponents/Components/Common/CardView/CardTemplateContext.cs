using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RyaComponents.Components.Common.CardView
{
    public class CardTemplateContext
    {
        public object? DataItem { get; set; }
        public RyaCardView? CardView { get; set; }
        public int VisibleIndex { get; set; }

        public CardTemplateContext(object? dataItem, RyaCardView cardView, int visibleIndex)
        {
            DataItem = dataItem;
            CardView = cardView;
            VisibleIndex = visibleIndex;
        }
    }
}
