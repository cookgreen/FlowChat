using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowChatControl
{
    public class FlowChatScrollBar
    {
        public int Height { get; set; }
        public int Offset { get; set; }
        internal bool isEnter { get; set; }
        internal bool isDragging { get; set; }
        internal bool isDragFinished { get; set; }
        internal Rectangle Rect { get; set; }
        internal Point dragStartPoint { get; set; }
        internal Point dragEndPoint { get; set; }
        internal Point dragTempPoint { get; set; }
        internal int draggingType { get; set; }

        public FlowChatScrollBar()
        {
            Height = 0;
            dragStartPoint = new Point(0, 0);
            dragEndPoint = new Point(0, 0);
            dragTempPoint = new Point(-1, -1);
            draggingType = -1;
        }
    }
}
