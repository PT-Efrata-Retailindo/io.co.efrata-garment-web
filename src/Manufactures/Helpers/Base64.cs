using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Manufactures.Helpers
{
    public static class Base64
    {
        public static string GetBase64File(string encoded)
        {
            return encoded.Substring(encoded.IndexOf(',') + 1);
        }

        public static string GetBase64Type(string encoded)
        {
            Regex regex = new Regex(@"data:([a-zA-Z0-9]+\/[a-zA-Z0-9-.+]+).*,.*");
            string match = regex.Match(encoded).Groups[1].Value;

            return match == null && match == String.Empty ? "image/jpeg" : match;
        }
    }
}
