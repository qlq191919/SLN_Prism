using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLN_Prism.Common
{
    public class ActionResult : IActionResult
    {
        public static readonly ActionResult OK = new ActionResult();

        public Exception Exception { get; set; }

        public virtual bool Success => Code == 0;

        public string Message { get; set; }

        public int Code { get; set; }

        public ActionResult()
            : this(0)
        {
        }

        public ActionResult(int code, string message = "")
        {
            Code = code;
            Message = message;
        }

        public ActionResult(int code, string message, Exception exception)
            : this(code, message)
        {
            Exception = exception;
        }

        public ActionResult(int code, Exception exception)
            : this(code, exception.Message)
        {
            Exception = exception;
        }

        public static implicit operator bool(ActionResult d)
        {
            return d.Success;
        }

        public Task ExecuteResultAsync(ActionContext context)
        {
            throw new NotImplementedException();
        }
    }
}
