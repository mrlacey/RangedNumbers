using System;
using System.Transactions;

namespace RangedNumbers
{
    public struct RangedInt
    {
        // TODO: support implicit conversion from sbyte
        // TODO: support implicit conversion from byte
        // TODO: support implicit conversion from short
        // TODO: support implicit conversion from ushort
        // TODO: support implicit conversion from char
        // TODO: support implicit conversion from RangedSByte
        // TODO: support implicit conversion from RangedByte
        // TODO: support implicit conversion from RangedShort
        // TODO: support implicit conversion from RangedUShort

        public int Value { get; private set; }

        public int MinValue { get; private set; }

        public int MaxValue { get; private set; }

        public event BoundaryExceededEventHandler BoundaryExceeded;

        public RangedInt(int minValue = int.MinValue, int maxValue = int.MaxValue, int value = 0, BoundaryExceededEventHandler eventHandler = null)
        {
            this.MinValue = minValue;
            this.MaxValue = maxValue;

            this.BoundaryExceeded = eventHandler;

            //this.Value = minValue > value ? minValue : maxValue < value ? maxValue : value;
            this.Value = value; // set this as can't leave it unassigned but let SetValue handle logic

           // if (eventHandler != null)
            {
                SetValue(value); // Call this even though it duplicates the logic above to ensure the appropriate event is raised
            }
        }

        public RangedInt(int value) : this(maxValue: int.MaxValue, value: value)
        {
        }

        public static implicit operator RangedInt(int value)
        {
            return new RangedInt(value);
        }

        public void SetValue(int newValue)
        {

            if (newValue < this.MinValue)
            {
                this.Value = this.MinValue;
                BoundaryExceeded?.Invoke(this, new BoundaryExceededEventHandlerArgs(newValue));
            }
            else if (newValue > this.MaxValue)
            {
                this.Value = this.MaxValue;
                BoundaryExceeded?.Invoke(this, new BoundaryExceededEventHandlerArgs(newValue));
            }
            else
            {
                this.Value = newValue;
            }
        }

        public static RangedInt operator +(RangedInt original, int addition)
        {
            var result = new RangedInt(original.MinValue, original.MaxValue, eventHandler: original.BoundaryExceeded);

            if (int.MaxValue - original.Value < addition)
            {
                result.SetValue(original.MaxValue);
                result.BoundaryExceeded.Invoke(result, new BoundaryExceededEventHandlerArgs($"{original.Value} + {addition} > int.MaxValue"));
            }
            else
            {
                result.SetValue(original.Value + addition);
            }

            return result;
        }

        public static RangedInt operator +(RangedInt original, RangedInt addition)
        {
            var result = new RangedInt(original.MinValue, original.MaxValue, eventHandler: original.BoundaryExceeded);

            if (int.MaxValue - original.Value < addition.Value)
            {
                result.SetValue(original.MaxValue);
                result.BoundaryExceeded.Invoke(result, new BoundaryExceededEventHandlerArgs($"{original.Value} + {addition.Value} > int.MaxValue"));
            }
            else
            {
                result.SetValue(original.Value + addition.Value);
            }

            return result;
        }

        public static RangedInt operator -(RangedInt original, int subtraction)
        {
            var result = new RangedInt(original.MinValue, original.MaxValue, eventHandler: original.BoundaryExceeded);

            if (int.MaxValue - original.Value < subtraction)
            {
                result.SetValue(original.MinValue);
                result.BoundaryExceeded.Invoke(result, new BoundaryExceededEventHandlerArgs($"{original.Value} - {subtraction} < int.MinValue"));
            }
            else
            {
                result.SetValue(original.Value - subtraction);
            }

            return result;
        }

        public static RangedInt operator -(RangedInt original, RangedInt subtraction)
        {
            var result = new RangedInt(original.MinValue, original.MaxValue, eventHandler: original.BoundaryExceeded);

            if (int.MaxValue - original.Value < subtraction.Value)
            {
                result.SetValue(original.MinValue);
                result.BoundaryExceeded.Invoke(result, new BoundaryExceededEventHandlerArgs($"{original.Value} - {subtraction.Value} < int.MinValue"));
            }
            else
            {
                result.SetValue(original.Value - subtraction.Value);
            }

            return result;
        }

        public override string ToString()
        {
            return this.Value.ToString();
        }
    }

    public delegate void BoundaryExceededEventHandler(object sender, BoundaryExceededEventHandlerArgs args);

    public class BoundaryExceededEventHandlerArgs : EventArgs
    {
        public object ImpossibleValue { get; }

        public BoundaryExceededEventHandlerArgs(object impossibleValue)
        {
            ImpossibleValue = impossibleValue;
        }
    }
}
