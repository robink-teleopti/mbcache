﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Caching;

namespace MbCache.Configuration
{
	[Serializable]
	public class AspNetCache : ICache
	{
		private readonly int _timeoutMinutes;
		private static readonly Cache _cache = HttpRuntime.Cache;

		public AspNetCache(int timeoutMinutes)
		{
			_timeoutMinutes = timeoutMinutes;
		}

		public object Get(string key)
		{
			return _cache.Get(key);
		}

		public void Put(string key, object value)
		{
			_cache.Insert(key,
							 value,
							 null,
							 DateTime.Now.Add(TimeSpan.FromMinutes(_timeoutMinutes)),
							 Cache.NoSlidingExpiration,
							 CacheItemPriority.Default,
							 null);
		}

		public void Delete(string keyStartingWith)
		{
			var keyList = new List<string>();
			var cacheEnum = _cache.GetEnumerator();
			while (cacheEnum.MoveNext())
			{
				var keyString = cacheEnum.Key.ToString();
				if (keyString.StartsWith(keyStartingWith, StringComparison.Ordinal))
					keyList.Add(keyString);
			}
			foreach (var key in keyList)
			{
				_cache.Remove(key);
			}
		}
	}
}