using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace API.Errors
{
    public class ApiException
    {
        public int StatucCode { get; set; }
        public string Message { get; set; }
        public string Details { get; set; }
        
        public enum Errors
        {
            InvalidFridgeId,
            ProductNotFound
        }
        public ApiException(int statucCode, string message = null, string details = null)
        {
            StatucCode = statucCode;
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