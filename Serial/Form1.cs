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

namespace Serial
{
    public partial class Form1 : Form
    {
        private Boolean M_State=false;
        private DateTime M_Time;

        const String mdbPath = "database.mdb";
        const String tableName = "table1";
        String[] columnsName;
        //连接字符串,用来连接Database数据库;
        //如果没有密码请去掉JET OLEDB:Database Password=***;
        public static string connString = @"
                Provider=Microsoft.Jet.OLEDB.4.0;
                Data Source=" + mdbPath + ";"
            ;
        //SQL查询语句,用来从Database数据库tblMat表中获取所有数据;
        private string sqlString = "SELECT * from " + tableName;

        
        

        

        //目标名和目标SIM号码的键值对表
        Dictionary<String, String> target=new Dictionary<String, String>();

        private static System.Timers.Timer timer_serial;

        private GSM gsm;
        private GSM.HandleSerialRecvDelegate gsmSerialProcess;

        public delegate void HandleInterfaceUpdataDelegate(UpdateUIwhich which, String str);
        private HandleInterfaceUpdataDelegate interfaceUpdataHandle;

        public Form1()
        {
            InitializeComponent();
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

        }
        private void Form1_Load(object sender, EventArgs e)
        {

            interfaceUpdataHandle = new HandleInterfaceUpdataDelegate(UpdateUI);
            
            //枚举本机串口号
            string[] portNames = SerialPort.GetPortNames();
            foreach (string name in portNames)
            {
                comboBox1.Items.Add(name);
            }
            //默认选中最后一个
            comboBox1.SelectedIndex = comboBox1.Items.Count - 1;
            
            //枚举波特率
            string[] baudrates =
            {
                "4800",
                "9600",
            };
            foreach(string baud in baudrates)
            {
                comboBox2.Items.Add(baud);
            }
            //comboBox2.SelectedIndex = 0;
            comboBox2.SelectedIndex = comboBox2.Items.Count - 1;


            String[] CommandName =
            {
                "启动",
                "停止",
            };
            foreach(String name in CommandName)
            {
                int i= dataGridView_cmd.Rows.Add();
                dataGridView_cmd.Rows[i].Cells[0].Value = name;
            }
            
            //target.Add("一号机", "15177325008");
            target.Add("一号机", "13377270802");
            if (target.Count > 0)
            {
                foreach (KeyValuePair<string, string> ky in target)
                {
                    comboBox3.Items.Add(ky.Key);
                }
                comboBox3.SelectedIndex = 0;
                textBox_number.Text = target[comboBox3.SelectedItem.ToString()];
            }

            //不可用窗体里的定时器控件，因为对控件的操作比如定时器的启动和停止，需要在UI线程操作，在串口接收的函数里并不是UI线程，无法操作

            //定时一秒的串口接收等待时间
            timer_serial = new System.Timers.Timer(1000);
            //定时十秒的短信发送超时时间
            
            //注册计时器的事件
            timer_serial.Elapsed += new ElapsedEventHandler(timer_serial_Tick);

            //设置是执行一次（false）还是一直执行(true)，默认为true
            timer_serial.AutoReset = false;

            //读取数据库
            readAccess();

            //不允许用户手动添加新行
            dataGridView.AllowUserToAddRows = false;
            
        }

        private void MessageProcess(Boolean isSend, String[] text)
        {
            if(isSend)
            {
                //处理发出的短信 text[0]

            }
            else
            {
                //处理收到短信 
                /*
                "来自:" + message[0] 
                "日期:" + message[1] 
                "时间:" + message[2] 
                "内容:" + message[3]
                */
                String[] values=new String[columnsName.Length];
   
                values[0] = text[0];//来自
                values[1] = text[3];//命令
                values[2] = "0";//text[3];//值
                values[3] = text[1];//日期
                values[4] = text[2];//时间

                DataBase.WriteDataByColumns(mdbPath, tableName, columnsName, values);
                readAccess();
                
                //dataUpdate();
            }
        }
        public enum UpdateUIwhich
        {
            TextboxSend,
            TextboxRecv,
            TextboxSerial,
        }
        private void UpdateUI(UpdateUIwhich which, String str)
        {
            textBox_serialDebug.Text += str + "-----------\r\n";
            switch(which)
            {
                case UpdateUIwhich.TextboxRecv:
                    String[] message = str.Split('|');
                    textBox_message_recv.Text += "来自:" + message[0] + "\r\n日期:" + message[1] + "\r\n时间:" + message[2] + "\r\n内容:" + message[3] + "\r\n----------\r\n";
                    MessageProcess(false, message);
                    break;
                case UpdateUIwhich.TextboxSend:
                    string[] temp = str.Split('|');
                    if(temp[1]=="OK")
                    {
                        textBox_message_send.Text += "-----Message-----\r\n" + temp[0] + "\r\n-----sent-----\r\n";
                        Console.Out.WriteLine("短信发送完成.");
                        MessageProcess(false, temp);
                    }
                    else
                    {
                        textBox_message_send.Text += "-----Message-----\r\n" + temp[0] + "\r\n---sent-failed-\r\n";
                        Console.Out.WriteLine("短信发送失败.");
                        MessageBox.Show("短信发送失败！", "错误");
                    }
                    btn_sendMessage.Enabled = true;
                    comboBox3.Enabled = true;
                    textBox_number.Enabled = true;
                    btn_command.Enabled = true;
                    btn_send.Enabled = true;
                    btn_sendMessage.Text = "发送短信";
                    break;
                case UpdateUIwhich.TextboxSerial:
                    break;
            }
        }
   
