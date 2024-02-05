using Microsoft.AspNetCore.Mvc;

namespace Common
{
    public class ApiResult
    {
        public string ResultMessage { get; set; }
        public object ResultBody { get; set; }
    }
}
