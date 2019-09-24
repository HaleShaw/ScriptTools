using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Reader
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            ///***设置窗体显示位置***///
            this.Left = Convert.ToInt32((System.Windows.Forms.SystemInformation.WorkingArea.Width + 900) / 2 - this.Width - 25);
            this.Top = Convert.ToInt32((System.Windows.Forms.SystemInformation.WorkingArea.Height + 700) / 2 - this.Height - 33);

            label1.Text = "Reader" + "\n" + "\n" + "阅读器" + "\n" + "\n" + "制作：剑无道" + "\n" + "\n" + "公司：幽竹轩" + "\n" + "\n" + "Reader是一款txt文本阅读软件，包含老板键、滚屏、添加书签等功能。" + "\n" + "\n" + "Mail：heart056571@sina.com";
            label2.Text = "v1.0.0.0";
            label3.Text = "个人使用者可以免费使用Reader，但请勿用于商业用途，版权必究！";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("老板键：		Alt+Q(可变)" + "\n\n" + "保存：		Ctrl+S" + "\n" + "保存书签：	F4" + "\n" + "上一页：		←" + "\n" + "下一页：		→" + "\n" + 
                "滚屏/停止滚屏：	Space" + "\n" + "滚屏加速：	↑" + "\n" + "滚屏减速：	↓" + "\n" + "查找：		Ctrl+F" + "\n" + "折叠/展开：	F9" + "\n" + 
                "置顶/取消置顶：	F10" + "\n" + "全屏：		F11" + "\n" + "退出全屏：	Esc", "快捷键说明");
        }
    }
}
