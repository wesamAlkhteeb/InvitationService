using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvitationCommandService.Domain.Exceptions
{
    public class RepeatPermissionException : Exception
    {
        public RepeatPermissionException(string message):base(message) { }
    }
}
