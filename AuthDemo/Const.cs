using System.Collections.Generic;

namespace AuthDemo
{
    public static class Const
    {
        
        /// <summary>
        /// 受理人，模擬強制Token失效
        /// </summary>
        public static string ValidAudience;

        /// <summary>
        /// 受理人，在建立資料庫前先透過靜態檔案的型式處理token資料，避免重複登入
        /// 未來可將其存至redis中
        /// </summary>
        //public static List<ValidAudienceData> ValidAudienceList ;
        public static Dictionary<string,string> ValidAudienceList ;
    }

    //public class ValidAudienceData 
    //{
    //    public string AccId { get; set; }
    //    public string ValidAudience { get; set; }
    //}

}
