using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine.Pool;

namespace Razorhead.Core
{
    public static class LocalizationFormatter
    {
        public static string Format(string input, Dictionary<string, string> values)
        {
            foreach(var item in values)
            {
                input = input.Replace(item.Key, item.Value);
            }

            return input;
        }
        public static string Format(string input, string key, string value)
        {
            return input.Replace(key, value);
        }
    }
}