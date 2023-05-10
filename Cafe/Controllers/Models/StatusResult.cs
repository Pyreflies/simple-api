using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cafe.Models.ReadModels
{
    public class StatusResult<T>
    {
        public bool isSuccess { get; set; }
        public string statusDesc { get; set; }
        public int statusCode { get; set; }
        public string message { get; set; }
        public DateTime timeStamp { get; set; }
        public List<T> Values { get; set; }

        public StatusResult(string imessage, int status, bool success, List<T> value) 
        {
            message = imessage;
            statusCode = status;
            isSuccess = success;
            Values = value;
            timeStamp = DateTime.UtcNow;
        }

    }




}