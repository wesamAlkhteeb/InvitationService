using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvitationQueryService.Domain.Exceptions
{
    public class OperationsOrderException : Exception
    {
        public OperationsOrderException(string message):base(message) {}

    }
}
