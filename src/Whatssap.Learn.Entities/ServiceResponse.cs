using System;
using System.Collections.Generic;
using System.Text;

namespace Whatssap.Learn.Entities
{
    public class ServiceResponse<T>
    {
        public bool Success { get; set; }
        public T Response { get; set; }
        public string ErrorMessage { get; set; }
    }
}
