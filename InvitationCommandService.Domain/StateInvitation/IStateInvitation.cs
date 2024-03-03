using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvitationCommandService.Domain.StateInvitation
{
    public interface IStateInvitation
    {
        void Send();
        void Cancel();
        void Accept();
        void Reject();
        void Remove();
        void Leave();
        void Join();
        void ChangePermissions();
        void Empty();
    }
}
