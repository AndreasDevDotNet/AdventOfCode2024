using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AoCToolbox
{
    public static class StringExtensions
    {
        private static readonly Regex NumberRegex = new(pattern: @"(-?\d+)");
        private static readonly Regex WhitespaceRegex = new(pattern: @"\s+");

        public static IList<int> ParseInts(this string str)
        {
            var matches = NumberRegex.Matches(str);
            var numbers = new List<int>(matches.Select(m => int.Parse(m.Value)));

            return numbers;
        }

        public static int ParseInt(this string str)
        {
            return ParseInts(str)[0];
        }

        public static ulong ParseULong(this string value)
        {
            return ulong.Parse(value);
        }

        public static uint ParseUInt(this string value)
        {
            return uint.Parse(value);
        }

        public static long ParseLong(this string str)
        {
            return ParseLongs(str)[0];
        }

        public static IList<long> ParseLongs(this string str)
        {
            var matches = NumberRegex.Matches(str);
            var numbers = new List<long>(matches.Select(m => long.Parse(m.Value)));

            return numbers;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int AsDigit(this char c)
        {
            return c - '0';
        }

        public static int NextIndexOf(this string str, int startIndex, char charToSearch)
        {
            for (int i = startIndex; i < str.Length; i++)
            {
                if (str[i] == charToSearch)
                    return i;
            }

            return -1;
        }

        public static bool IsAnyOf(this char c, char[] chars)
        {
            for (int i = 0; i < chars.Length; i++)
            {
                if (c == chars[i])
                    return true;
            }
            return false;
        }

        public static bool IsAnyOf(this string s, string[] strings)
        {
            for (int i = 0; i < strings.Length; i++)
            {
                if (s.Contains(strings[i]))
                    return true;
            }
            return false;
        }

        public static string RemoveWhitespace(this string str)
        {
            return WhitespaceRegex.Replace(input: str, replacement: string.Empty);
        }

        public static T ParseNumber<T>(this string s) where T : INumber<T>
        {
            return ParseNumbers<T>(s)[0];
        }

        public static T[] ParseNumbers<T>(this string s) where T : INumber<T>
        {
            return NumberRegex.Matches(s)
                .Select(m => T.Parse(s: m.Value.AsSpan(), provider: null))
                .ToArray();
        }

        /// <summary>
        /// Extracts all "Words" (including xnoppyt) from a string
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static IEnumerable<string> ExtractWords(this string str)
        {
            return Regex.Matches(str, "[a-zA-z]+").Select(a => a.Value);
        }
        public static IEnumerable<int> ExtractInts(this string str)
        {
            return Regex.Matches(str, "-?\\d+").Select(m => int.Parse(m.Value));
        }
        public static IEnumerable<long> ExtractLongs(this string str)
        {
            return Regex.Matches(str, "-?\\d+").Select(m => long.Parse(m.Value));
        }

        public static bool IsInteger(this string value)
        {
            return int.TryParse(value, out int intVal);
        }

        public static string SubstringReverse(this string source, int startIndex, int length)
        {
            var subStr = "";

            for (var i = startIndex; i >= length;i--)
            {
                subStr += source[i];
            }

            return subStr;
        }

        public static string RemoveSubstring(this string source, int startIndex, int length)
        {
            var arr = source.ToList();

            for (var i = startIndex; i < source.Length && length > 0;i++,length--)
            {
                arr.RemoveAt(i);
            }

            return new string(arr.ToArray());
        }

        public static string SwapChars(this string source, int indexA, int indexB)
        {
            var arr = source.ToArray();

            var c1 = arr[indexA];
            var c2 = arr[indexB];

            arr[indexB] = c1;
            arr[indexA] = c2;

            return new string(arr);
        }

        public static string SwapChars(this string source, char charA, char charB)
        {
            var arr = source.ToArray();

            var indexA = source.IndexOf(charA);
            var indexB = source.IndexOf(charB);

            var c1 = arr[indexA];
            var c2 = arr[indexB];

            arr[indexB] = c1;
            arr[indexA] = c2;

            return new string(arr);
        }
    }
}
