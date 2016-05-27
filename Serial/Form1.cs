using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Threading;
using System.Timers;
using Microsoft.VisualBasic;

namespace Serial
{
    public partial class Form1 : Form
    {
        private Boolean M_State = false;
        //private DateTime M_Time;

        public struct M_Time
        {
            public DateTime start;
            public DateTime end;
            public bool isBroken;
        }

        public M_Time[] m_time;

        const String mdbPath = "database.mdb";
        const String tableNameCmd = "cmd";
        const String tableNameIncome = "income";
        const String tableNamePrice = "price";
        const String tableNameAccount = "account";
        const String tableNameDeviceRecord = "deviceRecord";
        const String tableNameDeviceSIM = "deviceSIM";
        const String tableNameBreak = "deviceBreak";
        /// <summary>
        /// 密码
        /// </summary>
        const string PASSWORD = "a";

        /// <summary>
        /// 将更新的UI
        /// </summary>
        public enum UpdateUIwhich
        {
            /// <summary>
            /// 发短信的文本框
            /// </summary>
            TextboxSend,
            /// <summary>
            /// 收短信的文本框
            /// </summary>
            TextboxRecv,
            /// <summary>
            /// 串口调试的文本框
            /// </summary>
            TextboxSerial,
        }

        Table tableCmd;
        Table tableAccount;
        Table tablePrice;
        Table tableIncome;
        Table tableDeviceRecord;
        Table tableDeviceSIM;
        Table tableDeviceBreak;
        //目标名和目标SIM号码的键值对表
        public Dictionary<String, String> target = new Dictionary<String, String>();

        private static System.Timers.Timer timer_serial;

        private GSM gsm;
        private GSM.HandleSerialRecvDelegate gsmSerialProcess;

        public delegate void HandleInterfaceUpdataDelegate(UpdateUIwhich which, String str);
        private HandleInterfaceUpdataDelegate interfaceUpdataHandle;



        public Form1()
        {
            //初始化界面属性
            InitializeComponent();
        }

