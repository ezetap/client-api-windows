using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.eze.api
{
    public class Error
    {
        private string code;
        private string message;
        public string getCode()
        {
            return code;
        }
        public void setCode(string code)
        {
            this.code = code;
        }
        public string getMessage()
        {
            return message;
        }
        public void setMessage(string message)
        {
            this.message = message;
        }

    }
}
