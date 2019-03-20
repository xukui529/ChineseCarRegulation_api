using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace CustomCommon
{
    public class SqlHelper
    {
        public static string connstr = ConfigurationManager.ConnectionStrings["NFineDbContext"].ConnectionString;

        #region 建立数据库连接对象

        /// <summary>
        ///     建立数据库连接
        /// </summary>
        /// <returns>返回一个数据库的连接SqlConnection对象</returns>
        public static SqlConnection init()
        {
            var conn = new SqlConnection(connstr);
            conn.Open();
            return conn;
        }

        #endregion

        #region 设置SqlCommand对象

        /// <summary>
        ///     设置SqlCommand对象
        /// </summary>
        /// <param name="cmd">SqlCommand对象 </param>
        /// <param name="cmdText">命令文本</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cmdParms">参数集合</param>
        private static void SetCommand(SqlCommand cmd, string cmdText, CommandType cmdType, SqlParameter[] cmdParms, SqlConnection conn)
        {
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            cmd.CommandType = cmdType;
            if (cmdParms != null)
            {
                cmd.Parameters.AddRange(cmdParms);
            }
        }

        #endregion

        #region 执行相应的sql语句，返回相应的DataSet对象

        /// <summary>
        ///     执行相应的sql语句，返回相应的DataSet对象
        /// </summary>
        /// <param name="sqlstr">sql语句</param>
        /// <returns>返回相应的DataSet对象</returns>
        public static DataSet GetDataSet(string sqlstr)
        {
            var ds = new DataSet();
            try
            {
                using (var conn = init())
                {
                    var ada = new SqlDataAdapter(sqlstr, conn);
                    ada.Fill(ds);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return ds;
        }

        #endregion

        #region 执行相应的sql语句，返回相应的DataSet对象

        /// <summary>
        ///     执行相应的sql语句，返回相应的DataSet对象
        /// </summary>
        /// <param name="sqlstr">sql语句</param>
        /// <param name="tableName">表名</param>
        /// <returns>返回相应的DataSet对象</returns>
        public static DataSet GetDataSet(string sqlstr, string tableName)
        {
            var ds = new DataSet();
            try
            {
                using (var conn = init())
                {
                    var ada = new SqlDataAdapter(sqlstr, conn);
                    ada.Fill(ds, tableName);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return ds;
        }

        #endregion

        #region 执行不带参数sql语句，返回一个DataTable对象

        /// <summary>
        ///     执行不带参数sql语句，返回一个DataTable对象
        /// </summary>
        /// <param name="cmdText">相应的sql语句</param>
        /// <returns>返回一个DataTable对象</returns>
        public static DataTable GetDataTable(string cmdText)
        {
            var dt = new DataTable();
            try
            {
                using (var conn = init())
                using (var cmd = new SqlCommand(cmdText, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    dt.Load(reader);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return dt;
        }

        #endregion

        #region 执行带参数的sql语句或存储过程，返回一个DataTable对象

        /// <summary>
        ///     执行带参数的sql语句或存储过程，返回一个DataTable对象
        /// </summary>
        /// <param name="cmdText">sql语句或存储过程名</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cmdParms">参数集合</param>
        /// <returns>返回一个DataTable对象</returns>
        public static DataTable GetDataTable(string cmdText, CommandType cmdType, SqlParameter[] cmdParms)
        {
            var dt = new DataTable();
            try
            {
                using(var conn = init())
                using (var cmd = new SqlCommand())
                {
                    SetCommand(cmd, cmdText, cmdType, cmdParms, conn);
                    using (var reader = cmd.ExecuteReader())
                        dt.Load(reader);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return dt;
        }

        #endregion

        #region 执行不带参数sql语句，返回所影响的行数

        /// <summary>
        ///     执行不带参数sql语句，返回所影响的行数
        /// </summary>
        /// <param name="cmdText">增，删，改sql语句</param>
        /// <returns>返回所影响的行数</returns>
        public static int ExecuteNonQuery(string cmdText)
        {
            int count;
            try
            {
                using (var conn = init())
                using (var cmd = new SqlCommand(cmdText, conn))
                {
                    count = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return count;
        }

        #endregion

        #region 执行带参数sql语句或存储过程，返回所影响的行数

        /// <summary>
        ///     执行带参数sql语句或存储过程，返回所影响的行数
        /// </summary>
        /// <param name="cmdText">带参数的sql语句和存储过程名</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cmdParms">参数集合</param>
        /// <returns>返回所影响的行数</returns>
        public static int ExecuteNonQuery(string cmdText, CommandType cmdType, SqlParameter[] cmdParms)
        {
            int count;
            try
            {
                using (var conn = init())
                using (var cmd = new SqlCommand())
                {
                    SetCommand(cmd, cmdText, cmdType, cmdParms, conn);
                    count = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return count;
        }

        #endregion

        #region 执行不带参数sql语句，返回一个从数据源读取数据的SqlDataReader对象

        /// <summary>
        ///     执行不带参数sql语句，返回一个从数据源读取数据的SqlDataReader对象
        /// </summary>
        /// <param name="cmdText">相应的sql语句</param>
        /// <returns>返回一个从数据源读取数据的SqlDataReader对象</returns>
        public static SqlDataReader ExecuteReader(string cmdText)
        {
            SqlDataReader reader;
            try
            {
                var conn = init();
                var cmd = new SqlCommand(cmdText, conn);
                reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return reader;
        }

        #endregion

        #region 执行带参数的sql语句或存储过程，返回一个从数据源读取数据的SqlDataReader对象

        /// <summary>
        ///     执行带参数的sql语句或存储过程，返回一个从数据源读取数据的SqlDataReader对象
        /// </summary>
        /// <param name="cmdText">sql语句或存储过程名</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cmdParms">参数集合</param>
        /// <returns>返回一个从数据源读取数据的SqlDataReader对象</returns>
        public static SqlDataReader ExecuteReader(string cmdText, CommandType cmdType, SqlParameter[] cmdParms)
        {
            SqlDataReader reader;
            try
            {
                var conn = init();
                var cmd = new SqlCommand();
                SetCommand(cmd, cmdText, cmdType, cmdParms, conn);
                reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return reader;
        }

        #endregion

        #region 执行不带参数sql语句,返回结果集首行首列的值object

        /// <summary>
        ///     执行不带参数sql语句,返回结果集首行首列的值object
        /// </summary>
        /// <param name="cmdText">相应的sql语句</param>
        /// <returns>返回结果集首行首列的值object</returns>
        public static object ExecuteScalar(string cmdText)
        {
            object obj;
            try
            {
                using (var conn = init())
                using (var cmd = new SqlCommand(cmdText, conn))
                {
                    obj = cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return obj;
        }

        #endregion

        #region 执行带参数sql语句或存储过程,返回结果集首行首列的值object

        /// <summary>
        ///     执行带参数sql语句或存储过程,返回结果集首行首列的值object
        /// </summary>
        /// <param name="cmdText">sql语句或存储过程名</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cmdParms">返回结果集首行首列的值object</param>
        /// <returns></returns>
        public static object ExecuteScalar(string cmdText, CommandType cmdType, SqlParameter[] cmdParms)
        {
            object obj;
            try
            {
                using (var conn = init())
                using (var cmd = new SqlCommand())
                {
                    SetCommand(cmd, cmdText, cmdType, cmdParms, conn);
                    obj = cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return obj;
        }

        #endregion

        #region  ExecuteDataOfPage 执行分页sql方法 hjj 20160614
        /// <summary>
        /// 执行分页sql方法 hjj 20160614
        /// </summary>
        /// <param name="whereString">查询条件</param>
        /// <param name="Fields">查询字段</param>
        /// <param name="viewName">表名或者视图名称</param>
        /// <param name="sqlpageSize">页容</param>
        /// <param name="pageNo">当前页数</param>
        /// <param name="orderString">排序</param>
        /// <param name="count">返回总量</param>
        /// <returns></returns>
        public static DataSet ExecuteDataOfPage(string whereString, string Fields, string viewName, int sqlpageSize, int pageNo, string orderString, out int count,string db)
        {
            count = 0;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[db].ConnectionString))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandTimeout = 60;
                cmd.CommandType = CommandType.StoredProcedure; //指定执行存储过程操作
                cmd.CommandText = "P_GridViewPager"; //存储过程名称 
                //对应存储过程 表名或者视图名称
                SqlParameter parviewName = new SqlParameter("@viewName", SqlDbType.VarChar, 8000);
                parviewName.Value = viewName;

                SqlParameter parfieldName = new SqlParameter("@fieldName", SqlDbType.VarChar, 8000);
                parfieldName.Value = Fields;

                SqlParameter parpageSize = new SqlParameter("@pageSize", SqlDbType.Int, 8);
                parpageSize.Value = sqlpageSize;

                SqlParameter parpageNo = new SqlParameter("@pageNo", SqlDbType.Int, 8);
                parpageNo.Value = pageNo;

                SqlParameter parorderString = new SqlParameter("@orderString", SqlDbType.VarChar, 8000);
                parorderString.Value = orderString;

                SqlParameter parwhereString = new SqlParameter("@whereString", SqlDbType.VarChar, 8000);
                parwhereString.Value = whereString;

                SqlParameter parrecordTotal = new SqlParameter("@recordTotal", SqlDbType.Int, 8);
                parrecordTotal.Direction = ParameterDirection.Output; ;

                cmd.Parameters.Add(parviewName);
                cmd.Parameters.Add(parfieldName);
                cmd.Parameters.Add(parpageSize);
                cmd.Parameters.Add(parpageNo);
                cmd.Parameters.Add(parorderString);
                cmd.Parameters.Add(parwhereString);
                cmd.Parameters.Add(parrecordTotal);

                DataSet ds = new DataSet();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(ds);
                count = Convert.ToInt32(parrecordTotal.Value);
                return ds;
            }
        }
        #endregion

        #region  ExecuteDataOfPage 执行分页sql方法 hjj 20160614
        /// <summary>
        /// 执行分页sql方法 dc 2016/7/12
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="fields">查询字段</param>
        /// <param name="sqlWhere">查询条件</param>
        /// <param name="groupField">分组</param>
        /// <param name="orderField">排序</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="pageSize">页容</param>
        /// <param name="totalRecord">返回总量</param>
        /// <returns></returns>
        public static DataSet ExecuteCommon_Page(string tableName, string fields, string sqlWhere, string groupField, string orderField, int pageIndex, int pageSize, out int totalRecord, string db)
        {
            totalRecord = 0;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[db].ConnectionString))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure; //指定执行存储过程操作
                cmd.CommandText = "Common_Paging"; //存储过程名称 
                //对应存储过程 表名或者视图名称
                SqlParameter parviewName = new SqlParameter("@tableName", SqlDbType.VarChar, 8000);
                parviewName.Value = tableName;

                SqlParameter parfieldName = new SqlParameter("@fields", SqlDbType.VarChar, 8000);
                parfieldName.Value = fields;

                SqlParameter parpageSize = new SqlParameter("@pageSize", SqlDbType.Int, 8);
                parpageSize.Value = pageSize;

                SqlParameter parpageNo = new SqlParameter("@pageIndex", SqlDbType.Int, 8);
                parpageNo.Value = pageIndex;

                SqlParameter parorderString = new SqlParameter("@orderField", SqlDbType.VarChar, 8000);
                parorderString.Value = orderField;

                SqlParameter parwhereString = new SqlParameter("@sqlWhere", SqlDbType.VarChar, 8000);
                parwhereString.Value = sqlWhere;

                SqlParameter pargroupString = new SqlParameter("@groupField", SqlDbType.VarChar, 8000);
                pargroupString.Value = groupField;

                SqlParameter parrecordTotal = new SqlParameter("@totalRecord", SqlDbType.Int, 8);
                parrecordTotal.Direction = ParameterDirection.Output; ;

                cmd.Parameters.Add(parviewName);
                cmd.Parameters.Add(parfieldName);
                cmd.Parameters.Add(parpageSize);
                cmd.Parameters.Add(parpageNo);
                cmd.Parameters.Add(parorderString);
                cmd.Parameters.Add(parwhereString);
                cmd.Parameters.Add(pargroupString);
                cmd.Parameters.Add(parrecordTotal);

                DataSet ds = new DataSet();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(ds);
                totalRecord = Convert.ToInt32(parrecordTotal.Value);
                return ds;
            }
        }
        #endregion

        public static DataSet ExcuteDataSet(string sqlstr, CommandType type, SqlParameter[] prams, string db)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[db].ConnectionString))
            {
                DataSet ds = new DataSet();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlstr;
                cmd.CommandType = type;
                cmd.Parameters.AddRange(prams);
                cmd.CommandTimeout = 3600;  
                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    adapter.Fill(ds);
                }
                cmd.Dispose();
                conn.Close();
                conn.Dispose();
                return ds;
            }
        }
       
    }
}