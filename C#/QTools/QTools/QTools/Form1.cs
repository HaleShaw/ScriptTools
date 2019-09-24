using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

///***Process所在区间***///
using System.Diagnostics;

///***DllImport所在区间***///
using System.Runtime.InteropServices;

namespace QTools
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Opacity = 0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //F2 = new Form2();

            ///***设置窗体显示位置***///
            this.Left = Convert.ToInt32((System.Windows.Forms.SystemInformation.WorkingArea.Width - 350) / 2);
            this.Top = Convert.ToInt32((System.Windows.Forms.SystemInformation.WorkingArea.Height - 345) / 2);

            timer1.Enabled = true;
            checkbtn8();
            Show_ToolStripMenuItem.Enabled = false;
        }

        public Form2 F2;

        /// <summary>
        /// *声明定义文本框变量
        /// </summary>
        int t1_1 = 0;
        int t1_2 = 0;
        int t1_3 = 0;

        int t2_1 = 0;
        int t2_2 = 0;
        int t2_3 = 0;

        int t3_1 = 0;
        int t3_2 = 0;
        int t3_3 = 0;

        int t4_1 = 0;
        int t4_2 = 0;
        int t4_3 = 0;

        int t5_1 = 0;
        int t5_2 = 0;
        int t5_3 = 0;

        int t6_1 = 0;
        int t6_2 = 0;
        int t6_3 = 0;

        int t7_1 = 0;
        int t7_2 = 0;
        int t7_3 = 0;

        int t1 = 0;
        int t2 = 0;
        int t3 = 0;
        int t4 = 0;
        int t5 = 0;
        int t6 = 0;
        int t7 = 0;

        /// <summary>
        /// *声明Timer
        /// </summary>
        System.Timers.Timer myTimer2 = new System.Timers.Timer();
        System.Timers.Timer myTimer3 = new System.Timers.Timer();
        System.Timers.Timer myTimer4 = new System.Timers.Timer();
        System.Timers.Timer myTimer6 = new System.Timers.Timer();
        System.Timers.Timer myTimer7 = new System.Timers.Timer();

        /// ****************************************
        /// FunctionName：	docmd
        /// Param：			声明定义执行CMD函数
        /// Returns：			void
        /// CreateDate：		2014/09/11
        /// Remarks：		
        /// ****************************************
        public void docmd(string command)
        {
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            p.StandardInput.WriteLine(command);
            p.StandardInput.WriteLine("exit");
            p.WaitForExit();
            p.Close();
        }

        //息屏
        private const uint WM_SYSCOMMAND = 0x112;                       //系统消息
        private const int SC_MONITORPOWER = 0xF170;                     //关闭显示器的系统命令
        private const int MonitorPowerOff = 2;                                  //2为PowerOff, 1为省电状态，-1为开机
        private static readonly IntPtr HWND_BROADCAST = new IntPtr(0xffff);//广播消息，所有顶级窗体都会接收

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);
        private void cp(object source, System.Timers.ElapsedEventArgs e)        //供Timer调用执行函数
        {
            button2.Enabled = true;
            checkbtn8();
            myTimer2.Enabled = false;
            SendMessage(HWND_BROADCAST, WM_SYSCOMMAND, SC_MONITORPOWER, MonitorPowerOff);
        }

        //休眠
        string command3 = "rundll32 powrprof.dll,SetSuspendState";
        private void sp(object source, System.Timers.ElapsedEventArgs e)
        {
            button3.Enabled = true;
            checkbtn8();
            myTimer3.Enabled = false;
            docmd(command3);
        }

        //睡眠
        string command4 = "rundll32 powrprof.dll,SetSuspendState 0,1,0";
        private void gb(object source, System.Timers.ElapsedEventArgs e)
        {
            button4.Enabled = true;
            checkbtn8();
            myTimer4.Enabled = false;
            docmd(command4);
        }

        //注销
        string command6 = "logoff";
        private void log(object source, System.Timers.ElapsedEventArgs e)
        {
            button6.Enabled = true;
            checkbtn8();
            myTimer6.Enabled = false;
            docmd(command6);
        }

        //锁定
        string command7 = "rundll32 user32.dll,LockWorkStation";
        private void lo(object source, System.Timers.ElapsedEventArgs e)
        {
            button7.Enabled = true;
            checkbtn8();
            myTimer7.Enabled = false;
            docmd(command7);
        }

        /// ****************************************
        /// FunctionName：	checkbtn8
        /// Param：			检测button8按钮状态
        /// Returns：			void
        /// CreateDate：		2014/09/13
        /// Remarks：		
        /// ****************************************
        private void checkbtn8()
        {
            if (button1.Enabled == true && button2.Enabled == true && button3.Enabled == true && button4.Enabled == true && button5.Enabled == true && button6.Enabled == true && button7.Enabled == true)
            {
                button8.Enabled = false;
            }
            else
            {
                button8.Enabled = true;
            }
        }

        /// ****************************************
        /// FunctionName：	button1_Click
        /// Param：			关机
        /// Returns：			void
        /// CreateDate：		2014/09/11
        /// Remarks：		
        /// ****************************************
        private void button1_Click(object sender, EventArgs e)
        {
            t1 = t1_1 * 3600 + t1_2 * 60 + t1_3;
            string command1 = "shutdown -s -f -t " + t1;
            if (t1 == 0)
            {
                if (MessageBox.Show("要立即关机吗？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    docmd("shutdown -s -f -t 0");
                }
            }
            else
            {
                docmd(command1);
                button1.Enabled = false;
                button8.Enabled = true;
            }
        }

        /// ****************************************
        /// FunctionName：	button2_Click
        /// Param：			息屏
        /// Returns：			void
        /// CreateDate：		2014/09/11
        /// Remarks：		
        /// ****************************************
        private void button2_Click(object sender, EventArgs e)
        {
            t2 = t2_1 * 3600 + t2_2 * 60 + t2_3;
            if (t2 == 0)
            {
                if (MessageBox.Show("要立即息屏吗？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    SendMessage(HWND_BROADCAST, WM_SYSCOMMAND, SC_MONITORPOWER, MonitorPowerOff);
                }
            }
            else
            {
                button2.Enabled = false;
                button8.Enabled = true;
                myTimer2.Elapsed += new System.Timers.ElapsedEventHandler(cp);
                myTimer2.Interval = t2 * 1000;
                myTimer2.AutoReset = false;
                myTimer2.Enabled = true;
            }
        }

        /// ****************************************
        /// FunctionName：	button3_Click
        /// Param：			休眠
        /// Returns：			void
        /// CreateDate：		2014/09/11
        /// Remarks：		
        /// ****************************************
        private void button3_Click(object sender, EventArgs e)
        {
            t3 = t3_1 * 3600 + t3_2 * 60 + t3_3;
            if (t3 == 0)
            {
                if (MessageBox.Show("要立即休眠吗？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    docmd(command3);
                }
            }
            else
            {
                button3.Enabled = false;
                button8.Enabled = true;
                myTimer3.Elapsed += new System.Timers.ElapsedEventHandler(sp);
                myTimer3.Interval = t3 * 1000;
                myTimer3.AutoReset = false;
                myTimer3.Enabled = true;
            }
        }

        /// ****************************************
        /// FunctionName：	button4_Click
        /// Param：			睡眠
        /// Returns：			void
        /// CreateDate：		2014/09/11
        /// Remarks：		
        /// ****************************************
        private void button4_Click(object sender, EventArgs e)
        {
            t4 = t4_1 * 3600 + t4_2 * 60 + t4_3;
            if (t4 == 0)
            {
                if (MessageBox.Show("要立即睡眠吗？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    docmd(command4);
                }
            }
            else
            {
                button4.Enabled = false;
                button8.Enabled = true;
                myTimer4.Elapsed += new System.Timers.ElapsedEventHandler(gb);
                myTimer4.Interval = t4 * 1000;
                myTimer4.AutoReset = false;
                myTimer4.Enabled = true;
            }
        }

        /// ****************************************
        /// FunctionName：	button5_Click
        /// Param：			重启
        /// Returns：			void
        /// CreateDate：		2014/09/11
        /// Remarks：		
        /// ****************************************
        private void button5_Click(object sender, EventArgs e)
        {
            t5 = t5_1 * 3600 + t5_2 * 60 + t5_3;
            string command5 = "shutdown -r -f -t " + t5;
            if (t5 == 0)
            {
                if (MessageBox.Show("要立即重启吗？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    docmd("shutdown -r -f -t 0");
                }
            }
            else
            {
                button5.Enabled = false;
                button8.Enabled = true;
                docmd(command5);
            }
        }

        /// ****************************************
        /// FunctionName：	button6_Click
        /// Param：			注销
        /// Returns：			void
        /// CreateDate：		2014/09/11
        /// Remarks：		
        /// ****************************************
        private void button6_Click(object sender, EventArgs e)
        {
            t6 = t6_1 * 3600 + t6_2 * 60 + t6_3;
            if (t6 == 0)
            {
                if (MessageBox.Show("要立即注销吗？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    docmd(command6);
                }
            }
            else
            {
                button6.Enabled = false;
                button8.Enabled = true;
                myTimer6.Elapsed += new System.Timers.ElapsedEventHandler(log);
                myTimer6.Interval = t6 * 1000;
                myTimer6.AutoReset = false;
                myTimer6.Enabled = true;
            }
        }

        /// ****************************************
        /// FunctionName：	button7_Click
        /// Param：			锁定
        /// Returns：			void
        /// CreateDate：		2014/09/11
        /// Remarks：		
        /// ****************************************
        private void button7_Click(object sender, EventArgs e)
        {
            t7 = t7_1 * 3600 + t7_2 * 60 + t7_3;
            if (t7 == 0)
            {
                if (MessageBox.Show("要立即锁定吗？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    docmd(command7);
                }
            }
            else
            {
                button7.Enabled = false;
                button8.Enabled = true;
                myTimer7.Elapsed += new System.Timers.ElapsedEventHandler(lo);
                myTimer7.Interval = t7 * 1000;
                myTimer7.AutoReset = false;
                myTimer7.Enabled = true;
            }
        }

        /// ****************************************
        /// FunctionName：	button8_Click
        /// Param：			取消计划
        /// Returns：			void
        /// CreateDate：		2014/09/11
        /// Remarks：		
        /// ****************************************
        private void button8_Click(object sender, EventArgs e)
        {
            docmd("shutdown -a");
            myTimer2.Enabled = false;
            myTimer3.Enabled = false;
            myTimer4.Enabled = false;
            myTimer6.Enabled = false;
            myTimer7.Enabled = false;

            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;
            button5.Enabled = true;
            button6.Enabled = true;
            button7.Enabled = true;
            button8.Enabled = false;
        }

        /// <summary>
        /// *文本框赋值
        /// </summary>
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                t1_1 = 0;
            }
            else if (int.Parse(textBox1.Text) > 23)
            {
                MessageBox.Show("数字不能大于23");
                this.textBox1.Text = this.textBox1.Text.Substring(0, this.textBox1.Text.Length - 1);
                this.textBox1.SelectionStart = this.textBox1.Text.Length;
                this.textBox1.Focus();
                return;
            }
            else
            {
                t1_1 = int.Parse(textBox1.Text);
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                t1_2 = 0;
            }
            else if (int.Parse(textBox2.Text) > 59)
            {
                MessageBox.Show("数字不能大于59！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.textBox2.Text = this.textBox2.Text.Substring(0, this.textBox2.Text.Length - 1);
                this.textBox2.SelectionStart = this.textBox2.Text.Length;
                this.textBox2.Focus();
                return;
            }
            else
            {
                t1_2 = int.Parse(textBox2.Text);
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (textBox3.Text == "")
            {
                t1_3 = 0;
            }
            else if (int.Parse(textBox3.Text) > 59)
            {
                MessageBox.Show("数字不能大于59！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.textBox3.Text = this.textBox3.Text.Substring(0, this.textBox3.Text.Length - 1);
                this.textBox3.SelectionStart = this.textBox3.Text.Length;
                this.textBox3.Focus();
                return;
            }
            else
            {
                t1_3 = int.Parse(textBox3.Text);
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (textBox4.Text == "")
            {
                t2_1 = 0;
            }
            else if (int.Parse(textBox4.Text) > 23)
            {
                MessageBox.Show("数字不能大于23！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.textBox4.Text = this.textBox4.Text.Substring(0, this.textBox4.Text.Length - 1);
                this.textBox4.SelectionStart = this.textBox4.Text.Length;
                this.textBox4.Focus();
                return;
            }
            else
            {
                t2_1 = int.Parse(textBox4.Text);
            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            if (textBox5.Text == "")
            {
                t2_2 = 0;
            }
            else if (int.Parse(textBox5.Text) > 59)
            {
                MessageBox.Show("数字不能大于59！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.textBox5.Text = this.textBox5.Text.Substring(0, this.textBox5.Text.Length - 1);
                this.textBox5.SelectionStart = this.textBox5.Text.Length;
                this.textBox5.Focus();
                return;
            }
            else
            {
                t2_2 = int.Parse(textBox5.Text);
            }
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            if (textBox6.Text == "")
            {
                t2_3 = 0;
            }
            else if (int.Parse(textBox6.Text) > 59)
            {
                MessageBox.Show("数字不能大于59！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.textBox6.Text = this.textBox6.Text.Substring(0, this.textBox6.Text.Length - 1);
                this.textBox6.SelectionStart = this.textBox6.Text.Length;
                this.textBox6.Focus();
                return;
            }
            else
            {
                t2_3 = int.Parse(textBox6.Text);
            }
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            if (textBox7.Text == "")
            {
                t3_1 = 0;
            }
            else if (int.Parse(textBox7.Text) > 23)
            {
                MessageBox.Show("数字不能大于23！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.textBox7.Text = this.textBox7.Text.Substring(0, this.textBox7.Text.Length - 1);
                this.textBox7.SelectionStart = this.textBox7.Text.Length;
                this.textBox7.Focus();
                return;
            }
            else
            {
                t3_1 = int.Parse(textBox7.Text);
            }
        }
        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            if (textBox8.Text == "")
            {
                t3_2 = 0;
            }
            else if (int.Parse(textBox8.Text) > 59)
            {
                MessageBox.Show("数字不能大于59！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.textBox8.Text = this.textBox8.Text.Substring(0, this.textBox8.Text.Length - 1);
                this.textBox8.SelectionStart = this.textBox8.Text.Length;
                this.textBox8.Focus();
                return;
            }
            else
            {
                t3_2 = int.Parse(textBox8.Text);
            }
        }
        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            if (textBox9.Text == "")
            {
                t3_3 = 0;
            }
            else if (int.Parse(textBox9.Text) > 59)
            {
                MessageBox.Show("数字不能大于59！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.textBox9.Text = this.textBox9.Text.Substring(0, this.textBox9.Text.Length - 1);
                this.textBox9.SelectionStart = this.textBox9.Text.Length;
                this.textBox9.Focus();
                return;
            }
            else
            {
                t3_3 = int.Parse(textBox9.Text);
            }
        }
        private void textBox10_TextChanged(object sender, EventArgs e)
        {
            if (textBox10.Text == "")
            {
                t4_1 = 0;
            }
            else if (int.Parse(textBox10.Text) > 23)
            {
                MessageBox.Show("数字不能大于23！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.textBox10.Text = this.textBox10.Text.Substring(0, this.textBox10.Text.Length - 1);
                this.textBox10.SelectionStart = this.textBox10.Text.Length;
                this.textBox10.Focus();
                return;
            }
            else
            {
                t4_1 = int.Parse(textBox10.Text);
            }
        }
        private void textBox11_TextChanged(object sender, EventArgs e)
        {
            if (textBox11.Text == "")
            {
                t4_2 = 0;
            }
            else if (int.Parse(textBox11.Text) > 59)
            {
                MessageBox.Show("数字不能大于59！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.textBox11.Text = this.textBox11.Text.Substring(0, this.textBox11.Text.Length - 1);
                this.textBox11.SelectionStart = this.textBox11.Text.Length;
                this.textBox11.Focus();
                return;
            }
            else
            {
                t4_2 = int.Parse(textBox11.Text);
            }
        }
        private void textBox12_TextChanged(object sender, EventArgs e)
        {
            if (textBox12.Text == "")
            {
                t4_3 = 0;
            }
            else if (int.Parse(textBox12.Text) > 59)
            {
                MessageBox.Show("数字不能大于59！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.textBox12.Text = this.textBox12.Text.Substring(0, this.textBox12.Text.Length - 1);
                this.textBox12.SelectionStart = this.textBox12.Text.Length;
                this.textBox12.Focus();
                return;
            }
            else
            {
                t4_3 = int.Parse(textBox12.Text);
            }
        }
        private void textBox13_TextChanged(object sender, EventArgs e)
        {
            if (textBox13.Text == "")
            {
                t5_1 = 0;
            }
            else if (int.Parse(textBox13.Text) > 23)
            {
                MessageBox.Show("数字不能大于23！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.textBox13.Text = this.textBox13.Text.Substring(0, this.textBox13.Text.Length - 1);
                this.textBox13.SelectionStart = this.textBox13.Text.Length;
                this.textBox13.Focus();
                return;
            }
            else
            {
                t5_1 = int.Parse(textBox13.Text);
            }
        }

        private void textBox14_TextChanged(object sender, EventArgs e)
        {
            if (textBox14.Text == "")
            {
                t5_2 = 0;
            }
            else if (int.Parse(textBox14.Text) > 59)
            {
                MessageBox.Show("数字不能大于59！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.textBox14.Text = this.textBox14.Text.Substring(0, this.textBox14.Text.Length - 1);
                this.textBox14.SelectionStart = this.textBox14.Text.Length;
                this.textBox14.Focus();
                return;
            }
            else
            {
                t5_2 = int.Parse(textBox14.Text);
            }
        }

        private void textBox15_TextChanged(object sender, EventArgs e)
        {
            if (textBox15.Text == "")
            {
                t5_3 = 0;
            }
            else if (int.Parse(textBox15.Text) > 59)
            {
                MessageBox.Show("数字不能大于59！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.textBox15.Text = this.textBox15.Text.Substring(0, this.textBox15.Text.Length - 1);
                this.textBox15.SelectionStart = this.textBox15.Text.Length;
                this.textBox15.Focus();
                return;
            }
            else
            {
                t5_3 = int.Parse(textBox15.Text);
            }
        }

        private void textBox16_TextChanged(object sender, EventArgs e)
        {
            if (textBox16.Text == "")
            {
                t6_1 = 0;
            }
            else if (int.Parse(textBox16.Text) > 23)
            {
                MessageBox.Show("数字不能大于23！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.textBox16.Text = this.textBox16.Text.Substring(0, this.textBox16.Text.Length - 1);
                this.textBox16.SelectionStart = this.textBox16.Text.Length;
                this.textBox16.Focus();
                return;
            }
            else
            {
                t6_1 = int.Parse(textBox16.Text);
            }
        }

        private void textBox17_TextChanged(object sender, EventArgs e)
        {
            if (textBox17.Text == "")
            {
                t6_2 = 0;
            }
            else if (int.Parse(textBox17.Text) > 59)
            {
                MessageBox.Show("数字不能大于59！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.textBox17.Text = this.textBox17.Text.Substring(0, this.textBox17.Text.Length - 1);
                this.textBox17.SelectionStart = this.textBox17.Text.Length;
                this.textBox17.Focus();
                return;
            }
            else
            {
                t6_2 = int.Parse(textBox17.Text);
            }
        }

        private void textBox18_TextChanged(object sender, EventArgs e)
        {
            if (textBox18.Text == "")
            {
                t6_3 = 0;
            }
            else if (int.Parse(textBox18.Text) > 59)
            {
                MessageBox.Show("数字不能大于59！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.textBox18.Text = this.textBox18.Text.Substring(0, this.textBox18.Text.Length - 1);
                this.textBox18.SelectionStart = this.textBox18.Text.Length;
                this.textBox18.Focus();
                return;
            }
            else
            {
                t6_3 = int.Parse(textBox18.Text);
            }
        }

        private void textBox19_TextChanged(object sender, EventArgs e)
        {
            if (textBox19.Text == "")
            {
                t7_1 = 0;
            }
            else if (int.Parse(textBox19.Text) > 23)
            {
                MessageBox.Show("数字不能大于23！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.textBox19.Text = this.textBox19.Text.Substring(0, this.textBox19.Text.Length - 1);
                this.textBox19.SelectionStart = this.textBox19.Text.Length;
                this.textBox19.Focus();
                return;
            }
            else
            {
                t7_1 = int.Parse(textBox19.Text);
            }
        }

        private void textBox20_TextChanged(object sender, EventArgs e)
        {
            if (textBox20.Text == "")
            {
                t7_2 = 0;
            }
            else if (int.Parse(textBox20.Text) > 59)
            {
                MessageBox.Show("数字不能大于59！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.textBox20.Text = this.textBox20.Text.Substring(0, this.textBox20.Text.Length - 1);
                this.textBox20.SelectionStart = this.textBox20.Text.Length;
                this.textBox20.Focus();
                return;
            }
            else
            {
                t7_2 = int.Parse(textBox20.Text);
            }
        }

        private void textBox21_TextChanged(object sender, EventArgs e)
        {
            if (textBox21.Text == "")
            {
                t7_3 = 0;
            }
            else if (int.Parse(textBox21.Text) > 59)
            {
                MessageBox.Show("数字不能大于59！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.textBox21.Text = this.textBox21.Text.Substring(0, this.textBox21.Text.Length - 1);
                this.textBox21.SelectionStart = this.textBox21.Text.Length;
                this.textBox21.Focus();
                return;
            }
            else
            {
                t7_3 = int.Parse(textBox21.Text);
            }
        }
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar != 8 && !char.IsDigit(e.KeyChar)) && e.KeyChar != 13)
            {
                e.Handled = true;
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar != 8 && !char.IsDigit(e.KeyChar)) && e.KeyChar != 13)
            {
                e.Handled = true;
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar != 8 && !char.IsDigit(e.KeyChar)) && e.KeyChar != 13)
            {
                e.Handled = true;
            }
            else if (e.KeyChar == 13)
            {
                button1_Click(sender, e);
            }
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar != 8 && !char.IsDigit(e.KeyChar)) && e.KeyChar != 13)
            {
                e.Handled = true;
            }
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar != 8 && !char.IsDigit(e.KeyChar)) && e.KeyChar != 13)
            {
                e.Handled = true;
            }
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar != 8 && !char.IsDigit(e.KeyChar)) && e.KeyChar != 13)
            {
                e.Handled = true;
            }
            else if (e.KeyChar == 13)
            {
                button2_Click(sender, e);
            }
        }
        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar != 8 && !char.IsDigit(e.KeyChar)) && e.KeyChar != 13)
            {
                e.Handled = true;
            }
        }

        private void textBox8_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar != 8 && !char.IsDigit(e.KeyChar)) && e.KeyChar != 13)
            {
                e.Handled = true;
            }
        }

        private void textBox9_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar != 8 && !char.IsDigit(e.KeyChar)) && e.KeyChar != 13)
            {
                e.Handled = true;
            }
            else if (e.KeyChar == 13)
            {
                button3_Click(sender, e);
            }
        }

        private void textBox10_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar != 8 && !char.IsDigit(e.KeyChar)) && e.KeyChar != 13)
            {
                e.Handled = true;
            }
        }

        private void textBox11_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar != 8 && !char.IsDigit(e.KeyChar)) && e.KeyChar != 13)
            {
                e.Handled = true;
            }
        }

        private void textBox12_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar != 8 && !char.IsDigit(e.KeyChar)) && e.KeyChar != 13)
            {
                e.Handled = true;
            }
            else if (e.KeyChar == 13)
            {
                button4_Click(sender, e);
            }
        }

        private void textBox13_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar != 8 && !char.IsDigit(e.KeyChar)) && e.KeyChar != 13)
            {
                e.Handled = true;
            }
        }

        private void textBox14_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar != 8 && !char.IsDigit(e.KeyChar)) && e.KeyChar != 13)
            {
                e.Handled = true;
            }
        }

        private void textBox15_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar != 8 && !char.IsDigit(e.KeyChar)) && e.KeyChar != 13)
            {
                e.Handled = true;
            }
            else if (e.KeyChar == 13)
            {
                button5_Click(sender, e);
            }
        }

        private void textBox16_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar != 8 && !char.IsDigit(e.KeyChar)) && e.KeyChar != 13)
            {
                e.Handled = true;
            }
        }

        private void textBox17_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar != 8 && !char.IsDigit(e.KeyChar)) && e.KeyChar != 13)
            {
                e.Handled = true;
            }
        }

        private void textBox18_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar != 8 && !char.IsDigit(e.KeyChar)) && e.KeyChar != 13)
            {
                e.Handled = true;
            }
            else if (e.KeyChar == 13)
            {
                button6_Click(sender, e);
            }
        }

        private void textBox19_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar != 8 && !char.IsDigit(e.KeyChar)) && e.KeyChar != 13)
            {
                e.Handled = true;
            }
        }

        private void textBox20_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar != 8 && !char.IsDigit(e.KeyChar)) && e.KeyChar != 13)
            {
                e.Handled = true;
            }
        }

        private void textBox21_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar != 8 && !char.IsDigit(e.KeyChar)) && e.KeyChar != 13)
            {
                e.Handled = true;
            }
            else if (e.KeyChar == 13)
            {
                button7_Click(sender, e);
            }
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            textBox1.SelectAll();
        }

        private void textBox2_Click(object sender, EventArgs e)
        {
            textBox2.SelectAll();
        }

        private void textBox3_Click(object sender, EventArgs e)
        {
            textBox3.SelectAll();
        }

        private void textBox4_Click(object sender, EventArgs e)
        {
            textBox4.SelectAll();
        }

        private void textBox5_Click(object sender, EventArgs e)
        {
            textBox5.SelectAll();
        }

        private void textBox6_Click(object sender, EventArgs e)
        {
            textBox6.SelectAll();
        }

        private void textBox7_Click(object sender, EventArgs e)
        {
            textBox7.SelectAll();
        }

        private void textBox8_Click(object sender, EventArgs e)
        {
            textBox8.SelectAll();
        }

        private void textBox9_Click(object sender, EventArgs e)
        {
            textBox9.SelectAll();
        }

        private void textBox10_Click(object sender, EventArgs e)
        {
            textBox10.SelectAll();
        }

        private void textBox11_Click(object sender, EventArgs e)
        {
            textBox11.SelectAll();
        }

        private void textBox12_Click(object sender, EventArgs e)
        {
            textBox12.SelectAll();
        }

        private void textBox13_Click(object sender, EventArgs e)
        {
            textBox13.SelectAll();
        }

        private void textBox14_Click(object sender, EventArgs e)
        {
            textBox14.SelectAll();
        }

        private void textBox15_Click(object sender, EventArgs e)
        {
            textBox15.SelectAll();
        }

        private void textBox16_Click(object sender, EventArgs e)
        {
            textBox16.SelectAll();
        }

        private void textBox17_Click(object sender, EventArgs e)
        {
            textBox17.SelectAll();
        }

        private void textBox18_Click(object sender, EventArgs e)
        {
            textBox18.SelectAll();
        }

        private void textBox19_Click(object sender, EventArgs e)
        {
            textBox19.SelectAll();
        }

        private void textBox20_Click(object sender, EventArgs e)
        {
            textBox20.SelectAll();
        }

        private void textBox21_Click(object sender, EventArgs e)
        {
            textBox21.SelectAll();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            //label29.Text = "当前系统时间：" + DateTime.Now.TimeOfDay.ToString();
            label29.Text = "当前系统时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss dddd");
            this.Opacity += 0.04;
        }

        /// ****************************************
        /// FunctionName：	Form1_SizeChanged
        /// Param：			最小化到托盘
        /// Returns：			void
        /// CreateDate：		2014/09/11
        /// Remarks：		
        /// ****************************************
        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Minimized;// 将窗体变为最小化
                this.ShowInTaskbar = false; //不显示在系统任务栏
                notifyIcon1.Visible = true; //托盘图标可见
                Show_ToolStripMenuItem.Enabled = true;
            }
            else
            {
                Show_ToolStripMenuItem.Enabled = false;
            }
        }

        /// ****************************************
        /// FunctionName：	notifyIcon1_MouseDoubleClick
        /// Param：			双击显示窗口
        /// Returns：			void
        /// CreateDate：		2014/09/11
        /// Remarks：		
        /// ****************************************
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Normal;
                this.ShowInTaskbar = true;
                Show_ToolStripMenuItem.Enabled = false;
            }
            else
            {
                Show_ToolStripMenuItem.Enabled = true;
            }
            this.Activate();
        }

        /// ****************************************
        /// FunctionName：	Show_ToolStripMenuItem_Click
        /// Param：			托盘菜单－显示
        /// Returns：			void
        /// CreateDate：		2014/09/11
        /// Remarks：		
        /// ****************************************
        private void Show_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Normal;
                this.ShowInTaskbar = true;
            }
            this.Activate();
        }

        /// ****************************************
        /// FunctionName：	About_ToolStripMenuItem_Click
        /// Param：			托盘菜单－关于
        /// Returns：			void
        /// CreateDate：		2014/09/13
        /// Remarks：		
        /// ****************************************
        private void About_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (F2 != null)
            {
                if (F2.IsDisposed)
                {
                    F2 = new Form2();
                    F2.Show();
                    F2.Focus();
                }
                else
                {
                    F2.Show();
                    F2.Focus();
                }
            }
            else
            {
                F2 = new Form2();
                F2.Show();
                F2.Focus();
            }
            F2.Show();
        }

        /// ****************************************
        /// FunctionName：	Exit_ToolStripMenuItem_Click
        /// Param：			托盘菜单－退出
        /// Returns：			void
        /// CreateDate：		2014/09/11
        /// Remarks：		
        /// ****************************************
        private void Exit_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About_ToolStripMenuItem.Enabled = false;
            this.Close();
            Application.Exit();
        }        
    }
}
