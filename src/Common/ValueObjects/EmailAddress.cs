using System;
using System.Text.RegularExpressions;
using Common.Exceptions;
namespace Common.ValueObjects
{
    public class EmailAddress
    {
        private static readonly Regex EmailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

        public string Value { get; private set; }

        private EmailAddress()
        {
        }

        private EmailAddress(string value)
        {
            if (!EmailRegex.IsMatch(value))
            {
                throw new ArgumentException("Invalid email address", nameof(value));
            }

            Value = value;
        }

        public static EmailAddress Create(string value)
        {
            return new EmailAddress(value);
        }

        public override bool Equals(object obj)
        {
            if (obj is EmailAddress other)
            {
                return Value.Equals(other.Value, StringComparison.OrdinalIgnoreCase);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode(StringComparison.OrdinalIgnoreCase);
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
