using Dapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.APIS.Core.Entities
{
    public class JsonObjectTypeHandle : SqlMapper.ITypeHandler
    {
        public void SetValue(IDbDataParameter parameter, object value)
        {
            parameter.Value = (value == null)
                ? (object)DBNull.Value
                : JsonConvert.SerializeObject(value);
            parameter.DbType = DbType.String;
        }

        public object Parse(Type destinationType, object value)
        {
            return JsonConvert.DeserializeObject(value.ToString(), destinationType);
        }
    }
}
