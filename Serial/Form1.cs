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
        DataTable dt;
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

        private void btn_disconnect_Click(object sender, EventArgs e)
        {
            //serialPort1.Close();
             
            adapter.Update(ds.Tables["name"]);
        }

        OleDbDataAdapter adapter = null;
        DataSet ds = new DataSet();
        OleDbCommandBuilder builder;
        private void btn_send_Click(object sender, EventArgs e)
        {
            //serialPort1.WriteLine(textBox1.Text);
            //string strConn = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mdbPath + ";Jet OLEDB:Database Password=haoren";
            string strConn = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mdbPath + ";";
            OleDbConnection odcConnection = new OleDbConnection(strConn);

            adapter = new OleDbDataAdapter("select * from table1", odcConnection);
            new OleDbCommandBuilder(adapter);

            adapter.Fill(ds, "name");
            this.dataGridView1.DataSource = ds.Tables["name"];


        }

        private void button1_Click(object sender, EventArgs e)
        {
            Boolean isSuccess = false;
            
            dt = DataBase.ReadAllData(mdbPath, "table1", ref isSuccess);//读入数据
            
            String[] fields = { "name","age" };
            String[] values = { "yyq1","1"};

            //for(int i=0;i<5;i++)
            //{
            //    values[0] = "yyq" + i;
            //    values[1] = i.ToString();
            //    isSuccess = DataBase.WriteDataByColumns(mdbPath, "table1", fields, values);
            //    Console.WriteLine(isSuccess + i.ToString());
            //}

            chart1.Series.Clear();
  
            Series dataTableSeriesAge = new Series("age");
            dataTableSeriesAge.Points.DataBind(dt.AsEnumerable(), "name", "age","");

            dataGridView1.DataSource = dt.DataSet;
            dataGridView1.DataMember = "name";

            
            //Console.WriteLine(dt.DataSet.Tables[0].ToString());
            //添加表头
            foreach(DataColumn text in dt.Columns)
            {
                dataGridView1.Columns.Add(text.ToString(), text.ToString());
            }
            
            //dataGridView1.Columns.Add(dt.Columns[1].ToString(), dt.Columns[1].ToString());


            chart1.Series.Add(dataTableSeriesAge);


            //foreach (DataColumn dc in dt.Columns)
            //{

            //    Console.WriteLine(dc.ToString());//获取所有列名
            //}

            //foreach (DataRow row in dt.Rows)
            //{
            //    foreach (DataColumn column in dt.Columns)
            //    {

            //        Console.WriteLine(row[column]);//遍历所有数据
            //    }
            //}

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
