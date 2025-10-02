using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplePOS.Business.Exceptions
{
    public class AlreadyExistsException: Exception
    {
        public AlreadyExistsException(string entityName, string fieldName, string fieldValue)
            : base($"{entityName} con {fieldName} '{fieldValue}' ya existe.")
        {
        }
    }
}
