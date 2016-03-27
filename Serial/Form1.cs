using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Serial
{
    public partial class Form1 : Form
    {
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
            serialPort1.Close();
        }

        private void btn_send_Click(object sender, EventArgs e)
        {
            serialPort1.WriteLine(textBox1.Text);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Boolean isSuccess = false;
            //DataTable dt= DataBase.ReadAllData( "F:\\VSproject\\Serial\\mydatebase.mdb","table1", ref isSuccess);
            String[] values= { "Hello World!"};

            isSuccess = DataBase.WriteDataByColumns("F:\\VSproject\\Serial\\mydatebase.mdb", "table1", values);


            Console.WriteLine(isSuccess);

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
