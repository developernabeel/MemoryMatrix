using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MemoryMatrix.Helpers
{
    public static class StringHelper
    {
        public static bool IsStringInSet(this string text, string[] set)
        {
            if (text == null || set == null || set.Count() == 0)
                return false;

            foreach (string item in set)
                if (text == item)
                    return true;
            return false;
        }
    }
}