using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.OleDb;
using System.Data;
using System.Collections;

namespace Serial
{
    class DataBase
    {
        //public String connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=product.mdb";
        /*
        public static Boolean Connect()
        {
            //连接字符串：
            String connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=product.mdb";
            //建立连接：
            OleDbConnection connection = new OleDbConnection(connectionString);
            //使用OleDbCommand类来执行Sql语句：
            //OleDbCommand cmd = new OleDbCommand(sql, connection);
            connection.Open();
            //cmd.ExecuteNonQuery();

        }
        */
        public static DataTable ReadAllData(string mdbPath,string tableName, ref bool success)
        {
            DataTable dt = new DataTable();
            try
            {
                DataRow dr;
                //1、建立连接    
                string strConn
                    = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mdbPath + ";Jet OLEDB:Database Password=haoren";
                OleDbConnection odcConnection = new OleDbConnection(strConn);
                //2、打开连接    
                odcConnection.Open();
                //建立SQL查询    
                OleDbCommand odCommand = odcConnection.CreateCommand();
                //3、输入查询语句    
                odCommand.CommandText = "select * from " + tableName;
                //建立读取    
                OleDbDataReader odrReader = odCommand.ExecuteReader();
                //查询并显示数据    
                int size = odrReader.FieldCount;
                for (int i = 0; i < size; i++)
                {
                    DataColumn dc;
                    dc = new DataColumn(odrReader.GetName(i));
                    dt.Columns.Add(dc);
                }
                while (odrReader.Read())
                {
                    dr = dt.NewRow();
                    for (int i = 0; i < size; i++)
                    {
                        dr[odrReader.GetName(i)] = odrReader[odrReader.GetName(i)].ToString();
                    }
                    dt.Rows.Add(dr);
                }
                //关闭连接    
                odrReader.Close();
                odcConnection.Close();
                success = true;
                return dt;
            }
            catch
            {
                success = false;
                return dt;
            }
        }

        public static DataTable ReadDataByColumns(string mdbPath, string tableName, string[] columns, ref bool success)
        {
            DataTable dt = new DataTable();

            try
            {
                //1、建立连接    
                string strConn
                    = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mdbPath + ";Jet OLEDB:Database Password=haoren";
                OleDbConnection odcConnection = new OleDbConnection(strConn);
                //2、打开连接    
                odcConnection.Open();
                //建立SQL查询    
                OleDbCommand odCommand = odcConnection.CreateCommand();

                //3、输入查询语句    
                string strColumn = "";
                for (int i = 0; i < columns.Length; i++)
                {
                    strColumn += columns[i].ToString() + ",";
                }
                strColumn = strColumn.TrimEnd(',');
                odCommand.CommandText = "select " + strColumn + " from " + tableName;

                //建立读取
                OleDbDataReader odrReader = odCommand.ExecuteReader();
                
                //查询并显示数据

                //添加列标题
                int size = odrReader.FieldCount;
                for (int i = 0; i < size; i++)
                {
                    DataColumn dc;
                    dc = new DataColumn(odrReader.GetName(i));
                    dt.Columns.Add(dc);
                }

                DataRow dr;
                while (odrReader.Read())
                {
                    dr = dt.NewRow();
                    //为当前行填充各列的数据
                    for (int i = 0; i < size; i++)
                    {
                        dr[odrReader.GetName(i)] = odrReader[odrReader.GetName(i)].ToString();
                    }
                    dt.Rows.Add(dr);
                }

                //关闭连接    
                odrReader.Close();
                odcConnection.Close();
                success = true;
                return dt;
            }
            catch
            {
                success = false;
                return dt;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mdbPath"></param>
        /// <param name="tableName">表名</param>
        /// <param name="fields">列名</param>
        /// <param name="values">值</param>
        /// <returns></returns>
        public static Boolean WriteDataByColumns(string mdbPath, string tableName,string[] fields,string[] values)
        {
            Boolean result = false;
            try
            {
                //1、建立连接    
                string strConn
                    = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mdbPath + ";Jet OLEDB:Database Password=haoren";
                OleDbConnection odcConnection = new OleDbConnection(strConn);
                //2、打开连接    
                odcConnection.Open();
                //建立SQL查询    
                OleDbCommand odCommand = odcConnection.CreateCommand();
                //3、输入查询语句  
                string strFields = "";
                for (int i = 0; i < fields.Length; i++)
                {
                    if (i == fields.Length - 1)
                        strFields +=   fields[i] ;
                    else          
                        strFields +=  fields[i] + ",";
                }
                string strValues = "";
                for (int i = 0; i < values.Length; i++)
                {
                    if (i == values.Length - 1)
                        strValues += "'" + values[i] + "'";
                    else
                        strValues += "'" + values[i] + "'" + ",";
                        
                }
                //同时插入一行中的多个字段时，每个字段的值用‘ 包起来
                odCommand.CommandText = "insert into " + tableName + "(" + strFields + ") values(" + strValues + ");";
                odCommand.ExecuteNonQuery();

                result = true;
                odcConnection.Close();
            }
            catch (Exception ex)
            {
                result = false;
                Console.WriteLine(ex.Message);
             
            }
            return result;
        }
        /*
        public static bool CreateMDBTable(string mdbPath, string tableName, ArrayList mdbHead)
        {
            try
            {
                ADOX.CatalogClass cat = new ADOX.CatalogClass();
                string sAccessConnection
                    = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mdbPath;
                ADODB.Connection cn = new ADODB.Connection();
                cn.Open(sAccessConnection, null, null, -1);
                cat.ActiveConnection = cn;

                //新建一个表    
                ADOX.TableClass tbl = new ADOX.TableClass();
                tbl.ParentCatalog = cat;
                tbl.Name = tableName;

                int size = mdbHead.Count;
                for (int i = 0; i < size; i++)
                {
                    //增加一个文本字段    
                    ADOX.ColumnClass col2 = new ADOX.ColumnClass();
                    col2.ParentCatalog = cat;
                    col2.Name = mdbHead[i].ToString();//列的名称    
                    col2.Properties["Jet OLEDB:Allow Zero Length"].Value = false;
                    tbl.Columns.Append(col2, ADOX.DataTypeEnum.adVarWChar, 500);
                }
                cat.Tables.Append(tbl);   //这句把表加入数据库(非常重要)    
                tbl = null;
                cat = null;
                cn.Close();
                return true;
            }
            catch { return false; }
        }
        */
        /*
        //创建mdb    
        public static bool CreateMDBDataBase(string mdbPath)
        {
            try
            {
                ADOX.CatalogClass cat = new ADOX.CatalogClass();
                cat.Create("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mdbPath + ";");
                cat = null;
                return true;
            }
            catch { return false; }
        }
        */



    }
}
