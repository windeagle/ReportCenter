using System;
using System.Collections.Generic;
using System.Linq;

namespace DianPing.BA.ReportCenter.Domain.Util
{
    public static class CacheHelper
    {
        private static readonly Dictionary<string, KeyValuePair<DateTime, object>> CacheDict =
            new Dictionary<string, KeyValuePair<DateTime, object>>();

        private static readonly object LockObj = new object();

        public static void SetCache(string key, object objToCache, DateTime time)
        {
            lock (LockObj)
            {
                if (CacheDict.ContainsKey(key))
                {
                    CacheDict[key] = new KeyValuePair<DateTime, object>(time, objToCache);
                }
                else
                {
                    CacheDict.Add(key, new KeyValuePair<DateTime, object>(time, objToCache));
                }
            }
        }

        public static void SetCache<T, TK>(string key, Func<DateTime, bool> condition, IEnumerable<T> objToCache,
                                           Func<T, TK> keyFunc, DateTime time)
        {
            var objToCacheList = objToCache == null ? new List<T>() : objToCache.ToList();
            lock (LockObj)
            {
                List<T> listToCache = new List<T>();

                object cacheObj;
                if (GetFromCache(key, condition, out cacheObj))
                {
                    var listCache = cacheObj as IEnumerable<T>;
                    if (listCache != null)
                    {
                        listToCache.AddRange(listCache);
                    }
                }

                var tmpList = listToCache.Join(objToCacheList, keyFunc, keyFunc,
                                               (t, c) => c);
                var exceptList = objToCacheList.Except(tmpList);
                listToCache.AddRange(exceptList);

                CacheDict[key] = new KeyValuePair<DateTime, object>(time, listToCache);
            }
        }

        public static bool GetFromCache(string key, Func<DateTime, bool> condition, out object cacheObj)
        {
            cacheObj = null;
            if (CacheDict.ContainsKey(key))
            {
                var value = CacheDict[key];
                if (condition(value.Key))
                {
                    cacheObj = value.Value;
                    return true;
                }
            }
            return false;
        }
    }
}