using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Ats.Commons.Utilities
{
    public static class ArrayExtensions
    {
        public static string JoinToString(this string[] stringArray, string joinChar = ";")
        {
            return stringArray != null ? string.Join(joinChar, stringArray) : null;
        }
    }
}



