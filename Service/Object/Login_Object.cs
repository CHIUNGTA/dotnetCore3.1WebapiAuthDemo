using Interface.Repository;
using Interface.Service;
using Model.SqlServerInfo;
using Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Service.Object
{
    public class Login_Service : IAccount_Service
    {
        private ISQLServer_Repository _db = null;
        public Login_Service(ISQLServer_Repository db)
        {
            this._db = db; 
        }

        public ResponseModel<User> Login(string loginName, string password)
        {
            string sql = @"select * from User Where LoginName = @loginName and LoginPassword = @loginPassword ; ";
            object param = new
            {
                loginName = loginName,
                loginPassword = password
            };
            return new ResponseModel<User>()
            {
                Data = _db.Query<User>(sql, param).FirstOrDefault() ,
                Message = string.Empty , 
                StatsuCode = 200
            };
        }

        public List<User> GetAll()
        {
            return _db.GetDataByParam<User>();
        }

        public User GetById(int id)
        {
            return _db.GetDataByParam<User>(new { UserId = id }).FirstOrDefault();
        }

        public ResponseModel<string> Singup(User user)
        {
            try
            {
                bool status = _db.Insert<User>(user) != 0;
                if (status)
                    return new ResponseModel<string>() { StatsuCode = 200, Data = null, Message = "成功註冊，請用手機驗證" };
                return new ResponseModel<string>() { StatsuCode = 200, Data = null, Message = "註冊失敗" };

            }
            catch (Exception ex)
            {
                return new ResponseModel<string>() { StatsuCode = 500, Message = "發生不可預期之錯誤" };
            }
        }
    }
}