        /// <summary>
        /// 重新从数据库中读取设备名和SIM号码
        /// </summary>
        public void RefreashTarget()
        {
            tableDeviceSIM.ReadFromAccess();
            target.Clear();
            foreach (DataGridViewRow dr in tableDeviceSIM.dataGridView.Rows)
            {
                //Console.Out.WriteLine(dr[0].ToString());
                if (dr.Cells[1].Value != null)
                {
                    target.Add(dr.Cells[1].Value.ToString(), dr.Cells[2].Value.ToString());
                }
            }
            if (target.Count > 0)
            {
                comboBox1.Items.Clear();
                comboBox3.Items.Clear();
                foreach (KeyValuePair<string, string> ky in target)
                {
                    comboBox1.Items.Add(ky.Key);
                    comboBox3.Items.Add(ky.Key);
                }
                comboBox3.SelectedIndex = 0;
                comboBox1.SelectedIndex = 0;
                textBox_number.Text = target[comboBox3.SelectedItem.ToString()];

                m_time = new M_Time[target.Count];

            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int errorTimes = 0;
            interfaceUpdataHandle = new HandleInterfaceUpdataDelegate(UpdateUI);
            //密码处理函数////////////////////////////////////////////////
            login:
            string passwd = Interaction.InputBox("请输入管理员密码:\n\n留空则表示不登录系统直接退出", "登录");
            if (passwd != PASSWORD)
            {
                if (passwd == "")
                {
                    Application.Exit();
                }
                else
                {
                    if (errorTimes == 5)
                    {
                        MessageBox.Show("超过密码输入次数，电脑30秒后将自动关机。");

                    }
                    else
                    {
                        MessageBox.Show("密码错误！请重新输入", "你还剩余" + (5 - errorTimes) + "尝试机会");

                    }
                    errorTimes++;
                    if (errorTimes > 5)
                    {
                        System.Diagnostics.Process bootProcess = new System.Diagnostics.Process();
                        bootProcess.StartInfo.FileName = "shutdown";
                        bootProcess.StartInfo.Arguments = "/s /t 30 /f";
                        //bootProcess.StartInfo.Arguments += "";
                        bootProcess.Start();
                        while (true) ;
                        //Thread.Sleep(2000);
                        //Application.Exit();
                    }
                    goto login;
                }

            }
            //密码处理函数////////////////////////////////////////////////   


            //串口初始化////////////////////////////////////////////////
            //枚举本机串口号
            string[] portNames = SerialPort.GetPortNames();
            foreach (string name in portNames)
            {
                comboBox_serialNumber.Items.Add(name);
            }
            //默认选中最后一个串口号
            comboBox_serialNumber.SelectedIndex = comboBox_serialNumber.Items.Count - 1;

            //枚举波特率
            string[] baudrates =
            {
                "4800",
                "9600",
            };
            foreach (string baud in baudrates)
            {
                comboBox_baud.Items.Add(baud);
            }
            //comboBox_baud.SelectedIndex = 0;
            comboBox_baud.SelectedIndex = comboBox_baud.Items.Count - 1;
            //串口初始化////////////////////////////////////////////////


            String[] CommandName =
            {
                "启动",
                "停止",
            };
            foreach (String name in CommandName)
            {
                int i = dataGridView_cmd.Rows.Add();
                dataGridView_cmd.Rows[i].Cells[0].Value = name;
            }

            //不可以用窗体里的定时器控件，因为对控件的操作比如定时器的启动和停止，需要在UI线程操作，在串口接收的函数里并不是UI线程在执行，无法操作

            //定时一秒的串口接收等待时间
            timer_serial = new System.Timers.Timer(200);
            //定时十秒的短信发送超时时间

            //注册计时器的事件
            timer_serial.Elapsed += new ElapsedEventHandler(timer_serial_Tick);

            //设置是执行一次（false）还是一直执行(true)，默认为true
            timer_serial.AutoReset = false;

            //每一个列表一个Table类变量。
            tableCmd = new Table(mdbPath, tableNameCmd, dataGridView_c, null);
            tableAccount = new Table(mdbPath, tableNameAccount, dataGridView_yonghu, null);
            tableIncome = new Table(mdbPath, tableNameIncome, dataGridView_income, chart1);
            tablePrice = new Table(mdbPath, tableNamePrice, dataGridView_price, null);
            tableDeviceRecord = new Table(mdbPath, tableNameDeviceRecord, dataGridView_record, null);
            tableDeviceSIM = new Table(mdbPath, tableNameDeviceSIM, dataGridView_sim, null);
            tableDeviceBreak = new Table(mdbPath, tableNameBreak, dataGridView_break, null);

            dataGridView_sim.DefaultValuesNeeded += DataGridView_s_DefaultValuesNeeded;

            tablePrice.ReadFromAccess();
            tableIncome.ReadFromAccess();
            tableAccount.ReadFromAccess();
            tableCmd.ReadFromAccess();
            tableDeviceRecord.ReadFromAccess();
            tableDeviceBreak.ReadFromAccess();

            //将分页框和类的变量绑定
            tabPageChart.Tag = tableIncome;
            tabPageAccount.Tag = tableAccount;
            tabPageCmd.Tag = tableCmd;
            tabPageRecord.Tag = tableDeviceRecord;
            tabPageSim.Tag = tableDeviceSIM;
            tabPagePrice.Tag = tablePrice;
            tabPageIncome.Tag = tableIncome;
            tabPageBreak.Tag = tableDeviceBreak;

            RefreashTarget();

            //启动系统时间更新
            timer_cur.Enabled = true;
            tabControl1.SelectedIndex = 5;
            tabControl1.SelectedIndex = 3;
        }

        private void DataGridView_s_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.Cells[3].Value = 0;
            e.Row.Cells[4].Value = "未启动";
        }
        /// <summary>
        /// 接收到的短信的处理
        /// </summary>
        /// <param name="isSend"></param>
        /// <param name="text"></param>
        private void MessageProcess(Boolean isSend, String[] text)
        {
            if (isSend)
            {
                //处理发出的短信 text[0]

            }
            else
            {
                //处理收到短信 
                /*
                "来自:" + text[0] 
                "日期:" + text[1] 
                "时间:" + text[2] 
                "内容:" + text[3]
                */

                foreach (var te in text)
                {
                    Console.WriteLine("text" + te);

                }
                int i = 0;
                foreach (KeyValuePair<string, string> ky in target)
                {
                    //ky.Value SIM号码
                    //ky.Key 设备名
                    if (ky.Value.Equals(text[0]))
                    {
                        //添加到命令表格
                        String[] temp =
                        {
                            ky.Key,
                            text[3],
                            "0",
                            text[1],
                            text[2],
                        };
                        tableCmd.addNew(temp);

                        if (text[3].Contains("break"))
                        {
                            m_time[i].isBroken = true;
                            changeSkinState(true, i + 1);
                            string[] t = new string[]
                            {
                                ky.Key,
                                DateTime.Now.ToLongDateString(),
                                DateTime.Now.ToLongTimeString(),

                            };

                            tableDeviceBreak.addNew(t);
                        }
                        else if (text[3].Contains("Help"))
                        {
                            Thread t1 = new Thread(new ParameterizedThreadStart(ShowMessageBox));
                            t1.Start(ky.Key + "请求帮助！！！");

                        }
                        else if (text[3].Contains("stop"))
                        {
                            m_time[i].isBroken = true;
                            changeSkinState(true, i + 1);
                            string[] t = new string[]
                            {
                                ky.Key,
                                DateTime.Now.ToLongDateString(),
                                DateTime.Now.ToLongTimeString(),
                            };
                            tableDeviceBreak.addNew(t);
                        }
                        else if (text[3].Contains("Low"))
                        {
                            Thread t1 = new Thread(new ParameterizedThreadStart(ShowMessageBox));
                            t1.Start(ky.Key + "低电压！！！");

                        }

                        break;
                    }
                    else
                    {
                        textBox_serialDebug.Text += "收到一封短信\r\n";
                        textBox_serialDebug.Text += "SIM号：" + text[0];
                        textBox_serialDebug.Text += "内容：" + text[3];
                        textBox_serialDebug.Text += "\r\n无法为其匹配设备名\r\n";
                        textBox_serialDebug.Text += "---Error---\r\n";
                    }

                    i++;
                }
                //dataUpdate();
            }
        }

