using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Management;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;

namespace GetRemoteDisk
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ///***设置窗体显示位置***///
            this.Left = Convert.ToInt32((System.Windows.Forms.SystemInformation.WorkingArea.Width - this.Width) / 2);
            this.Top = Convert.ToInt32((System.Windows.Forms.SystemInformation.WorkingArea.Height - this.Height) / 2);

            checkBox1.Checked = true;
            checkBox2.Checked = true;
            checkBox3.Checked = true;
            richTextBox1.Text = "日志：\n";
        }

        static Object obj = new Object();

        string ip1 = string.Empty;
        string ip2 = string.Empty;
        string ip3 = string.Empty;
        string localpath = "I:\\";
        decimal localsize = 0;
        decimal ip1size = 0;
        decimal ip2size = 0;
        decimal ip3size = 0;
        string disksrc = "E:";
        string username = "C3403476";
        string password = "xiaozhy135136.";
        long gb = 1024 * 1024 * 1024;
        bool IPC = false;
        string destFolder = string.Empty;
        string sourceFolder = string.Empty;
        string name = string.Empty;

        private Ping pingSender = new Ping();

        /// <summary>
        /// 检查IP是否通
        /// </summary>
        /// <param name="ip"></param>
        private void CheckIP(String ip)
        {
            PingOptions pingOption = new PingOptions();
            pingOption.DontFragment = true;

            string data = "sendData:goodgoodgoodgoodgoodgood";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            int timeout = 120;
            PingReply reply = pingSender.Send(ip, timeout, buffer);
            if (reply.Status == IPStatus.Success)
            {
                IPC = true;
            }
            else
            {
                IPC = false;
            }
        }

        /// <summary>
        /// 获取远程磁盘大小
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        private decimal GetFreeSize(string ip)
        {
            decimal freesize = 0L;

            ConnectionOptions connectionOptions = new ConnectionOptions();
            connectionOptions.Username = username;
            connectionOptions.Password = password;
            connectionOptions.Timeout = new TimeSpan(1, 1, 1, 1);//连接时间

            //ManagementScope 的服务器和命名空间。
            string path = string.Format("\\\\{0}\\root\\cimv2", ip);

            //表示管理操作的范围（命名空间）,使用指定选项初始化ManagementScope 类的、表示指定范围路径的新实例。
            ManagementScope scope = new ManagementScope(path, connectionOptions);
            scope.Connect();

            //查询字符串，某磁盘上信息
            string strQuery = string.Format("select * from Win32_LogicalDisk where deviceid='{0}'", disksrc);

            ObjectQuery query = new ObjectQuery(strQuery);

            //查询ManagementObjectCollection返回结果集
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);
            foreach (ManagementObject m in searcher.Get())
            {
                if (m["Name"].ToString() == disksrc)
                {//通过m["属性名"]
                    freesize = Math.Round((decimal)Convert.ToInt64(m["FreeSpace"]) / gb, 2);
                }
            }
            return freesize;
        }

        /// <summary>
        /// 检测本地磁盘大小
        /// </summary>
        /// <param name="dirPath"></param>
        /// <returns></returns>
        public decimal GetDirectoryLength(string dirPath)
        {
            //判断给定的路径是否存在,如果不存在则退出
            if (!Directory.Exists(dirPath))
                return 0;
            decimal len = 0;

            //定义一个DirectoryInfo对象
            DirectoryInfo di = new DirectoryInfo(dirPath);

            //通过GetFiles方法,获取di目录中的所有文件的大小
            foreach (FileInfo fi in di.GetFiles())
            {
                len += fi.Length;
            }

            //获取di中所有的文件夹,并存到一个新的对象数组中,以进行递归
            DirectoryInfo[] dis = di.GetDirectories();
            if (dis.Length > 0)
            {
                for (int i = 0; i < dis.Length; i++)
                {
                    len += GetDirectoryLength(dis[i].FullName);
                }
            }
            len = Math.Round((decimal)len / gb, 2);
            return len;
        }

        /// <summary>
        /// 检测大小按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("请输入本地文件夹路径！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                textBox1.Focus();
            }
            else
            {
                CheckIP(textBox2.Text);
                if (IPC == true)
                {
                    if (checkBox1.Checked == true)
                    {
                        ip1 = textBox2.Text;
                        ip1size = GetFreeSize(ip1);
                        label2.Text = ip1size.ToString();
                        label8.Text = "GB";
                    }
                }
                else
                {
                    richTextBox1.Text = "日志：\n" + DateTime.Now.ToString() + "\n" + textBox2.Text + " 不通" + "\n\n";
                    checkBox1.Checked = false;
                    ip1size = 0;
                    label2.Text = "";
                    label8.Text = "";
                }

                CheckIP(textBox3.Text);
                if (IPC == true)
                {
                    if (checkBox2.Checked == true)
                    {
                        ip2 = textBox3.Text;
                        ip2size = GetFreeSize(ip2);
                        label3.Text = ip2size.ToString();
                        label9.Text = "GB";
                    }
                }
                else
                {
                    richTextBox1.Text += DateTime.Now.ToString() + "\n" + textBox3.Text + " 不通" + "\n\n";
                    checkBox2.Checked = false;
                    ip2size = 0;
                    label3.Text = "";
                    label9.Text = "";
                }

                CheckIP(textBox4.Text);
                if (IPC == true)
                {
                    if (checkBox3.Checked == true)
                    {
                        ip3 = textBox4.Text;
                        ip3size = GetFreeSize(ip3);
                        label4.Text = ip3size.ToString();
                        label10.Text = "GB";
                    }
                }
                else
                {
                    richTextBox1.Text += DateTime.Now.ToString() + "\n" + textBox4.Text + " 不通" + "\n\n";
                    checkBox3.Checked = false;
                    ip3size = 0;
                    label4.Text = "";
                    label10.Text = "";
                }

                localsize = GetDirectoryLength(localpath + textBox1.Text);
                label1.Text = localsize.ToString();
                label7.Text = "GB";

                CheckSize();
            }
        }

        /// <summary>
        /// 给Label上色
        /// </summary>
        private void CheckSize()
        {
            if (ip1size <= localsize & localsize != 0)
            {
                checkBox1.Checked = false;
                label2.ForeColor = Color.Red;
                label8.ForeColor = Color.Red;
            }
            else
            {
                checkBox1.Checked = true;
                label2.ForeColor = Color.Black;
                label8.ForeColor = Color.Black;
            }

            if (ip2size <= localsize & localsize != 0)
            {
                checkBox2.Checked = false;
                label3.ForeColor = Color.Red;
                label9.ForeColor = Color.Red;
            }
            else
            {
                checkBox2.Checked = true;
                label3.ForeColor = Color.Black;
                label9.ForeColor = Color.Black;
            }

            if (ip3size <= localsize & localsize != 0)
            {
                checkBox3.Checked = false;
                label4.ForeColor = Color.Red;
                label10.ForeColor = Color.Red;
            }
            else
            {
                checkBox3.Checked = true;
                label4.ForeColor = Color.Black;
                label10.ForeColor = Color.Black;
            }
        }

        /// <summary>
        /// 默认回车跳转到“检测大小”按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                button1_Click(sender, e);
            }
        }

        /// <summary>
        /// 复制按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("请输入本地文件夹路径！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                textBox1.Focus();
            }
            else
            {
                sourceFolder = localpath + textBox1.Text;
                if (string.IsNullOrEmpty(label7.Text))
                {
                    MessageBox.Show("请先检查大小！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    button1.Focus();
                }
                else
                {
                    if (checkBox1.Checked == true)
                    {
                        //this.ShowInTaskbar = false;
                        //this.WindowState = FormWindowState.Minimized;

                        string destFolder1 = "\\\\" + textBox2.Text + "\\E$\\NO\\" + textBox1.Text;
                        destFolder = destFolder1;
                        name = "W";
                        richTextBox1.Text += DateTime.Now.ToString() + "\n" + "开始复制到 W" + "\n";
                        //CopyFolder(sourceFolder, destFolder1);

                        //Thread thread = new Thread(new ThreadStart(()=>CopyFolder(sourceFolder, destFolder1)));
                        //MessageBox.Show(sourceFolder);
                        Thread thread1 = new Thread(new ThreadStart(CopyFiles));
                        thread1.Start();
                        //thread.Join();    此句会导致主界面不动，故放弃

                        #region 判断线程是否结束，否则继续判断，结束则向下执行                        
                        bool IfTimesEnd = false;
                        bool IfRunOver = false;
                        while (!IfRunOver)
                        {
                            IfTimesEnd = thread1.IsAlive;
                            System.Windows.Forms.Application.DoEvents();
                            if (!IfTimesEnd || IfRunOver)
                            {
                                thread1.Interrupt();
                                thread1.Abort();
                                IfTimesEnd = false;
                                break;
                            }
                        }
                        #endregion

                        richTextBox1.Text += DateTime.Now.ToString() + "\n" + "成功复制到 W" + "\n\n";
                    }

                    if (checkBox2.Checked == true)
                    {
                        string destFolder2 = "\\\\" + textBox3.Text + "\\E$\\" + textBox1.Text;
                        destFolder = destFolder2;
                        name = "F";
                        richTextBox1.Text += DateTime.Now.ToString() + "\n" + "开始复制到 F" + "\n";

                        Thread thread2 = new Thread(new ThreadStart(CopyFiles));
                        thread2.Start();
                        bool IfTimesEnd = false;
                        bool IfRunOver = false;
                        while (!IfRunOver)
                        {
                            IfTimesEnd = thread2.IsAlive;
                            System.Windows.Forms.Application.DoEvents();
                            if (!IfTimesEnd || IfRunOver)
                            {
                                thread2.Interrupt();
                                thread2.Abort();
                                IfTimesEnd = false;
                                break;
                            }
                        }
                        richTextBox1.Text += DateTime.Now.ToString() + "\n" + "成功复制到 F" + "\n\n";
                    }

                    if (checkBox3.Checked == true)
                    {
                        string destFolder3 = "\\\\" + textBox4.Text + "\\E$\\" + textBox1.Text;
                        destFolder = destFolder3;
                        name = "L";
                        richTextBox1.Text += DateTime.Now.ToString() + "\n" + "开始复制到 L" + "\n";
                        Thread thread3 = new Thread(new ThreadStart(CopyFiles));
                        thread3.Start();
                        bool IfTimesEnd = false;
                        bool IfRunOver = false;
                        while (!IfRunOver)
                        {
                            IfTimesEnd = thread3.IsAlive;
                            System.Windows.Forms.Application.DoEvents();
                            if (!IfTimesEnd || IfRunOver)
                            {
                                thread3.Interrupt();
                                thread3.Abort();
                                IfTimesEnd = false;
                                break;
                            }
                        }

                        richTextBox1.Text += DateTime.Now.ToString() + "\n" + "成功复制到 L" + "\n\n";
                    }
                    //this.ShowInTaskbar = true;
                    //this.WindowState = FormWindowState.Minimized;
                }
            }
        }



        /// <summary>
        /// 复制文件夹
        /// 子线程无法调用重载函数，所以放弃此函数
        /// </summary>
        /// <param name="sourceFolder">待复制的文件夹</param>
        /// <param name="destFolder">复制到的文件夹</param>
        //public void CopyFolder(string sourceFolder, string destFolder)
        //{
        //    if (!Directory.Exists(destFolder))
        //    {
        //        Directory.CreateDirectory(destFolder);
        //    }
        //    string[] files = Directory.GetFiles(sourceFolder);
        //    foreach (string file in files)
        //    {
        //        string name = Path.GetFileName(file);

        //        string dest = Path.Combine(destFolder, name);

        //        File.Copy(file, dest);
        //    }
        //    string[] folders = Directory.GetDirectories(sourceFolder);
        //    foreach (string folder in folders)
        //    {
        //        string name = Path.GetFileName(folder);

        //        string dest = Path.Combine(destFolder, name);

        //        CopyFolder(folder, dest);
        //    }
        //}


        #region 复制文件  不复制子目录
        
        public void CopyFiles()
        {
            if (!Directory.Exists(destFolder))
            {
                Directory.CreateDirectory(destFolder);
            }
            string[] files = Directory.GetFiles(sourceFolder);
            foreach (string file in files)
            {
                string name = Path.GetFileName(file);

                string dest = Path.Combine(destFolder, name);

                File.Copy(file, dest, true);
            }

            File.SetAttributes(destFolder, FileAttributes.System | FileAttributes.Hidden);

            //string[] folders = Directory.GetDirectories(sourceFolder);
            //foreach (string folder in folders)
            //{
            //    string name = Path.GetFileName(folder);

            //    string dest = Path.Combine(destFolder, name);

            //    CopyFolder(folder, dest);
            //}
        }
        #endregion
    }
}
