/*˵��
 * ��   ��:���
 * �ļ���:DataHelper.cs
 * ��   ��:һ���������ݿ�Ļ��������֧࣬��Data.OleDb��Data.SqlClinet�����ռ��µ����ݿ�
 * ժ   Ҫ:����ִ��SQL��䣨ִ��SQL���ʹ�ò��������񣩣�ִ�д洢���̣�����DataTable
 * ������:DataHelper
 * ��   ��:V 1.0
 * ��   ��:.NET 2.0(�Ѿ��޸�Ϊ3.5��Ϊ����linq...)
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
    /// DataHelper���ݿ������
    /// </summary>
    public sealed class DataHelper : IDisposable
    {
        #region �ֶμ�����

        private SqlConnection SqlConn = null;
        private SqlCommand SqlCmd = null;
        private SqlDataReader SqlDr = null;

        private OleDbConnection OleConn = null;
        private OleDbCommand OleCmd = null;
        private OleDbDataReader OleDr = null;

        private DataType DT;
        private string Conn;

        #endregion �ֶμ�����

        #region ö�ټ����캯��

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

        #endregion ö�ټ����캯��

        #region ˽�з���

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

        #endregion ˽�з���

        #region ��������

        /// <summary>
        /// ListתDataTable
        /// </summary>
        /// <typeparam name="T">ʵ��</typeparam>
        /// <param name="list">Listʵ��</param>
        /// <param name="tableName">��������дĬ��"dt"��</param>
        /// <returns>DataTable</returns>
        public static DataTable ListToDataTable<T>(List<T> list, string tableName = "dt")
        {
            //���ʵ�弯�ϲ���Ϊ��
            if (list == null || list.Count < 1)
            {
                return new DataTable();
            }
            //ȡ����һ��ʵ�������Propertie
            Type entityType = list[0].GetType();
            PropertyInfo[] entityProperties = entityType.GetProperties();
            //����DataTable��structure
            //���������У�Ӧ�����ɵ�DataTable�ṹCache�������˴���
            DataTable dt = new DataTable(tableName);
            for (int i = 0; i < entityProperties.Length; i++)
            {
                //dt.Columns.Add(entityProperties[i].Name, entityProperties[i].PropertyType);
                dt.Columns.Add(entityProperties[i].Name);
            }
            //������entity��ӵ�DataTable��
            foreach (object entity in list)
            {
                //������еĵ�ʵ�嶼Ϊͬһ����
                if (entity.GetType() != entityType)
                {
                    throw new Exception("Ҫת���ļ���Ԫ�����Ͳ�һ��");
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
        /// DataTableתList
        /// </summary>
        /// <typeparam name="T">ʵ��</typeparam>
        /// <param name="dt">DataTable</param>
        /// <returns>Listʵ��</returns>
        public static List<T> DataTableToList<T>(DataTable dt) where T : new()
        {
            if (dt == null || dt.Rows.Count <= 0)
                return null;
            List<T> list = new List<T>();
            Type type = typeof(T);
            PropertyInfo[] propertyInfos = type.GetProperties();  //��ȡ���͵�����
            List<DataColumn> listColumns = dt.Columns.Cast<DataColumn>().ToList();  //��ȡ���ݼ��ı�ͷ���Ա���ƥ��
            foreach (DataRow dr in dt.Rows)
            {
                T t = new T();
                foreach (PropertyInfo propertyInfo in propertyInfos)
                {
                    try
                    {
                        DataColumn dColumn = listColumns.Find(name => name.ToString().ToUpper() == propertyInfo.Name.ToUpper());  //�鿴�Ƿ���ڶ�Ӧ������
                        if (dColumn != null)
                            propertyInfo.SetValue(t, dr[propertyInfo.Name], null);  //��ֵ
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
        /// ִ��MSSQL������������ɾ��SQL��伯��(�����������������Ĳ�ѯ)
        /// </summary>
        /// <param name="CmdText">��ѯSQL����洢��������</param>
        /// <param name="TimeOut">CommandTimeoutʱ��</param>
        /// <param name="CT">ѡ����SQL��仹�Ǵ洢����</param>
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
        /// ִ�в�����������ɾ��SQL����洢����
        /// </summary>
        /// <param name="CmdText">��ɾ��SQL����洢��������</param>
        /// <param name="CT">ѡ����SQL��仹�Ǵ洢����</param>
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
        /// SqlClientִ�д���������ɾ��SQL����洢����
        /// </summary>
        /// <param name="CmdText">��ɾ��SQL����洢��������</param>
        /// <param name="Paras">����</param>
        /// <param name="CT">ѡ����SQL��仹�Ǵ洢����</param>
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
        /// OleDbִ�д���������ɾ��SQL����洢����
        /// </summary>
        /// <param name="CmdText">��ɾ��SQL����洢��������</param>
        /// <param name="Paras">����</param>
        /// <param name="CT">ѡ����SQL��仹�Ǵ洢����</param>
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
        /// ִ��������Ĵ�������list�б����out ÿһ��ִ����������ⴥ��������������׼��������Ͳ���������һ�·���false��
        /// 2016-11-24�����������
        /// </summary>
        /// <param name="listCmdText">����</param>
        /// <param name="listParas">����</param>
        /// <param name="CT">����</param>
        /// <param name="listResult">outÿ����¼ִ�н��</param>
        /// <returns>ִ���޴��󣬷���true���������=out count��ÿһ�������ִ�н�������������=out count��������û�ɹ���</returns>
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
        /// ִ��������Ĵ�������list�б����out ÿһ��ִ����������ⴥ��������������׼��������Ͳ���������һ�·���false��
        /// 2016-11-24�����������
        /// </summary>
        /// <param name="listCmdText">����</param>
        /// <param name="listParas">����</param>
        /// <param name="CT">����</param>
        /// <param name="listResult">outÿ����¼ִ�н��</param>
        /// <returns>ִ���޴��󣬷���true���������=out count��ÿһ�������ִ�н�������������=out count��������û�ɹ���</returns>
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
        /// ִ��������Ĳ�����������ɾ��SQL��伯��
        /// </summary>
        /// <param name="CmdText">(SQL)��伯��</param>
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
        /// ��ѯ����������SQL����洢����(����DataTable)
        /// </summary>
        /// <param name="CmdText">��ѯSQL����洢��������</param>
        /// <param name="CT">ѡ����SQL��仹�Ǵ洢����</param>
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
        /// ��ѯ����������SQL����洢����(����DataTable)
        /// </summary>
        /// <param name="CmdText">��ѯSQL����洢��������</param>
        /// <param name="CT">ѡ����SQL��仹�Ǵ洢����</param>
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
        /// SqlClient��ѯ��������SQL����洢����(����DataTable)
        /// </summary>
        /// <param name="CmdText">��ѯSQL����洢��������</param>
        /// <param name="Paras">����</param>
        /// <param name="CT">ѡ����SQL��仹�Ǵ洢����</param>
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
        /// OleDb��ѯ��������SQL����洢����(����DataTable)
        /// </summary>
        /// <param name="CmdText">��ѯSQL����洢��������</param>
        /// <param name="Paras">����</param>
        /// <param name="CT">ѡ����SQL��仹�Ǵ洢����</param>
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

        #endregion ��������

        #region IDisposable Support

        private bool disposedValue = false; // Ҫ����������

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

                // TODO: �ͷ�δ�йܵ���Դ(δ�йܵĶ���)������������������ս�����
                // TODO: �������ֶ�����Ϊ null��

                disposedValue = true;
            }
        }

        // TODO: �������� Dispose(bool disposing) ӵ�������ͷ�δ�й���Դ�Ĵ���ʱ������ս�����
        // ~DataHelper() {
        //   // ������Ĵ˴��롣���������������� Dispose(bool disposing) �С�
        //   Dispose(false);
        // }

        // ��Ӵ˴�������ȷʵ�ֿɴ���ģʽ��
        public void Dispose()
        {
            // ������Ĵ˴��롣���������������� Dispose(bool disposing) �С�
            Dispose(true);
            // TODO: ���������������������ս�������ȡ��ע�������С�
            // GC.SuppressFinalize(this);
        }

        #endregion IDisposable Support
    }
}