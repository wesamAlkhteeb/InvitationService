using FluentValidation;
using Grpc.Core;
using Grpc.Core.Interceptors;
using InvitationCommandService.Domain.Exceptions;

namespace InvitationCommandService.Presentation.Interceptors
{
    public class HandleErrorInterceptor : Interceptor
    {
        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
        {
            try
            {
                return await continuation(request, context);
            }
            catch (OperationException ex)
            {
                throw new RpcException(new Status(StatusCode.FailedPrecondition, ex.Message));
            }
            catch (ValidationException ex)
            {
                throw new RpcException(new Status(StatusCode.FailedPrecondition, ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                throw new RpcException(new Status(StatusCode.Unknown, ex.Message));
            }
            catch (Exception ex)
            {
                throw new RpcException(new Status(StatusCode.Unknown, ex.Message));
            }
        }
    }
}
