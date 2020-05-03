using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace KalWeb.Models
{
    public class ResponseMessage
    {
        public string Type { get; set; }
        public bool IsSuccess { get; set; }
        public dynamic Data { get; set; }
        public string Message { get; set; }
        public HttpStatusCode StatusCode { get; set; }

        public ResponseMessage() { }

        public ResponseMessage(ResponseType type, bool isSuccess, dynamic data, string msg, HttpStatusCode statusCode)
        {
            Type = type.ToString();
            IsSuccess = isSuccess;
            Data = data;
            Message = msg;
            StatusCode = statusCode;
        }
    }
    public class Default
    {
        public bool IsCache { get; set; }
    }
    public enum ResponseType
    {
        ERROR,
        SUCCESS,
        EXCEPTION
    }


    public class ResponseFormat
    {
        public string Type { get; set; }
        public bool IsSuccess { get; set; }
        public dynamic Data { get; set; }
        public string Message { get; set; }
        public HttpStatusCode StatusCode { get; set; }

        public ResponseFormat(ResponseType type, bool isSuccess, dynamic data, string msg)
        {
            Type = type.ToString();
            IsSuccess = isSuccess;
            Data = data;
            Message = msg;
        }
    }
}