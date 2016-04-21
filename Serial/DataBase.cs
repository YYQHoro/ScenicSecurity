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
        /// <summary>
        /// 按照列来把数据到Access数据库
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
                    = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mdbPath + ";";
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

                //Console.Out.WriteLine("SQL语句构造完成：\r\n"+ odCommand.CommandText+"\r\n");

                odCommand.ExecuteNonQuery();

                odcConnection.Close();
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                Console.WriteLine(ex.Message);
             
            }
            return result;
        }
    }
}
