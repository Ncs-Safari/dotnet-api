using System;
using System.Text.RegularExpressions;

namespace Common.ValueObjects
{
    public class PhoneNumber
    {
        private static readonly Regex PhoneRegex = new Regex(@"^\+?[1-9]\d{1,14}$", RegexOptions.Compiled);

        public string Value { get; private set; }

        private PhoneNumber() { }

        private PhoneNumber(string value)
        {
            if (!PhoneRegex.IsMatch(value))
            {
                throw new ArgumentException("Invalid phone number", nameof(value));
            }

            Value = value;
        }

        public static PhoneNumber Create(string value)
        {
            return new PhoneNumber(value);
        }

        public override bool Equals(object obj)
        {
            if (obj is PhoneNumber other)
            {
                return Value.Equals(other.Value);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