        public void ShowMessageBox(object text)
        {
            MessageBox.Show((string)text);
        }
        /// <summary>
        /// 更新UI界面的内容
        /// </summary>
        /// <param name="which"></param>
        /// <param name="str"></param>
        private void UpdateUI(UpdateUIwhich which, String str)
        {
            switch (which)
            {
                case UpdateUIwhich.TextboxRecv:
                    String[] message = str.Split('|');
                    textBox_message_recv.Text += "来自:" + message[0] + "\r\n日期:" + message[1] + "\r\n时间:" + message[2] + "\r\n内容:" + message[3] + "\r\n----------\r\n";
                    MessageProcess(false, message);
                    break;
                case UpdateUIwhich.TextboxSend:
                    btn_sendMessage.Enabled = true;
                    comboBox3.Enabled = true;
                    textBox_number.Enabled = true;
                    btn_command.Enabled = true;
                    btn_send.Enabled = true;
                    btn_sendMessage.Text = "发送短信";
                    string[] temp = str.Split('|');
                    if (temp[1] == "OK")
                    {
                        textBox_message_send.Text += "-----Message-----\r\n" + temp[0] + "\r\n-----sent-----\r\n";
                        Console.Out.WriteLine("短信发送完成.");
                        MessageProcess(false, temp);
                    }
                    else
                    {
                        textBox_message_send.Text += "-----Message-----\r\n" + temp[0] + "\r\n---sent-failed-\r\n";
                        Console.Out.WriteLine("短信发送失败.");
                        Thread t1 = new Thread(new ParameterizedThreadStart(ShowMessageBox));
                        t1.Start("短信发送失败.");
                    }

                    break;
                case UpdateUIwhich.TextboxSerial:
                    textBox_serialDebug.Text += str + "\r\n-----------\r\n";
                    break;
            }
        }

