﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoCToolbox
{
    public static class Numerics
    {
        /// <summary>
        /// Compute the Greatest Common Divisor of <paramref name="a"/> and <paramref name="b"/>
        /// </summary>
        /// <returns>The GCD of <paramref name="a"/> and <paramref name="b"/></returns>
        public static long Gcd(long a, long b)
        {
            while (a != 0 && b != 0)
            {
                if (a > b)
                {
                    a %= b;
                }
                else
                {
                    b %= a;
                }
            }

            return a | b;
        }

        public static int Gcd(int a, int b)
        {
            while (a != 0 && b != 0)
            {
                if (a > b)
                {
                    a %= b;
                }
                else
                {
                    b %= a;
                }
            }

            return a | b;
        }

        /// <summary>
        /// Compute the Least Common Multiple of <paramref name="a"/> and <paramref name="b"/>
        /// </summary>
        /// <returns>The LCM of <paramref name="a"/> and <paramref name="b"/></returns>
        public static long Lcm(long a, long b)
        {
            return a * b / Gcd(a, b);
        }

        public static int Lcm(int a, int b)
        {
            return a * b / Gcd(a, b);
        }

        /// <summary>
        /// Compute the Least Common Multiple of the provided <paramref name="numbers"/>
        /// </summary>
        /// <returns>The LCM of <paramref name="numbers"/></returns>
        public static long Lcm(ICollection<long> numbers)
        {
            return numbers
                .Skip(1)
                .Aggregate(
                    seed: numbers.First(),
                    func: Lcm);
        }
    }
}
