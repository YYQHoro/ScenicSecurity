using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace Serial
{
    class GSM
    {
        private static SerialPort serialPort;

        /// <summary>
        /// 是否正在发送短信
        /// </summary>
        int sendingMessage = 0;

        //定义处理 串口数据 的方法的委托类型
        public delegate void HandleSerialRecvDelegate(string str);
        //定义处理 串口数据 的方法的委托对象
        private GSM.HandleSerialRecvDelegate gsmSerialHandle;
        //定义处理 更新主界面UI 的方法的委托对象
        private Form1.HandleInterfaceUpdataDelegate updateMainUI;
        
        //短信发送超时定时器
        private static System.Timers.Timer timer_message_out;

        private string message_number;
        private string message_text;

        internal HandleSerialRecvDelegate GsmSerialHandle
        {
            get
            {
                return gsmSerialHandle;
            }
        }

        public GSM(SerialPort com, Form1.HandleInterfaceUpdataDelegate ui)
        {
            if(com==null||!com.IsOpen)
            {
                throw new Exception("GSM初始化出错：串口实例不可用");
            }
            else
            {
                serialPort = com;
                //创建委托对象
                gsmSerialHandle = new HandleSerialRecvDelegate(serialRecv);

                //得到从主线程获得的更新UI的委托对象
                updateMainUI = ui;
                
                timer_message_out = new System.Timers.Timer(10000);
                timer_message_out.AutoReset = false;
                timer_message_out.Elapsed += timer_message_out_tick;
            }
        }
        private void timer_message_out_tick(object sender, EventArgs e)
        {
            if(sendingMessage!=0)
            {
                Console.Out.WriteLine("短信发送失败！超时");
                sendingMessage = -1;
                sendMessage(null, null);
            }
        }
        public void serialRecv(string str)
        {
            //将收到的串口信息以回车+换行符分隔，并丢弃空的项
            String[] package = str.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (package.Length == 0)
                return;

            //先把串口原始信息显示在串口调试窗口
            updateMainUI(Form1.UpdateUIwhich.TextboxSerial, str);

            if (package[0].StartsWith("AT+CMGR="))
            {
                if (package.Last() != "OK")
                    goto RecvError;

                string message = "";
                string message_date = "";
                string message_time = "";
                string message_from = "";
                //包里是短信的内容
                //package[1]为短信来源信息
                //+CMGR: "REC READ","+8613377270802","","16/04/19,13:25:39+32"
                //以逗号来分隔到各个组
                string[] temp = package[1].Split(',');
                //去除双引号后同时去除前面的+86
                message_from = temp[1].Trim('"').Remove(0, 3);
                message_date = "20"+temp[3].Trim('"');
                //去除双引号后去除时间后面的+32
                message_time = temp[4].Trim('"').Remove(temp[4].Length - 4);
                Console.WriteLine("Message_received.");
                Console.WriteLine("Message_from:" + message_from);
                Console.WriteLine("Message_date:" + message_date);
                Console.WriteLine("Message_time:" + message_time);
                Console.WriteLine("Message_text:");
                //接下来到最后一项OK之前都是短信的内容，通常只有一项，如果有多项，即短信内容里有换行符
                for (int i = 2; i < package.Length - 1; i++)
                {
                    message += package[i] + "\n";
                    Console.WriteLine(package[i]);
                }
                //去除最后一个'\n'
                message = message.Remove(message.Length - 1);
                Console.WriteLine("Message_end");

                String comb = message_from + "|" + message_date + "|" + message_time + "|" + message;

                 updateMainUI(Form1.UpdateUIwhich.TextboxRecv, comb);
            }
            else if (package[0].StartsWith("+CMTI: \"SM\""))
            {
                string index = package[0].Split(',')[1];
                Console.WriteLine("有新短信到达.ID=" + index + "开始获取短信内容：");
                serialPort.Write("AT+CMGR=" + index + "\r\n");
            }
            else if (package[0].StartsWith("AT+CSCS=\"GSM\""))
            {
                if (package[1] != "OK")
                {
                    Console.Out.WriteLine("发送短信过程中出错：命令AT+CSCS=\"GSM\"不返回OK");
                    sendingMessage = -1;
                }
                sendMessage(null, null);
            }
            else if (package[0].StartsWith("AT+CMGF=1"))
            {
                if (package[1] != "OK")
                {
                    Console.Out.WriteLine("发送短信过程中出错：命令AT+CMGF=1 不返回OK");
                    sendingMessage = -1;
                }
                sendMessage(null, null);
            }
            else if (package[0].StartsWith("AT+CMGS"))
            {
                if (!package[1].StartsWith(">"))
                {
                    Console.Out.WriteLine("发送短信过程中出错：命令AT+CMGS 不返回 > ");
                    sendingMessage = -1;
                }
                sendMessage(null, null);
            }
            else if (package[0].StartsWith("+CMGS"))
            {
                
                sendMessage(null, null);
            }
            else
            {
                goto RecvError;
            }
            return;
RecvError:
            if(sendingMessage!=4)
            {
                Console.Out.WriteLine("---------------");
                Console.Out.WriteLine("收到一个无法识别其类型的GSM数据包：");
                foreach (String i in package)
                {
                    Console.Out.WriteLine(i);
                }
                Console.Out.WriteLine("\n---------------");
            }
            
        }
        public void deleteMessage()
        {
            if (serialPort.IsOpen)
            {
                serialPort.Write("AT+CMGD=1\r\n");
            }
        }
        public void sendMessage(String number, String text)
        {
            if (serialPort.IsOpen)
            {
                switch (sendingMessage)
                {
                    case 0:
                        if (number == null || text == null)
                            return;
                        message_number = number;
                        message_text = text;

                        timer_message_out.Enabled = true;//开始计时

                        Console.Out.WriteLine("进入发短信第一阶段");
                        //进入发短信的第一阶段
                        sendingMessage = 1;
                        serialPort.Write("AT+CSCS=\"GSM\"" + "\r\n");
                        break;
                    case 1:
                        Console.Out.WriteLine("进入发短信第二阶段");
                        //第二阶段，设置为文本模式
                        sendingMessage = 2;
                        serialPort.Write("AT+CMGF=1" + "\r\n");
                        break;
                    case 2:
                        Console.Out.WriteLine("进入发短信第三阶段");
                        sendingMessage = 3;
                        serialPort.Write("AT+CMGS=\"" + message_number + "\"" + "\r\n");
                        break;
                    case 3:
                        Console.Out.WriteLine("进入发短信第四阶段");
                        sendingMessage = 4;
                        serialPort.Write(message_text);
                        byte[] b = new byte[1];
                        b[0] = 0x1A;
                        serialPort.Write(b, 0, 1);
                        break;
                    case 4:
                        //发送成功，停止计时
                        timer_message_out.Enabled = false;
                        updateMainUI(Form1.UpdateUIwhich.TextboxSend, message_text + "|OK");
                        message_number = null;
                        message_text = null;
                        sendingMessage = 0;
                        break;
                    case -1:
                        timer_message_out.Enabled = false;
                        updateMainUI(Form1.UpdateUIwhich.TextboxSend, message_text + "|NO");
                        message_number = null;
                        message_text = null;
                        sendingMessage = 0;
                        break;
                }
            }
        }
        
    }
}
