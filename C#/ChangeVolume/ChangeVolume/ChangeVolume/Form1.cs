using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace ChangeVolume
{
    public enum KeyModifiers //组合键枚举
    {
        None = 0,
        Alt = 1,
        Control = 2,
        Shift = 4,
        Windows = 8
    }
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        [DllImport("user32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, UInt32 dwFlags, UInt32 dwExtraInfo);

        [DllImport("user32.dll")]
        static extern Byte MapVirtualKey(UInt32 uCode, UInt32 uMapType);

        private const byte VK_VOLUME_MUTE = 0xAD;
        private const byte VK_VOLUME_DOWN = 0xAE;
        private const byte VK_VOLUME_UP = 0xAF;
        private const UInt32 KEYEVENTF_EXTENDEDKEY = 0x0001;
        private const UInt32 KEYEVENTF_KEYUP = 0x0002;


        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, Keys vk);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);
        private void Form1_Load(object sender, EventArgs e)
        {
            this.Left = Convert.ToInt32((System.Windows.Forms.SystemInformation.WorkingArea.Width - 242) / 2);
            this.Top = Convert.ToInt32((System.Windows.Forms.SystemInformation.WorkingArea.Height - 293) / 2);

            RegHotKey();
            SetListView();
            RegisterHotKey(Handle, 800, 0, Keys.Escape);
            this.label1.Text = "本程序无窗口\n\n关闭之后将继续在后台运行";
        }

        /// <summary>
        /// 设置ListView的值
        /// </summary>
        private void SetListView()
        {
            this.listView1.Columns.Add("快捷键", 140, HorizontalAlignment.Left);
            this.listView1.Columns.Add("功能", 60, HorizontalAlignment.Left);
            this.listView1.View = System.Windows.Forms.View.Details;

            listView1.Items.Add("Ctrl + Win + Down");
            listView1.Items.Add("VolumeDown");
            listView1.Items.Add("Ctrl + Win + Up");
            listView1.Items.Add("VolumeUp");
            listView1.Items.Add("Ctrl + Win + NumPad0");
            listView1.Items.Add("VolumeMute");
            listView1.Items.Add("Ctrl + Win + Esc");

            this.listView1.Items[0].SubItems.Add("减小音量");
            this.listView1.Items[1].SubItems.Add("减小音量");
            this.listView1.Items[2].SubItems.Add("增加音量");
            this.listView1.Items[3].SubItems.Add("增加音量");
            this.listView1.Items[4].SubItems.Add("静音");
            this.listView1.Items[5].SubItems.Add("静音");
            this.listView1.Items[6].SubItems.Add("退出");
        }

        /// <summary>
        /// 注册热键
        /// </summary>
        private void RegHotKey()
        {
            RegisterHotKey(Handle, 100, 2 | 8, Keys.Down);
            RegisterHotKey(Handle, 200, 2 | 8, Keys.Up);
            RegisterHotKey(Handle, 300, 2 | 8, Keys.NumPad0);
            RegisterHotKey(Handle, 400, 0, Keys.VolumeDown);
            RegisterHotKey(Handle, 500, 0, Keys.VolumeUp);
            RegisterHotKey(Handle, 600, 0, Keys.VolumeMute);
            RegisterHotKey(Handle, 700, 2 | 8, Keys.Escape);
        }

        /// <summary>
        /// 注销热键
        /// </summary>
        private void UnregHotKey()
        {
            UnregisterHotKey(Handle, 100);
            UnregisterHotKey(Handle, 200);
            UnregisterHotKey(Handle, 300);
            UnregisterHotKey(Handle, 400);
            UnregisterHotKey(Handle, 500);
            UnregisterHotKey(Handle, 600);
            UnregisterHotKey(Handle, 700);
        }

        /// <summary>
        /// 判断热键
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            const int WM_HOTKEY = 0x0312;
            switch (m.Msg)
            {
                case WM_HOTKEY:
                    switch (m.WParam.ToInt32())
                    {
                        case 100:
                        case 400:
                            //音量减小
                            keybd_event(VK_VOLUME_DOWN, MapVirtualKey(VK_VOLUME_DOWN, 0), KEYEVENTF_EXTENDEDKEY, 0);
                            keybd_event(VK_VOLUME_DOWN, MapVirtualKey(VK_VOLUME_DOWN, 0), KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
                            break;
                        case 200:
                        case 500:
                            //音量增加
                            keybd_event(VK_VOLUME_UP, MapVirtualKey(VK_VOLUME_UP, 0), KEYEVENTF_EXTENDEDKEY, 0);
                            keybd_event(VK_VOLUME_UP, MapVirtualKey(VK_VOLUME_UP, 0), KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
                            break;
                        case 300:
                        case 600:
                            //静音
                            keybd_event(VK_VOLUME_MUTE, MapVirtualKey(VK_VOLUME_MUTE, 0), KEYEVENTF_EXTENDEDKEY, 0);
                            keybd_event(VK_VOLUME_MUTE, MapVirtualKey(VK_VOLUME_MUTE, 0), KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
                            break;
                        case 700:
                            //退出
                            UnregHotKey();
                            System.Environment.Exit(0);
                            break;
                        case 800:                            
                            CloseSetting();
                            break;
                    }
                    break;
            }
            base.WndProc(ref m);
        }

        /// <summary>
        /// 更改关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            CloseSetting();
        }

        /// <summary>
        /// 关闭窗口时设置程序
        /// </summary>
        private void CloseSetting()
        {
            this.Enabled = false;
            this.ShowInTaskbar = false;
            this.WindowState = FormWindowState.Minimized;
            this.Hide();
            RegHotKey();
        }
    }
}
