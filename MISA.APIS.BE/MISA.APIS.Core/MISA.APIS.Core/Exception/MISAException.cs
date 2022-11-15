using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.APIS.Core
{
    public class MISAException: Exception
    {
        string MISAMessage;
        IDictionary errors;
        public MISAException(string msg = null, List<string> listMsgs = null)
        {
            this.MISAMessage = msg;
            errors = new Dictionary<string, List<string>>();
            errors.Add("validateError", listMsgs);
        }
        public override string Message => this.MISAMessage;
        public override IDictionary Data => this.errors;
    }
}
