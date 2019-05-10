using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers
{
    public static class Guard
    {
        public static T GetNotNull<T>(T argument, string name) where T : class
        {
            CheckNotNull(argument, name);
            return argument;
        }
        public static void CheckContainsText<TException>(string argument, Func<TException> exceptionCreator)
            where TException : Exception
        {
            if (string.IsNullOrEmpty(argument) || string.IsNullOrWhiteSpace(argument))
            {
                throw CreateUserDefinedException(exceptionCreator);
            }
        }
        private static TException CreateUserDefinedException<TException>(Func<TException> exceptionCreator)
            where TException : Exception
        {
            if (exceptionCreator == null)
            {
                throw CreateExceptionForNullReference("exceptionCreator");
            }

            var exception = exceptionCreator();
            if (exception == null)
            {
                throw new Exception("Cannot throw an exception: exceptionCreator returned null.");
            }

            return exception;
        }
        private static Exception CreateExceptionForNullReference(string name)
        {
            return new Exception("Object reference '" + name + "' is null.");
        }
        private static ArgumentNullException CreateArgumentNullException(string name)
        {
            return new ArgumentNullException(name, string.Format("Argument '{0}' cannot be null.", name));
        }

        public static void CheckNotNull<T>(T argument, string name) where T : class
        {
            if (argument == null)
            {
                throw CreateArgumentNullException(name);
            }
        }

        public static void CheckContainsText(string argument, string name)
        {
            if (argument == null)
            {
                throw CreateArgumentNullException(name);
            }

            if (string.IsNullOrWhiteSpace(argument))
            {
                throw new Exception(
                    string.Format("Argument '{0}' cannot be empty or contain whitespaces only : '{1}'.", name, argument));
            }
        }

    }
}
