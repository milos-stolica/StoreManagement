using System;
using System.Collections.Generic;
using System.Linq;

namespace StoreManagement.API.Utils
{
    public static class ArgumentValidator
    {
        public static void ThrowIfNull(object arg, string argName)
        {
            if(arg == null)
            {
                throw new ArgumentNullException(argName);
            }
        }

        public static void ThrowIfNullOrEmpty(IEnumerable<object> arg, string argName)
        {
            if (arg == null)
            {
                throw new ArgumentNullException(argName);
            }

            if (!arg.Any())
            {
                throw new ArgumentException($"{argName} is empty");
            }
        }

        public static void ThrowIfNullOrEmpty(string arg, string argName)
        {
            if (string.IsNullOrEmpty(arg))
            {
                throw new ArgumentNullException(argName);
            }
        }

        public static void ThrowOnUnexpectedType(object obj, Type expectedType)
        {
            if(obj == null)
            {
                throw new ArgumentException($"Expected type is {expectedType}, but it was {obj}.");
            }

            if(obj.GetType() != expectedType && !obj.GetType().IsSubclassOf(expectedType))
            {
                throw new ArgumentException($"Expected type is {expectedType}, but it was {obj.GetType()}.");
            }
        }
    }
}
