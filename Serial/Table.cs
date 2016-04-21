using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Serial
{
    class Table
    {
        /// <summary>
        /// 数据库文件名
        /// </summary>
        String mdbPath;
        /// <summary>
        /// 表名
        /// </summary>
        public String tableName;
        /// <summary>
        /// 列名
        /// </summary>
        String[] columnsName;

        /// <summary>
        /// 用户是否做了修改
        /// </summary>
        public Boolean hasChanged = false;

        string connString;

        string sqlString;

        private DataTable dt = new DataTable();

        /// <summary>
        /// DataAdapter,使数据库的表和内存中的表DataTable通讯
        /// </summary>
        private OleDbDataAdapter da;

        /// <summary>
        /// BindingSource,使内存中的表DataTable与窗体的显示控件datagridview通讯
        /// </summary>
        private BindingSource bs;

        public DataGridView dataGridView;
        public Chart chart;

        public Table(String mdbPath, String tableName, DataGridView dgv,Chart chart)
        {
            if (dgv != null)
            {
                this.mdbPath = mdbPath;
                this.tableName = tableName;
                dataGridView = dgv;
                this.chart = chart;

                //连接字符串,用来连接Database数据库;
                connString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mdbPath + ";";
                
                //SQL查询语句,用来从Database数据库tblMat表中获取所有数据;
                sqlString = "SELECT * from " + tableName;

                //设置列宽
                dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            }

        }

        /// <summary>
        /// 从Access中读取数据到dataGridView
        /// </summary>
        public void ReadFromAccess()
        {
            using (OleDbConnection conn = new OleDbConnection(connString))
            {
                //新建datatable
                da = new OleDbDataAdapter(sqlString, conn);

                dt.Clear();

                //如果数据适配器填充内存表时,没有设置主键列,而access数据表有,那么设置主键;
                //如果access数据表的主键是自动递增,那么设置内存表的主键也是自动递增.
                da.MissingSchemaAction = MissingSchemaAction.AddWithKey;

                //填充内存表
                da.Fill(dt);

                //获取每列的标题名，从1开始跳过首列（ID主键列）
                columnsName = new string[dt.Columns.Count - 1];
                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    columnsName[i - 1] = dt.Columns[i].ColumnName;
                    //Console.Out.WriteLine(columnsName[i - 1]);
                }

                //新建bindingsource
                bs = new BindingSource();

                //bindingsource绑定内存表
                bs.DataSource = dt;

                //datagridview绑定bindingsource，至此dataGridView就自动显示内容了
                dataGridView.DataSource = bs;

                //更新图表（如果有）
                updataToChart();
            }
        }

        /// <summary>
        /// 把dataGridView的数据写到Access
        /// </summary>
        public void UpdateToAccess()
        {
            using (OleDbConnection conn = new OleDbConnection(connString))
            {
                da = new OleDbDataAdapter(sqlString, conn);
                //调用一下OleDbCommandBuilder的构造方法，不然会无法自动生成SQL语句
                new OleDbCommandBuilder(da);
                //用dataadapter的update方法自动更新access数据库
                da.Update((DataTable)bs.DataSource);
                hasChanged = false;
            }
        }

        /// <summary>
        /// 从dataGridView中更新数据到Chart
        /// </summary>
        public void updataToChart()
        {
            if(chart!=null)
                chart.Series[0].Points.DataBind(dt.AsEnumerable(),columnsName[1],columnsName[0], "");
        }
        /// <summary>
        /// 添加新数据到数据库然后重新刷新表格
        /// </summary>
        /// <param name="text">数据内容数组，数组内部顺序等同相应表格的列顺序(舍弃第一个ID列)</param>
        public void addNew(String []text)
        {
            DataBase.WriteDataByColumns(mdbPath, tableName, columnsName, text);
            ReadFromAccess();
        }
    }
}
