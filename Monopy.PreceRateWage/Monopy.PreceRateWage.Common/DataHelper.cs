/*说明
 * 作   者:黄昊
 * 文件名:DataHelper.cs
 * 功   能:一个常见数据库的基本操作类，支持Data.OleDb和Data.SqlClinet命名空间下的数据库
 * 摘   要:包括执行SQL语句（执行SQL语句使用参数、事务），执行存储过程，返回DataTable
 * 类名称:DataHelper
 * 版   本:V 1.0
 * 框   架:.NET 2.0(已经修改为3.5，为了用linq...)
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace HH.CS.Com
{
    /// <summary>
    /// DataHelper数据库操作类
    /// </summary>
    public sealed class DataHelper : IDisposable
    {
        #region 字段及属性

        private SqlConnection SqlConn = null;
        private SqlCommand SqlCmd = null;
        private SqlDataReader SqlDr = null;

        private OleDbConnection OleConn = null;
        private OleDbCommand OleCmd = null;
        private OleDbDataReader OleDr = null;

        private DataType DT;
        private string Conn;

        #endregion 字段及属性

        #region 枚举及构造函数

        public enum DataType
        {
            Oledb, Sqlclient
        }

        public DataHelper(DataType DType, string DataConn)
        {
            DT = DType;
            if (DT == DataType.Oledb)
            {
                OleConn = new OleDbConnection(DataConn);
            }
            else
            {
                SqlConn = new SqlConnection(DataConn);
            }
            Conn = DataConn;
        }

        #endregion 枚举及构造函数

        #region 私有方法

        private SqlConnection GetSqlConn()
        {
            if (SqlConn.State == ConnectionState.Closed)
            {
                SqlConn.Open();
            }
            return SqlConn;
        }

        private OleDbConnection GetOleConn()
        {
            if (OleConn.State == ConnectionState.Closed)
            {
                OleConn.Open();
            }
            return OleConn;
        }

        #endregion 私有方法

        #region 公共方法

        /// <summary>
        /// List转DataTable
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="list">List实体</param>
        /// <param name="tableName">表名（不写默认"dt"）</param>
        /// <returns>DataTable</returns>
        public static DataTable ListToDataTable<T>(List<T> list, string tableName = "dt")
        {
            //检查实体集合不能为空
            if (list == null || list.Count < 1)
            {
                return new DataTable();
            }
            //取出第一个实体的所有Propertie
            Type entityType = list[0].GetType();
            PropertyInfo[] entityProperties = entityType.GetProperties();
            //生成DataTable的structure
            //生产代码中，应将生成的DataTable结构Cache起来，此处略
            DataTable dt = new DataTable(tableName);
            for (int i = 0; i < entityProperties.Length; i++)
            {
                //dt.Columns.Add(entityProperties[i].Name, entityProperties[i].PropertyType);
                dt.Columns.Add(entityProperties[i].Name);
            }
            //将所有entity添加到DataTable中
            foreach (object entity in list)
            {
                //检查所有的的实体都为同一类型
                if (entity.GetType() != entityType)
                {
                    throw new Exception("要转换的集合元素类型不一致");
                }
                object[] entityValues = new object[entityProperties.Length];
                for (int i = 0; i < entityProperties.Length; i++)
                {
                    entityValues[i] = entityProperties[i].GetValue(entity, null);
                }
                dt.Rows.Add(entityValues);
            }
            return dt;
        }

        /// <summary>
        /// DataTable转List
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="dt">DataTable</param>
        /// <returns>List实体</returns>
        public static List<T> DataTableToList<T>(DataTable dt) where T : new()
        {
            if (dt == null || dt.Rows.Count <= 0)
                return null;
            List<T> list = new List<T>();
            Type type = typeof(T);
            PropertyInfo[] propertyInfos = type.GetProperties();  //获取泛型的属性
            List<DataColumn> listColumns = dt.Columns.Cast<DataColumn>().ToList();  //获取数据集的表头，以便于匹配
            foreach (DataRow dr in dt.Rows)
            {
                T t = new T();
                foreach (PropertyInfo propertyInfo in propertyInfos)
                {
                    try
                    {
                        DataColumn dColumn = listColumns.Find(name => name.ToString().ToUpper() == propertyInfo.Name.ToUpper());  //查看是否存在对应的列名
                        if (dColumn != null)
                            propertyInfo.SetValue(t, dr[propertyInfo.Name], null);  //赋值
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                list.Add(t);
            }
            return list;
        }

        /// <summary>
        /// 执行MSSQL不带参数的增删改SQL语句集合(针对于运算数据量大的查询)
        /// </summary>
        /// <param name="CmdText">查询SQL语句或存储过程名称</param>
        /// <param name="TimeOut">CommandTimeout时间</param>
        /// <param name="CT">选择是SQL语句还是存储过程</param>
        /// <returns></returns>
        public DataTable ExecuteMSSQLTimeOutQuery(string CmdText, int TimeOut, CommandType CT)
        {
            DataTable dt = new DataTable();
            SqlCmd = new SqlCommand(CmdText, GetSqlConn());
            SqlCmd.CommandType = CT;
            SqlCmd.CommandTimeout = TimeOut;
            using (SqlDr = SqlCmd.ExecuteReader(CommandBehavior.CloseConnection))
            {
                dt.Load(SqlDr);
            }
            if (SqlConn.State == ConnectionState.Open)
            {
                SqlConn.Close();
            }
            return dt;
        }

        public int ExecuteNonQuery(string CmdText)
        {
            return ExecuteNonQuery(CmdText, CommandType.Text);
        }

        /// <summary>
        /// 执行不带参数的增删改SQL语句或存储过程
        /// </summary>
        /// <param name="CmdText">增删改SQL语句或存储过程名称</param>
        /// <param name="CT">选择是SQL语句还是存储过程</param>
        /// <returns></returns>
        public int ExecuteNonQuery(string CmdText, CommandType CT)
        {
            int i = 0;
            if (DT == DataType.Oledb)
            {
                using (OleCmd = new OleDbCommand(CmdText, GetOleConn()))
                {
                    OleCmd.CommandType = CT;
                    i = OleCmd.ExecuteNonQuery();
                }
                if (OleConn.State == ConnectionState.Open)
                {
                    OleConn.Close();
                }
            }
            else
            {
                using (SqlCmd = new SqlCommand(CmdText, GetSqlConn()))
                {
                    SqlCmd.CommandType = CT;
                    i = SqlCmd.ExecuteNonQuery();
                }
                if (SqlConn.State == ConnectionState.Open)
                {
                    SqlConn.Close();
                }
            }
            return i;
        }

        public int ExecuteNonQuery(string CmdText, SqlParameter[] Paras)
        {
            return ExecuteNonQuery(CmdText, Paras, CommandType.Text);
        }

        /// <summary>
        /// SqlClient执行带参数的增删改SQL语句或存储过程
        /// </summary>
        /// <param name="CmdText">增删改SQL语句或存储过程名称</param>
        /// <param name="Paras">参数</param>
        /// <param name="CT">选择是SQL语句还是存储过程</param>
        /// <returns></returns>
        public int ExecuteNonQuery(string CmdText, SqlParameter[] Paras, CommandType CT)
        {
            int i;
            using (SqlCmd = new SqlCommand(CmdText, GetSqlConn()))
            {
                SqlCmd.CommandType = CT;
                SqlCmd.Parameters.AddRange(Paras);
                i = SqlCmd.ExecuteNonQuery();
            }
            if (SqlConn.State == ConnectionState.Open)
            {
                SqlConn.Close();
            }
            return i;
        }

        /// <summary>
        /// OleDb执行带参数的增删改SQL语句或存储过程
        /// </summary>
        /// <param name="CmdText">增删改SQL语句或存储过程名称</param>
        /// <param name="Paras">参数</param>
        /// <param name="CT">选择是SQL语句还是存储过程</param>
        /// <returns></returns>
        public int ExecuteNonQuery(string CmdText, OleDbParameter[] Paras, CommandType CT)
        {
            int i;
            using (OleCmd = new OleDbCommand(CmdText, GetOleConn()))
            {
                OleCmd.CommandType = CT;
                OleCmd.Parameters.AddRange(Paras);
                i = OleCmd.ExecuteNonQuery();
            }
            if (OleConn.State == ConnectionState.Open)
            {
                OleConn.Close();
            }
            return i;
        }

        /// <summary>
        /// 执行有事物的带参数的list列表命令（out 每一条执行情况，避免触发器导致数量不准），命令和参数个数不一致返回false。
        /// 2016-11-24黄昊新增方法
        /// </summary>
        /// <param name="listCmdText">命令</param>
        /// <param name="listParas">参数</param>
        /// <param name="CT">类型</param>
        /// <param name="listResult">out每条记录执行结果</param>
        /// <returns>执行无错误，返回true：命令个数=out count，每一条命令都有执行结果；命令个数！=out count，有命令没成功！</returns>
        public bool ExecuteNonQuery(List<string> listCmdText, List<SqlParameter[]> listParas, CommandType CT, out List<int> listResult)
        {
            listResult = new List<int>();
            if (listCmdText.Count != listParas.Count)
            {
                return false;
            }
            SqlTransaction Trans = null;
            SqlConnection cn = GetSqlConn();
            SqlCmd = cn.CreateCommand();
            try
            {
                cn.Open();
                Trans = cn.BeginTransaction();
                for (int i = 0; i < listCmdText.Count; i++)
                {
                    SqlCmd.CommandType = CT;
                    SqlCmd.Parameters.AddRange(listParas[i]);
                    SqlCmd.CommandText = listCmdText[i];
                    SqlCmd.Transaction = Trans;
                    listResult.Add(SqlCmd.ExecuteNonQuery());
                }
                Trans.Commit();
                return true;
            }
            catch
            {
                Trans.Rollback();
                return false;
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
            }
        }

        /// <summary>
        /// 执行有事物的带参数的list列表命令（out 每一条执行情况，避免触发器导致数量不准），命令和参数个数不一致返回false。
        /// 2016-11-24黄昊新增方法
        /// </summary>
        /// <param name="listCmdText">命令</param>
        /// <param name="listParas">参数</param>
        /// <param name="CT">类型</param>
        /// <param name="listResult">out每条记录执行结果</param>
        /// <returns>执行无错误，返回true：命令个数=out count，每一条命令都有执行结果；命令个数！=out count，有命令没成功！</returns>
        public bool ExecuteNonQuery(List<string> listCmdText, List<OleDbParameter[]> listParas, CommandType CT, out List<int> listResult)
        {
            listResult = new List<int>();
            if (listCmdText.Count != listParas.Count)
            {
                return false;
            }
            OleDbTransaction Trans = null;
            OleDbConnection cn = GetOleConn();
            OleCmd = cn.CreateCommand();
            try
            {
                cn.Open();
                Trans = cn.BeginTransaction();
                for (int i = 0; i < listCmdText.Count; i++)
                {
                    OleCmd.CommandType = CT;
                    OleCmd.Parameters.AddRange(listParas[i]);
                    OleCmd.CommandText = listCmdText[i].ToString();
                    OleCmd.Transaction = Trans;
                    listResult.Add(OleCmd.ExecuteNonQuery());
                }
                Trans.Commit();
                return true;
            }
            catch
            {
                Trans.Rollback();
                return false;
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
            }
        }

        /// <summary>
        /// 执行有事物的不带参数的增删改SQL语句集合
        /// </summary>
        /// <param name="CmdText">(SQL)语句集合</param>
        /// <returns></returns>
        public bool ExecuteNonQuery(ArrayList CmdText)
        {
            if (DT == DataType.Sqlclient)//MS SQL
            {
                SqlTransaction Trans = null;
                SqlConnection cn = new SqlConnection(Conn);
                SqlCmd = cn.CreateCommand();
                try
                {
                    cn.Open();
                    Trans = cn.BeginTransaction();
                    for (int i = 0; i < CmdText.Count; i++)
                    {
                        SqlCmd.CommandText = CmdText[i].ToString();
                        SqlCmd.Transaction = Trans;
                        SqlCmd.ExecuteNonQuery();
                    }
                    Trans.Commit();
                    return true;
                }
                catch
                {
                    Trans.Rollback();
                    return false;
                }
                finally
                {
                    if (cn.State == ConnectionState.Open)
                    {
                        cn.Close();
                    }
                }
            }
            else
            {
                OleDbTransaction Trans = null;
                OleDbConnection cn = new OleDbConnection(Conn);
                OleCmd = cn.CreateCommand();
                try
                {
                    cn.Open();
                    Trans = cn.BeginTransaction();
                    for (int i = 0; i < CmdText.Count; i++)
                    {
                        OleCmd.CommandText = CmdText[i].ToString();
                        OleCmd.Transaction = Trans;
                        OleCmd.ExecuteNonQuery();
                    }
                    Trans.Commit();
                    return true;
                }
                catch
                {
                    Trans.Rollback();
                    return false;
                }
                finally
                {
                    if (cn.State == ConnectionState.Open)
                    {
                        cn.Close();
                    }
                }
            }
        }

        public DataTable ExecuteQuery(string CmdText)
        {
            return ExecuteQuery(CmdText, CommandType.Text);
        }

        /// <summary>
        /// 查询不带参数的SQL语句或存储过程(返回DataTable)
        /// </summary>
        /// <param name="CmdText">查询SQL语句或存储过程名称</param>
        /// <param name="CT">选择是SQL语句还是存储过程</param>
        /// <returns></returns>
        public DataTable ExecuteQuery(string CmdText, CommandType CT)
        {
            DataTable dt = new DataTable();
            if (DT == DataType.Oledb)
            {
                OleCmd = new OleDbCommand(CmdText, GetOleConn());
                OleCmd.CommandType = CT;
                using (OleDr = OleCmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    dt.Load(OleDr);
                }
                if (OleConn.State == ConnectionState.Open)
                {
                    OleConn.Close();
                }
            }
            else
            {
                SqlCmd = new SqlCommand(CmdText, GetSqlConn());
                SqlCmd.CommandType = CT;
                using (SqlDr = SqlCmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    dt.Load(SqlDr);
                }
                if (SqlConn.State == ConnectionState.Open)
                {
                    SqlConn.Close();
                }
            }

            return dt;
        }

        /// <summary>
        /// 查询不带参数的SQL语句或存储过程(返回DataTable)
        /// </summary>
        /// <param name="CmdText">查询SQL语句或存储过程名称</param>
        /// <param name="CT">选择是SQL语句还是存储过程</param>
        /// <returns></returns>
        public DataTable ExecuteQuery(string CmdText, CommandType CT, string TableName)
        {
            DataTable dt = new DataTable();
            if (DT == DataType.Oledb)
            {
                OleCmd = new OleDbCommand(CmdText, GetOleConn());
                OleCmd.CommandType = CT;
                using (OleDr = OleCmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    dt.Load(OleDr);
                    dt.TableName = TableName;
                }
                if (OleConn.State == ConnectionState.Open)
                {
                    OleConn.Close();
                }
            }
            else
            {
                SqlCmd = new SqlCommand(CmdText, GetSqlConn());
                SqlCmd.CommandType = CT;
                using (SqlDr = SqlCmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    dt.Load(SqlDr);
                    dt.TableName = TableName;
                }
                if (SqlConn.State == ConnectionState.Open)
                {
                    SqlConn.Close();
                }
            }
            return dt;
        }

        public DataTable ExecuteQuery(string CmdText, SqlParameter[] Paras)
        {
            return ExecuteQuery(CmdText, Paras, CommandType.Text);
        }

        /// <summary>
        /// SqlClient查询带参数的SQL语句或存储过程(返回DataTable)
        /// </summary>
        /// <param name="CmdText">查询SQL语句或存储过程名称</param>
        /// <param name="Paras">参数</param>
        /// <param name="CT">选择是SQL语句还是存储过程</param>
        /// <returns></returns>
        public DataTable ExecuteQuery(string CmdText, SqlParameter[] Paras, CommandType CT)
        {
            DataTable dt = new DataTable();
            SqlCmd = new SqlCommand(CmdText, GetSqlConn());
            SqlCmd.CommandType = CT;
            SqlCmd.Parameters.AddRange(Paras);
            using (SqlDr = SqlCmd.ExecuteReader(CommandBehavior.CloseConnection))
            {
                dt.Load(SqlDr);
            }
            if (SqlConn.State == ConnectionState.Open)
            {
                SqlConn.Close();
            }
            return dt;
        }

        /// <summary>
        /// OleDb查询带参数的SQL语句或存储过程(返回DataTable)
        /// </summary>
        /// <param name="CmdText">查询SQL语句或存储过程名称</param>
        /// <param name="Paras">参数</param>
        /// <param name="CT">选择是SQL语句还是存储过程</param>
        /// <returns></returns>
        public DataTable ExecuteQuery(string CmdText, OleDbParameter[] Paras, CommandType CT)
        {
            DataTable dt = new DataTable();
            OleCmd = new OleDbCommand(CmdText, GetOleConn());
            OleCmd.CommandType = CT;
            OleCmd.Parameters.AddRange(Paras);
            using (OleDr = OleCmd.ExecuteReader(CommandBehavior.CloseConnection))
            {
                dt.Load(OleDr);
            }
            if (OleConn.State == ConnectionState.Open)
            {
                OleConn.Close();
            }
            return dt;
        }

        #endregion 公共方法

        #region IDisposable Support

        private bool disposedValue = false; // 要检测冗余调用

        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (SqlConn != null && SqlConn.State == ConnectionState.Open)
                    {
                        SqlConn.Close();
                        SqlConn.Dispose();
                    }
                    if (OleConn != null && OleConn.State == ConnectionState.Open)
                    {
                        OleConn.Close();
                        OleConn.Dispose();
                    }
                }

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。

                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~DataHelper() {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            // GC.SuppressFinalize(this);
        }

        #endregion IDisposable Support
    }
}