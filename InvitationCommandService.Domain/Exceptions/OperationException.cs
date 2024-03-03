using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvitationCommandService.Domain.Exceptions
{
    public class OperationException:Exception
    {
        public OperationException(string messsage):base(messsage) { }
        
    }
}
