using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace FlowChatControl
{
    public class FlowChatMessageContentListBox : FlowChatScrollableListBox
    {
        private const int MAX_MSG_TEXT_WIDTH = 175;
        private const int MAX_MSG_RECT_WIDTH = 195;
        private const int MSG_BETWEEN_TEXT_AND_RECT_WIDTH = 9;
        public List<FlowChatMessageContentItemModel> ChatItems;
        public FlowChatMessageContentListBox()
        {
            ChatItems = new List<FlowChatMessageContentItemModel>();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            y = 0;
            view_y = 0;
            Graphics g = e.Graphics;
            Rectangle rect = new Rectangle(0, 0, Width, Height);
            g.FillRectangle(new SolidBrush(Color.FromArgb(245, 245, 245)), rect);
            for (int i = 0; i < ChatItems.Count; i++)
            {
                CalculateViewY();
                int avatarLeft;
                int msgLeft;
                int msgTextLeft;
                Color msgColor;
                StringFormat sf = new StringFormat();
                sf.FormatFlags = StringFormatFlags.MeasureTrailingSpaces;
                float finalMsgTextWidth = g.MeasureString(ChatItems[i].MessageText, Font, 500).Width;
                float finalMsgRectWidth = finalMsgTextWidth + MSG_BETWEEN_TEXT_AND_RECT_WIDTH * 2;
                int tringleLeftUp;
                int tringleLeftMid;
                int tringleLeftDown;

                if (ChatItems[i].IsMe)
                {
                    avatarLeft = Width - 30 - 34;
                    msgLeft = Width - 74 - ((int)finalMsgRectWidth > MAX_MSG_RECT_WIDTH ? MAX_MSG_RECT_WIDTH : (int)finalMsgRectWidth);
                    msgTextLeft = Width - 83 - ((int)finalMsgTextWidth > MAX_MSG_TEXT_WIDTH ? MAX_MSG_TEXT_WIDTH : (int)finalMsgTextWidth);
                    msgColor = Color.FromArgb(151, 236, 119);
                    tringleLeftUp = Width - 74;
                    tringleLeftMid = Width - 69;
                    tringleLeftDown = Width - 74;
                }
                else
                {
                    avatarLeft = 30;
                    msgLeft = 74;
                    msgTextLeft = 83;
                    msgColor = Color.White;
                    tringleLeftUp = 74;
                    tringleLeftMid = 69;
                    tringleLeftDown = 74;
                }
                Rectangle avatarRect = new Rectangle(avatarLeft, view_y + 15, 34, 34);
                if (!string.IsNullOrEmpty(ChatItems[i].ImageUrl))
                {
                    g.DrawImage(new Bitmap(ChatItems[i].ImageUrl), avatarRect);
                }
                else
                {
                    g.DrawImage(new Bitmap(ChatItems[i].Image), avatarRect);
                }
                Point[] poins = new Point[3] {
                    new Point(tringleLeftUp, view_y + 26),
                    new Point(tringleLeftMid, view_y + 30),
                    new Point(tringleLeftDown, view_y + 34)
                };
                g.FillPolygon(new SolidBrush(msgColor), poins);
                Rectangle msgRect = new Rectangle(msgLeft, view_y + 14, (int)finalMsgRectWidth, 35);
                g.FillRectangle(new SolidBrush(msgColor), msgRect);
                Rectangle msgTextRect = new Rectangle(msgTextLeft, view_y + 24, (int)finalMsgTextWidth, 16);
                g.DrawString(ChatItems[i].MessageText, new Font(FontFamily.GenericMonospace, 10, FontStyle.Regular), Brushes.Black, msgTextRect);
                y += 50;
                base.OnPaint(e);
            }
        }
    }
}
