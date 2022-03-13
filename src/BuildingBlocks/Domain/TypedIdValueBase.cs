using System;

namespace HelpLine.BuildingBlocks.Domain
{
    public abstract class TypedGuidIdValueBase : IEquatable<TypedGuidIdValueBase>
    {
        public Guid Value { get; }

        protected TypedGuidIdValueBase(Guid value)
        {
            if (value == Guid.Empty)
                throw new InvalidOperationException("Id value cannot be empty!");
            Value = value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is TypedGuidIdValueBase other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public bool Equals(TypedGuidIdValueBase other)
        {
            return Value == other?.Value;
        }

        public static bool operator ==(TypedGuidIdValueBase? obj1, TypedGuidIdValueBase? obj2)
        {
            if (object.Equals(obj1, null))
            {
                if (object.Equals(obj2, null))
                {
                    return true;
                }
                return false;
            }

            return obj1.Equals(obj2);
        }
        public static bool operator !=(TypedGuidIdValueBase? x, TypedGuidIdValueBase? y)
        {
            return !(x == y);
        }
    }

    public abstract class TypedStringIdValueBase : IEquatable<TypedStringIdValueBase>
    {
        public string Value { get; }

        protected TypedStringIdValueBase(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new InvalidOperationException("Value cannot be empty!");
            Value = value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is TypedStringIdValueBase other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public bool Equals(TypedStringIdValueBase other)
        {
            return Value == other?.Value;
        }

        public static bool operator ==(TypedStringIdValueBase obj1, TypedStringIdValueBase obj2)
        {
            if (object.Equals(obj1, null))
            {
                if (object.Equals(obj2, null))
                {
                    return true;
                }
                return false;
            }
            return obj1.Equals(obj2);
        }
        public static bool operator !=(TypedStringIdValueBase x, TypedStringIdValueBase y)
        {
            return !(x == y);
        }

        public override string ToString()
        {
            return Value;
        }
    }

    public abstract class TypedNumberIdValueBase : IEquatable<TypedNumberIdValueBase>
    {
        public long Value { get; }

        protected TypedNumberIdValueBase(long value)
        {
            Value = value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is TypedNumberIdValueBase other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public bool Equals(TypedNumberIdValueBase other)
        {
            return Value == other?.Value;
        }

        public static bool operator ==(TypedNumberIdValueBase obj1, TypedNumberIdValueBase obj2)
        {
            if (object.Equals(obj1, null))
            {
                if (object.Equals(obj2, null))
                {
                    return true;
                }
                return false;
            }
            return obj1.Equals(obj2);
        }
        public static bool operator !=(TypedNumberIdValueBase x, TypedNumberIdValueBase y)
        {
            return !(x == y);
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
