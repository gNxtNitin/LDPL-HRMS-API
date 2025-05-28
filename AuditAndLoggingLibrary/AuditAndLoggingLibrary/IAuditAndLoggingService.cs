using AuditAndLoggingLibrary.Models;
using ModelsLibrary.Models;

namespace AuditAndLoggingLibrary
{
    public interface IAuditAndLoggingService
    {
        //Maintaining the AuditLog of a User
        public Task<ResponseModel> AuditLogAction(AuditMasterReqModel req);

    }
}
