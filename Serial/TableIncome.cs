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
    class TableIncome
    {
        String mdbPath;
        String tableName;
        String[] columnsName;

        public Boolean hasChanged = false;

        //连接字符串,用来连接Database数据库;
        //如果没有密码请去掉JET OLEDB:Database Password=***;
        string connString;
        //SQL查询语句,用来从Database数据库tblMat表中获取所有数据;
        string sqlString;

        //dataadapter,使数据库的表和内存中的表datatable通讯
        private OleDbDataAdapter da;
        //bindingsource,使内存中的表datatable与窗体的显示控件datagridview通讯
        private BindingSource bs;

        private DataTable dt = new DataTable();

        private DataGridView dataGridView;
        private Chart chart;
        public TableIncome(String mdbPath, String tableName, DataGridView dgv,Chart chart)
        {
            if (dgv != null)
            {
                this.mdbPath = mdbPath;
                this.tableName = tableName;
                dataGridView = dgv;
                this.chart = chart;
                connString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mdbPath + ";";
                sqlString = "SELECT * from " + tableName; 
            }
        }

        /// <summary>
        /// 从Access中读取数据到dataGridView1
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

                //datagridview绑定bindingsource
                dataGridView.DataSource = bs;
                updataToChart();
            }
        }

        /// <summary>
        /// 从dataGridView中更新数据到Access
        /// </summary>
        public void UpdateToAccess()
        {
            using (OleDbConnection conn = new OleDbConnection(connString))
            {
                da = new OleDbDataAdapter(sqlString, conn);
                OleDbCommandBuilder cb = new OleDbCommandBuilder(da);
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
            chart.Series[0].Points.DataBind(dt.AsEnumerable(),columnsName[1],columnsName[0], "");
        }

        public void addNew(String []text)
        {
            DataBase.WriteDataByColumns(mdbPath, tableName, columnsName, text);
        }
    }
}
