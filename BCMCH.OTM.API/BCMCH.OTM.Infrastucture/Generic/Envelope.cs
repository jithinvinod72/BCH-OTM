using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCMCH.OTM.Infrastucture.Generic
{
    public class Envelope<T>
    {
        public bool Success { get; set; }
        public string Response { get; set; }
        public T Data { get; set; }
        public string ExceptionMessage { get; set; }
        public Guid ErrorIdentifier { get; set; }
        public string ErrorType { get; set; }
        public Envelope()
        {

        }
        public Envelope(bool success, string Response)
        {
            this.Success = success;
            this.Response = Response;
 
        }
        public Envelope(bool success, string key, T data) : this(success, key)
        {
            this.Data = data;
        }

    }
}
