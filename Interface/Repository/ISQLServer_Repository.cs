using Interface.Autofac;
using System;
using System.Collections.Generic;
using System.Text;

namespace Interface.Repository
{
    public interface ISQLServer_Repository: IAutoInject
    {
        List<T> Query<T>(string sql, object param = null);
        bool IsExist(string sql, object param);
        void Execute(string sql, object param);
        /// <summary>
        /// 簡單依參數取得資料
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="param"></param>
        /// <returns></returns>
        List<T> GetDataByParam<T>(object param=null);
        int? Insert<T>(object param);
    }
}
