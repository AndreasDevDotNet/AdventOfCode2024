using System.Numerics;
using System.Xml.Linq;
namespace AoCToolbox;

public static class NumberExtensions
{
    public static T Modulo<T>(this T a, T modulus) where T : INumber<T>
    {
        return (a % modulus + modulus) % modulus;
    }

    public delegate int Power(int n);

    public static int BinaryToDecimal(this string binaryNumberString)
    {
        Power power = null;
        power = delegate (int n)
        {
            if (n == 0) return 1;
            else return 2 * power(n - 1);
        };

        string s1 = binaryNumberString;
        string s2 = "";
        for (int i = s1.Length - 1; i >= 0; i--)
        {
            s2 += s1[i];
        }
        int sum = 0;
        for (int i = 0; i < s2.Length; i++)
        {
            int res = power(i);
            sum = sum + (res * int.Parse(s2[i].ToString()));
        }
        return sum;
    }

    public static int GetWrappedIndex(int index, int max)
    {
        while (index >= max) index -= max;
        while (index < 0) index += max;
        return index;
    }

    public static bool IsWholeNumber(this double number)
    {
        return Math.Abs(number - Math.Round(number)) < double.Epsilon;
    }
}