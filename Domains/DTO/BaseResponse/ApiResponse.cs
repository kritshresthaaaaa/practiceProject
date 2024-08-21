using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.DTO.BaseResponse
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public ApiResponse(T data, string message = null)
        {
            Success = true;
            Message = message ?? "Request was successful";
            Data = data;

        }
        public ApiResponse(string errorMessage)
        {
            Success = false;
            Message = errorMessage;
        }
    }
}
