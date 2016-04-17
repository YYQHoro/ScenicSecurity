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
namespace Serial
{
    public partial class Form1 : Form
    {
        private SystemState systemState = 0;

        private Boolean M_State=false;
        private DateTime M_Time;

        private enum SystemState
        {
            Disconnected = 0,
            Getting = 1,
            Ready = 2,
        };
        public enum UpdateEvent
        {
            SerialState=0,
        }
        const String mdbPath = "F:\\VSproject\\Serial\\yyq.mdb";
        const String tableName = "table1";
        //连接字符串,用来连接Database数据库;
        //如果没有密码请去掉JET OLEDB:Database Password=***;
        public static string connString = @"
                Provider=Microsoft.Jet.OLEDB.4.0;
                Data Source=" + mdbPath + ";"
            ;

        //SQL查询语句,用来从Database数据库tblMat表中获取所有数据;
        private string sqlString = "SELECT * from " + tableName;

        //dataadapter,使数据库的表和内存中的表datatable通讯
        private OleDbDataAdapter da;
        //bindingsource,使内存中的表datatable与窗体的显示控件datagridview通讯
        private BindingSource bs;

        private DataTable dt = new DataTable();

        public Form1()
        {
            InitializeComponent();

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
            comboBox1.SelectedIndex = 0;

            String[] CommandName =
            {
                "启动",
                "停止",
            };
            foreach(String name in CommandName)
            {
                int i= dataGridView2.Rows.Add();
                dataGridView2.Rows[i].Cells[0].Value = name;
            }
            
        }
        public delegate void HandleInterfaceUpdataDelegate(String str);
   
        private HandleInterfaceUpdataDelegate interfaceUpdataHandle;

        private void UpdateUI(String str)
        {
            textBox1.Text = str;
            if (str.EndsWith("OK"))
            {
                str=str.TrimEnd("OK".ToCharArray());

                label_state_serial.Text = "串口状态：已连接";
                changeSkinState(true);
            }
            
            if (systemState==SystemState.Getting)
            {

            }
            
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
                    systemState = SystemState.Disconnected;
                }
                else
                {
                    serialPort1.PortName = comboBox1.SelectedItem.ToString();
                    serialPort1.Open();
                    label_state_serial.Text = "串口状态：已连接";
                    btn_connect.Text = "断开串口";
                   
                    serial_getInfo();
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
            using (OleDbConnection conn = new OleDbConnection(connString))
            {
                da = new OleDbDataAdapter(sqlString, conn);
                //新建datatable
                dt.Clear();
                //DataTable dt = new DataTable();
                //如果数据适配器填充内存表时,没有设置主键列,而access数据表有,那么设置主键;
                //如果access数据表的主键是自动递增,那么设置内存表的主键也是自动递增.
                da.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                //填充内存表
                da.Fill(dt);
                //如果设置了MissingSchemaAction,下面语句可以省略;
                //dt.PrimaryKey = new DataColumn[] { dt.Columns[0] };
                //dt.Columns[0].AutoIncrement = true;
                //dt.Columns[0].AutoIncrementSeed = 1;
                //dt.Columns[0].AutoIncrementStep = 1;

                //新建bindingsource
                bs = new BindingSource();
                //bindingsource绑定内存表
                bs.DataSource = dt;
                
                //datagridview绑定bindingsource
                dataGridView1.DataSource = bs;
            }
        }

        /// <summary>
        /// 从dataGridView中更新数据到Access
        /// </summary>
        private void dataUpdate()
        {
            using (OleDbConnection conn = new OleDbConnection(connString))
            {
                da = new OleDbDataAdapter(sqlString, conn);
                OleDbCommandBuilder cb = new OleDbCommandBuilder(da);
                //用dataadapter的update方法自动更新access数据库
                da.Update((DataTable)bs.DataSource);
            }
        }

        /// <summary>
        /// 从dataGridView中更新数据到Chart
        /// </summary>
        private void dataUpdataChart()
        {
            //chart1.Series.Clear();
            chart1.Series[0].Points.DataBind(dt.AsEnumerable(), "name", "age", "");

            //Series dataTableSeries1 = new Series("name-age");
            //dataTableSeries1.Points.DataBind(dt.AsEnumerable(), "name", "age", "");
            
            //chart1.Series.Add(dataTableSeries1);
        }


        private void btn_send_Click(object sender, EventArgs e)
        {
            if(serialPort1.IsOpen)
            {
                Encoding gb = Encoding.GetEncoding("gb2312");
                byte[] bytes = gb.GetBytes("中文");
                serialPort1.Write(bytes, 0, bytes.Length);

                //serialPort1.WriteLine(textBox1.Text);
            }
            else
            {
                MessageBox.Show("请先连接串口", "温馨提示");
            }

        }

        /// <summary>
        /// 串口接收处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        
        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {

            byte[] readBuffer = new byte[serialPort1.ReadBufferSize];
            serialPort1.NewLine = "OK";
            int i=serialPort1.Read(readBuffer, 0, readBuffer.Length);
            Console.Out.WriteLine(i);

            //String result = Encoding.UTF8.GetString(readBuffer).TrimEnd('\0');
            String result = System.Text.Encoding.Default.GetString(readBuffer);
            //if(result.EndsWith("OK"))
            {
                
                this.BeginInvoke(interfaceUpdataHandle, result);
            }

            
        }
        private void serial_getInfo()
        {
            label_state_serial.Text = "串口状态：已连接，正在获取数据";
            systemState = SystemState.Getting;

            serialPort1.WriteLine("Hello world");

           
        }
        

        private void btn_readAccess_Click(object sender, EventArgs e)
        {
            DataBind();
            dataUpdataChart();
        }

        private void btn_getCurRow_Click(object sender, EventArgs e)
        {
            if(dataGridView1.CurrentRow==null)
            {
                Console.WriteLine("行未选择。");
                return;
            }
            Console.WriteLine(dataGridView1.CurrentRow.Cells[0].Value);
            Console.WriteLine(dataGridView1.CurrentRow.Cells[1].Value);
            Console.WriteLine(dataGridView1.CurrentRow.Cells[2].Value);
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
            label3.Text = "待发送的命令：" + dataGridView2.CurrentRow.Cells[0].Value+"\n附带的参数值："+ dataGridView2.CurrentRow.Cells[1].Value;
        }

        private void dataGridView2_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            Console.WriteLine(dataGridView2.CurrentRow.Cells[0].Value);
            Console.WriteLine(dataGridView2.CurrentRow.Cells[1].Value);
        }
    }
}
