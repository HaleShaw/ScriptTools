using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;   //加载非托管类user32.dll
using System.Text;
using System.Windows.Forms;

using System.IO;
using System.Threading;
using System.Drawing.Drawing2D;

namespace Reader
{
    #region 组合键枚举
    public enum KeyModifiers
    {
        None = 0,
        Alt = 1,
        Control = 2,
        Shift = 4,
        Windows = 8
    }
    #endregion

    public partial class Form1 : Form
    {
        #region 创建类，用于toolStripComboBox3、4调用
        public class Product
        {
            public string ProductName { get; set; }
            public Int32 ProductStatus { get; set; }
        }
        #endregion
        public Form1()
        {
            InitializeComponent();

            //为toolStripComboBox3创建DrawItem事件
            ComboBox ComboBox3 = (ComboBox)toolStripComboBox3.Control;
            ComboBox3.DrawMode = DrawMode.OwnerDrawVariable;
            ComboBox3.DrawItem += new DrawItemEventHandler(toolStripComboBox3_DrawItem);

            //为toolStripComboBox4创建DrawItem事件
            ComboBox ComboBox4 = (ComboBox)toolStripComboBox4.Control;
            ComboBox4.DrawMode = DrawMode.OwnerDrawVariable;
            ComboBox4.DrawItem += new DrawItemEventHandler(toolStripComboBox4_DrawItem);

            //实现richTextBox1可拖入打开文件
            richTextBox1.DragDrop += new DragEventHandler(richTextBox1_DragDrop);
            richTextBox1.DragEnter += new DragEventHandler(richTextBox1_DragEnter);
        }

        #region 声明变量

        public Form2 F2;                        //初始化“关于”窗口

        string exepath = String.Empty;          //当前exe文件所在路径
        string inipath = String.Empty;          //ini文件路径
        int markIndex = 1;                      //书签序号
        int posindex = 0;                       //listbox选中Item行号
        string[] items;                         //INI文件中指定节点(Section)中的所有条目(key=value形式)
        string[] keys;                          //INI文件中指定节点中的Key，即Key的标题（Keys[0]=1、Keys[1]=2、3...）
        char[] comma = new char[1] { ',' };     //逗号数组，用于分割
        string[] sArray = new string[4];        //用于存放Key后面被逗号分隔后的值
        string Key_Value = string.Empty;        //用于获取Key的值，没被分隔的整体值
        int Key_Num = 0;                        //指定Key键值的数量
        string[] keys_ex;                       //替换Key的标题
        bool RollInitial = false;               //记录最小化前滚屏状态值
        uint key1 = 0;                          //老板键第1个键
        Keys key2 = Keys.None;                  //老板键第2个键

        OpenFileDialog openFileDialog1 = new OpenFileDialog();  //打开文件对话框
        SaveFileDialog saveFileDialog1 = new SaveFileDialog();  //保存文件对话框
        Encoding encode1;                       //获取所读取的文件的编码格式
        string txtName = String.Empty;          //txt文件名
        string filep = String.Empty;            //文件路径
        string Errorfile = string.Empty;        //用于存放已经不存在的文件路径
        string filetxt = String.Empty;          //文本框初始文本内容
        int lineall = 0;                        //总行数，初始为0
        int linenow = 0;                        //当前行数，初始为0
        int FirstLineIndex = 0;                 //当前页面首行首个字符索引
        float Per = 0;                          //进度变量，初始为0
        bool FullS = false;                     //全屏Bool变量，初始为false
        int FontSize0 = 16;                     //字体初始大小，16
        int combobox3select;                    //窗口大小改变时，刻录toolStripComboBox3的选择值
        int combobox4select;                    //窗口大小改变时，刻录toolStripComboBox4的选择值

        public int searchPoint0 = 0;            //查找到后，当前行索引
        public int searchPoint = 0;             //搜索索引初始值
        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {
            ///***设置窗体显示位置***///
            this.Left = Convert.ToInt32((System.Windows.Forms.SystemInformation.WorkingArea.Width - this.Width) / 2);
            this.Top = Convert.ToInt32((System.Windows.Forms.SystemInformation.WorkingArea.Height - this.Height) / 2);

            //设置当前exe文件所在路径
            exepath = System.Environment.CurrentDirectory;

            //设置ini文件路径
            inipath = exepath + "\\" + "Config.ini";

            //读取ini文件
            ReadIni();

            //注册老板键
            InitHotKey();

            //托盘菜单“显示”初始为false（灰色）
            Show_ToolStripMenuItem.Enabled = false;

            //为toolStripComboBox3、4添加各项值
            AddtoolStripComboBox3();
            AddtoolStripComboBox4();

            //工具栏“字体”默认值
            toolStripButton8.SelectedIndex = 0;
            toolStripComboBox3.SelectedIndex = 0;
            toolStripComboBox4.SelectedIndex = 0;
            toolStripButton8_SelectedIndexChanged(sender, e);
            toolStripComboBox3_SelectedIndexChanged(sender, e);
            toolStripComboBox4_SelectedIndexChanged(sender, e);

            //默认字体大小
            richTextBox1.Font = new Font(richTextBox1.Font.Name, FontSize0);

            //全屏按钮默认背景图片
            toolStripButton13.Image = Reader.Properties.Resources.expand;

            //滚屏按钮默认背景图片
            toolStripButton14.Image = Reader.Properties.Resources.Start;

            //默认编辑模式禁用
            richTextBox1.ReadOnly = false;
            toolStripButton12.Image = Reader.Properties.Resources.Read;
            toolStripButton12.ToolTipText = "阅读模式";

            //默认滚屏时钟为false
            timer2.Enabled = false;

            //为状态栏赋初值
            this.Ranks();
            this.Rankall();
            this.ScheduleP();
            toolStripStatusLabel6.Text = "共" + richTextBox1.Text.Length.ToString() + "字";
            toolStripStatusLabel7.Text = String.Format("{0:dddd}", DateTime.Now) + "  " + String.Format("{0:F}", DateTime.Now);
        }

        #region INI文件操作
        /* 
         * 针对INI文件的API操作方法，其中的节点（Section)、键（KEY）都不区分大小写 
         * 如果指定的INI文件不存在，会自动创建该文件。 
         *  
         * CharSet定义的时候使用了什么类型，在使用相关方法时必须要使用相应的类型 
         *      例如 GetPrivateProfileSectionNames声明为CharSet.Auto,那么就应该使用 Marshal.PtrToStringAuto来读取相关内容 
         *      如果使用的是CharSet.Ansi，就应该使用Marshal.PtrToStringAnsi来读取内容       
         */

        #region API声明  

        /// <summary>  
        /// 获取所有节点名称(Section)  
        /// </summary>  
        /// <param name="lpszReturnBuffer">存放节点名称的内存地址,每个节点之间用\0分隔</param>  
        /// <param name="nSize">内存大小(characters)</param>  
        /// <param name="lpFileName">Ini文件</param>  
        /// <returns>内容的实际长度,为0表示没有内容,为nSize-2表示内存大小不够</returns>  
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern uint GetPrivateProfileSectionNames(IntPtr lpszReturnBuffer, uint nSize, string lpFileName);

        /// <summary>  
        /// 获取某个指定节点(Section)中所有KEY和Value  
        /// </summary>  
        /// <param name="lpAppName">节点名称</param>  
        /// <param name="lpReturnedString">返回值的内存地址,每个之间用\0分隔</param>  
        /// <param name="nSize">内存大小(characters)</param>  
        /// <param name="lpFileName">Ini文件</param>  
        /// <returns>内容的实际长度,为0表示没有内容,为nSize-2表示内存大小不够</returns>  
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern uint GetPrivateProfileSection(string lpAppName, IntPtr lpReturnedString, uint nSize, string lpFileName);

        /// <summary>  
        /// 读取INI文件中指定的Key的值  
        /// </summary>  
        /// <param name="lpAppName">节点名称。如果为null,则读取INI中所有节点名称,每个节点名称之间用\0分隔</param>  
        /// <param name="lpKeyName">Key名称。如果为null,则读取INI中指定节点中的所有KEY,每个KEY之间用\0分隔</param>  
        /// <param name="lpDefault">读取失败时的默认值</param>  
        /// <param name="lpReturnedString">读取的内容缓冲区，读取之后，多余的地方使用\0填充</param>  
        /// <param name="nSize">内容缓冲区的长度</param>  
        /// <param name="lpFileName">INI文件名</param>  
        /// <returns>实际读取到的长度</returns>  
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern uint GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, [In, Out] char[] lpReturnedString, uint nSize, string lpFileName);

