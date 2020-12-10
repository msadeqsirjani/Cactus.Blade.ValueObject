using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Cactus.Blade.ValueObject
{
    public abstract class ValueObject : IEquatable<ValueObject>
    {
        private List<PropertyInfo> _properties;
        private List<FieldInfo> _fields;

        public static bool operator ==(ValueObject @this, ValueObject value)
        {
            return @this?.Equals(value) ?? Equals(value, null);
        }

        public static bool operator !=(ValueObject @this, ValueObject value)
        {
            return !(@this == value);
        }

        public bool Equals(ValueObject @this)
        {
            return Equals(@this as object);
        }

        public override bool Equals(object @this)
        {
            if (@this == null || GetType() != @this.GetType()) return false;

            return GetProperties().All(p => PropertiesAreEqual(@this, p)) &&
                   GetFields().All(f => FieldsAreEqual(@this, f));
        }

        private bool PropertiesAreEqual(object @this, PropertyInfo propertyInfo)
        {
            return Equals(propertyInfo.GetValue(this, null), propertyInfo.GetValue(@this, null));
        }

        private bool FieldsAreEqual(object @this, FieldInfo fieldInfo)
        {
            return Equals(fieldInfo.GetValue(this), fieldInfo.GetValue(@this));
        }

        private IEnumerable<PropertyInfo> GetProperties()
        {
            return _properties ??= GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => !Attribute.IsDefined(p, typeof(IgnoreMemberAttribute)))
                .ToList();
        }

        private IEnumerable<FieldInfo> GetFields()
        {
            return _fields ??= GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.Public)
                .Where(f => !Attribute.IsDefined(f, typeof(IgnoreMemberAttribute)))
                .ToList();
        }

        public override int GetHashCode()
        {
            var hash = GetProperties()
                .Select(prop => prop.GetValue(this, null))
                .Aggregate(17, HashValue);

            return GetFields()
                .Select(field => field.GetValue(this))
                .Aggregate(hash, HashValue);
        }

        private static int HashValue(int seed, object value)
        {
            var currentHash = value?.GetHashCode() ?? 0;

            return seed * 23 + currentHash;
        }
    }
}
