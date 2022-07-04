using System.Reflection;

namespace DDDMart.Application.EventBus
{
    public sealed class HandlerSubscription : IEquatable<HandlerSubscription>
    {
        public HandlerSubscription(Type handlerType)
        {
            Type = handlerType;
            MethodInfo = handlerType.GetMethod("HandleAsync");
        }

        public readonly Type Type;
        public readonly MethodInfo MethodInfo;

        public override bool Equals(object obj)
        {
            return Equals(obj as HandlerSubscription);
        }

        public bool Equals(HandlerSubscription other)
        {
            return other != null && Type.Equals(other.Type);
        }

        public override int GetHashCode()
        {
            return Type.GetHashCode();
        }
    }
}
