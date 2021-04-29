using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlowChatControl
{
    public class FlowChatGroupListBox : FlowChatScrollableListBox
    {
        public List<FlowChatItemModel> Users;
        private FlowChatItemModel lastClickItem;
        private FlowChatItemModel lastMouseMoveItem;
        private string lastGroupName;
        public FlowChatGroupListBox()
        {
            Users = new List<FlowChatItemModel>();
            DoubleBuffered = true;
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            y = 0;
            view_y = 0;
            for (int i = 0; i < Users.Count; i++)
            {
                CalculateViewY();
                string userName = Users[i].Text;
                string firstUserLetter = userName[0].ToString();
                int finalHeight;
                int finalAvatarTop;
                int finalNameTop;
                int finalItemTop;
                bool isDrawGroup;
                bool isDrawSepLine;
                if (firstUserLetter != lastGroupName)
                {
                    isDrawGroup = true;
                    finalHeight = 33 + 70;
                    lastGroupName = firstUserLetter;
                    finalAvatarTop = 15 + 33;
                    finalNameTop = 27 + 33;
                    finalItemTop = 33 + 0;
                }
                else
                {
                    isDrawGroup = false;
                    finalHeight = 70;
                    finalAvatarTop = 15;
                    finalNameTop = 27;
                    finalItemTop = 0;
                }

                if (i != 0)
                {
                    string nextUserNameLetter = Users[i - 1].Text[0].ToString();
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
                g.FillRectangle(new SolidBrush(Color.FromArgb(231, 230, 228)), itemRect);
                if (isDrawSepLine)
                {
                    //lineHeight = itemTempRect.Y + finalHeight;
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
                if (Users[i].IsClick)
                {
                    color = Color.FromArgb(200, 199, 197);
                }
                else if (Users[i].IsFocus)
                {
                    color = Color.FromArgb(215, 216, 218);
                }
                else
                {
                    color = Color.FromArgb(231, 230, 228);
                }
                g.FillRectangle(new SolidBrush(color), itemTempRect);
                Rectangle avatarRect = new Rectangle(15, view_y + finalAvatarTop, 40, 40);
                if(!string.IsNullOrEmpty(Users[i].ImageUrl))
                {
                    g.DrawImage(new Bitmap(Users[i].ImageUrl), avatarRect);
                }
                else
                {
                    g.DrawImage(new Bitmap(Users[i].Image), avatarRect);
                }
                Rectangle textRect = new Rectangle(66, view_y + finalNameTop, 140, 14);
                g.DrawString(Users[i].Text, new Font(FontFamily.GenericMonospace, 10, FontStyle.Regular), Brushes.Black, textRect);
                Users[i].Rect = itemTempRect;
                y += finalHeight;
                base.OnPaint(e);
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            for (int i = 0; i < Users.Count; i++)
            {
                if (e.X > Users[i].Rect.X && e.X < Users[i].Rect.X + Users[i].Rect.Width &&
                   e.Y > Users[i].Rect.Y && e.Y < Users[i].Rect.Y + Users[i].Rect.Height)
                {
                    if (lastMouseMoveItem != null)
                    {
                        lastMouseMoveItem.IsFocus = false;
                    }
                    Users[i].IsFocus = true;
                    lastMouseMoveItem = Users[i];
                    Invalidate();
                    break;
                }
            }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            for (int i = 0; i < Users.Count; i++)
            {
                if (e.X > Users[i].Rect.X && e.X < Users[i].Rect.X + Users[i].Rect.Width &&
                   e.Y > Users[i].Rect.Y && e.Y < Users[i].Rect.Y + Users[i].Rect.Height)
                {
                    if (lastClickItem != null)
                    {
                        lastClickItem.IsClick = false;
                    }
                    Users[i].IsClick = true;
                    lastClickItem = Users[i];
                    Invalidate();
                    break;
                }
            }
        }
        public void AddUser(FlowChatItemModel newUser)
        {
            Users.Add(newUser);
            Users = (from user in Users
                     orderby user.Text
                     select user).ToList();
        }
    }
}
