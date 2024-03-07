using Grpc.Core;
using Grpc.Core.Interceptors;
using InvitationQueryService.Presentation.Exceptions;

namespace InvitationQueryService.Presentation.Interceptors
{
    public class HandleErrorInterceptor : Interceptor
    {
        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
        {
            try
            {
                return await continuation(request, context);
            }
            catch (BadPageException ex)
            {
                throw new RpcException(new Grpc.Core.Status(StatusCode.InvalidArgument, ex.Message));
            }
            catch (Exception ex)
            {
                throw new RpcException(new Grpc.Core.Status(StatusCode.Unknown, ex.Message));
            }
        }
    }
}
