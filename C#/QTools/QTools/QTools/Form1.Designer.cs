namespace QTools
{
    partial class Form1
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.Show_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.About_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Exit_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label26 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.button8 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.textBox13 = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.textBox14 = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.textBox15 = new System.Windows.Forms.TextBox();
            this.button6 = new System.Windows.Forms.Button();
            this.label16 = new System.Windows.Forms.Label();
            this.textBox16 = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.textBox17 = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.textBox18 = new System.Windows.Forms.TextBox();
            this.button7 = new System.Windows.Forms.Button();
            this.label19 = new System.Windows.Forms.Label();
            this.textBox19 = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.textBox20 = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.textBox21 = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textBox8 = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.textBox9 = new System.Windows.Forms.TextBox();
            this.button4 = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.textBox10 = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.textBox11 = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.textBox12 = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label29 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "快捷工具";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Show_ToolStripMenuItem,
            this.About_ToolStripMenuItem,
            this.Exit_ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(115, 70);
            this.contextMenuStrip1.Text = "托盘菜单";
            // 
            // Show_ToolStripMenuItem
            // 
            this.Show_ToolStripMenuItem.Name = "Show_ToolStripMenuItem";
            this.Show_ToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.Show_ToolStripMenuItem.Text = "显示(&S)";
            this.Show_ToolStripMenuItem.Click += new System.EventHandler(this.Show_ToolStripMenuItem_Click);
            // 
            // About_ToolStripMenuItem
            // 
            this.About_ToolStripMenuItem.Name = "About_ToolStripMenuItem";
            this.About_ToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.About_ToolStripMenuItem.Text = "关于(&A)";
            this.About_ToolStripMenuItem.Click += new System.EventHandler(this.About_ToolStripMenuItem_Click);
            // 
            // Exit_ToolStripMenuItem
            // 
            this.Exit_ToolStripMenuItem.Name = "Exit_ToolStripMenuItem";
            this.Exit_ToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.Exit_ToolStripMenuItem.Text = "退出(&X)";
            this.Exit_ToolStripMenuItem.Click += new System.EventHandler(this.Exit_ToolStripMenuItem_Click);
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label26.Location = new System.Drawing.Point(12, 241);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(22, 17);
            this.label26.TabIndex = 113;
            this.label26.Text = "⑦";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label27.Location = new System.Drawing.Point(12, 208);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(22, 17);
            this.label27.TabIndex = 112;
            this.label27.Text = "⑥";
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label28.Location = new System.Drawing.Point(12, 175);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(22, 17);
            this.label28.TabIndex = 111;
            this.label28.Text = "⑤";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label24.Location = new System.Drawing.Point(12, 142);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(22, 17);
            this.label24.TabIndex = 110;
            this.label24.Text = "④";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label25.Location = new System.Drawing.Point(12, 109);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(22, 17);
            this.label25.TabIndex = 109;
            this.label25.Text = "③";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label23.Location = new System.Drawing.Point(12, 76);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(22, 17);
            this.label23.TabIndex = 108;
            this.label23.Text = "②";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label22.Location = new System.Drawing.Point(12, 43);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(22, 17);
            this.label22.TabIndex = 107;
            this.label22.Text = "①";
            // 
            // button8
            // 
            this.button8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.button8.Location = new System.Drawing.Point(105, 271);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(130, 25);
            this.button8.TabIndex = 29;
            this.button8.Text = "取消所有关机计划(&C)";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // button5
            // 
            this.button5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.button5.Location = new System.Drawing.Point(256, 171);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(60, 25);
            this.button5.TabIndex = 20;
            this.button5.Text = "重启(&R)";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label13.Location = new System.Drawing.Point(207, 176);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(37, 15);
            this.label13.TabIndex = 104;
            this.label13.Text = "秒  后";
            // 
            // textBox13
            // 
            this.textBox13.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.textBox13.Location = new System.Drawing.Point(40, 173);
            this.textBox13.Name = "textBox13";
            this.textBox13.Size = new System.Drawing.Size(30, 21);
            this.textBox13.TabIndex = 17;
            this.textBox13.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox13.Click += new System.EventHandler(this.textBox13_Click);
            this.textBox13.TextChanged += new System.EventHandler(this.textBox13_TextChanged);
            this.textBox13.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox13_KeyPress);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label14.Location = new System.Drawing.Point(140, 176);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(19, 15);
            this.label14.TabIndex = 102;
            this.label14.Text = "分";
            // 
            // textBox14
            // 
            this.textBox14.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.textBox14.Location = new System.Drawing.Point(107, 173);
            this.textBox14.Name = "textBox14";
            this.textBox14.Size = new System.Drawing.Size(30, 21);
            this.textBox14.TabIndex = 18;
            this.textBox14.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox14.Click += new System.EventHandler(this.textBox14_Click);
            this.textBox14.TextChanged += new System.EventHandler(this.textBox14_TextChanged);
            this.textBox14.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox14_KeyPress);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label15.Location = new System.Drawing.Point(73, 176);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(19, 15);
            this.label15.TabIndex = 100;
            this.label15.Text = "时";
            // 
            // textBox15
            // 
            this.textBox15.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.textBox15.Location = new System.Drawing.Point(174, 173);
            this.textBox15.Name = "textBox15";
            this.textBox15.Size = new System.Drawing.Size(30, 21);
            this.textBox15.TabIndex = 19;
            this.textBox15.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox15.Click += new System.EventHandler(this.textBox15_Click);
            this.textBox15.TextChanged += new System.EventHandler(this.textBox15_TextChanged);
            this.textBox15.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox15_KeyPress);
            // 
            // button6
            // 
            this.button6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.button6.Location = new System.Drawing.Point(256, 204);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(60, 25);
            this.button6.TabIndex = 24;
            this.button6.Text = "注销(&G)";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label16.Location = new System.Drawing.Point(207, 209);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(37, 15);
            this.label16.TabIndex = 97;
            this.label16.Text = "秒  后";
            // 
            // textBox16
            // 
            this.textBox16.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.textBox16.Location = new System.Drawing.Point(40, 206);
            this.textBox16.Name = "textBox16";
            this.textBox16.Size = new System.Drawing.Size(30, 21);
            this.textBox16.TabIndex = 21;
            this.textBox16.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox16.Click += new System.EventHandler(this.textBox16_Click);
            this.textBox16.TextChanged += new System.EventHandler(this.textBox16_TextChanged);
            this.textBox16.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox16_KeyPress);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label17.Location = new System.Drawing.Point(140, 209);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(19, 15);
            this.label17.TabIndex = 95;
            this.label17.Text = "分";
            // 
            // textBox17
            // 
            this.textBox17.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.textBox17.Location = new System.Drawing.Point(107, 206);
            this.textBox17.Name = "textBox17";
            this.textBox17.Size = new System.Drawing.Size(30, 21);
            this.textBox17.TabIndex = 22;
            this.textBox17.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox17.Click += new System.EventHandler(this.textBox17_Click);
            this.textBox17.TextChanged += new System.EventHandler(this.textBox17_TextChanged);
            this.textBox17.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox17_KeyPress);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label18.Location = new System.Drawing.Point(73, 209);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(19, 15);
            this.label18.TabIndex = 93;
            this.label18.Text = "时";
            // 
            // textBox18
            // 
            this.textBox18.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.textBox18.Location = new System.Drawing.Point(174, 206);
            this.textBox18.Name = "textBox18";
            this.textBox18.Size = new System.Drawing.Size(30, 21);
            this.textBox18.TabIndex = 23;
            this.textBox18.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox18.Click += new System.EventHandler(this.textBox18_Click);
            this.textBox18.TextChanged += new System.EventHandler(this.textBox18_TextChanged);
            this.textBox18.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox18_KeyPress);
            // 
            // button7
            // 
            this.button7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.button7.Location = new System.Drawing.Point(256, 237);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(60, 25);
            this.button7.TabIndex = 28;
            this.button7.Text = "锁定(&L)";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label19.Location = new System.Drawing.Point(207, 242);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(37, 15);
            this.label19.TabIndex = 90;
            this.label19.Text = "秒  后";
            // 
            // textBox19
            // 
            this.textBox19.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.textBox19.Location = new System.Drawing.Point(40, 239);
            this.textBox19.Name = "textBox19";
            this.textBox19.Size = new System.Drawing.Size(30, 21);
            this.textBox19.TabIndex = 25;
            this.textBox19.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox19.Click += new System.EventHandler(this.textBox19_Click);
            this.textBox19.TextChanged += new System.EventHandler(this.textBox19_TextChanged);
            this.textBox19.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox19_KeyPress);
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label20.Location = new System.Drawing.Point(140, 242);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(19, 15);
            this.label20.TabIndex = 88;
            this.label20.Text = "分";
            // 
            // textBox20
            // 
            this.textBox20.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.textBox20.Location = new System.Drawing.Point(107, 239);
            this.textBox20.Name = "textBox20";
            this.textBox20.Size = new System.Drawing.Size(30, 21);
            this.textBox20.TabIndex = 26;
            this.textBox20.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox20.Click += new System.EventHandler(this.textBox20_Click);
            this.textBox20.TextChanged += new System.EventHandler(this.textBox20_TextChanged);
            this.textBox20.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox20_KeyPress);
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label21.Location = new System.Drawing.Point(73, 242);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(19, 15);
            this.label21.TabIndex = 86;
            this.label21.Text = "时";
            // 
            // textBox21
            // 
            this.textBox21.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.textBox21.Location = new System.Drawing.Point(174, 239);
            this.textBox21.Name = "textBox21";
            this.textBox21.Size = new System.Drawing.Size(30, 21);
            this.textBox21.TabIndex = 27;
            this.textBox21.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox21.Click += new System.EventHandler(this.textBox21_Click);
            this.textBox21.TextChanged += new System.EventHandler(this.textBox21_TextChanged);
            this.textBox21.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox21_KeyPress);
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.button3.Location = new System.Drawing.Point(256, 105);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(60, 25);
            this.button3.TabIndex = 12;
            this.button3.Text = "休眠(&A)";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label7.Location = new System.Drawing.Point(207, 110);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(37, 15);
            this.label7.TabIndex = 83;
            this.label7.Text = "秒  后";
            // 
            // textBox7
            // 
            this.textBox7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.textBox7.Location = new System.Drawing.Point(40, 107);
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new System.Drawing.Size(30, 21);
            this.textBox7.TabIndex = 9;
            this.textBox7.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox7.Click += new System.EventHandler(this.textBox7_Click);
            this.textBox7.TextChanged += new System.EventHandler(this.textBox7_TextChanged);
            this.textBox7.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox7_KeyPress);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label8.Location = new System.Drawing.Point(140, 110);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(19, 15);
            this.label8.TabIndex = 81;
            this.label8.Text = "分";
            // 
            // textBox8
            // 
            this.textBox8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.textBox8.Location = new System.Drawing.Point(107, 107);
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new System.Drawing.Size(30, 21);
            this.textBox8.TabIndex = 10;
            this.textBox8.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox8.Click += new System.EventHandler(this.textBox8_Click);
            this.textBox8.TextChanged += new System.EventHandler(this.textBox8_TextChanged);
            this.textBox8.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox8_KeyPress);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label9.Location = new System.Drawing.Point(73, 110);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(19, 15);
            this.label9.TabIndex = 79;
            this.label9.Text = "时";
            // 
            // textBox9
            // 
            this.textBox9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.textBox9.Location = new System.Drawing.Point(174, 107);
            this.textBox9.Name = "textBox9";
            this.textBox9.Size = new System.Drawing.Size(30, 21);
            this.textBox9.TabIndex = 11;
            this.textBox9.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox9.Click += new System.EventHandler(this.textBox9_Click);
            this.textBox9.TextChanged += new System.EventHandler(this.textBox9_TextChanged);
            this.textBox9.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox9_KeyPress);
            // 
            // button4
            // 
            this.button4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.button4.Location = new System.Drawing.Point(256, 138);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(60, 25);
            this.button4.TabIndex = 16;
            this.button4.Text = "睡眠(&Q)";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label10.Location = new System.Drawing.Point(207, 143);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(37, 15);
            this.label10.TabIndex = 76;
            this.label10.Text = "秒  后";
            // 
            // textBox10
            // 
            this.textBox10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.textBox10.Location = new System.Drawing.Point(40, 140);
            this.textBox10.Name = "textBox10";
            this.textBox10.Size = new System.Drawing.Size(30, 21);
            this.textBox10.TabIndex = 13;
            this.textBox10.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox10.Click += new System.EventHandler(this.textBox10_Click);
            this.textBox10.TextChanged += new System.EventHandler(this.textBox10_TextChanged);
            this.textBox10.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox10_KeyPress);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label11.Location = new System.Drawing.Point(140, 143);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(19, 15);
            this.label11.TabIndex = 74;
            this.label11.Text = "分";
            // 
            // textBox11
            // 
            this.textBox11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.textBox11.Location = new System.Drawing.Point(107, 140);
            this.textBox11.Name = "textBox11";
            this.textBox11.Size = new System.Drawing.Size(30, 21);
            this.textBox11.TabIndex = 14;
            this.textBox11.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox11.Click += new System.EventHandler(this.textBox11_Click);
            this.textBox11.TextChanged += new System.EventHandler(this.textBox11_TextChanged);
            this.textBox11.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox11_KeyPress);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label12.Location = new System.Drawing.Point(73, 143);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(19, 15);
            this.label12.TabIndex = 72;
            this.label12.Text = "时";
            // 
            // textBox12
            // 
            this.textBox12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.textBox12.Location = new System.Drawing.Point(174, 140);
            this.textBox12.Name = "textBox12";
            this.textBox12.Size = new System.Drawing.Size(30, 21);
            this.textBox12.TabIndex = 15;
            this.textBox12.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox12.Click += new System.EventHandler(this.textBox12_Click);
            this.textBox12.TextChanged += new System.EventHandler(this.textBox12_TextChanged);
            this.textBox12.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox12_KeyPress);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.button2.Location = new System.Drawing.Point(256, 72);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(60, 25);
            this.button2.TabIndex = 8;
            this.button2.Text = "息屏(&P)";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label4.Location = new System.Drawing.Point(207, 77);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 15);
            this.label4.TabIndex = 69;
            this.label4.Text = "秒  后";
            // 
            // textBox4
            // 
            this.textBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.textBox4.Location = new System.Drawing.Point(40, 74);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(30, 21);
            this.textBox4.TabIndex = 5;
            this.textBox4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox4.Click += new System.EventHandler(this.textBox4_Click);
            this.textBox4.TextChanged += new System.EventHandler(this.textBox4_TextChanged);
            this.textBox4.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox4_KeyPress);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label5.Location = new System.Drawing.Point(140, 77);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(19, 15);
            this.label5.TabIndex = 67;
            this.label5.Text = "分";
            // 
            // textBox5
            // 
            this.textBox5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.textBox5.Location = new System.Drawing.Point(107, 74);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(30, 21);
            this.textBox5.TabIndex = 6;
            this.textBox5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox5.Click += new System.EventHandler(this.textBox5_Click);
            this.textBox5.TextChanged += new System.EventHandler(this.textBox5_TextChanged);
            this.textBox5.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox5_KeyPress);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label6.Location = new System.Drawing.Point(73, 77);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(19, 15);
            this.label6.TabIndex = 65;
            this.label6.Text = "时";
            // 
            // textBox6
            // 
            this.textBox6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.textBox6.Location = new System.Drawing.Point(174, 74);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(30, 21);
            this.textBox6.TabIndex = 7;
            this.textBox6.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox6.Click += new System.EventHandler(this.textBox6_Click);
            this.textBox6.TextChanged += new System.EventHandler(this.textBox6_TextChanged);
            this.textBox6.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox6_KeyPress);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.button1.Location = new System.Drawing.Point(256, 39);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(60, 25);
            this.button1.TabIndex = 4;
            this.button1.Text = "关机(&S)";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label3.Location = new System.Drawing.Point(207, 44);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 15);
            this.label3.TabIndex = 62;
            this.label3.Text = "秒  后";
            // 
            // textBox3
            // 
            this.textBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.textBox3.Location = new System.Drawing.Point(174, 41);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(30, 21);
            this.textBox3.TabIndex = 3;
            this.textBox3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox3.Click += new System.EventHandler(this.textBox3_Click);
            this.textBox3.TextChanged += new System.EventHandler(this.textBox3_TextChanged);
            this.textBox3.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox3_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.Location = new System.Drawing.Point(140, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(19, 15);
            this.label2.TabIndex = 60;
            this.label2.Text = "分";
            // 
            // textBox2
            // 
            this.textBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.textBox2.Location = new System.Drawing.Point(107, 41);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(30, 21);
            this.textBox2.TabIndex = 2;
            this.textBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox2.Click += new System.EventHandler(this.textBox2_Click);
            this.textBox2.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            this.textBox2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox2_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(73, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(19, 15);
            this.label1.TabIndex = 58;
            this.label1.Text = "时";
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.textBox1.Location = new System.Drawing.Point(40, 41);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(30, 21);
            this.textBox1.TabIndex = 1;
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox1.Click += new System.EventHandler(this.textBox1_Click);
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            this.textBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label29.Location = new System.Drawing.Point(12, 8);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(34, 17);
            this.label29.TabIndex = 114;
            this.label29.Text = "time";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(236, 273);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(95, 31);
            this.pictureBox1.TabIndex = 115;
            this.pictureBox1.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(330, 303);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label29);
            this.Controls.Add(this.label26);
            this.Controls.Add(this.label27);
            this.Controls.Add(this.label28);
            this.Controls.Add(this.label24);
            this.Controls.Add(this.label25);
            this.Controls.Add(this.label23);
            this.Controls.Add(this.label22);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.textBox13);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.textBox14);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.textBox15);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.textBox16);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.textBox17);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.textBox18);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.textBox19);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.textBox20);
            this.Controls.Add(this.label21);
            this.Controls.Add(this.textBox21);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textBox7);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.textBox8);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.textBox9);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.textBox10);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.textBox11);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.textBox12);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBox5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textBox6);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "QTools";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.SizeChanged += new System.EventHandler(this.Form1_SizeChanged);
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem Show_ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem About_ToolStripMenuItem;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox textBox13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox textBox14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox textBox15;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox textBox16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox textBox17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox textBox18;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox textBox19;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox textBox20;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.TextBox textBox21;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBox7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBox8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBox9;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox textBox10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textBox11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox textBox12;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ToolStripMenuItem Exit_ToolStripMenuItem;
    }
}

