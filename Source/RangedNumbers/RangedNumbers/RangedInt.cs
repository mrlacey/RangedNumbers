using System;

namespace RangedNumbers
{
    public struct RangedInt
    {
        public int Value { get; private set; }

        public int MinValue { get; }

        public int MaxValue { get; }

        // TODO: test this being invoked even after original instance has been changed several times.
        public Action<BoundaryExceededArgs> OnBoundaryExceeded;

        public RangedInt(int value = 0, int minValue = int.MinValue, int maxValue = int.MaxValue, Action<BoundaryExceededArgs> onBoundaryExceeded = null)
        {
            // TODO: test min being less than max
            this.MinValue = minValue;
            this.MaxValue = maxValue;

            this.OnBoundaryExceeded = onBoundaryExceeded;

            // TODO: better handle initial value being invalid
            //this.Value = minValue > value ? minValue : maxValue < value ? maxValue : value;
            this.Value = value; // set this as can't leave it unassigned but let SetValue handle logic

            // if (eventHandler != null)
            {
                SetValue(value); // Call this even though it duplicates the logic above to ensure the appropriate event is raised
            }
        }

        //public RangedInt(byte value, int minValue = int.MinValue, int maxValue = int.MaxValue, Action<BoundaryExceededArgs> onBoundaryExceeded = null)
        //    : this((int)value, minValue, maxValue, onBoundaryExceeded);
        //{
        //}

        public static implicit operator RangedInt(int value)
        {
            return new RangedInt(value);
        }

        public static implicit operator RangedInt(string value)
        {
            return new RangedInt(int.Parse(value));
        }

        public static implicit operator RangedInt(byte value)
        {
            return new RangedInt(value);
        }

        public static implicit operator RangedInt(sbyte value)
        {
            return new RangedInt(value);
        }

        public static implicit operator RangedInt(short value)
        {
            return new RangedInt(value);
        }

        public static implicit operator RangedInt(ushort value)
        {
            return new RangedInt(value);
        }

        public static implicit operator RangedInt(char value)
        {
            return new RangedInt(value);
        }

        public static implicit operator RangedInt(RangedByte value)
        {
            return new RangedInt(value.Value);
        }

        public static implicit operator RangedInt(RangedSByte value)
        {
            return new RangedInt(value.Value);
        }

        public static implicit operator RangedInt(RangedShort value)
        {
            return new RangedInt(value.Value);
        }

        public static implicit operator RangedInt(RangedUShort value)
        {
            return new RangedInt(value.Value);
        }

        // TODO: reveiew: what if this returned a new instance
        public void SetValue(int newValue)
        {
            if (newValue < this.MinValue)
            {
                this.Value = this.MinValue;
                OnBoundaryExceeded?.Invoke(new BoundaryExceededArgs(newValue));
            }
            else if (newValue > this.MaxValue)
            {
                this.Value = this.MaxValue;
                OnBoundaryExceeded?.Invoke(new BoundaryExceededArgs(newValue));
            }
            else
            {
                this.Value = newValue;
            }
        }

        // TODO: Review this experimental method. If keep it need to add overloads that support setting from other types and include in other Ranged Types too.
        public bool TrySetValue(int newValue, out RangedInt result)
        {
            if (newValue < MinValue || newValue > MaxValue)
            {
                result = null;
                return false;
            }
            else
            {
                result = new RangedInt(newValue, MinValue, MaxValue, OnBoundaryExceeded);
                return true;
            }


        }

        // TODO: test adding negative numbers
        public static RangedInt operator +(RangedInt original, int addition)
        {
            var result = new RangedInt(minValue: original.MinValue, maxValue: original.MaxValue, onBoundaryExceeded: original.OnBoundaryExceeded);

            if (int.MaxValue - original.Value < addition)
            {
                result.SetValue(original.MaxValue);
                result.OnBoundaryExceeded?.Invoke(new BoundaryExceededArgs($"{original.Value} + {addition} > int.MaxValue"));
            }
            else
            {
                result.SetValue(original.Value + addition);
            }

            return result;
        }

        public static RangedInt operator +(RangedInt original, RangedInt addition)
        {
            var result = new RangedInt(minValue: original.MinValue, maxValue: original.MaxValue, onBoundaryExceeded: original.OnBoundaryExceeded);

            if (int.MaxValue - original.Value < addition.Value)
            {
                result.SetValue(original.MaxValue);
                result.OnBoundaryExceeded?.Invoke(new BoundaryExceededArgs($"{original.Value} + {addition.Value} > int.MaxValue"));
            }
            else
            {
                result.SetValue(original.Value + addition.Value);
            }

            return result;
        }

