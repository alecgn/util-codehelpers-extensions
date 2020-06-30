using System;

namespace Util.CodeHelpers
{
    public static class Methods
    {
        public static bool RunIgnoringExceptions(Action action)
        {
            if (action == null)
                return false;
            
            try
            {
                action.Invoke();
            }
            catch
            {
                return false;
            }

            return true;
        }

        public static T RunIgnoringExceptions<T>(Func<T> func, T defaultValue = default)
        {
            if (func == null)
                return defaultValue;

            T result;

            try
            {
                result = func.Invoke();
            }
            catch
            {
                result = defaultValue;
            }

            return result;
        }
    }
}
