using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Models
{
    public class Result
    {
        public bool IsSuccess { get; }
        public string? ErrorMessage { get; }

        protected Result(bool isSuccess, string? errorMessage = null)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
        }

        public static Result Success() => new Result(true);

        public static Result Failure(string errorMessage) => new Result(false, errorMessage);
    }

    public class Result<T> : Result
    {
        public T Value { get; }

        protected Result(bool isSuccess, T value, string? errorMessage = null) 
            : base(isSuccess, errorMessage)
        {
            Value = value;
        }

        public static Result<T> Success(T value) => new Result<T>(true, value);

        public static new Result<T> Failure(string errorMessage) => new Result<T>(false, default!, errorMessage);
    }
}
