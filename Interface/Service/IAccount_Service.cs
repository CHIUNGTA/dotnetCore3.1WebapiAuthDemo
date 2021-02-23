using Interface.Autofac;
using Model.SqlServerInfo;
using Model.ViewModel;
using System.Collections.Generic;

namespace Interface.Service
{
    public interface IAccount_Service: IAutoInject
    {
        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="acc"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        ResponseModel Login(string acc, string password);
        /// <summary>
        /// 註冊
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        ResponseModel Singup(User user); 
        /// <summary>
        /// 取得所有資料(測試用)
        /// </summary>
        /// <returns></returns>
        List<User> GetAll();
        /// <summary>
        /// 依xx撈會員資料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        User GetById(int id); 

    }
}