        private void btn_sendMessage_Click(object sender, EventArgs e)
        {
            
            btn_sendMessage.Enabled = false;
            btn_command.Enabled = false;
            comboBox3.Enabled = false;
            textBox_number.Enabled = false;
            btn_send.Enabled = false;

            btn_sendMessage.Text = "正在发送";
            
            gsm.sendMessage(textBox_number.Text, textBox_messageText.Text);
            
        }
        private void changeSkinState(Boolean isSafe)
        {
            if(isSafe)
            {
                label2.Text = "安全";
                label2.ForeColor = Color.Lime;
            }
            else
            {
                label2.Text = "已损";
                label2.ForeColor = Color.Red;
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

                    comboBox1.Enabled = true;
                    comboBox2.Enabled = true;
                }
                else
                {
                    serialPort1.PortName = comboBox1.SelectedItem.ToString();
                    serialPort1.Open();
                    label_state_serial.Text = "串口状态：已连接";
                    btn_connect.Text = "断开串口";

                    gsm = new GSM(serialPort1, interfaceUpdataHandle);
                    gsmSerialProcess = gsm.GsmSerialHandle;

                    comboBox1.Enabled = false;
                    comboBox2.Enabled = false;

                }
            }catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 从Access中读取数据到dataGridView1
        /// </summary>
        private void DataBind()
        {
            DataTable dt;
            //dataadapter,使数据库的表和内存中的表datatable通讯
            OleDbDataAdapter da;
            //bindingsource,使内存中的表datatable与窗体的显示控件datagridview通讯
            BindingSource bs;
            using (OleDbConnection conn = new OleDbConnection(connString))
            {
                //新建datatable
                da = new OleDbDataAdapter(sqlString, conn);
                dt = new DataTable();
                
                //如果数据适配器填充内存表时,没有设置主键列,而access数据表有,那么设置主键;
                //如果access数据表的主键是自动递增,那么设置内存表的主键也是自动递增.
                da.MissingSchemaAction = MissingSchemaAction.AddWithKey;

                //填充内存表
                da.Fill(dt);


                //获取每列的标题名，从1开始跳过首列（ID主键列）
                columnsName = new string[dt.Columns.Count-1];
                for(int i=1;i<dt.Columns.Count;i++)
                {
                    columnsName[i-1] = dt.Columns[i].ColumnName;
                    Console.Out.WriteLine(columnsName[i-1]);
                }
            
                //新建bindingsource
                bs = new BindingSource();

                //bindingsource绑定内存表
                bs.DataSource = dt;
                
                //datagridview绑定bindingsource
                dataGridView.DataSource = bs;
            }
        }

        /// <summary>
        /// 从dataGridView中更新数据到Access
        /// </summary>
        private void dataUpdate()
        {
            OleDbDataAdapter da;
            using (OleDbConnection conn = new OleDbConnection(connString))
            {
                da = new OleDbDataAdapter(sqlString, conn);
                //OleDbCommandBuilder cb = new OleDbCommandBuilder(da);
                //用dataadapter的update方法自动更新access数据库
                da.Update((DataTable)dataGridView_c.DataSource);
            }
        }

        /// <summary>
        /// 从dataGridView中更新数据到Chart
        /// </summary>
        private void dataUpdataChart()
        {
            //chart1.Series[0].Points.DataBind(dt.AsEnumerable(), "name", "age", "");
        }

        private void addNewToData()
        {

        }
        private void btn_send_Click(object sender, EventArgs e)
        {
            if(serialPort1.IsOpen)
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
            isReceiving = false;
            Console.Out.WriteLine("串口数据接收完成");

            //将收到的串口数据委托给GSM类处理
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

                timer_serial.Enabled = true;

                Console.Out.WriteLine("串口有新数据到达，开始接收");
            }
            foreach (Byte b in buf)
            {
                //Console.Out.WriteLine(b);
                recvBuf += Convert.ToChar(b);// == '\n' ? '@' : Convert.ToChar(b);
            }
        }
        private void readAccess()
        {
            DataBind();
            dataUpdataChart();
        }

        private void btn_getCurRow_Click(object sender, EventArgs e)
        {
            if(dataGridView.CurrentRow==null)
            {
                Console.WriteLine("行未选择。");
                return;
            }
            Console.WriteLine(dataGridView.CurrentRow.Cells[0].Value);
            Console.WriteLine(dataGridView.CurrentRow.Cells[1].Value);
            Console.WriteLine(dataGridView.CurrentRow.Cells[2].Value);
        }

        private void btn_m_ctl_Click(object sender, EventArgs e)
        {
            if(serialPort1.IsOpen)
            {
                if (M_State)
                {
                    btn_m_ctl.Text = "启动";
                    M_State = false;
                    timer_m.Enabled = false;
                }
                else
                {
                    btn_m_ctl.Text = "停止";
                    M_Time = new DateTime();
                    label_m_time.Text = M_Time.ToLongTimeString();
                    M_State = true;
                    timer_m.Enabled = true;
                }
            }
            else
            {
                MessageBox.Show("请先连接串口","温馨提示");
            }
           
        }

        private void timer_m_Tick(object sender, EventArgs e)
        {
            if (M_State)
            {
                M_Time = M_Time.AddSeconds(1);
                label_m_time.Text = M_Time.ToLongTimeString();
            }
        }

        private void dataGridView2_CurrentCellChanged(object sender, EventArgs e)
        {
            label3.Text = "待发送的命令：" + dataGridView_cmd.CurrentRow.Cells[0].Value+"\n附带的参数值："+ dataGridView_cmd.CurrentRow.Cells[1].Value;
            
            
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
            dataUpdate();
            readAccess();
            btn_savedata.Enabled = false;
        }

        private void dataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            btn_savedata.Enabled = true;
        }

        private void btn_readAccess_Click(object sender, EventArgs e)
        {
            readAccess();
        }

        private void dataGridView_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            btn_savedata.Enabled = true;
        }
    }
}
