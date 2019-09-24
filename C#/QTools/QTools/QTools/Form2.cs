using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace QTools
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
            this.Left = Convert.ToInt32((System.Windows.Forms.SystemInformation.WorkingArea.Width - 350) / 2 + 350);
            this.Top = Convert.ToInt32((System.Windows.Forms.SystemInformation.WorkingArea.Height - 345) / 2);

            label1.Text = "QTools" + "\n" + "\n" + "快捷工具" + "\n" + "\n" + "制作：剑无道" + "\n" + "\n" + "公司：幽竹轩" + "\n" + "\n" + "QTools是一款快捷操作工具，可以帮助用户快速执行一些常用命令。" + "\n" + "\n" + "Mail：heart056571@sina.com";
            label2.Text = "v1.0.0.0";
            label3.Text = "个人使用者可以免费使用QTools，但请勿用于商业用途，版权必究！";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
