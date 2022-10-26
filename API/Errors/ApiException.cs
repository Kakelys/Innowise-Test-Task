using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace API.Errors
{
    public class ApiException : Exception
    {
        public int StatusCode { get; set; }
        public new string Message { get; set; }
        public string Details { get; set; }
        
        public enum Errors
        {
            InvalidFridgeId,
            ProductNotFound
        }
        public ApiException(int statusCode, string message = null, string details = null)
        {
            StatusCode = statusCode;
            Message = message;
            Details = details;
        }

        public static string GetErrorMessage(Errors error)
        {
            switch (error)
            {
                case Errors.InvalidFridgeId: 
                    return "Invalid fridge id";
                case Errors.ProductNotFound: 
                    return "Product not found";
                default:
                    return "Something going wrong";
            }
        }
    }
}