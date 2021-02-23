using Interface.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Dapper;
using System.Text;
using Model.SqlServerInfo;

namespace Repository.SqlserverRepository
{
    public class SQLServer_Repository : ISQLServer_Repository
    {
        private SqlConnection _sqlConn = null; 
        public SQLServer_Repository(SqlConnection sqlConn)
        {
            if(this._sqlConn == null)
            {
                this._sqlConn = sqlConn; 
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        public void Execute(string sql, object param)
        {
            try
            {
                using (SqlConnection cn = this._sqlConn)
                {
                    cn.Execute(sql, param);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("SQLServer_DataObject Query Error:{0}", ex.Message));
            }
        }
        /// <summary>
        /// 依參數取得資料
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="param"></param>
        /// <returns></returns>
        public List<T> GetDataByParam<T>(object param)
        {
            try
            {
                using (SqlConnection cn = this._sqlConn)
                {
                    return cn.GetList<T>(param).AsList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"SQLServer_DataObject Query Error:${ex.Message }"); 
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="param"></param>
        /// <returns></returns>
        public int? Insert<T>(object param)
        {
            try
            {
                using (SqlConnection cn = this._sqlConn)
                {
                    return cn.Insert<T>((T)param);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"SQLServer_DataObject Insert() Error:${ex.Message }");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public bool IsExist(string sql, object param)
        {
            try
            {
                using (SqlConnection cn = this._sqlConn)
                {
                    return cn.ExecuteScalar<int>(sql, param) > 0; 
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("SQLServer_DataObject Query Error:{0}", ex.Message));
            }
        }

        public List<T> Query<T>(string sql, object param)
        {
            try
            {
                using (SqlConnection cn = this._sqlConn)
                {
                    return cn.Query<T>(sql, param).AsList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("SQLServer_DataObject Query Error:{0}", ex.Message));
            }
        }
    }
}