        private void btn_sendMessage_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                btn_sendMessage.Enabled = false;
                btn_command.Enabled = false;
                comboBox3.Enabled = false;
                textBox_number.Enabled = false;
                btn_send.Enabled = false;

                btn_sendMessage.Text = "正在发送";

                gsm.sendMessage(textBox_number.Text, textBox_messageText.Text);
            }
            else
            {
                MessageBox.Show("请先连接设备");
            }


        }
        private void changeSkinState(Boolean isBroken, int id)
        {
            if (isBroken)
            {
                label2.Text = id + "号已损";
                label2.ForeColor = Color.Red;
            }
            else
            {
                label2.Text = "安全";
                label2.ForeColor = Color.Lime;
            }

        }

        private void btn_connect_Click(object sender, EventArgs e)
        {
            try
            {
                if (serialPort1.IsOpen)
                {
                    serialPort1.Close();
                    label_state_serial.Text = "串口状态：已断开";
                    btn_connect.Text = "连接串口";

                    comboBox_serialNumber.Enabled = true;
                    comboBox_baud.Enabled = true;

                    btn_delMessage.Enabled = false;
                    btn_command.Enabled = false;
                    btn_sendMessage.Enabled = false;
                    btn_m_ctl.Enabled = false;
                    button_info.Enabled = false;
                }
                else
                {
                    serialPort1.PortName = comboBox_serialNumber.SelectedItem.ToString();
                    serialPort1.BaudRate = Convert.ToInt32(comboBox_baud.SelectedItem.ToString());
                    serialPort1.Open();
                    label_state_serial.Text = "串口状态：已连接";
                    btn_connect.Text = "断开串口";

                    gsm = new GSM(serialPort1, interfaceUpdataHandle);
                    gsmSerialProcess = gsm.GsmSerialHandle;

                    comboBox_serialNumber.Enabled = false;
                    comboBox_baud.Enabled = false;

                    btn_delMessage.Enabled = true;
                    btn_command.Enabled = true;
                    btn_sendMessage.Enabled = true;
                    btn_m_ctl.Enabled = true;
                    button_info.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btn_send_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                String temp = textBox_command.Text + "\r\n";
                serialPort1.Write(temp);
            }
            else
            {
                MessageBox.Show("请先连接串口", "温馨提示");
            }

        }

        private String recvBuf = "";
        private Boolean isReceiving = false;
        private void timer_serial_Tick(object sender, EventArgs e)
        {
            //时间到后算是接收完成
            isReceiving = false;
            Console.Out.WriteLine("串口数据接收完成");

            //将这段定时的时间内收到的串口数据委托给GSM类处理
            this.BeginInvoke(gsmSerialProcess, recvBuf);
        }
        /// <summary>
        /// 串口接收处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            byte[] buf = new byte[serialPort1.BytesToRead];
            serialPort1.Read(buf, 0, buf.Length);

            if (!isReceiving)
            {
                isReceiving = true;
                recvBuf = "";
                //启动定时器计时
                timer_serial.Enabled = true;

                Console.Out.WriteLine("串口有新数据到达，开始接收");
            }
            foreach (Byte b in buf)
            {
                //Console.Out.WriteLine(b);
                recvBuf += Convert.ToChar(b);// == '\n' ? '@' : Convert.ToChar(b);//如果是回车的话用@代替，便于调试
            }
        }
        private void readAccess()
        {
            tableCmd.ReadFromAccess();
        }

        private void btn_getCurRow_Click(object sender, EventArgs e)
        {
            if (dataGridView_c.CurrentRow == null)
            {
                Console.WriteLine("行未选择。");
                return;
            }
            Console.WriteLine(dataGridView_c.CurrentRow.Cells[0].Value);
            Console.WriteLine(dataGridView_c.CurrentRow.Cells[1].Value);
            Console.WriteLine(dataGridView_c.CurrentRow.Cells[2].Value);
        }

        private void btn_m_ctl_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                if (btn_m_ctl.Text == "启动")
                {
                    //发送启动命令
                    textBox_messageText.Text = "OPENOPEN";
                    btn_sendMessage_Click(null, null);

                    //启动编辑
                    dataGridView_sim.BeginEdit(false);
                    dataGridView_sim.Rows[comboBox3.SelectedIndex].Cells[4].Value = "已启动";

                    //结束编辑
                    dataGridView_sim.EndEdit();

                    m_time[comboBox3.SelectedIndex].start = DateTime.Now;
                    btn_m_ctl.Text = "停止";
                }
                else
                {
                    //发送停止命令
                    textBox_messageText.Text = "stop";
                    btn_sendMessage_Click(null, null);

                    timer_cur_Tick(null, null);

                    m_time[comboBox3.SelectedIndex].end = DateTime.Now;


                    UInt64 ini = Convert.ToUInt64(dataGridView_sim.Rows[comboBox3.SelectedIndex].Cells[3].Value);

                    ini += (UInt64)(m_time[comboBox3.SelectedIndex].end - m_time[comboBox3.SelectedIndex].start).TotalSeconds;

                    label_m_time.Text = ini.ToString();

                    //启动编辑
                    dataGridView_sim.CurrentCell = dataGridView_sim.Rows[comboBox3.SelectedIndex].Cells[4];
                    dataGridView_sim.BeginEdit(false);

                    dataGridView_sim.Rows[comboBox3.SelectedIndex].Cells[4].Value = "已停止";
                    dataGridView_sim.Rows[comboBox3.SelectedIndex].Cells[3].Value = ini;

                    //结束编辑
                    dataGridView_sim.EndEdit();


                    String[] temp = new String[]
                    {
                        (string)dataGridView_sim.Rows[comboBox3.SelectedIndex].Cells[1].Value,
                        m_time[comboBox3.SelectedIndex].start.ToLongDateString(),
                        m_time[comboBox3.SelectedIndex].start.ToLongTimeString(),
                        m_time[comboBox3.SelectedIndex].end.ToLongDateString(),
                        m_time[comboBox3.SelectedIndex].end.ToLongTimeString(),
                        ini.ToString(),

                    };
                    tableDeviceRecord.addNew(temp);

                    AddTodayIncome(Convert.ToUInt64(label_sum.Text));

                    btn_m_ctl.Text = "启动";

                }
                tableDeviceSIM.UpdateToAccess();
            }
            else
            {
                MessageBox.Show("请先连接设备", "温馨提示");
            }

        }
        /// <summary>
        /// 未理解
        /// </summary>
        /// <param name="income"></param>
        private void AddTodayIncome(UInt64 income)
        {

            if (dataGridView_income.RowCount != 0)
            {
                if ((String)dataGridView_income.Rows[dataGridView_income.RowCount - 1].Cells[2].Value == DateTime.Today.ToLongDateString())
                {
                    UInt64 last = Convert.ToUInt64(dataGridView_income.Rows[dataGridView_income.RowCount - 1].Cells[1].Value);

                    last += income;

                    dataGridView_income.BeginEdit(false);
                    dataGridView_income.Rows[dataGridView_income.RowCount - 1].Cells[1].Value = last;
                    dataGridView_income.EndEdit();

                    tableIncome.UpdateToAccess();
                    tableIncome.updataToChart();
                    return;
                }
            }
            String[] temp = new String[]
            {
                    income.ToString(),
                    DateTime.Today.ToLongDateString(),
            };
            tableIncome.addNew(temp);

            tableIncome.UpdateToAccess();
            tableIncome.updataToChart();
        }
        /// <summary>
        /// 调试用最右边
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView2_CurrentCellChanged(object sender, EventArgs e)
        {
            label3.Text = "待发送的命令：" + dataGridView_cmd.CurrentRow.Cells[0].Value + "\n附带的参数值：" + dataGridView_cmd.CurrentRow.Cells[1].Value;
        }

        private void dataGridView2_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            Console.WriteLine(dataGridView_cmd.CurrentRow.Cells[0].Value);
            Console.WriteLine(dataGridView_cmd.CurrentRow.Cells[1].Value);
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox_number.Text = target[comboBox3.SelectedItem.ToString()];
        }
        private void btn_savedata_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab.Tag != null)
            {
                ((Table)tabControl1.SelectedTab.Tag).UpdateToAccess();

                //如果改动的是Sim号码配对那一页
                if (((Table)tabControl1.SelectedTab.Tag).tableName == tableNameDeviceSIM)
                {
                    RefreashTarget();
                }
                else
                {
                    ((Table)tabControl1.SelectedTab.Tag).ReadFromAccess();
                }
            }
            btn_savedata.Enabled = false;
        }
        private void btn_readAccess_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab.Tag != null)
            {
                ((Table)tabControl1.SelectedTab.Tag).ReadFromAccess();
            }
        }
        /// <summary>
        /// 检测是否修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            ((Table)tabControl1.SelectedTab.Tag).hasChanged = true;
            btn_savedata.Enabled = true;
        }
        private void dataGridView_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            ((Table)tabControl1.SelectedTab.Tag).hasChanged = true;
            btn_savedata.Enabled = true;
        }
        /// <summary>
        /// 选择分页分栏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab.Tag != null)
            {
                btn_savedata.Enabled = ((Table)tabControl1.SelectedTab.Tag).hasChanged;
            }
            else
            {
                btn_savedata.Enabled = false;
            }
        }
        /// <summary>
        /// 每秒触发一次记录设备运行时间。刷新系统时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_cur_Tick(object sender, EventArgs e)
        {
            label_curTime.Text = "当前时间：" + DateTime.Now.ToLocalTime().ToString();

            UInt64 time = 0;
            if ((string)dataGridView_sim.Rows[comboBox3.SelectedIndex].Cells[4].Value == "已启动")
            {
                time = (UInt64)(DateTime.Now - m_time[comboBox3.SelectedIndex].start).TotalSeconds;
            }
            else
            {
                label_m_time.Text = "0";
            }
            UInt64 price = Convert.ToUInt64(dataGridView_price.Rows[comboBox3.SelectedIndex].Cells[2].Value);
            label_m_time.Text = time.ToString();
            label_price.Text = price.ToString();
            label_sum.Text = (time * price).ToString();

            //m_time[1].isBroken = true;

        }
        /// <summary>
        /// GSM清除短信
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_delMessage_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                gsm.deleteMessage();
            }
            else
            {
                MessageBox.Show("请先连接串口", "温馨提示");
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            comboBox3.SelectedIndex = comboBox1.SelectedIndex;
            //Console.WriteLine(dataGridView_s.Rows.Count);
            string state = (string)dataGridView_sim.Rows[comboBox3.SelectedIndex].Cells[4].Value;
            if (state == "已启动")
            {
                btn_m_ctl.Text = "停止";
            }
            else
            {
                btn_m_ctl.Text = "启动";
            }

            if (m_time != null)
                changeSkinState(m_time[comboBox1.SelectedIndex].isBroken, comboBox1.SelectedIndex + 1);
            //Console.WriteLine(state);

        }

        private void button_info_Click(object sender, EventArgs e)
        {
            //发送查询命令
            textBox_messageText.Text = "SEARCH";
            btn_sendMessage_Click(null, null);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox_serialDebug.Text = "";
        }
    }
}
