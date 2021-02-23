using Dapper;
using System;

namespace Model.SqlServerInfo
{
    /// <summary>
    /// 使用者表表
    /// </summary>
    [Table("User")]
    public class User
    {
        /// <summary>
        /// 使用流水號
        /// </summary>
        [Key]
        [IgnoreInsert]
        [IgnoreUpdate]
        [Column("UserId")]
        public int UserId { get; set; }
        /// <summary>
        /// 使用者登入帳號
        /// </summary>
        [Column("LoginName")]
        public string LoginName { get; set; }
        /// <summary>
        /// 使用者登入密碼
        /// </summary>
        [Column("LoginPassword")]
        public string LoginPassword { get; set; }
        /// <summary>
        /// 信箱
        /// </summary>
        [Column("Email")]
        public string Email { get; set; }
        /// <summary>
        /// 使用者名稱
        /// </summary>
        [Column("Name")]
        public string Name { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        [Column("Birthday")]
        public DateTime Birthday { get; set; }
        /// <summary>
        /// FirebaseToken
        /// </summary>
        [Column("FirebaseToken")]
        public string FirebaseToken { get; set; }
        /// <summary>
        /// 電話
        /// </summary>
        [Column("Phone")]
        public string Phone { get; set; }
    }
}
