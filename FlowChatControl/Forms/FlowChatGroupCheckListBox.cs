using FlowChatControl.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlowChatControl
{
    public delegate void FlowChatCheckItemChangedHanlder(object sender, FlowChatCheckItemModel checkedUser);
    public class FlowChatGroupCheckListBox : FlowChatScrollableListBox
    {
        public event FlowChatCheckItemChangedHanlder OnSelectItemChanged;
        private FlowChatCheckItemModel lastClickItem;
        private FlowChatCheckItemModel lastMouseMoveItem;
        private string lastGroupName;
        private List<FlowChatCheckItemModel> items;
        private List<FlowChatCheckItemModel> checkedItems;
        public List<FlowChatCheckItemModel> CheckedItems
        {
            get
            {
                return checkedItems;
            }
        }

        public FlowChatGroupCheckListBox()
        {
            items = new List<FlowChatCheckItemModel>();
            checkedItems = new List<FlowChatCheckItemModel>();
            DoubleBuffered = true;
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            scrollBar = new FlowChatScrollBar();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            y = 0;
            view_y = 0;
            for (int i = 0; i < items.Count; i++)
            {

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

                else
                {
                    view_y = y;
                }
                string userName = items[i].Text;
                string firstUserLetter = userName[0].ToString();
                int finalHeight;
                int finalAvatarTop;
                int finalNameTop;
                int finalItemTop;
                bool isDrawGroup;
                bool isDrawSepLine;
                int finalCheckTopBorder;
                int finalCheckTop;
                if (firstUserLetter != lastGroupName)
                {
                    isDrawGroup = true;
                    finalHeight = 33 + 70;
                    lastGroupName = firstUserLetter;
                    finalAvatarTop = 15 + 33;
                    finalNameTop = 27 + 33;
                    finalItemTop = 33 + 0;
                    finalCheckTopBorder = 33 + 24;
                    finalCheckTop = 33 + 25;
                }
                else
                {
                    isDrawGroup = false;
                    finalHeight = 70;
                    finalAvatarTop = 15;
                    finalNameTop = 27;
                    finalItemTop = 0;
                    finalCheckTopBorder = 24;
                    finalCheckTop = 25;
                }

                if (i != 0)
                {
                    string nextUserNameLetter = items[i - 1].Text[0].ToString();
                    if(firstUserLetter != nextUserNameLetter)
                    {
                        isDrawSepLine = true;
                    }
                    else
                    {
                        isDrawSepLine = false;
                    }
                }
                else
                {
                    isDrawSepLine = false;
                }
                Rectangle itemRect = new Rectangle(0, view_y, Width, finalHeight);
                g.FillRectangle(new SolidBrush(Color.White), itemRect);

                if (isDrawSepLine)
                {
                    Rectangle sepRect = new Rectangle(0, view_y, Width, 1);
                    g.FillRectangle(new SolidBrush(Color.FromArgb(218, 217, 215)), sepRect);
                }

                if (isDrawGroup)
                {
                    Rectangle groupRect = new Rectangle(12, view_y + 19, 120, 15);
                    Color groupColor = Color.FromArgb(153, 153, 153);
                    g.DrawString(firstUserLetter, new Font(FontFamily.GenericMonospace, 10, FontStyle.Regular), new SolidBrush(groupColor), groupRect);
                }

                Rectangle itemTempRect = new Rectangle(0, view_y + finalItemTop, Width, 70);
                Color color = Color.Black;
                if (items[i].IsClick)
                {
                    color = Color.FromArgb(200, 199, 197);
                }
                else if (items[i].IsFocus)
                {
                    color = Color.FromArgb(215, 216, 218);
                }
                else
                {
                    color = Color.White;
                }
                g.FillRectangle(new SolidBrush(color), itemTempRect);
                Rectangle avatarRect = new Rectangle(15, view_y + finalAvatarTop, 40, 40);
                if (!string.IsNullOrEmpty(items[i].ImageUrl))
                {
                    g.DrawImage(new Bitmap(items[i].ImageUrl), avatarRect);
                }
                else
                {
                    g.DrawImage(new Bitmap(items[i].Image), avatarRect);
                }
                Rectangle textRect = new Rectangle(66, view_y + finalNameTop, 140, 14);
                g.DrawString(items[i].Text, new Font(FontFamily.GenericMonospace, 10, FontStyle.Regular), Brushes.Black, textRect);


                Rectangle checkRect = new Rectangle(Width - 45, view_y + finalCheckTopBorder, 22, 22);

                Rectangle checkRectFillArea = new Rectangle(Width - 44, view_y + finalCheckTop, 20, 20);
                g.DrawEllipse(Pens.LightGray, checkRect);

                if (items[i].IsChecked)
                {
                    g.DrawImage(new Bitmap(Resources.duihao), checkRectFillArea);
                }
                else
                {
                    g.FillEllipse(new SolidBrush(Color.White), checkRectFillArea);
                }
                items[i].Rect = itemTempRect;
                y += finalHeight;
            }
            base.OnPaint(e);
            lastGroupName = string.Empty;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            for (int i = 0; i < items.Count; i++)
            {
                if (e.X > items[i].Rect.X && e.X < items[i].Rect.X + items[i].Rect.Width &&
                    e.Y > items[i].Rect.Y && e.Y < items[i].Rect.Y + items[i].Rect.Height)
                {
                    if (lastMouseMoveItem != null)
                    {
                        lastMouseMoveItem.IsFocus = false;
                    }
                    items[i].IsFocus = true;
                    lastMouseMoveItem = items[i];
                    Invalidate();
                    break;
                }
            }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (IsEnterRectange(items[i].Rect, e.X, e.Y))
                {
                    if (lastClickItem != null)
                    {
                        lastClickItem.IsClick = false;
                    }
                    items[i].IsClick = true;
                    items[i].IsChecked = !items[i].IsChecked;
                    if(items[i].IsChecked)
                    {
                        checkedItems.Add(items[i]);
                    }
                    else
                    {
                        checkedItems.Remove(items[i]);
                    }
                    lastClickItem = items[i];
                    OnSelectItemChanged?.Invoke(this, items[i]);
                    Invalidate();
                    break;
                }
            }
        }

        public void AddItem(FlowChatCheckItemModel newItem)
        {
            items.Add(newItem);
            items = (from item in items
                     orderby item.Text
                     select item).ToList();
        }
    }
}
