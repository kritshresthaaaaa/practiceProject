using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebHost.DTO.BaseResponse
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public List<string> Errors { get; set; }
        public ApiResponse()
        {
            Errors = new List<string>();
        }
        public ApiResponse(T data, string message = null)
        {
            Success = true;
            Message = message ?? "Request was successful";
            Data = data;
            Errors = new List<string>();
        }
        public ApiResponse(string errorMessage)
        {
            Success = false;
            Message = errorMessage;
            Errors = new List<string> { errorMessage };
        }
    }
}
