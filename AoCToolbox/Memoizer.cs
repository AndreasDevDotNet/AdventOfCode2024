﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoCToolbox
{
    public static class Memoizer
    {
        public static Func<R> Memoize<R>(Func<R> func)
        {
            object cache = null;
            return () =>
            {
                if (cache == null)
                    cache = func();
                return (R)cache;
            };
        }

        public static Func<A, R> Memoize<A, R>(Func<A, R> func)
        {
            var cache = new Dictionary<A, R>();
            return a =>
            {
                if (cache.TryGetValue(a, out R value))
                    return value;
                value = func(a);
                cache.Add(a, value);
                return value;
            };
        }

        public static Func<A, R> ThreadSafeMemoize<A, R>(Func<A, R> func)
        {
            var cache = new ConcurrentDictionary<A, R>();
            return argument => cache.GetOrAdd(argument, func);
        }
    }

    public static class MemoizerExtensions
    {
        public static Func<R> Memoize<R>(this Func<R> func)
        {
            return Memoizer.Memoize(func);
        }

        public static Func<A, R> Memoize<A, R>(this Func<A, R> func)
        {
            return Memoizer.Memoize(func);
        }

        public static Func<A, R> ThreadSafeMemoize<A, R>(this Func<A, R> func)
        {
            return Memoizer.ThreadSafeMemoize(func);
        }
    }
}
