namespace Serial
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
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Label label;
            System.Windows.Forms.Label label5;
            System.Windows.Forms.Label label6;
            System.Windows.Forms.Label label1;
            System.Windows.Forms.GroupBox groupBox5;
            System.Windows.Forms.Label label8;
            System.Windows.Forms.Label label7;
            System.Windows.Forms.GroupBox groupBox2;
            System.Windows.Forms.GroupBox groupBox3;
            System.Windows.Forms.Label label4;
            System.Windows.Forms.TabControl tabControl1;
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.GroupBox groupBox6;
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.textBox_message_recv = new System.Windows.Forms.TextBox();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.textBox_message_send = new System.Windows.Forms.TextBox();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.textBox_serialDebug = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_m_ctl = new System.Windows.Forms.Button();
            this.label_m_time = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dataGridView_c = new System.Windows.Forms.DataGridView();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.btn_savedata = new System.Windows.Forms.Button();
            this.btn_readAccess = new System.Windows.Forms.Button();
            this.btn_getCurRow = new System.Windows.Forms.Button();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.btn_connect = new System.Windows.Forms.Button();
            this.btn_send = new System.Windows.Forms.Button();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label_state_serial = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btn_sendMessage = new System.Windows.Forms.Button();
            this.textBox_messageText = new System.Windows.Forms.TextBox();
            this.textBox_number = new System.Windows.Forms.TextBox();
            this.comboBox3 = new System.Windows.Forms.ComboBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.btn_command = new System.Windows.Forms.Button();
            this.textBox_command = new System.Windows.Forms.TextBox();
            this.timer_m = new System.Windows.Forms.Timer(this.components);
            this.dataGridView_cmd = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label_comm_back = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.dataGridView_m = new System.Windows.Forms.DataGridView();
            this.tabPage7 = new System.Windows.Forms.TabPage();
            this.dataGridView_p = new System.Windows.Forms.DataGridView();
            label = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            label6 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            groupBox5 = new System.Windows.Forms.GroupBox();
            label8 = new System.Windows.Forms.Label();
            label7 = new System.Windows.Forms.Label();
            groupBox2 = new System.Windows.Forms.GroupBox();
            groupBox3 = new System.Windows.Forms.GroupBox();
            label4 = new System.Windows.Forms.Label();
            tabControl1 = new System.Windows.Forms.TabControl();
            groupBox6 = new System.Windows.Forms.GroupBox();
            groupBox5.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tabPage4.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_c)).BeginInit();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_cmd)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.tabPage6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_m)).BeginInit();
            this.tabPage7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_p)).BeginInit();
            this.SuspendLayout();
            // 
            // label
            // 
            label.AutoSize = true;
            label.ForeColor = System.Drawing.SystemColors.ControlText;
            label.Location = new System.Drawing.Point(11, 38);
            label.Name = "label";
            label.Size = new System.Drawing.Size(53, 12);
            label.TabIndex = 20;
            label.Text = "波特率：";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(4, 61);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(65, 12);
            label5.TabIndex = 23;
            label5.Text = "远程目标：";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new System.Drawing.Point(3, 82);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(41, 12);
            label6.TabIndex = 25;
            label6.Text = "号码：";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(11, 16);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(53, 12);
            label1.TabIndex = 11;
            label1.Text = "串口号：";
            // 
            // groupBox5
            // 
            groupBox5.Controls.Add(this.tabControl2);
            groupBox5.Location = new System.Drawing.Point(6, 145);
            groupBox5.Name = "groupBox5";
            groupBox5.Size = new System.Drawing.Size(223, 227);
            groupBox5.TabIndex = 30;
            groupBox5.TabStop = false;
            groupBox5.Text = "调试";
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage3);
            this.tabControl2.Controls.Add(this.tabPage5);
            this.tabControl2.Controls.Add(this.tabPage4);
            this.tabControl2.Location = new System.Drawing.Point(6, 19);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(211, 202);
            this.tabControl2.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.textBox_message_recv);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(203, 176);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "收短信";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // textBox_message_recv
            // 
            this.textBox_message_recv.Location = new System.Drawing.Point(-1, -1);
            this.textBox_message_recv.Multiline = true;
            this.textBox_message_recv.Name = "textBox_message_recv";
            this.textBox_message_recv.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_message_recv.Size = new System.Drawing.Size(205, 181);
            this.textBox_message_recv.TabIndex = 3;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.textBox_message_send);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(203, 176);
            this.tabPage5.TabIndex = 2;
            this.tabPage5.Text = "发短信";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // textBox_message_send
            // 
            this.textBox_message_send.Location = new System.Drawing.Point(-1, -1);
            this.textBox_message_send.Multiline = true;
            this.textBox_message_send.Name = "textBox_message_send";
            this.textBox_message_send.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_message_send.Size = new System.Drawing.Size(205, 177);
            this.textBox_message_send.TabIndex = 3;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.textBox_serialDebug);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(203, 176);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "串口调试";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // textBox_serialDebug
            // 
            this.textBox_serialDebug.Location = new System.Drawing.Point(-2, 0);
            this.textBox_serialDebug.Multiline = true;
            this.textBox_serialDebug.Name = "textBox_serialDebug";
            this.textBox_serialDebug.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_serialDebug.Size = new System.Drawing.Size(205, 176);
            this.textBox_serialDebug.TabIndex = 2;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new System.Drawing.Point(4, 121);
            label8.Name = "label8";
            label8.Size = new System.Drawing.Size(41, 12);
            label8.TabIndex = 28;
            label8.Text = "短信：";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new System.Drawing.Point(4, 102);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(41, 12);
            label7.TabIndex = 27;
            label7.Text = "命令：";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(this.label2);
            groupBox2.Location = new System.Drawing.Point(8, 9);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new System.Drawing.Size(151, 88);
            groupBox2.TabIndex = 14;
            groupBox2.TabStop = false;
            groupBox2.Text = "系统外壳状态";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 42F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.Color.Lime;
            this.label2.Location = new System.Drawing.Point(9, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(136, 56);
            this.label2.TabIndex = 0;
            this.label2.Text = "安全";
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(this.btn_m_ctl);
            groupBox3.Controls.Add(this.label_m_time);
            groupBox3.Location = new System.Drawing.Point(165, 13);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new System.Drawing.Size(155, 84);
            groupBox3.TabIndex = 15;
            groupBox3.TabStop = false;
            groupBox3.Text = "发动机运行时间";
            // 
            // btn_m_ctl
            // 
            this.btn_m_ctl.Location = new System.Drawing.Point(36, 54);
            this.btn_m_ctl.Name = "btn_m_ctl";
            this.btn_m_ctl.Size = new System.Drawing.Size(75, 23);
            this.btn_m_ctl.TabIndex = 2;
            this.btn_m_ctl.Text = "启动";
            this.btn_m_ctl.UseVisualStyleBackColor = true;
            this.btn_m_ctl.Click += new System.EventHandler(this.btn_m_ctl_Click);
            // 
            // label_m_time
            // 
            this.label_m_time.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_m_time.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.label_m_time.Location = new System.Drawing.Point(6, 17);
            this.label_m_time.Name = "label_m_time";
            this.label_m_time.Size = new System.Drawing.Size(139, 34);
            this.label_m_time.TabIndex = 1;
            this.label_m_time.Text = "0";
            this.label_m_time.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(14, 91);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(65, 12);
            label4.TabIndex = 19;
            label4.Text = "命令反馈：";
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(this.tabPage6);
            tabControl1.Controls.Add(this.tabPage2);
            tabControl1.Controls.Add(this.tabPage7);
            tabControl1.Controls.Add(this.tabPage1);
            tabControl1.Location = new System.Drawing.Point(6, 42);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new System.Drawing.Size(451, 329);
            tabControl1.TabIndex = 18;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.dataGridView_c);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(443, 303);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "命令记录";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // dataGridView_c
            // 
            this.dataGridView_c.AllowUserToAddRows = false;
            this.dataGridView_c.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_c.Location = new System.Drawing.Point(0, 0);
            this.dataGridView_c.Name = "dataGridView_c";
            this.dataGridView_c.RowTemplate.Height = 23;
            this.dataGridView_c.Size = new System.Drawing.Size(443, 303);
            this.dataGridView_c.TabIndex = 7;
            this.dataGridView_c.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_CellValueChanged);
            this.dataGridView_c.UserDeletedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.dataGridView_UserDeletedRow);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.chart1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(443, 303);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "条形图";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // chart1
            // 
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(0, 0);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.CustomProperties = "EmptyPointValue=Zero";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(455, 345);
            this.chart1.TabIndex = 6;
            this.chart1.Text = "chart1";
            // 
            // groupBox6
            // 
            groupBox6.Controls.Add(this.btn_savedata);
            groupBox6.Controls.Add(this.btn_readAccess);
            groupBox6.Controls.Add(tabControl1);
            groupBox6.Controls.Add(this.btn_getCurRow);
            groupBox6.Location = new System.Drawing.Point(249, 104);
            groupBox6.Name = "groupBox6";
            groupBox6.Size = new System.Drawing.Size(463, 377);
            groupBox6.TabIndex = 19;
            groupBox6.TabStop = false;
            groupBox6.Text = "数据分析";
            // 
            // btn_savedata
            // 
            this.btn_savedata.Enabled = false;
            this.btn_savedata.Location = new System.Drawing.Point(122, 16);
            this.btn_savedata.Name = "btn_savedata";
            this.btn_savedata.Size = new System.Drawing.Size(122, 23);
            this.btn_savedata.TabIndex = 20;
            this.btn_savedata.Text = "将更改保存到数据库";
            this.btn_savedata.UseVisualStyleBackColor = true;
            this.btn_savedata.Click += new System.EventHandler(this.btn_savedata_Click);
            // 
            // btn_readAccess
            // 
            this.btn_readAccess.Location = new System.Drawing.Point(10, 16);
            this.btn_readAccess.Name = "btn_readAccess";
            this.btn_readAccess.Size = new System.Drawing.Size(106, 23);
            this.btn_readAccess.TabIndex = 21;
            this.btn_readAccess.Text = "重新读取数据库";
            this.btn_readAccess.UseVisualStyleBackColor = true;
            this.btn_readAccess.Click += new System.EventHandler(this.btn_readAccess_Click);
            // 
            // btn_getCurRow
            // 
            this.btn_getCurRow.Location = new System.Drawing.Point(250, 16);
            this.btn_getCurRow.Name = "btn_getCurRow";
            this.btn_getCurRow.Size = new System.Drawing.Size(75, 23);
            this.btn_getCurRow.TabIndex = 9;
            this.btn_getCurRow.Text = "读取当前行";
            this.btn_getCurRow.UseVisualStyleBackColor = true;
            this.btn_getCurRow.Click += new System.EventHandler(this.btn_getCurRow_Click);
            // 
            // serialPort1
            // 
            this.serialPort1.BaudRate = 4800;
            this.serialPort1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPort1_DataReceived);
            // 
            // btn_connect
            // 
            this.btn_connect.Location = new System.Drawing.Point(134, 11);
            this.btn_connect.Name = "btn_connect";
            this.btn_connect.Size = new System.Drawing.Size(68, 23);
            this.btn_connect.TabIndex = 0;
            this.btn_connect.Text = "连接串口";
            this.btn_connect.UseVisualStyleBackColor = true;
            this.btn_connect.Click += new System.EventHandler(this.btn_connect_Click);
            // 
            // btn_send
            // 
            this.btn_send.Location = new System.Drawing.Point(50, 62);
            this.btn_send.Name = "btn_send";
            this.btn_send.Size = new System.Drawing.Size(75, 23);
            this.btn_send.TabIndex = 4;
            this.btn_send.Text = "下达命令";
            this.btn_send.UseVisualStyleBackColor = true;
            this.btn_send.Click += new System.EventHandler(this.btn_send_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(70, 13);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(58, 20);
            this.comboBox1.TabIndex = 10;
            // 
            // label_state_serial
            // 
            this.label_state_serial.AutoSize = true;
            this.label_state_serial.Location = new System.Drawing.Point(132, 43);
            this.label_state_serial.Name = "label_state_serial";
            this.label_state_serial.Size = new System.Drawing.Size(101, 12);
            this.label_state_serial.TabIndex = 12;
            this.label_state_serial.Text = "串口状态：已断开";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(groupBox5);
            this.groupBox1.Controls.Add(this.btn_sendMessage);
            this.groupBox1.Controls.Add(label8);
            this.groupBox1.Controls.Add(label7);
            this.groupBox1.Controls.Add(this.textBox_messageText);
            this.groupBox1.Controls.Add(label6);
            this.groupBox1.Controls.Add(this.textBox_number);
            this.groupBox1.Controls.Add(label5);
            this.groupBox1.Controls.Add(this.comboBox3);
            this.groupBox1.Controls.Add(this.comboBox2);
            this.groupBox1.Controls.Add(label);
            this.groupBox1.Controls.Add(this.btn_command);
            this.groupBox1.Controls.Add(this.textBox_command);
            this.groupBox1.Controls.Add(label1);
            this.groupBox1.Controls.Add(this.label_state_serial);
            this.groupBox1.Controls.Add(this.comboBox1);
            this.groupBox1.Controls.Add(this.btn_connect);
            this.groupBox1.Location = new System.Drawing.Point(8, 103);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(235, 378);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "通讯";
            // 
            // btn_sendMessage
            // 
            this.btn_sendMessage.Location = new System.Drawing.Point(151, 118);
            this.btn_sendMessage.Name = "btn_sendMessage";
            this.btn_sendMessage.Size = new System.Drawing.Size(65, 21);
            this.btn_sendMessage.TabIndex = 29;
            this.btn_sendMessage.Text = "发送短信";
            this.btn_sendMessage.UseVisualStyleBackColor = true;
            this.btn_sendMessage.Click += new System.EventHandler(this.btn_sendMessage_Click);
            // 
            // textBox_messageText
            // 
            this.textBox_messageText.Location = new System.Drawing.Point(45, 118);
            this.textBox_messageText.Name = "textBox_messageText";
            this.textBox_messageText.Size = new System.Drawing.Size(100, 21);
            this.textBox_messageText.TabIndex = 26;
            // 
            // textBox_number
            // 
            this.textBox_number.Location = new System.Drawing.Point(45, 77);
            this.textBox_number.Name = "textBox_number";
            this.textBox_number.Size = new System.Drawing.Size(100, 21);
            this.textBox_number.TabIndex = 24;
            // 
            // comboBox3
            // 
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Location = new System.Drawing.Point(70, 56);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size(75, 20);
            this.comboBox3.TabIndex = 22;
            this.comboBox3.SelectedIndexChanged += new System.EventHandler(this.comboBox3_SelectedIndexChanged);
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(70, 35);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(58, 20);
            this.comboBox2.TabIndex = 21;
            // 
            // btn_command
            // 
            this.btn_command.Location = new System.Drawing.Point(151, 97);
            this.btn_command.Name = "btn_command";
            this.btn_command.Size = new System.Drawing.Size(65, 21);
            this.btn_command.TabIndex = 19;
            this.btn_command.Text = "下达命令";
            this.btn_command.UseVisualStyleBackColor = true;
            this.btn_command.Click += new System.EventHandler(this.btn_send_Click);
            // 
            // textBox_command
            // 
            this.textBox_command.Location = new System.Drawing.Point(45, 98);
            this.textBox_command.Name = "textBox_command";
            this.textBox_command.Size = new System.Drawing.Size(100, 21);
            this.textBox_command.TabIndex = 18;
            // 
            // timer_m
            // 
            this.timer_m.Interval = 1000;
            this.timer_m.Tick += new System.EventHandler(this.timer_m_Tick);
            // 
            // dataGridView_cmd
            // 
            this.dataGridView_cmd.AllowUserToDeleteRows = false;
            this.dataGridView_cmd.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_cmd.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2});
            this.dataGridView_cmd.Location = new System.Drawing.Point(9, 127);
            this.dataGridView_cmd.Name = "dataGridView_cmd";
            this.dataGridView_cmd.RowTemplate.Height = 23;
            this.dataGridView_cmd.Size = new System.Drawing.Size(158, 335);
            this.dataGridView_cmd.TabIndex = 16;
            this.dataGridView_cmd.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView2_CellEndEdit);
            this.dataGridView_cmd.CurrentCellChanged += new System.EventHandler(this.dataGridView2_CurrentCellChanged);
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.Column1.HeaderText = "命令";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 54;
            // 
            // Column2
            // 
            this.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.Column2.HeaderText = "值";
            this.Column2.Name = "Column2";
            this.Column2.Width = 42;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(label4);
            this.groupBox4.Controls.Add(this.label_comm_back);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.dataGridView_cmd);
            this.groupBox4.Controls.Add(this.btn_send);
            this.groupBox4.Location = new System.Drawing.Point(718, 9);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(175, 472);
            this.groupBox4.TabIndex = 17;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "控制命令";
            // 
            // label_comm_back
            // 
            this.label_comm_back.AutoSize = true;
            this.label_comm_back.Location = new System.Drawing.Point(60, 112);
            this.label_comm_back.Name = "label_comm_back";
            this.label_comm_back.Size = new System.Drawing.Size(53, 12);
            this.label_comm_back.TabIndex = 18;
            this.label_comm_back.Text = "执行成功";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 12);
            this.label3.TabIndex = 17;
            this.label3.Text = "待发送的命令：";
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.dataGridView_m);
            this.tabPage6.Location = new System.Drawing.Point(4, 22);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Size = new System.Drawing.Size(443, 303);
            this.tabPage6.TabIndex = 2;
            this.tabPage6.Text = "电机运行记录";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // dataGridView_m
            // 
            this.dataGridView_m.AllowUserToAddRows = false;
            this.dataGridView_m.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_m.Location = new System.Drawing.Point(0, 0);
            this.dataGridView_m.Name = "dataGridView_m";
            this.dataGridView_m.RowTemplate.Height = 23;
            this.dataGridView_m.Size = new System.Drawing.Size(443, 301);
            this.dataGridView_m.TabIndex = 8;
            // 
            // tabPage7
            // 
            this.tabPage7.Controls.Add(this.dataGridView_p);
            this.tabPage7.Location = new System.Drawing.Point(4, 22);
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.Size = new System.Drawing.Size(443, 303);
            this.tabPage7.TabIndex = 3;
            this.tabPage7.Text = "单价配置";
            this.tabPage7.UseVisualStyleBackColor = true;
            // 
            // dataGridView_p
            // 
            this.dataGridView_p.AllowUserToAddRows = false;
            this.dataGridView_p.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_p.Location = new System.Drawing.Point(0, 1);
            this.dataGridView_p.Name = "dataGridView_p";
            this.dataGridView_p.RowTemplate.Height = 23;
            this.dataGridView_p.Size = new System.Drawing.Size(443, 301);
            this.dataGridView_p.TabIndex = 9;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(905, 493);
            this.Controls.Add(groupBox6);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(groupBox3);
            this.Controls.Add(groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "景区计时收费系统";
            this.Load += new System.EventHandler(this.Form1_Load);
            groupBox5.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage5.ResumeLayout(false);
            this.tabPage5.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox3.ResumeLayout(false);
            tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_c)).EndInit();
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            groupBox6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_cmd)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.tabPage6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_m)).EndInit();
            this.tabPage7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_p)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.Button btn_connect;
        private System.Windows.Forms.TextBox textBox_serialDebug;
        private System.Windows.Forms.Button btn_send;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.DataGridView dataGridView_c;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.Button btn_getCurRow;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label_state_serial;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_m_ctl;
        private System.Windows.Forms.Label label_m_time;
        private System.Windows.Forms.Timer timer_m;
        private System.Windows.Forms.DataGridView dataGridView_cmd;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label_comm_back;
        private System.Windows.Forms.TextBox textBox_command;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Button btn_command;
        private System.Windows.Forms.TextBox textBox_number;
        private System.Windows.Forms.ComboBox comboBox3;
        private System.Windows.Forms.Button btn_sendMessage;
        private System.Windows.Forms.TextBox textBox_messageText;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TextBox textBox_message_recv;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.TextBox textBox_message_send;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btn_savedata;
        private System.Windows.Forms.Button btn_readAccess;
        private System.Windows.Forms.TabPage tabPage6;
        private System.Windows.Forms.DataGridView dataGridView_m;
        private System.Windows.Forms.TabPage tabPage7;
        private System.Windows.Forms.DataGridView dataGridView_p;
    }
}

