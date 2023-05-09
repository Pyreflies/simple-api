using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cafe.Models.ReadModels
{
	public abstract class StatusResultBase
	{
		public bool IsSuccess;
		public int StatusCode;
		public IComparable StatusCodeDecimal;
		public string Message;
		public DateTime StatusDateTime;
	}
	public class StatusResult : StatusResultBase 
	{
		public StatusResult()
		{
			StatusDateTime = DateTime.Now;
		}

		public StatusResult(string message, int status) : this()
		{
			Message = message;
			StatusCode = status;
		}

		public StatusResult(string message, IComparable status, bool success) : this()
		{
			Message = message;
			StatusCodeDecimal = status;
			IsSuccess = success;
		}

		public static StatusResult Ok()
		{
			return new StatusResult("", 200, true);
		}

		public static StatusResult<T> Ok <T>(T value)
		{
			return new StatusResult<T>("", 200, true, value);
		}

		public static StatusResult Error(string message)
		{
			//NOTE: 505 คือ เกิดข้อผิดพลาดจากการตรวจสอบของ api
			return new StatusResult(message, 505, false);
		}

		public static StatusResult Unauthorized(string message = "")
		{
			return new StatusResult(message, 401, false);
		}

		public static StatusResult Forbidden(string message = "")
		{
			return new StatusResult(message, 403, false);
		}
	}

	public class StatusResult <TResult> : StatusResultBase
	{
		public StatusResult()
		{
			StatusDateTime = DateTime.Now;
		}

		public StatusResult(TResult value, bool success) : this("", 200, success, value)
		{
		}

		public StatusResult(string message, int status, bool success, TResult value) : this()
		{
			Message = message;
			StatusCode = status;
			IsSuccess = success;
			Value = value;
		}

		public TResult Value { get; set; }

		public static implicit operator StatusResult<TResult>(StatusResult result)
		{
			return new StatusResult<TResult>(
				result.Message, result.StatusCode, result.IsSuccess, default);
		}

		public static implicit operator StatusResult(StatusResult<TResult> result)
		{
			return new StatusResult(result.Message, result.StatusCode, result.IsSuccess);
		}
	}
}