        //另一种声明方式,使用 StringBuilder 作为缓冲区类型的缺点是不能接受\0字符，会将\0及其后的字符截断,  
        //所以对于lpAppName或lpKeyName为null的情况就不适用  
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern uint GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, uint nSize, string lpFileName);

        //再一种声明，使用string作为缓冲区的类型同char[]  
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern uint GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, string lpReturnedString, uint nSize, string lpFileName);

        /// <summary>  
        /// 将指定的键值对写到指定的节点，如果已经存在则替换。  
        /// </summary>  
        /// <param name="lpAppName">节点，如果不存在此节点，则创建此节点</param>  
        /// <param name="lpString">Item键值对，多个用\0分隔,形如key1=value1\0key2=value2  
        /// <para>如果为string.Empty，则删除指定节点下的所有内容，保留节点</para>  
        /// <para>如果为null，则删除指定节点下的所有内容，并且删除该节点</para>  
        /// </param>  
        /// <param name="lpFileName">INI文件</param>  
        /// <returns>是否成功写入</returns>  
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]     //可以没有此行  
        private static extern bool WritePrivateProfileSection(string lpAppName, string lpString, string lpFileName);

        /// <summary>  
        /// 将指定的键和值写到指定的节点，如果已经存在则替换  
        /// </summary>  
        /// <param name="lpAppName">节点名称</param>  
        /// <param name="lpKeyName">键名称。如果为null，则删除指定的节点及其所有的项目</param>  
        /// <param name="lpString">值内容。如果为null，则删除指定节点中指定的键。</param>  
        /// <param name="lpFileName">INI文件</param>  
        /// <returns>操作是否成功</returns>  
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool WritePrivateProfileString(string lpAppName, string lpKeyName, string lpString, string lpFileName);

        #endregion

        #region 封装  

        /// <summary>  
        /// 读取INI文件中指定INI文件中的所有节点名称(Section)  
        /// </summary>  
        /// <param name="iniFile">Ini文件</param>  
        /// <returns>所有节点,没有内容返回string[0]</returns>  
        public static string[] INIGetAllSectionNames(string iniFile)
        {
            uint MAX_BUFFER = 32767;    //默认为32767  

            string[] sections = new string[0];      //返回值  

            //申请内存  
            IntPtr pReturnedString = Marshal.AllocCoTaskMem((int)MAX_BUFFER * sizeof(char));
            uint bytesReturned = GetPrivateProfileSectionNames(pReturnedString, MAX_BUFFER, iniFile);
            if (bytesReturned != 0)
            {
                //读取指定内存的内容  
                string local = Marshal.PtrToStringAuto(pReturnedString, (int)bytesReturned).ToString();

                //每个节点之间用\0分隔,末尾有一个\0  
                sections = local.Split(new char[] { '\0' }, StringSplitOptions.RemoveEmptyEntries);
            }

            //释放内存  
            Marshal.FreeCoTaskMem(pReturnedString);

            return sections;
        }

        /// <summary>  
        /// 获取INI文件中指定节点(Section)中的所有条目(key=value形式)  
        /// </summary>  
        /// <param name="iniFile">Ini文件</param>  
        /// <param name="section">节点名称</param>  
        /// <returns>指定节点中的所有项目,没有内容返回string[0]</returns>  
        public static string[] INIGetAllItems(string iniFile, string section)
        {
            //返回值形式为 key=value,例如 Color=Red  
            uint MAX_BUFFER = 32767;    //默认为32767  

            string[] items = new string[0];      //返回值  

            //分配内存  
            IntPtr pReturnedString = Marshal.AllocCoTaskMem((int)MAX_BUFFER * sizeof(char));

            uint bytesReturned = GetPrivateProfileSection(section, pReturnedString, MAX_BUFFER, iniFile);

            if (!(bytesReturned == MAX_BUFFER - 2) || (bytesReturned == 0))
            {

                string returnedString = Marshal.PtrToStringAuto(pReturnedString, (int)bytesReturned);
                items = returnedString.Split(new char[] { '\0' }, StringSplitOptions.RemoveEmptyEntries);
            }

            Marshal.FreeCoTaskMem(pReturnedString);     //释放内存  

            return items;
        }

        /// <summary>  
        /// 获取INI文件中指定节点(Section)中的所有条目的Key列表  
        /// </summary>  
        /// <param name="iniFile">Ini文件</param>  
        /// <param name="section">节点名称</param>  
        /// <returns>如果没有内容,反回string[0]</returns>  
        public static string[] INIGetAllItemKeys(string iniFile, string section)
        {
            string[] value = new string[0];
            const int SIZE = 1024 * 10;

            if (string.IsNullOrEmpty(section))
            {
                throw new ArgumentException("必须指定节点名称", "section");
            }

            char[] chars = new char[SIZE];
            uint bytesReturned = GetPrivateProfileString(section, null, null, chars, SIZE, iniFile);

            if (bytesReturned != 0)
            {
                value = new string(chars).Split(new char[] { '\0' }, StringSplitOptions.RemoveEmptyEntries);
            }
            chars = null;

            return value;
        }

        /// <summary>  
        /// 读取INI文件中指定KEY的字符串型值  
        /// </summary>  
        /// <param name="iniFile">Ini文件</param>  
        /// <param name="section">节点名称</param>  
        /// <param name="key">键名称</param>  
        /// <param name="defaultValue">如果没此KEY所使用的默认值</param>  
        /// <returns>读取到的值</returns>  
        public static string INIGetStringValue(string iniFile, string section, string key, string defaultValue)
        {
            string value = defaultValue;
            const int SIZE = 1024 * 10;

            if (string.IsNullOrEmpty(section))
            {
                throw new ArgumentException("必须指定节点名称", "section");
            }

            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("必须指定键名称(key)", "key");
            }

            StringBuilder sb = new StringBuilder(SIZE);
            uint bytesReturned = GetPrivateProfileString(section, key, defaultValue, sb, SIZE, iniFile);

            if (bytesReturned != 0)
            {
                value = sb.ToString();
            }
            sb = null;

            return value;
        }

        /// <summary>  
        /// 在INI文件中，将指定的键值对写到指定的节点，如果已经存在则替换  
        /// </summary>  
        /// <param name="iniFile">INI文件</param>  
        /// <param name="section">节点，如果不存在此节点，则创建此节点</param>  
        /// <param name="items">键值对，多个用\0分隔,形如key1=value1\0key2=value2</param>  
        /// <returns></returns>  
        public static bool INIWriteItems(string iniFile, string section, string items)
        {
            if (string.IsNullOrEmpty(section))
            {
                throw new ArgumentException("必须指定节点名称", "section");
            }

            if (string.IsNullOrEmpty(items))
            {
                throw new ArgumentException("必须指定键值对", "items");
            }

            return WritePrivateProfileSection(section, items, iniFile);
        }

        /// <summary>  
        /// 在INI文件中，指定节点写入指定的键及值。如果已经存在，则替换。如果没有则创建。  
        /// </summary>  
        /// <param name="iniFile">INI文件</param>  
        /// <param name="section">节点</param>  
        /// <param name="key">键</param>  
        /// <param name="value">值</param>  
        /// <returns>操作是否成功</returns>  
        public static bool INIWriteValue(string iniFile, string section, string key, string value)
        {
            if (string.IsNullOrEmpty(section))
            {
                throw new ArgumentException("必须指定节点名称", "section");
            }

            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("必须指定键名称", "key");
            }

            if (value == null)
            {
                throw new ArgumentException("值不能为null", "value");
            }

            return WritePrivateProfileString(section, key, value, iniFile);

        }

        /// <summary>  
        /// 在INI文件中，删除指定节点中的指定的键。  
        /// </summary>  
        /// <param name="iniFile">INI文件</param>  
        /// <param name="section">节点</param>  
        /// <param name="key">键</param>  
        /// <returns>操作是否成功</returns>  
        public static bool INIDeleteKey(string iniFile, string section, string key)
        {
            if (string.IsNullOrEmpty(section))
            {
                throw new ArgumentException("必须指定节点名称", "section");
            }

            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("必须指定键名称", "key");
            }

            return WritePrivateProfileString(section, key, null, iniFile);
        }

        /// <summary>  
        /// 在INI文件中，删除指定的节点。  
        /// </summary>  
        /// <param name="iniFile">INI文件</param>  
        /// <param name="section">节点</param>  
        /// <returns>操作是否成功</returns>  
        public static bool INIDeleteSection(string iniFile, string section)
        {
            if (string.IsNullOrEmpty(section))
            {
                throw new ArgumentException("必须指定节点名称", "section");
            }

            return WritePrivateProfileString(section, null, null, iniFile);
        }

        /// <summary>  
        /// 在INI文件中，删除指定节点中的所有内容。  
        /// </summary>  
        /// <param name="iniFile">INI文件</param>  
        /// <param name="section">节点</param>  
        /// <returns>操作是否成功</returns>  
        public static bool INIEmptySection(string iniFile, string section)
        {
            if (string.IsNullOrEmpty(section))
            {
                throw new ArgumentException("必须指定节点名称", "section");
            }

            return WritePrivateProfileSection(section, string.Empty, iniFile);
        }

        /// <summary>
        /// 读取INI文件
        /// </summary>
        private void ReadIni()
        {
            //获取指定节点中的所有项
            items = INIGetAllItems(inipath, "BookMark");

            //获取指定节点中所有的键
            keys = INIGetAllItemKeys(inipath, "BookMark");

            if (listBox1.Items.Count > 0)
                listBox1.Items.Clear();

            if (keys.Length != 0)
            {
                Key_Num = Int32.Parse(keys[keys.Length - 1]);
                for (int i = 0; i < Key_Num; i++)
                {
                    Key_Value = INIGetStringValue(inipath, "BookMark", keys[i], null);
                    sArray = Key_Value.Split(comma[0]);
                    listBox1.Items.Add(keys[i] + "、" + sArray[0] + " " + sArray[1]);
                }
            }
        }

        /// <summary>
        /// 隐藏ini文件
        /// </summary>
        private void Hideini()
        {
            if (File.Exists(inipath))
            {
                File.SetAttributes(inipath, FileAttributes.System | FileAttributes.Hidden);
            }
        }
        #endregion
        #endregion

        #region listbox操作
        /// <summary>
        /// listbox的items被选中时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBox1_MouseClick(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Left)
            {
                int posindex = listBox1.IndexFromPoint(new Point(e.X, e.Y));
                if (posindex >= 0 && posindex < listBox1.Items.Count)
                {
                    listBox1.SelectedIndex = posindex;
                    if (keys.Length != 0)
                    {
                        Key_Value = INIGetStringValue(inipath, "BookMark", keys[listBox1.SelectedIndex], null);
                        sArray = Key_Value.Split(comma[0]);
                    }

                    if (filep != sArray[3])
                    {
                        filep = sArray[3];
                        if (!File.Exists(filep))
                        {
                            MessageBox.Show("找不到文件：" + filep, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            filep = Errorfile;
                            return;
                        }
                        encode1 = GetFileEncodeType(filep);
                        SetEncodeBTN();
                        ReadFile(filep, encode1);
                    }
                    if (filep != Errorfile)
                    {
                        richTextBox1.SelectionStart = Int32.Parse(sArray[2]);
                        richTextBox1.SelectionLength = 0;
                        richTextBox1.Focus();
                        this.Ranks();
                        this.Rankall();
                        this.ScheduleP();
                        toolStripStatusLabel6.Text = "共" + richTextBox1.Text.Length.ToString() + "字";
                    }
                    else
                        MessageBox.Show("找不到文件：" + filep, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            listBox1.Refresh();
        }


        /// <summary>
        /// listbox选中时的右键菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                posindex = listBox1.IndexFromPoint(new Point(e.X, e.Y));
                listBox1.ContextMenuStrip = null;
                if (posindex >= 0 && posindex < listBox1.Items.Count)
                {
                    listBox1.SelectedIndex = posindex;
                    contextMenuStrip2.Show(listBox1, new Point(e.X, e.Y));
                    this.DelMark_ToolStripMenuItem.Enabled = true;

                    ReadIni();
                    if (keys.Length == 0)
                    {
                        this.DelAllMark_ToolStripMenuItem.Enabled = false;
                    }
                    else
                        this.DelAllMark_ToolStripMenuItem.Enabled = true;
                }
                else
                {
                    contextMenuStrip2.Show(listBox1, new Point(e.X, e.Y));
                    this.DelMark_ToolStripMenuItem.Enabled = false;

                    ReadIni();
                    if (keys.Length == 0)
                    {
                        this.DelAllMark_ToolStripMenuItem.Enabled = false;
                    }
                    else
                        this.DelAllMark_ToolStripMenuItem.Enabled = true;
                }
            }
            listBox1.Refresh();
        }

        /// <summary>
        /// 删除此书签
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void DelMark_Click(object sender, EventArgs e)
        {
            if (keys.Length == 1)
            {
                INIEmptySection(inipath, "BookMark");
                listBox1.Items.Clear();
                keys = new string[0];
                keys_ex = new string[0];
                markIndex = 1;
            }
            else
            {
                listBox1.SelectedIndex = posindex;
                string[] keys_ex = new string[keys.Length - 1];
                string[] Key_Value_ex = new string[keys.Length - 1];
                for (int i = 0; i < keys.Length - 1; i++)
                {
                    if (i < listBox1.SelectedIndex)
                    {
                        keys_ex[i] = keys[i];
                        Key_Value_ex[i] = INIGetStringValue(inipath, "BookMark", keys[i], null);
                    }
                    else
                    {
                        keys_ex[i] = keys[i];
                        Key_Value_ex[i] = INIGetStringValue(inipath, "BookMark", keys[i + 1], null);
                    }
                }
                INIEmptySection(inipath, "BookMark");
                for (int i = 0; i < keys_ex.Length; i++)
                    INIWriteValue(inipath, "BookMark", keys_ex[i], Key_Value_ex[i]);
            }
            ReadIni();
        }

        /// <summary>
        /// 删除所有书签
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DelAllMark_Click(object sender, EventArgs e)
        {
            INIEmptySection(inipath, "BookMark");
            listBox1.Items.Clear();
            keys = new string[0];
            keys_ex = new string[0];
            markIndex = 1;
        }
        #endregion

        #region 窗口处理
        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                combobox3select = toolStripComboBox3.SelectedIndex;
                combobox4select = toolStripComboBox4.SelectedIndex;
                if (timer2.Enabled == true)
                {
                    timer2.Enabled = false;
                    toolStripButton14.Image = Reader.Properties.Resources.Start;
                    RollInitial = true;
                }
                else RollInitial = false;
                this.WindowState = FormWindowState.Minimized;// 将窗体变为最小化
                //this.ShowInTaskbar = false; //不显示在系统任务栏
                notifyIcon1.Visible = true; //托盘图标可见
                Show_ToolStripMenuItem.Enabled = true;
                RegHotKey();
            }
            else
            {
                Show_ToolStripMenuItem.Enabled = false;
                RegHotKey();
                if (RollInitial == true)
                {
                    GetScrollRange(richTextBox1.Handle, SB_VERT, out min, out max);
                    endPos = max - richTextBox1.ClientRectangle.Height;
                    timer2.Enabled = true;
                    toolStripButton14.Image = Reader.Properties.Resources.Stop;
                }
                //窗口恢复时，toolStripComboBox的选择项不变
                toolStripComboBox3.SelectedIndex = combobox3select;
                toolStripComboBox4.SelectedIndex = combobox4select;
            }

            //Panel2的最小宽度始终是窗口大小减去Panel1的宽度，这样两个Panel的大小都不会变，不能被拉伸
            //splitContainer1.Panel2MinSize = this.Size.Width - 204;
            splitContainer1.Panel1MinSize = 220;
            splitContainer1.SplitterDistance = 223;
        }


        /// <summary>
        /// 双击显示窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Normal;
                this.ShowInTaskbar = true;
                Show_ToolStripMenuItem.Enabled = false;
                RegHotKey();
                this.Activate();

                if (RollInitial == true)
                {
                    GetScrollRange(richTextBox1.Handle, SB_VERT, out min, out max);
                    endPos = max - richTextBox1.ClientRectangle.Height;
                    timer2.Enabled = true;
                    toolStripButton14.Image = Reader.Properties.Resources.Stop;
                }
                //窗口恢复时，toolStripComboBox的选择项不变
                toolStripComboBox3.SelectedIndex = combobox3select;
                toolStripComboBox4.SelectedIndex = combobox4select;
            }
            else
            {
                this.WindowState = FormWindowState.Minimized;// 将窗体变为最小化
                this.ShowInTaskbar = false; //不显示在系统任务栏
                notifyIcon1.Visible = true; //托盘图标可见
                Show_ToolStripMenuItem.Enabled = true;
                RegHotKey();

                combobox3select = toolStripComboBox3.SelectedIndex;
                combobox4select = toolStripComboBox4.SelectedIndex;

                if (timer2.Enabled == true)
                {
                    timer2.Enabled = false;
                    toolStripButton14.Image = Reader.Properties.Resources.Start;
                    RollInitial = true;
                }
                else RollInitial = false;
            }
        }

        /// <summary>
        /// 单击“显示”
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Show_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Normal;
                this.ShowInTaskbar = true;
                Show_ToolStripMenuItem.Enabled = false;
                RegHotKey();
            }
            this.Activate();

            if (RollInitial == true)
            {
                GetScrollRange(richTextBox1.Handle, SB_VERT, out min, out max);
                endPos = max - richTextBox1.ClientRectangle.Height;
                timer2.Enabled = true;
                toolStripButton14.Image = Reader.Properties.Resources.Stop;
            }
            //窗口恢复时，toolStripComboBox的选择项不变
            toolStripComboBox3.SelectedIndex = combobox3select;
            toolStripComboBox4.SelectedIndex = combobox4select;
        }

        /// <summary>
        /// 单击“关于”
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// 单击“退出”
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Exit_XToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.richTextBox1.Text != filetxt)
            {
                if (MessageBox.Show("是否保存文档?", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    toolStripButton2_Click(sender, e);
                    UnregHotKey();
                    UnregFormHotKey();
                    Hideini();
                    System.Environment.Exit(0);
                }
                else
                {
                    UnregHotKey();
                    UnregFormHotKey();
                    Hideini();
                    System.Environment.Exit(0);
                }
            }
            else
            {
                UnregHotKey();
                UnregFormHotKey();
                Hideini();
                System.Environment.Exit(0);
            }
        }

        /// <summary>
        /// 窗口关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Exit_XToolStripMenuItem_Click(sender, e);

            //取消关闭
            e.Cancel = true;

            combobox3select = toolStripComboBox3.SelectedIndex;
            combobox4select = toolStripComboBox4.SelectedIndex;

            if (timer2.Enabled == true)
            {
                timer2.Enabled = false;
                toolStripButton14.Image = Reader.Properties.Resources.Start;
                RollInitial = true;
            }
            else RollInitial = false;

            this.WindowState = FormWindowState.Minimized;// 将窗体变为最小化
            this.ShowInTaskbar = false; //不显示在系统任务栏
            notifyIcon1.Visible = true; //托盘图标可见
            Show_ToolStripMenuItem.Enabled = true;
            RegHotKey();
        }

        #region 拖放文件到窗口
        /// <summary>
        /// 当文件拖到控件上时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.All;
            }
        }

        /// <summary>
        /// 当拖放动作完成时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                filep = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
                string Path = openFileDialog1.FileName.ToString();
                txtName = Path.Substring(Path.LastIndexOf("\\") + 1);
                if (string.IsNullOrEmpty(filep)) return;

                encode1 = GetFileEncodeType(filep);
                SetEncodeBTN();
                ReadFile(filep, encode1);

                richTextBox1.ReadOnly = true;
                toolStripButton12.Image = Reader.Properties.Resources.Edit;
                toolStripButton12.ToolTipText = "编辑模式";
                RegInputHotKey();
            }
        }
        #endregion
        #endregion

        #region 工具栏

        #region 文件读取
        /// <summary>
        /// 获取文件编码格式
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static System.Text.Encoding GetFileEncodeType(string filename)
        {
            System.IO.FileStream fs = new System.IO.FileStream(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            System.IO.BinaryReader br = new System.IO.BinaryReader(fs);
            Byte[] buffer = br.ReadBytes(2);
            br.Close();
            fs.Close();

            if (buffer[0] >= 0xEF)
            {
                if (buffer[0] == 0xEF && buffer[1] == 0xBB)
                {
                    return Encoding.UTF8;
                }
                else if (buffer[0] == 0xFE && buffer[1] == 0xFF)
                {
                    return Encoding.BigEndianUnicode;
                }
                else if (buffer[0] == 0xFF && buffer[1] == 0xFE)
                {
                    return Encoding.Unicode;
                }
                else
                {
                    return Encoding.Default;
                }
            }
            else
            {
                return Encoding.Default;
            }
        }

        /// <summary>
        /// 设置toolStrip_Encode的选择项
        /// </summary>
        private void SetEncodeBTN()
        {
            const string Encode_Default = "System.Text.DBCSCodePageEncoding";
            const string Encode_UTF8 = "System.Text.UTF8Encoding";
            const string Encode_ASCII = "System.Text.ASCIIEncoding";
            const string Encode_Unicode = "System.Text.UnicodeEncoding";
            switch (encode1.ToString())
            {
                case Encode_Default:
                    toolStrip_Encode.SelectedIndex = 0;
                    break;
                case Encode_UTF8:
                    toolStrip_Encode.SelectedIndex = 1;
                    break;
                case Encode_ASCII:
                    toolStrip_Encode.SelectedIndex = 2;
                    break;
                case Encode_Unicode:
                    toolStrip_Encode.SelectedIndex = 3;
                    break;
            }
        }

        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="FilePath"></param>
        private void ReadFile(string FilePath, Encoding encode)
        {
            if (string.IsNullOrEmpty(FilePath)) return;
            try
            {
                txtName = FilePath.ToString().Substring(FilePath.ToString().LastIndexOf("\\") + 1);
                if (txtName != null)
                {
                    this.Text = @"Reader" + " - " + txtName;
                }
                else this.Text = @"Reader - JianWudao";




                FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(fs, encode);
                if (fs.CanRead)
                {
                    string strline = sr.ReadLine();
                    StringBuilder sb = new StringBuilder();
                    while (strline != null)
                    {

                        sb = sb.Append(strline + "\n");
                        strline = sr.ReadLine();
                    }
                    sr.Close();
                    richTextBox1.Text = sb.ToString();
                }

                sr.Dispose();
                fs.Dispose();
                filetxt = richTextBox1.Text;
                //获取当前文件当前光标所在行数
                this.Ranks();
                //获取当前文件总行数
                this.Rankall();
                //获取进度
                this.ScheduleP();
                //给filetext赋值
                filetxt = this.richTextBox1.Text;
                //获取总字数
                toolStripStatusLabel6.Text = "共" + richTextBox1.Text.Length.ToString() + "字";

                richTextBox1.ReadOnly = true;
                toolStripButton12.Image = Reader.Properties.Resources.Edit;
                toolStripButton12.ToolTipText = "编辑模式";
            }
            catch
            {
                Errorfile = FilePath;
                return;
            }
        }

        /// <summary>
        /// 打开文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///         
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "TXT files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.RestoreDirectory = true;   //记忆之前打开的目录
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                filep = this.openFileDialog1.FileName;
                string Path = openFileDialog1.FileName.ToString();
                txtName = Path.Substring(Path.LastIndexOf("\\") + 1);
                if (string.IsNullOrEmpty(filep)) return;

                encode1 = GetFileEncodeType(filep);
                SetEncodeBTN();
                ReadFile(filep, encode1);

                //richTextBox1.ReadOnly = true;
                //toolStripButton12.Image = Reader.Properties.Resources.Edit;
                //toolStripButton12.ToolTipText = "编辑模式";
                RegInputHotKey();
            }
        }
        #endregion


        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (richTextBox1.Text == string.Empty)
            {
                MessageBox.Show("文本为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                if (string.IsNullOrEmpty(filep))
                {
                    saveFileDialog1.Filter = "TXT files (*.txt)|*.txt|All files (*.*)|*.*";
                    //saveFileDialog1.RestoreDirectory = true;
                    saveFileDialog1.FilterIndex = 1;
                    saveFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        string file = this.saveFileDialog1.FileName;
                        if (string.IsNullOrEmpty(file)) return;
                        FileStream fs = new FileStream(file, FileMode.OpenOrCreate, FileAccess.Write);
                        StreamWriter sw = new StreamWriter(fs, Encoding.Default);
                        sw.Write(this.richTextBox1.Text);
                        sw.Dispose();
                        fs.Dispose();
                        filetxt = this.richTextBox1.Text;
                        this.Rankall();
                        toolStripStatusLabel6.Text = "共" + richTextBox1.Text.Length.ToString() + "字";
                    }
                }
                else
                {
                    FileStream fs = new FileStream(filep, FileMode.Create, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(fs, encode1);
                    sw.Write(this.richTextBox1.Text);
                    sw.Dispose();
                    fs.Dispose();
                    filetxt = this.richTextBox1.Text;
                    this.Rankall();
                    toolStripStatusLabel6.Text = "共" + richTextBox1.Text.Length.ToString() + "字";
                }
            }
        }

        /// <summary>
        /// 另存为文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (richTextBox1.Text == string.Empty)
            {
                MessageBox.Show("文本为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                saveFileDialog1.Filter = "TXT files (*.txt)|*.txt|All files (*.*)|*.*";
                //saveFileDialog1.RestoreDirectory = true;
                saveFileDialog1.FilterIndex = 1;
                saveFileDialog1.InitialDirectory = filep;
                saveFileDialog1.FileName = txtName;
                //saveFileDialog1.AddExtension = false;
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string file = this.saveFileDialog1.FileName;
                    if (string.IsNullOrEmpty(file)) return;
                    FileStream fs = new FileStream(file, FileMode.OpenOrCreate, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(fs, encode1);
                    sw.Write(this.richTextBox1.Text);
                    sw.Dispose();
                    fs.Dispose();
                    filetxt = this.richTextBox1.Text;
                }
            }
        }

        /// <summary>
        /// 减小字体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            richTextBox1.Font = new Font(richTextBox1.Font.Name, --FontSize0);
            this.Rankall();
        }

        /// <summary>
        /// 增大字体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            richTextBox1.Font = new Font(richTextBox1.Font.Name, ++FontSize0);
            this.Rankall();
        }


        /// <summary>
        /// 设置字体样式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton8_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (toolStripButton8.SelectedIndex)
            {
                case 0:
                    richTextBox1.Font = new Font("微软雅黑", richTextBox1.Font.Size);
                    toolStripButton8.Font = new Font("微软雅黑", toolStripButton8.Font.Size);
                    this.Rankall();
                    break;
                case 1:
                    richTextBox1.Font = new Font("楷体", richTextBox1.Font.Size);
                    toolStripButton8.Font = new Font("楷体", toolStripButton8.Font.Size);
                    this.Rankall();
                    break;
                case 2:
                    richTextBox1.Font = new Font("宋体", richTextBox1.Font.Size);
                    toolStripButton8.Font = new Font("宋体", toolStripButton8.Font.Size);
                    this.Rankall();
                    break;
                case 3:
                    richTextBox1.Font = new Font("华文新魏", richTextBox1.Font.Size);
                    toolStripButton8.Font = new Font("华文新魏", toolStripButton8.Font.Size);
                    this.Rankall();
                    break;
                case 4:
                    richTextBox1.Font = new Font("华文行楷", richTextBox1.Font.Size);
                    toolStripButton8.Font = new Font("华文行楷", toolStripButton8.Font.Size);
                    this.Rankall();
                    break;
            }
        }

        #region 设置字体颜色
        /// <summary>
        /// 设置字体颜色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripComboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

            switch (toolStripComboBox3.SelectedIndex)
            {
                case 0:
                    richTextBox1.ForeColor = Color.Black;
                    break;
                case 1:
                    richTextBox1.ForeColor = Color.Red;
                    break;
                case 2:
                    richTextBox1.ForeColor = Color.Gray;
                    break;
                case 3:
                    richTextBox1.ForeColor = Color.White;
                    break;
                case 4:
                    richTextBox1.ForeColor = Color.Green;
                    break;
            }
        }

        /// <summary>
        /// 为toolStripComboBox3添加各项值
        /// </summary>
        private void AddtoolStripComboBox3()
        {
            List<Product> listPdt = new List<Product>();

            Product pdt = new Product();
            pdt.ProductName = "黑色";
            pdt.ProductStatus = 1;
            listPdt.Add(pdt);

            Product pdt1 = new Product();
            pdt1.ProductName = "红色";
            pdt1.ProductStatus = 1;
            listPdt.Add(pdt1);

            Product pdt2 = new Product();
            pdt2.ProductName = "灰色";
            pdt2.ProductStatus = 1;
            listPdt.Add(pdt2);

            Product pdt3 = new Product();
            pdt3.ProductName = "白色";
            pdt3.ProductStatus = 1;
            listPdt.Add(pdt3);

            Product pdt4 = new Product();
            pdt4.ProductName = "绿色";
            pdt4.ProductStatus = 1;
            listPdt.Add(pdt4);

            ComboBox ComboBox3 = (ComboBox)toolStripComboBox3.Control;
            ComboBox3.DataSource = listPdt;
            ComboBox3.DisplayMember = "ProductName";
            ComboBox3.ValueMember = "ProductStatus";
            ComboBox3.DrawMode = DrawMode.OwnerDrawVariable;
        }

        /// <summary>
        /// 为toolStripComboBox3绘制颜色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripComboBox3_DrawItem(object sender, DrawItemEventArgs e)
        {
            Brush brush = null;
            ComboBox combo;

            try
            {
                e.DrawBackground();
                combo = (ComboBox)sender;
                Product pdt = (Product)combo.Items[e.Index];

                switch (pdt.ProductName)
                {
                    case "黑色":
                        brush = Brushes.Black;
                        break;
                    case "红色":
                        brush = Brushes.Red;
                        break;
                    case "灰色":
                        brush = Brushes.Gray;
                        break;
                    case "白色":
                        brush = Brushes.Black;
                        break;
                    case "绿色":
                        brush = Brushes.Green;
                        break;
                }

                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.DrawString(pdt.ProductName, combo.Font, brush, e.Bounds.X, e.Bounds.Y);
            }
            catch (Exception)
            {

            }
        }
        #endregion

        #region 设置背景颜色
        /// <summary>
        /// 设置背景颜色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripComboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (toolStripComboBox4.SelectedIndex)
            {
                case 0:
                    richTextBox1.BackColor = Color.White;
                    toolStripComboBox4.BackColor = Color.White;
                    break;
                case 1:
                    richTextBox1.BackColor = Color.LightGreen;
                    toolStripComboBox4.BackColor = Color.LightGreen;
                    break;
                case 2:
                    richTextBox1.BackColor = Color.Khaki;
                    toolStripComboBox4.BackColor = Color.Khaki;
                    break;
                case 3:
                    richTextBox1.BackColor = Color.Silver;
                    toolStripComboBox4.BackColor = Color.Silver;
                    break;
                case 4:
                    richTextBox1.BackColor = Color.DimGray;
                    toolStripComboBox4.BackColor = Color.DimGray;
                    break;
            }
        }

        /// <summary>
        /// 为toolStripComboBox4添加Item各项值
        /// </summary>
        private void AddtoolStripComboBox4()
        {
            List<Product> listPdt = new List<Product>();

            Product pdt = new Product();
            pdt.ProductName = "白色";
            pdt.ProductStatus = 1;
            listPdt.Add(pdt);

            Product pdt1 = new Product();
            pdt1.ProductName = "淡绿";
            pdt1.ProductStatus = 1;
            listPdt.Add(pdt1);

            Product pdt2 = new Product();
            pdt2.ProductName = "淡黄";
            pdt2.ProductStatus = 1;
            listPdt.Add(pdt2);

            Product pdt3 = new Product();
            pdt3.ProductName = "灰色";
            pdt3.ProductStatus = 1;
            listPdt.Add(pdt3);

            Product pdt4 = new Product();
            pdt4.ProductName = "黑色";
            pdt4.ProductStatus = 1;
            listPdt.Add(pdt4);

            ComboBox ComboBox4 = (ComboBox)toolStripComboBox4.Control;
            ComboBox4.DataSource = listPdt;
            ComboBox4.DisplayMember = "ProductName";
            ComboBox4.ValueMember = "ProductStatus";
            ComboBox4.DrawMode = DrawMode.OwnerDrawVariable;
        }

        /// <summary>
        /// 为toolStripComboBox4绘制Item颜色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripComboBox4_DrawItem(object sender, DrawItemEventArgs e)
        {
            Brush brush = null;
            ComboBox combo;

            try
            {
                e.DrawBackground();

                combo = (ComboBox)sender;
                Product pdt = (Product)combo.Items[e.Index];

                switch (pdt.ProductName)
                {
                    case "白色":
                        brush = Brushes.White;
                        break;
                    case "淡绿":
                        brush = Brushes.LightGreen;
                        break;
                    case "淡黄":
                        brush = Brushes.Khaki;
                        break;
                    case "灰色":
                        brush = Brushes.Silver;
                        break;
                    case "黑色":
                        brush = Brushes.DimGray;
                        break;
                }

                //获取Item矩形框
                Rectangle rect = e.Bounds;

                //为美观，可缩小选定项区域1个像素
                //rect.Inflate(-1, -1);

                //填充颜色
                e.Graphics.FillRectangle(brush, rect);

                // 用黑色绘制颜色边框
                //e.Graphics.DrawRectangle(Pens.Black, rect);

                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.DrawString(pdt.ProductName, combo.Font, Brushes.Black, e.Bounds.X, e.Bounds.Y);
            }
            catch (Exception)
            {

            }
        }
        #endregion

        /// <summary>
        /// 侧边栏按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            if (this.toolStripButton6.Text == "折叠")
            {
                this.toolStripButton6.Text = "展开";
                //this.splitContainer1.PanelVisibility = DevExpress.XtraEditors.SplitPanelVisibility.Panel2;
                this.splitContainer1.Panel1Collapsed = true;
                this.toolStripButton6.Image = Reader.Properties.Resources.ShowSidebar;
            }
            else
            {
                this.toolStripButton6.Text = "折叠";
                //this.splitContainer1.PanelVisibility = DevExpress.XtraEditors.SplitPanelVisibility.Both;
                this.splitContainer1.Panel1Collapsed = false;
                this.toolStripButton6.Image = Reader.Properties.Resources.HideSidebar;
            }
        }

        /// <summary>
        /// 添加书签
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton11_Click(object sender, EventArgs e)
        {
            keys = INIGetAllItemKeys(inipath, "BookMark");
            if (keys.Length != 0)
            {
                markIndex = Int32.Parse(keys[keys.Length - 1]) + 1;
            }
            INIWriteValue(inipath, "BookMark", markIndex.ToString(), txtName + "," + toolStripStatusLabel3.Text + "," + richTextBox1.SelectionStart.ToString() + "," + filep);
            ReadIni();
            Hideini();
        }

        /// <summary>
        /// 编辑模式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton12_Click(object sender, EventArgs e)
        {
            if (richTextBox1.ReadOnly == true)
            {
                richTextBox1.ReadOnly = false;
                toolStripButton12.Image = Reader.Properties.Resources.Read;
                toolStripButton12.ToolTipText = "阅读模式";
                UnregInputHotKey();
            }
            else
            {
                richTextBox1.ReadOnly = true;
                toolStripButton12.Image = Reader.Properties.Resources.Edit;
                toolStripButton12.ToolTipText = "编辑模式";
                RegInputHotKey();
                this.Rankall();
                toolStripStatusLabel6.Text = "共" + richTextBox1.Text.Length.ToString() + "字";
            }
        }

        /// <summary>
        /// 设置编辑模式下的鼠标样式
        /// </summary>
        private void SetMouse()
        {
            if (richTextBox1.ReadOnly == true)
            {
                this.richTextBox1.Cursor = Cursors.Arrow;
            }
            else
                this.richTextBox1.Cursor = Cursors.IBeam;
        }

        /// <summary>
        /// 置顶
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Stick_Click(object sender, EventArgs e)
        {
            if (this.Stick.Text == "置顶")
            {
                this.Stick.Text = "取消置顶";
                this.TopMost = true;
                this.Focus();
                this.Stick.Image = Reader.Properties.Resources.Unstick;
            }
            else
            {
                this.Stick.Text = "置顶";
                this.TopMost = false;
                this.Stick.Image = Reader.Properties.Resources.Stick;
            }
        }

        /// <summary>
        /// 全屏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton13_Click(object sender, EventArgs e)
        {
            if (FullS == false)
            {
                combobox3select = toolStripComboBox3.SelectedIndex;
                combobox4select = toolStripComboBox4.SelectedIndex;
                this.TopMost = true;
                this.FormBorderStyle = FormBorderStyle.None;
                this.WindowState = FormWindowState.Normal;
                this.WindowState = FormWindowState.Maximized;
                FullS = true;
                toolStripButton13.Image = Reader.Properties.Resources.contract;
                toolStripButton13.Text = "退出全屏";
                //窗口恢复时，toolStripComboBox的选择项不变
                toolStripComboBox3.SelectedIndex = combobox3select;
                toolStripComboBox4.SelectedIndex = combobox4select;
            }
            else
            {
                this.TopMost = false;
                this.FormBorderStyle = FormBorderStyle.Sizable;
                this.WindowState = FormWindowState.Normal;
                FullS = false;
                toolStripButton13.Image = Reader.Properties.Resources.expand;
                toolStripButton13.Text = "全屏";
                //窗口恢复时，toolStripComboBox的选择项不变
                toolStripComboBox3.SelectedIndex = combobox3select;
                toolStripComboBox4.SelectedIndex = combobox4select;
            }
        }

        /// <summary>
        /// 退出按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Close_Click(object sender, EventArgs e)
        {
            Exit_XToolStripMenuItem_Click(sender, e);
        }

        #region 滚屏
        //滚屏时钟所用变量
        private int posp = 1;
        private int min, max;
        private int pos = 0;
        private int endPos = 0;
        private const int SB_VERT = 0x1;
        private const int WM_VSCROLL = 0x115;
        private const int SB_THUMBPOSITION = 4;
        [DllImport("user32.dll")]
        private static extern int SetScrollPos(IntPtr hwnd, int nBar, int nPos, bool bRedraw);


        [DllImport("user32.dll")]
        private static extern int GetScrollPos(IntPtr hwnd, int nBar);


        [DllImport("user32.dll")]
        private static extern bool PostMessage(IntPtr hWnd, int nBar, int wParam, int lParam);


        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern bool GetScrollRange(IntPtr hWnd, int nBar, out int lpMinPos, out int lpMaxPos);


        /// <summary>
        /// 滚屏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton14_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(richTextBox1.Text))
            {
                if (timer2.Enabled == false)
                {
                    //得到滚动条的最大最小值
                    GetScrollRange(richTextBox1.Handle, SB_VERT, out min, out max);
                    //得到滚动条到最底下的实际位置
                    endPos = max - richTextBox1.ClientRectangle.Height;
                    timer2.Enabled = true;
                    toolStripButton14.Image = Reader.Properties.Resources.Stop;
                    toolStripButton14.Text = "停止滚屏";
                }
                else
                {
                    timer2.Enabled = false;
                    toolStripButton14.Image = Reader.Properties.Resources.Start;
                    toolStripButton14.Text = "滚屏";
                }
            }
            else
                MessageBox.Show("文件为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            //加上这句是为了如果用户手动拖拽滚动条，可以保证滚动条继续从拖拽的位置走
            pos = GetScrollPos(richTextBox1.Handle, SB_VERT);
            pos = pos + posp;
            //如果已经到底，那么停止Timer
            if (pos > endPos)
            {
                this.timer2.Enabled = false;
                toolStripButton14.Image = Reader.Properties.Resources.Start;
                return;
            }
            SetScrollPos(richTextBox1.Handle, SB_VERT, pos, true);
            PostMessage(richTextBox1.Handle, WM_VSCROLL, SB_THUMBPOSITION + 0x10000 * pos, 0);
            RankRoll();
            ScheduleP();
        }

        /// <summary>
        /// 滚屏减速
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton15_Click(object sender, EventArgs e)
        {
            timer2.Interval = timer2.Interval + 5;
        }

        /// <summary>
        /// 滚屏加速
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton16_Click(object sender, EventArgs e)
        {
            if (timer2.Interval > 5)
            {
                timer2.Interval = timer2.Interval - 5;
            }
            else
                posp++;
        }
        #endregion

        #region 热键

        #region 声明注册注销热键函数
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, Keys vk);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);
        #endregion

        /// <summary>
        /// 定义全局热键
        /// </summary>
        private void RegHotKey()
        {
            UnregisterHotKey(Handle, 001);
            RegisterHotKey(Handle, 001, key1, key2);//定义快捷键为Alt+Q
        }

        /// <summary>
        /// 注销全局热键
        /// </summary>
        private void UnregHotKey()
        {
            UnregisterHotKey(Handle, 001);//卸载快捷键Alt+Q
        }

        /// <summary>
        /// 定义窗口激活时的热键
        /// </summary>
        private void RegFormHotKey()
        {
            RegisterHotKey(Handle, 002, 2, Keys.S); //定义快捷键为Ctrl+S，保存
            RegisterHotKey(Handle, 003, 0, Keys.F4);//定义快捷键为F4，保存书签
            RegisterHotKey(Handle, 009, 2, Keys.F);//定义快捷键为Ctrl+F，查找
            RegisterHotKey(Handle, 010, 0, Keys.F9);//定义快捷键为F9，折叠/展开
            RegisterHotKey(Handle, 011, 0, Keys.F10);//定义快捷键为F10，置顶/取消置顶
            RegisterHotKey(Handle, 012, 0, Keys.F11);//定义快捷键为F11，全屏
            RegisterHotKey(Handle, 013, 0, Keys.Escape);//定义快捷键为Esc，退出全屏
            RegisterHotKey(Handle, 800, 2, Keys.H);//定义快捷键为Ctrl+H
        }

        /// <summary>
        /// 注销窗口激活时的热键
        /// </summary>
        private void UnregFormHotKey()
        {
            UnregisterHotKey(Handle, 002);//卸载快捷键Ctrl+S，保存
            UnregisterHotKey(Handle, 003);//卸载快捷键F4，保存书签
            UnregisterHotKey(Handle, 009);//卸载快捷键Ctrl+F，查找
            UnregisterHotKey(Handle, 010);//卸载快捷键F9，折叠/展开
            UnregisterHotKey(Handle, 011);//卸载快捷键F10，置顶/取消置顶
            UnregisterHotKey(Handle, 012);//卸载快捷键F11，全屏
            UnregisterHotKey(Handle, 013);//卸载快捷键Esc，退出全屏
            UnregisterHotKey(Handle, 800);//卸载快捷键Ctrl+H
        }

        /// <summary>
        /// 未输入时注册热键
        /// </summary>
        private void RegInputHotKey()
        {

            RegisterHotKey(Handle, 004, 0, Keys.Left); //定义快捷键为←，上一页
            RegisterHotKey(Handle, 005, 0, Keys.Right);//定义快捷键为→，下一页
            RegisterHotKey(Handle, 006, 0, Keys.Space);//定义快捷键为空格键，滚屏/停止滚屏
            RegisterHotKey(Handle, 007, 0, Keys.Up); //定义快捷键为↑，滚屏加速
            RegisterHotKey(Handle, 008, 0, Keys.Down); //定义快捷键为↓，滚屏减速

        }

        /// <summary>
        /// 输入时注销热键
        /// </summary>
        private void UnregInputHotKey()
        {
            UnregisterHotKey(Handle, 004);//卸载快捷键←，上一页
            UnregisterHotKey(Handle, 005);//卸载快捷键→，下一页
            UnregisterHotKey(Handle, 006);//卸载快捷键空格键，滚屏/停止滚屏
            UnregisterHotKey(Handle, 007);//卸载快捷键↑，滚屏加速
            UnregisterHotKey(Handle, 008);//卸载快捷键↓，滚屏减速
        }

        /// <summary>
        /// 判断老板键选择设定
        /// </summary>
        private void CheckSeleted()
        {
            if (toolStripComboBox1.SelectedIndex == 0 && toolStripComboBox2.SelectedIndex == 0)
            {
                toolStripLabel1.Enabled = false;
                toolStripComboBox1.Enabled = false;
            }
            else
            {
                toolStripLabel1.Enabled = true;
                toolStripComboBox1.Enabled = true;
            }
        }

        /// <summary>
        /// 初始化老板键
        /// </summary>
        private void InitHotKey()
        {
            toolStripComboBox1.SelectedIndex = 2;
            toolStripComboBox2.SelectedIndex = 27;
            RegisterHotKey(Handle, 910, 1, Keys.Q);
        }

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (toolStripComboBox1.SelectedIndex)
            {
                case 0:
                    key1 = 0;
                    break;
                case 1:
                    key1 = 2;
                    RegHotKey();
                    break;
                case 2:
                    key1 = 1;
                    RegHotKey();
                    break;
                case 3:
                    key1 = 4;
                    RegHotKey();
                    break;
                case 4:
                    key1 = 8;
                    break;
            }
            CheckSeleted();
            RegHotKey();
        }

        private void toolStripComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (toolStripComboBox2.SelectedIndex)
            {
                case 0:
                    key2 = Keys.None;
                    break;
                case 1:
                    key2 = Keys.D1;
                    break;
                case 2:
                    key2 = Keys.D2;
                    break;
                case 3:
                    key2 = Keys.D3;
                    break;
                case 4:
                    key2 = Keys.D4;
                    break;
                case 5:
                    key2 = Keys.D5;
                    break;
                case 6:
                    key2 = Keys.D6;
                    break;
                case 7:
                    key2 = Keys.D7;
                    break;
                case 8:
                    key2 = Keys.D8;
                    break;
                case 9:
                    key2 = Keys.D9;
                    break;
                case 10:
                    key2 = Keys.D0;
                    break;
                case 11:
                    key2 = Keys.A;
                    break;
                case 12:
                    key2 = Keys.B;
                    break;
                case 13:
                    key2 = Keys.C;
                    break;
                case 14:
                    key2 = Keys.D;
                    break;
                case 15:
                    key2 = Keys.E;
                    break;
                case 16:
                    key2 = Keys.F;
                    break;
                case 17:
                    key2 = Keys.G;
                    break;
                case 18:
                    key2 = Keys.H;
                    break;
                case 19:
                    key2 = Keys.I;
                    break;
                case 20:
                    key2 = Keys.J;
                    break;
                case 21:
                    key2 = Keys.K;
                    break;
                case 22:
                    key2 = Keys.L;
                    break;
                case 23:
                    key2 = Keys.M;
                    break;
                case 24:
                    key2 = Keys.N;
                    break;
                case 25:
                    key2 = Keys.O;
                    break;
                case 26:
                    key2 = Keys.P;
                    break;
                case 27:
                    key2 = Keys.Q;
                    break;
                case 28:
                    key2 = Keys.R;
                    break;
                case 29:
                    key2 = Keys.S;
                    break;
                case 30:
                    key2 = Keys.T;
                    break;
                case 31:
                    key2 = Keys.U;
                    break;
                case 32:
                    key2 = Keys.V;
                    break;
                case 33:
                    key2 = Keys.W;
                    break;
                case 34:
                    key2 = Keys.X;
                    break;
                case 35:
                    key2 = Keys.Y;
                    break;
                case 36:
                    key2 = Keys.Z;
                    break;
                case 37:
                    key2 = Keys.F1;
                    break;
                case 38:
                    key2 = Keys.F2;
                    break;
                case 39:
                    key2 = Keys.F3;
                    break;
                case 40:
                    key2 = Keys.F4;
                    break;
                case 41:
                    key2 = Keys.F5;
                    break;
                case 42:
                    key2 = Keys.F6;
                    break;
                case 43:
                    key2 = Keys.F7;
                    break;
                case 44:
                    key2 = Keys.F8;
                    break;
                case 45:
                    key2 = Keys.F19;
                    break;
                case 46:
                    key2 = Keys.F10;
                    break;
                case 47:
                    key2 = Keys.F11;
                    break;
                case 48:
                    key2 = Keys.F12;
                    break;
                case 49:
                    key2 = Keys.Divide;
                    break;
                case 50:
                    key2 = Keys.Multiply;
                    break;
                case 51:
                    key2 = Keys.Subtract;
                    break;
                case 52:
                    key2 = Keys.Add;
                    break;
                case 53:
                    key2 = Keys.Decimal;
                    break;
            }
            CheckSeleted();
            RegHotKey();
        }

        /// <summary>
        /// 窗口激活时注册窗口快捷键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Activated(object sender, EventArgs e)
        {
            RegFormHotKey();
            RegInputHotKey();
        }

        /// <summary>
        /// 窗口没激活时注销窗口快捷键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Deactivate(object sender, EventArgs e)
        {
            UnregFormHotKey();
            UnregInputHotKey();
        }
        /// <summary>
        /// 判断热键，并执行相关操作事件
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
                        case 001:
                            //Alt+Q，老板键
                            if (this.WindowState == FormWindowState.Minimized)
                            {
                                this.WindowState = FormWindowState.Normal;
                                this.ShowInTaskbar = true;
                                Show_ToolStripMenuItem.Enabled = false;
                                RegHotKey();
                                RegFormHotKey();
                                this.Activate();
                            }
                            else
                            {
                                this.WindowState = FormWindowState.Minimized;
                                this.ShowInTaskbar = false;
                                Show_ToolStripMenuItem.Enabled = true;
                                RegHotKey();
                                UnregFormHotKey();
                            }
                            break;
                        case 002:
                            //Ctrl+S快捷键，保存
                            if (richTextBox1.Text == string.Empty)
                            {
                                MessageBox.Show("文本为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(filep))
                                {
                                    saveFileDialog1.Filter = "TXT files (*.txt)|*.txt|All files (*.*)|*.*";
                                    saveFileDialog1.FilterIndex = 1;
                                    saveFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                                    {
                                        string file = this.saveFileDialog1.FileName;
                                        if (string.IsNullOrEmpty(file)) return;
                                        FileStream fs = new FileStream(file, FileMode.OpenOrCreate, FileAccess.Write);
                                        StreamWriter sw = new StreamWriter(fs, Encoding.Default);
                                        sw.Write(this.richTextBox1.Text);
                                        sw.Dispose();
                                        fs.Dispose();
                                        filetxt = this.richTextBox1.Text;
                                    }
                                }
                                else
                                {
                                    FileStream fs = new FileStream(filep, FileMode.Create, FileAccess.Write);
                                    StreamWriter sw = new StreamWriter(fs, encode1);
                                    sw.Write(this.richTextBox1.Text);
                                    sw.Dispose();
                                    fs.Dispose();
                                    filetxt = this.richTextBox1.Text;
                                }
                            }
                            break;
                        case 003:
                            //F4快捷键，保存书签
                            if (!string.IsNullOrEmpty(richTextBox1.Text))
                            {
                                //toolStripButton11_Click();
                                keys = INIGetAllItemKeys(inipath, "BookMark");
                                if (keys.Length != 0)
                                {
                                    markIndex = Int32.Parse(keys[keys.Length - 1]) + 1;
                                }
                                INIWriteValue(inipath, "BookMark", markIndex.ToString(), txtName + "," + toolStripStatusLabel3.Text + "," + richTextBox1.SelectionStart.ToString() + "," + filep);
                                ReadIni();
                            }
                            else
                                MessageBox.Show("文件为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;
                        case 004:
                            //←快捷键，上一页
                            if (richTextBox1.ReadOnly == true)
                            {
                                SendKeys.Send("{PGUP}");
                            }
                            else
                                SendKeys.Send("{LEFT}");
                            break;
                        case 005:
                            //→快捷键，下一页
                            if (richTextBox1.ReadOnly == true)
                            {
                                SendKeys.Send("{PGDN}");
                            }
                            else
                                SendKeys.Send("{RIGHT}");
                            break;
                        case 006:
                            //空格键快捷键，滚屏/停止滚屏
                            if (richTextBox1.ReadOnly == true)
                            {
                                if (!string.IsNullOrEmpty(richTextBox1.Text))
                                {
                                    if (timer2.Enabled == false)
                                    {
                                        GetScrollRange(richTextBox1.Handle, SB_VERT, out min, out max);
                                        endPos = max - richTextBox1.ClientRectangle.Height;
                                        this.timer2.Enabled = true;
                                        toolStripButton14.Image = Reader.Properties.Resources.Stop;
                                        toolStripButton14.Text = "停止滚屏";
                                    }
                                    else
                                    {
                                        this.timer2.Enabled = false;
                                        toolStripButton14.Image = Reader.Properties.Resources.Start;
                                        toolStripButton14.Text = "滚屏";
                                    }
                                }
                                else
                                    MessageBox.Show("文件为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            break;
                        case 007:
                            //↑快捷键，滚屏加速
                            if (this.timer2.Enabled == true)
                            {
                                if (timer2.Interval > 5)
                                {
                                    timer2.Interval = timer2.Interval - 5;
                                }
                                else
                                    posp++;
                            }
                            else
                                SendKeys.Send("{UP}");
                            break;
                        case 008:
                            //↓快捷键，滚屏减速
                            if (this.timer2.Enabled == true)
                            {
                                timer2.Interval = timer2.Interval + 5;
                            }
                            else
                                SendKeys.Send("{DOWN}");
                            break;
                        case 009:
                            //Ctrl+F快捷键，查找
                            TextBox_Find.Focus();
                            break;
                        case 010:
                            //F9快捷键，折叠/展开
                            if (this.toolStripButton6.Text == "折叠")
                            {
                                this.toolStripButton6.Text = "展开";
                                this.splitContainer1.Panel1Collapsed = true;
                                this.toolStripButton6.Image = Reader.Properties.Resources.ShowSidebar;
                            }
                            else
                            {
                                this.toolStripButton6.Text = "折叠";
                                this.splitContainer1.Panel1Collapsed = false;
                                this.toolStripButton6.Image = Reader.Properties.Resources.HideSidebar;
                            }
                            break;
                        case 011:
                            //F10快捷键，置顶/取消置顶
                            if (this.Stick.Text == "置顶")
                            {
                                this.Stick.Text = "取消置顶";
                                this.TopMost = true;
                                this.Focus();
                                this.Stick.Image = Reader.Properties.Resources.Unstick;
                            }
                            else
                            {
                                this.Stick.Text = "置顶";
                                this.TopMost = false;
                                this.Stick.Image = Reader.Properties.Resources.Stick;
                            }
                            break;
                        case 012:
                            //F11快捷键，全屏
                            if (FullS == false)
                            {
                                combobox3select = toolStripComboBox3.SelectedIndex;
                                combobox4select = toolStripComboBox4.SelectedIndex;
                                this.TopMost = true;
                                this.FormBorderStyle = FormBorderStyle.None;
                                this.WindowState = FormWindowState.Normal;
                                this.WindowState = FormWindowState.Maximized;
                                FullS = true;
                                toolStripButton13.Image = Reader.Properties.Resources.contract;
                                toolStripButton13.Text = "退出全屏";
                                //窗口恢复时，toolStripComboBox的选择项不变
                                toolStripComboBox3.SelectedIndex = combobox3select;
                                toolStripComboBox4.SelectedIndex = combobox4select;
                            }
                            else
                            {
                                this.TopMost = false;
                                this.FormBorderStyle = FormBorderStyle.Sizable;
                                this.WindowState = FormWindowState.Normal;
                                FullS = false;
                                toolStripButton13.Image = Reader.Properties.Resources.expand;
                                toolStripButton13.Text = "全屏";
                                //窗口恢复时，toolStripComboBox的选择项不变
                                toolStripComboBox3.SelectedIndex = combobox3select;
                                toolStripComboBox4.SelectedIndex = combobox4select;
                            }
                            break;
                        case 013:
                            //Esc快捷键，退出全屏
                            if (FullS == true)
                            {
                                this.TopMost = false;
                                this.FormBorderStyle = FormBorderStyle.Sizable;
                                this.WindowState = FormWindowState.Normal;
                                FullS = false;
                                toolStripButton13.Image = Reader.Properties.Resources.expand;
                            }
                            break;
                        case 800:
                            //备用
                            //this.Rankall();
                            break;
                    }
                    break;
            }
            base.WndProc(ref m);
        }
        #endregion

        #region 查找
        /// <summary>
        /// 查找按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Find_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(richTextBox1.Text))
            {
                if (TextBox_Find.Text == "查找...")
                    TextBox_Find.Text = String.Empty;

                //当焦点所在行与上一查找到的目标行不在同一行时，从鼠标所在位置开始查找
                if (richTextBox1.SelectionStart != searchPoint0)
                    searchPoint = richTextBox1.SelectionStart;
                if (!string.IsNullOrEmpty(TextBox_Find.Text))
                {
                    searchPoint = richTextBox1.Text.IndexOf(TextBox_Find.Text, searchPoint);
                    if (searchPoint == -1)
                    {
                        MessageBox.Show("已到文本末尾，将从文本首端开始查找", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        searchPoint = 0;
                    }
                    else
                    {
                        richTextBox1.Select(searchPoint, TextBox_Find.Text.Length);
                        searchPoint = searchPoint + TextBox_Find.Text.Length;
                        richTextBox1.Focus();
                        searchPoint0 = richTextBox1.SelectionStart;
                    }
                }
                else
                {
                    MessageBox.Show("文本框为空，请输入查找内容！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    TextBox_Find.Focus();
                }
            }
            else
                MessageBox.Show("文件为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        /// <summary>
        /// 查找文本框按下回车键时，执行查找按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_Find_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                Button_Find_Click(sender, e);
            }
        }

        /// <summary>
        /// 焦点进入查找文本框时，文本框值及格式设定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_Find_Enter(object sender, EventArgs e)
        {
            UnregInputHotKey();
            //this.notifyIcon1.ShowBalloonTip(1000, "提示", "系统仍在运行！\n如要打开，请点击图标", ToolTipIcon.Info);
            if (TextBox_Find.Text == "查找...")
            {
                TextBox_Find.Text = String.Empty;
                TextBox_Find.ForeColor = Color.Black;
            }
            //延时之后再全选，否则无法全选。
            Thread.Sleep(100);
            Application.DoEvents();
            TextBox_Find.SelectAll();
        }

        /// <summary>
        /// 焦点离开查找文本框时，文本框值及格式设定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_Find_Leave(object sender, EventArgs e)
        {
            RegInputHotKey();
            if (TextBox_Find.Text == String.Empty)
            {
                TextBox_Find.Text = "查找...";
                TextBox_Find.ForeColor = Color.Silver;
            }
        }
        #endregion

        /// <summary>
        /// 字符编码按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStrip_Encode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(filep)) return;
            switch (toolStrip_Encode.SelectedIndex)
            {
                case 0:
                    encode1 = System.Text.Encoding.Default;
                    ReadFile(filep, encode1);
                    break;
                case 1:
                    encode1 = System.Text.Encoding.UTF8;
                    ReadFile(filep, encode1);
                    break;
                case 2:
                    encode1 = System.Text.Encoding.ASCII;
                    ReadFile(filep, encode1);
                    break;
                case 3:
                    encode1 = System.Text.Encoding.Unicode;
                    ReadFile(filep, encode1);
                    break;
            }
        }
        #endregion

        #region 状态栏
        /// <summary>
        ///  获取文本中当前光标所在行数
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        private void Ranks()
        {
            if (lineall != 0)
            {
                /*得到光标行第一个字符的索引，即从第1个字符开始到光标行的第1个字符索引*/
                int index = richTextBox1.GetFirstCharIndexOfCurrentLine();
                /*得到光标行的行号,第1行从0开始计算，习惯上我们是从1开始计算，所以+1 */
                linenow = richTextBox1.GetLineFromCharIndex(index) + 1;
            }
            else linenow = 0;
            toolStripStatusLabel1.Text = String.Format("第{0}行", linenow.ToString());
        }

        /// <summary>
        /// 滚屏时获取当前页面首行行数
        /// </summary>
        private void RankRoll()
        {
            if (lineall != 0)
            {
                GetFirstLine();
                linenow = richTextBox1.GetLineFromCharIndex(FirstLineIndex) + 1;
            }
            else linenow = 0;
            toolStripStatusLabel1.Text = String.Format("第{0}行", linenow.ToString());
        }

        /// <summary>
        /// 获取总行数
        /// </summary>
        private void Rankall()
        {
            GetFirstLine();

            richTextBox1.Select(richTextBox1.TextLength, 0);
            richTextBox1.ScrollToCaret();

            int indexall = richTextBox1.GetFirstCharIndexOfCurrentLine();
            lineall = richTextBox1.GetLineFromCharIndex(indexall) + 1;

            this.richTextBox1.Select(FirstLineIndex, 0);
            this.richTextBox1.ScrollToCaret();

            toolStripStatusLabel2.Text = "共" + lineall.ToString() + "行";
        }

        /// <summary>
        /// 获取当前页面首行首个字符索引
        /// </summary>
        private void GetFirstLine()
        {
            Point p = new System.Drawing.Point();
            p.X = 0;
            p.Y = 0;
            FirstLineIndex = richTextBox1.GetCharIndexFromPosition(p);
        }

        /// <summary>
        /// 获取进度
        /// </summary>
        private void ScheduleP()
        {
            if (lineall != 0)
            {
                Per = (float)linenow / lineall * 100;
            }
            else
                Per = 0;
            toolStripStatusLabel3.Text = Per.ToString("f2") + "%";
        }

        /// <summary>
        /// 上一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripStatusLabel4_Click(object sender, EventArgs e)
        {
            SendKeys.Send("{PGUP}");
        }
        private void toolStripStatusLabel4_MouseUp(object sender, MouseEventArgs e)
        {
            toolStripStatusLabel4.ForeColor = Color.Black;
            toolStripStatusLabel4.Font = new Font(toolStripStatusLabel5.Font, FontStyle.Regular);
        }

        private void toolStripStatusLabel4_MouseDown(object sender, MouseEventArgs e)
        {
            toolStripStatusLabel4.ForeColor = Color.Blue;
            toolStripStatusLabel4.Font = new Font(toolStripStatusLabel5.Font, FontStyle.Bold);
        }

        private void toolStripStatusLabel4_MouseEnter(object sender, EventArgs e)
        {
            toolStripStatusLabel4.ForeColor = Color.Blue;
            toolStripStatusLabel4.BackColor = Color.LightGray;
        }

        private void toolStripStatusLabel4_MouseLeave(object sender, EventArgs e)
        {
            toolStripStatusLabel4.ForeColor = Color.Black;
            toolStripStatusLabel4.BackColor = Color.Empty;
        }

        /// <summary>
        /// 下一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripStatusLabel5_Click(object sender, EventArgs e)
        {
            SendKeys.Send("{PGDN}");
        }
        private void toolStripStatusLabel5_MouseUp(object sender, MouseEventArgs e)
        {
            toolStripStatusLabel5.ForeColor = Color.Black;
            toolStripStatusLabel5.Font = new Font(toolStripStatusLabel5.Font, FontStyle.Regular);
        }

        private void toolStripStatusLabel5_MouseDown(object sender, MouseEventArgs e)
        {
            toolStripStatusLabel5.ForeColor = Color.Blue;
            toolStripStatusLabel5.Font = new Font(toolStripStatusLabel5.Font, FontStyle.Bold);
        }

        private void toolStripStatusLabel5_MouseEnter(object sender, EventArgs e)
        {
            toolStripStatusLabel5.ForeColor = Color.Blue;
            toolStripStatusLabel5.BackColor = Color.LightGray;
        }

        private void toolStripStatusLabel5_MouseLeave(object sender, EventArgs e)
        {
            toolStripStatusLabel5.ForeColor = Color.Black;
            toolStripStatusLabel5.BackColor = Color.Empty;
        }

        /// <summary>
        /// 时钟1，用于显示状态栏时间和星期
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            toolStripStatusLabel7.Text = String.Format("{0:dddd}", DateTime.Now) + "  " + String.Format("{0:F}", DateTime.Now);
        }
        #endregion

        #region richtextbox操作
        /// <summary>
        /// 鼠标键盘操作时，刷新richTextBox1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void richTextBox1_KeyUp(object sender, KeyEventArgs e)
        {
            this.Ranks();
            //this.Rankall();
            this.ScheduleP();
            SetMouse();
            toolStripStatusLabel6.Text = "共" + richTextBox1.Text.Length.ToString() + "字";
        }

        /// <summary>
        /// 鼠标在文本框内点击时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void richTextBox1_MouseUp(object sender, MouseEventArgs e)
        {
            this.Ranks();
            this.ScheduleP();
            SetMouse();
        }

        /// <summary>
        /// 鼠标在文本框内双击时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void richTextBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (!string.IsNullOrEmpty(richTextBox1.Text))
            {
                if (richTextBox1.ReadOnly == true)
                {
                    if (timer2.Enabled == false)
                    {
                        GetScrollRange(richTextBox1.Handle, SB_VERT, out min, out max);
                        endPos = max - richTextBox1.ClientRectangle.Height;
                        this.timer2.Enabled = true;
                        toolStripButton14.Image = Reader.Properties.Resources.Stop;
                    }
                    else
                    {
                        this.timer2.Enabled = false;
                        toolStripButton14.Image = Reader.Properties.Resources.Start;
                    }
                }
            }
        }

        /// <summary>
        /// 鼠标在文本框内移动时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void richTextBox1_MouseMove(object sender, MouseEventArgs e)
        {
            SetMouse();
        }

        private void richTextBox1_Enter(object sender, EventArgs e)
        {
            if (richTextBox1.ReadOnly == false)
            {
                UnregInputHotKey();
            }
            else
                RegInputHotKey();
        }


        /// <summary>
        /// 当richtextbox尺寸改变时，重新获取总行数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void richTextBox1_SizeChanged(object sender, EventArgs e)
        {
            Ranks();
            Rankall();
        }

        #region 实现拖入文件打开
        /// <summary>
        /// 当文件拖到控件上时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void richTextBox1_DragEnter(object sender, DragEventArgs e)
        {
            //if (richTextBox1.ReadOnly == true)
            //{
            //richTextBox1.ReadOnly = false;
            //toolStripButton12.Image = Reader.Properties.Resources.Read;
            //toolStripButton12.ToolTipText = "阅读模式";
            //UnregInputHotKey();

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.All;
            }
            //}            
        }

        /// <summary>
        /// 当拖放动作完成时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void richTextBox1_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                filep = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
                string Path = openFileDialog1.FileName.ToString();
                txtName = Path.Substring(Path.LastIndexOf("\\") + 1);
                if (string.IsNullOrEmpty(filep)) return;

                encode1 = GetFileEncodeType(filep);
                SetEncodeBTN();
                ReadFile(filep, encode1);

                //richTextBox1.ReadOnly = true;
                //toolStripButton12.Image = Reader.Properties.Resources.Edit;
                //toolStripButton12.ToolTipText = "编辑模式";
                RegInputHotKey();
            }
        }
    }
    #endregion
    #endregion
}

