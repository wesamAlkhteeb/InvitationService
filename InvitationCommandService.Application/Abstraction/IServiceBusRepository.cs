using InvitationCommandService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvitationCommandService.Application.Abstraction
{
    public interface IServiceBusRepository
    {
        public Task PublicMessage();
    }
}
