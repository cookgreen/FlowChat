using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlowChatControl
{
    public class FlowChatScrollableListBox : Control
    {
        protected int y;
        protected int view_y;
        protected FlowChatScrollBar scrollBar;

        public FlowChatScrollableListBox()
        {
            scrollBar = new FlowChatScrollBar();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            //Draw scroll bar if too heigh
            if (y > Height)
            {
                //Calculate the ratio between the control height and the real Y we recorded
                double ratio = (double)Height / (double)y;
                double times = (double)y / (double)Height;
                scrollBar.Height = (int)((double)Height * ratio);
                int deltaPointPos = 0;
                if (scrollBar.isDragging)
                {
                    if (scrollBar.dragTempPoint.Y != -1)
                    {
                        deltaPointPos = scrollBar.dragTempPoint.Y - scrollBar.dragStartPoint.Y;
                        scrollBar.dragTempPoint = new Point(-1, -1);
                    }
                    else
                    {
                        deltaPointPos = scrollBar.dragEndPoint.Y - scrollBar.dragStartPoint.Y;
                    }
                }
                else
                {
                    deltaPointPos = scrollBar.dragEndPoint.Y - scrollBar.dragStartPoint.Y;
                }
                int scrollBarTop = 0;
                if (5 + deltaPointPos < 0)//If we scroll to the top
                {
                    scrollBarTop = 5;
                }
                else if (5 + deltaPointPos + scrollBar.Height > Height) // if we scroll to the bottom
                {
                    scrollBarTop = Height - scrollBar.Height - 5;

                }
                else// if we scroll inside the area
                {
                    scrollBarTop = 5 + deltaPointPos;
                }
                Rectangle scrollBarRect = new Rectangle(Width - 10 - 5, scrollBarTop, 10, scrollBar.Height);
                g.FillRectangle(new SolidBrush(Color.FromArgb(200, 218, 218, 218)), scrollBarRect);
                scrollBar.Rect = scrollBarRect;
                scrollBar.Offset = (int)Math.Round((double)scrollBarTop * times);
                if (deltaPointPos > 0)
                {
                    scrollBar.draggingType = 0;//down
                    view_y = y - scrollBar.Offset;
                }
                else if (deltaPointPos < 0)
                {
                    scrollBar.draggingType = 1;
                    view_y = y - -1 * scrollBar.Offset;
                }
                else
                {
                    scrollBar.draggingType = -1;
                    view_y = y;
                }
            }
        }

        protected bool IsEnterRectange(Rectangle rect, int X, int Y)
        {
            return X > rect.X && X < rect.X + rect.Width &&
                   Y > rect.Y && Y < rect.Y + rect.Height;
        }

        protected void CalculateViewY()
        {
            //Are we dragging the scroll bar or we just finish the dragging?
            if (scrollBar.isDragging ||
                scrollBar.isDragFinished)
            {
                int offset = 0;
                switch (scrollBar.draggingType)
                {
                    case 0:
                        offset = scrollBar.Offset;
                        break;
                    case 1:
                        offset = -1 * scrollBar.Offset;
                        break;
                }
                view_y = y - offset;
            }
            //if not
            else
            {
                view_y = y;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (scrollBar.isDragging)
            {
                scrollBar.dragEndPoint = new Point(e.X, e.Y);
                Invalidate();
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (scrollBar.Rect != null && IsEnterRectange(scrollBar.Rect, e.X, e.Y))
            {
                scrollBar.isDragging = true;
                scrollBar.dragStartPoint = new Point(e.X, e.Y);
                scrollBar.isDragFinished = false;
                Invalidate();
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (scrollBar.isDragging)
            {
                scrollBar.isDragging = false;
                scrollBar.dragEndPoint = new Point(e.X, e.Y);
                int length = scrollBar.dragEndPoint.Y - scrollBar.dragStartPoint.Y;
                scrollBar.dragTempPoint = new Point(scrollBar.dragEndPoint.X, scrollBar.dragEndPoint.Y + length);
                scrollBar.isDragFinished = true;
                Invalidate();
            }
        }
    }
}
