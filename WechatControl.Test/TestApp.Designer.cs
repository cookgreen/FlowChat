namespace WechatControl.Test
{
    partial class TestApp
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.weChatAddressList1 = new WechatControl.FlowChatGroupListBox();
            this.weChatFlowChatContent1 = new WechatControl.FlowChatMessageContentListBox();
            this.weChatFlowListBox1 = new WechatControl.FlowChatMessageListBox();
            this.flowChatCheckAddressList1 = new WechatControl.FlowChatGroupCheckListBox();
            this.SuspendLayout();
            // 
            // weChatAddressList1
            // 
            this.weChatAddressList1.Location = new System.Drawing.Point(891, -1);
            this.weChatAddressList1.Name = "weChatAddressList1";
            this.weChatAddressList1.Size = new System.Drawing.Size(393, 532);
            this.weChatAddressList1.TabIndex = 6;
            this.weChatAddressList1.Text = "weChatAddressList1";
            // 
            // weChatFlowChatContent1
            // 
            this.weChatFlowChatContent1.Location = new System.Drawing.Point(357, -1);
            this.weChatFlowChatContent1.Name = "weChatFlowChatContent1";
            this.weChatFlowChatContent1.Size = new System.Drawing.Size(528, 532);
            this.weChatFlowChatContent1.TabIndex = 5;
            this.weChatFlowChatContent1.Text = "weChatFlowChatContent1";
            // 
            // weChatFlowListBox1
            // 
            this.weChatFlowListBox1.Location = new System.Drawing.Point(2, -1);
            this.weChatFlowListBox1.Name = "weChatFlowListBox1";
            this.weChatFlowListBox1.Size = new System.Drawing.Size(349, 532);
            this.weChatFlowListBox1.TabIndex = 3;
            // 
            // flowChatCheckAddressList1
            // 
            this.flowChatCheckAddressList1.Location = new System.Drawing.Point(2, 537);
            this.flowChatCheckAddressList1.Name = "flowChatCheckAddressList1";
            this.flowChatCheckAddressList1.Size = new System.Drawing.Size(349, 407);
            this.flowChatCheckAddressList1.TabIndex = 7;
            this.flowChatCheckAddressList1.Text = "flowChatCheckAddressList1";
            // 
            // TestApp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1286, 947);
            this.Controls.Add(this.flowChatCheckAddressList1);
            this.Controls.Add(this.weChatAddressList1);
            this.Controls.Add(this.weChatFlowChatContent1);
            this.Controls.Add(this.weChatFlowListBox1);
            this.Name = "TestApp";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion
        private FlowChatMessageListBox weChatFlowListBox1;
        private FlowChatMessageContentListBox weChatFlowChatContent1;
        private FlowChatGroupListBox weChatAddressList1;
        private FlowChatGroupCheckListBox flowChatCheckAddressList1;
    }
}

