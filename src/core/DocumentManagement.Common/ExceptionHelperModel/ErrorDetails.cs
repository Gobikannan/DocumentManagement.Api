using Newtonsoft.Json;

namespace DocumentManagement.Common.ExceptionHelperModel
{
    /// <summary>
    /// 
    /// </summary>
    public class ErrorDetails
    {
        /// <summary>
        /// Status Code
        /// </summary>
        public int StatusCode { get; set; }
        /// <summary>
        /// Error message
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// JSON parser
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
