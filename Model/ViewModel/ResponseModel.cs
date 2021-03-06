﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Model.ViewModel
{
    /// <summary>
    /// API統一回應模型
    /// </summary>
    public class ResponseModel<T>
    {
        /// <summary>
        /// 
        /// </summary>
        public int StatsuCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Response Body
        /// </summary>
        public T Data { get; set; }
    }
}