        public static RangedInt operator -(RangedInt original, int subtraction)
        {
            var result = new RangedInt(minValue: original.MinValue, maxValue: original.MaxValue, onBoundaryExceeded: original.OnBoundaryExceeded);

            if (int.MaxValue - original.Value < subtraction)
            {
                result.SetValue(original.MinValue);
                result.OnBoundaryExceeded?.Invoke(new BoundaryExceededArgs($"{original.Value} - {subtraction} < int.MinValue"));
            }
            else
            {
                result.SetValue(original.Value - subtraction);
            }

            return result;
        }

        public static RangedInt operator -(RangedInt original, RangedInt subtraction)
        {
            var result = new RangedInt(minValue: original.MinValue, maxValue: original.MaxValue, onBoundaryExceeded: original.OnBoundaryExceeded);

            if (int.MaxValue - original.Value < subtraction.Value)
            {
                result.SetValue(original.MinValue);
                result.OnBoundaryExceeded?.Invoke(new BoundaryExceededArgs($"{original.Value} - {subtraction.Value} < int.MinValue"));
            }
            else
            {
                result.SetValue(original.Value - subtraction.Value);
            }

            return result;
        }

        public static RangedInt operator ++(RangedInt original)
        {
            return original + 1;
        }

        public static RangedInt operator --(RangedInt original)
        {
            return original - 1;
        }

        public static RangedInt operator *(RangedInt original, int right)
        {
            // TODO: test for boundary exceeding
            return new RangedInt(original.Value * right, original.MinValue, original.MaxValue, original.OnBoundaryExceeded);
        }

        public static RangedInt operator *(RangedInt original, RangedInt right)
        {
            // TODO: test for boundary exceeding
            return new RangedInt(original.Value * right.Value, original.MinValue, original.MaxValue, original.OnBoundaryExceeded);
        }

        public static RangedInt operator /(RangedInt original, int right)
        {
            // TODO: test for boundary exceeding
            return new RangedInt(original.Value / right, original.MinValue, original.MaxValue, original.OnBoundaryExceeded);
        }

        public static RangedInt operator /(RangedInt original, RangedInt right)
        {
            // TODO: test for boundary exceeding
            return new RangedInt(original.Value / right.Value, original.MinValue, original.MaxValue, original.OnBoundaryExceeded);
        }

        public static RangedInt operator %(RangedInt original, int right)
        {
            return new RangedInt(original.Value % right, original.MinValue, original.MaxValue, original.OnBoundaryExceeded);
        }

        public static RangedInt operator %(RangedInt original, RangedInt right)
        {
            return new RangedInt(original.Value % right.Value, original.MinValue, original.MaxValue, original.OnBoundaryExceeded);
        }

        public static bool operator <(RangedInt original, int right)
        {
            return original.Value < right;
        }

        public static bool operator >(RangedInt original, int right)
        {
            return original.Value > right;
        }

        public static bool operator <=(RangedInt original, int right)
        {
            return original.Value <= right;
        }

        public static bool operator >=(RangedInt original, int right)
        {
            return original.Value >= right;
        }

        public static bool operator ==(RangedInt original, int right)
        {
            return original.Value == right;
        }

        public static bool operator !=(RangedInt original, int right)
        {
            return original.Value != right;
        }

        // TODO: Bitwise operator also for RangedUInt, RangedLong, RangedUlong
        public static RangedInt operator ~(RangedInt original)
        {
            return new RangedInt(~original.Value, original.MinValue, original.MaxValue, original.OnBoundaryExceeded);
        }

        public bool Equals(RangedInt other)
        {
            return Equals(OnBoundaryExceeded, other.OnBoundaryExceeded)
                && Value == other.Value
                && MinValue == other.MinValue
                && MaxValue == other.MaxValue;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is RangedInt i && Equals(i);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = OnBoundaryExceeded?.GetHashCode() ?? 0;
                hashCode = (hashCode * 397) ^ Value;
                hashCode = (hashCode * 397) ^ MinValue;
                hashCode = (hashCode * 397) ^ MaxValue;
                return hashCode;
            }
        }

        public static implicit operator RangedInt((int value, int min) initializer)
        {
            return new RangedInt(initializer.value, initializer.min);
        }

        public static implicit operator RangedInt((int value, int min, int max) initializer)
        {
            return new RangedInt(initializer.value, initializer.min, initializer.max);
        }

        public static implicit operator RangedInt((int value, int min, int max, Action<BoundaryExceededArgs> action) initializer)
        {
            return new RangedInt(initializer.value, initializer.min, initializer.max, initializer.action);
        }

        public override string ToString()
        {
            return this.Value.ToString();
        }

        public string ToString(string format)
        {
            return this.Value.ToString(format);
        }

        public string ToString(IFormatProvider provider)
        {
            return this.Value.ToString(provider);
        }

        public string ToString(string format, IFormatProvider provider)
        {
            return this.Value.ToString(format, provider);
        }
    }

    public class BoundaryExceededArgs : EventArgs
    {
        public object ImpossibleValue { get; }

        public BoundaryExceededArgs(object impossibleValue)
        {
            ImpossibleValue = impossibleValue;
        }
    }
}
