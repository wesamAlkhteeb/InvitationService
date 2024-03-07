// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: Protos/Subscriptions.proto
// </auto-generated>
#pragma warning disable 0414, 1591, 8981, 0612
#region Designer generated code

using grpc = global::Grpc.Core;

namespace InvitationQueryService {
  public static partial class Subscriptions
  {
    static readonly string __ServiceName = "Subscriptions.v1.Subscriptions";

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static void __Helper_SerializeMessage(global::Google.Protobuf.IMessage message, grpc::SerializationContext context)
    {
      #if !GRPC_DISABLE_PROTOBUF_BUFFER_SERIALIZATION
      if (message is global::Google.Protobuf.IBufferMessage)
      {
        context.SetPayloadLength(message.CalculateSize());
        global::Google.Protobuf.MessageExtensions.WriteTo(message, context.GetBufferWriter());
        context.Complete();
        return;
      }
      #endif
      context.Complete(global::Google.Protobuf.MessageExtensions.ToByteArray(message));
    }

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static class __Helper_MessageCache<T>
    {
      public static readonly bool IsBufferMessage = global::System.Reflection.IntrospectionExtensions.GetTypeInfo(typeof(global::Google.Protobuf.IBufferMessage)).IsAssignableFrom(typeof(T));
    }

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static T __Helper_DeserializeMessage<T>(grpc::DeserializationContext context, global::Google.Protobuf.MessageParser<T> parser) where T : global::Google.Protobuf.IMessage<T>
    {
      #if !GRPC_DISABLE_PROTOBUF_BUFFER_SERIALIZATION
      if (__Helper_MessageCache<T>.IsBufferMessage)
      {
        return parser.ParseFrom(context.PayloadAsReadOnlySequence());
      }
      #endif
      return parser.ParseFrom(context.PayloadAsNewBuffer());
    }

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::InvitationQueryService.UserSubscriptor> __Marshaller_Subscriptions_v1_UserSubscriptor = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::InvitationQueryService.UserSubscriptor.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::InvitationQueryService.ManyUserSubscriptorReuslt> __Marshaller_Subscriptions_v1_ManyUserSubscriptorReuslt = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::InvitationQueryService.ManyUserSubscriptorReuslt.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::InvitationQueryService.UserSubscription> __Marshaller_Subscriptions_v1_UserSubscription = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::InvitationQueryService.UserSubscription.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::InvitationQueryService.OwnerSubscription> __Marshaller_Subscriptions_v1_OwnerSubscription = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::InvitationQueryService.OwnerSubscription.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::InvitationQueryService.ManyOwnerSubscriptionReuslt> __Marshaller_Subscriptions_v1_ManyOwnerSubscriptionReuslt = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::InvitationQueryService.ManyOwnerSubscriptionReuslt.Parser));

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::InvitationQueryService.UserSubscriptor, global::InvitationQueryService.ManyUserSubscriptorReuslt> __Method_GetAllSubscriptorInSubscription = new grpc::Method<global::InvitationQueryService.UserSubscriptor, global::InvitationQueryService.ManyUserSubscriptorReuslt>(
        grpc::MethodType.Unary,
        __ServiceName,
        "GetAllSubscriptorInSubscription",
        __Marshaller_Subscriptions_v1_UserSubscriptor,
        __Marshaller_Subscriptions_v1_ManyUserSubscriptorReuslt);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::InvitationQueryService.UserSubscription, global::InvitationQueryService.ManyUserSubscriptorReuslt> __Method_GetAllSubscriptionForSubscriptor = new grpc::Method<global::InvitationQueryService.UserSubscription, global::InvitationQueryService.ManyUserSubscriptorReuslt>(
        grpc::MethodType.Unary,
        __ServiceName,
        "GetAllSubscriptionForSubscriptor",
        __Marshaller_Subscriptions_v1_UserSubscription,
        __Marshaller_Subscriptions_v1_ManyUserSubscriptorReuslt);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::InvitationQueryService.OwnerSubscription, global::InvitationQueryService.ManyOwnerSubscriptionReuslt> __Method_GetAllSubscriptionForOwner = new grpc::Method<global::InvitationQueryService.OwnerSubscription, global::InvitationQueryService.ManyOwnerSubscriptionReuslt>(
        grpc::MethodType.Unary,
        __ServiceName,
        "GetAllSubscriptionForOwner",
        __Marshaller_Subscriptions_v1_OwnerSubscription,
        __Marshaller_Subscriptions_v1_ManyOwnerSubscriptionReuslt);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::InvitationQueryService.SubscriptionsReflection.Descriptor.Services[0]; }
    }

    /// <summary>Base class for server-side implementations of Subscriptions</summary>
    [grpc::BindServiceMethod(typeof(Subscriptions), "BindService")]
    public abstract partial class SubscriptionsBase
    {
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task<global::InvitationQueryService.ManyUserSubscriptorReuslt> GetAllSubscriptorInSubscription(global::InvitationQueryService.UserSubscriptor request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task<global::InvitationQueryService.ManyUserSubscriptorReuslt> GetAllSubscriptionForSubscriptor(global::InvitationQueryService.UserSubscription request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task<global::InvitationQueryService.ManyOwnerSubscriptionReuslt> GetAllSubscriptionForOwner(global::InvitationQueryService.OwnerSubscription request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

    }

    /// <summary>Creates service definition that can be registered with a server</summary>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    public static grpc::ServerServiceDefinition BindService(SubscriptionsBase serviceImpl)
    {
      return grpc::ServerServiceDefinition.CreateBuilder()
          .AddMethod(__Method_GetAllSubscriptorInSubscription, serviceImpl.GetAllSubscriptorInSubscription)
          .AddMethod(__Method_GetAllSubscriptionForSubscriptor, serviceImpl.GetAllSubscriptionForSubscriptor)
          .AddMethod(__Method_GetAllSubscriptionForOwner, serviceImpl.GetAllSubscriptionForOwner).Build();
    }

    /// <summary>Register service method with a service binder with or without implementation. Useful when customizing the service binding logic.
    /// Note: this method is part of an experimental API that can change or be removed without any prior notice.</summary>
    /// <param name="serviceBinder">Service methods will be bound by calling <c>AddMethod</c> on this object.</param>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    public static void BindService(grpc::ServiceBinderBase serviceBinder, SubscriptionsBase serviceImpl)
    {
      serviceBinder.AddMethod(__Method_GetAllSubscriptorInSubscription, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::InvitationQueryService.UserSubscriptor, global::InvitationQueryService.ManyUserSubscriptorReuslt>(serviceImpl.GetAllSubscriptorInSubscription));
      serviceBinder.AddMethod(__Method_GetAllSubscriptionForSubscriptor, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::InvitationQueryService.UserSubscription, global::InvitationQueryService.ManyUserSubscriptorReuslt>(serviceImpl.GetAllSubscriptionForSubscriptor));
      serviceBinder.AddMethod(__Method_GetAllSubscriptionForOwner, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::InvitationQueryService.OwnerSubscription, global::InvitationQueryService.ManyOwnerSubscriptionReuslt>(serviceImpl.GetAllSubscriptionForOwner));
    }

  }
}
#endregion
