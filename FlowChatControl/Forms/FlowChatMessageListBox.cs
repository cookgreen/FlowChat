using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlowChatControl
{
    public class FlowChatMessageListBox : FlowChatScrollableListBox
    {
        private FlowChatMessageItemModel lastMouseMoveItem;
        private FlowChatMessageItemModel lastClickItem;
        private List<FlowChatMessageItemModel> items;
        public List<FlowChatMessageItemModel> Items { get { return items; } }

        public event Action<object, FlowChatMessageItemModel> SelectedMessageItemChanged;

        public FlowChatMessageListBox()
        {
            items = new List<FlowChatMessageItemModel>();

            DoubleBuffered = true;

            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        public void AddMessageItem(FlowChatMessageItemModel messageItem)
        {
            Items.Add(messageItem);
            PainNewItem(messageItem);
        }

        public void RemoveMessageItem(FlowChatMessageItemModel messageItem)
        {
            if (messageItem != null)
            {
                int index = Items.IndexOf(messageItem);

                for (int i = index; i < Items.Count; i++)
                {
                    ClearItemArea(Items[i]);

                    y -= 66;

                    PainNewItem(Items[i]);
                }
            }
        }

        private void ClearItemArea(FlowChatMessageItemModel messageItem)
        {
            Graphics g = CreateGraphics();

            g.FillRectangle(Brushes.Transparent, messageItem.Rect);
        }

        private void PainNewItem(FlowChatMessageItemModel messageItem)
        {
            Graphics g = this.CreateGraphics();

            CalculateViewY();

            RenderSingleItem(g, ref messageItem);
        }

        private void RenderSingleItem(Graphics g, ref FlowChatMessageItemModel item)
        {
            Rectangle itemRect = new Rectangle(0, y, Width, 70);
            Color color = Color.Black;
            if (item.IsClick)
            {
                color = Color.FromArgb(200, 199, 197);
            }
            else if (item.IsFocus)
            {
                color = Color.FromArgb(215, 216, 218);
            }
            else
            {
                color = Color.FromArgb(233, 232, 230);
            }
            g.FillRectangle(new SolidBrush(color), itemRect);
            Rectangle avatarRect = new Rectangle(15, view_y + 15, 40, 40);

            if (File.Exists(item.ImageUrl))
            {
                g.DrawImage(new Bitmap(item.ImageUrl), avatarRect);
            }
            else
            {
                g.DrawImage(new Bitmap("./Images/avatar-default.png"), avatarRect);
            }

            Rectangle textRect = new Rectangle(66, view_y + 20, 140, 14);
            g.DrawString(item.MessagerName, new Font(FontFamily.GenericMonospace, 10, FontStyle.Regular), Brushes.Black, textRect);

            Rectangle chatTextRect = new Rectangle(66, view_y + 40, 140, 14);
            g.DrawString(item.LastMessageText, new Font(FontFamily.GenericMonospace, 9, FontStyle.Regular), new SolidBrush(Color.FromArgb(177, 178, 180)), chatTextRect);

            Rectangle timeTextRect = new Rectangle(Width - 47 - 15, view_y + 17, 47, 14);
            g.DrawString(item.LastMessageTime, new Font(FontFamily.GenericMonospace, 10, FontStyle.Regular), new SolidBrush(Color.FromArgb(177, 178, 180)), timeTextRect);

            y += 66;

            item.Rect = itemRect;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            y = 0;
            view_y = 0;

            for (int i = 0; i < Items.Count; i++)
            {
                CalculateViewY();

                var item = Items[i];
                RenderSingleItem(g, ref item);

                base.OnPaint(e);
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                if (e.X > Items[i].Rect.X && e.X < Items[i].Rect.X + Items[i].Rect.Width &&
                   e.Y > Items[i].Rect.Y && e.Y < Items[i].Rect.Y + Items[i].Rect.Height)
                {
                    if (lastMouseMoveItem != null)
                    {
                        lastMouseMoveItem.IsFocus = false;
                    }
                    Items[i].IsFocus = true;
                    lastMouseMoveItem = Items[i];
                    Invalidate();
                    break;
                }
                else
                {
                    Items[i].IsFocus = false;
                    Invalidate();
                }
            }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                if (e.X > Items[i].Rect.X && e.X < Items[i].Rect.X + Items[i].Rect.Width &&
                   e.Y > Items[i].Rect.Y && e.Y < Items[i].Rect.Y + Items[i].Rect.Height)
                {
                    if (lastClickItem != null)
                    {
                        lastClickItem.IsClick = false;
                    }
                    Items[i].IsClick = true;
                    lastClickItem = Items[i];
                    Invalidate();

                    SelectedMessageItemChanged?.Invoke(this, lastMouseMoveItem);

                    break;
                }
            }
        }
    }
}
