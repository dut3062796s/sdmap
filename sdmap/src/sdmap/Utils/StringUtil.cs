﻿using sdmap.Functional;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace sdmap.Utils
{
    internal class StringUtil
    {
        public static Result<string> Parse(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return Result.Fail<string>($"string literial must not be empty.");

            if (input[0] == '@')
            {
                return AtStringParse(input);
            }
            else
            {
                return RegularStringParse(input);
            }
        }

        private static Result<string> RegularStringParse(string input)
        {
            if (input.Length < 2)
                return Result.Fail<string>($"string literial length must > 2.");

            if (input.First() != input.Last())
                return Result.Fail<string>($"string literial's first and last char must be the same.");

            var firstChar = input[0];
            var escapedChar = Details.GetEscapedChar(firstChar);
            if (escapedChar.IsFailure)
                return Result.Fail<string>(escapedChar.Error);

            var inner = input.Substring(1, input.Length - 2);
            return Result.Ok(Details.EscapeNoCheck(inner, escapedChar.Value));
        }

        private static Result<string> AtStringParse(string input)
        {
            if (input.Length < 3)
                return Result.Fail<string>($"AtString literial length must > 3.");

            var inner = input.Substring(2, input.Length - 3);
            var escaped = inner.Replace("\"\"", "\"");
            return Result.Ok(escaped);
        }

        public class Details
        {
            public static Result<char> GetEscapedChar(char ch)
            {
                if (ch == '\'')
                {
                    return Result.Ok('"');
                }
                else if (ch == '"')
                {
                    return Result.Ok('\'');
                }
                else
                {
                    return Result.Fail<char>($"char {ch} not escappable.");
                }
            }

            public static string EscapeNoCheck(string input, char escapeChar)
            {
                var unicodeRegex = new Regex(@"\\[uU]([0-9A-Fa-f]{4})");

                var result = input
                    .Replace(@"\\", @"\")
                    .Replace(@"\/", @"/")
                    .Replace(@"\t", "\t")
                    .Replace(@"\b", "\b")
                    .Replace(@"\f", "\f")
                    .Replace(@"\n", "\n")
                    .Replace(@"\r", "\r")
                    .Replace(@"\t", "\t")
                    .Replace($@"\{escapeChar}", escapeChar.ToString());

                return unicodeRegex.Replace(result, match =>
                    ((char)int.Parse(match.Value.Substring(2), NumberStyles.HexNumber)).ToString());
            }
        }
    }
}
