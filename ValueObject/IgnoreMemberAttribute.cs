using System;

namespace Cactus.Blade.ValueObject
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class IgnoreMemberAttribute : System.Attribute
    {
    }
}
