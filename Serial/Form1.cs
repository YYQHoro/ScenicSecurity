using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Serial
{
    public partial class Form1 : Form
    {
        const String mdbPath = "F:\\VSproject\\Serial\\yyq.mdb";

        //连接字符串,用来连接Database数据库;
        //如果没有密码请去掉JET OLEDB:Database Password=***;
        public static string connString = @"
                Provider=Microsoft.Jet.OLEDB.4.0;
                Data Source=" + mdbPath + ";"
            ;

        //SQL查询语句,用来从Database数据库tblMat表中获取所有数据;
        private string sqlString = "SELECT * from table1";
        //dataadapter,使数据库的表和内存中的表datatable通讯
        private OleDbDataAdapter da;
        //bindingsource,使内存中的表datatable与窗体的显示控件datagridview通讯
        private BindingSource bs;

        private DataTable dt = new DataTable();

        public Form1()
        {
            InitializeComponent();

        }
        public delegate void HandleInterfaceUpdataDelegate(string text);
        private HandleInterfaceUpdataDelegate interfaceUpdataHandle;

        private void btn_connect_Click(object sender, EventArgs e)
        {
            interfaceUpdataHandle = new HandleInterfaceUpdataDelegate(UpdateTextBox);

            serialPort1.Open();
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


        private void btn_disconnect_Click(object sender, EventArgs e)
        {
            //serialPort1.Close();
            DataBind();
        }

        private void btn_send_Click(object sender, EventArgs e)
        {
            // serialPort1.WriteLine(textBox1.Text);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Boolean isSuccess = false;
            //DataBase.ReadAllData(mdbPath, "table1", ref isSuccess);//读入数据
            
            String[] fields = { "name","age" };
            String[] values = { "yyq1","1"};

            dataUpdataChart();

            Console.WriteLine(isSuccess );
        }

        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            byte[] readBuffer = new byte[serialPort1.ReadBufferSize];

            serialPort1.Read(readBuffer, 0, readBuffer.Length);

            this.Invoke(interfaceUpdataHandle, Encoding.UTF8.GetString(readBuffer));
        }
        private void UpdateTextBox(string text)
        {
            textBox2.Text = text;
        }

    }
}
