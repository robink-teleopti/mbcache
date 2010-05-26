using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MbCache.Core;
using MbCache.Logic;

namespace MbCache.Configuration
{
    public class CacheBuilder
    {
        private readonly IDictionary<Type, ImplementationAndMethods> _cachedMethods;

        public CacheBuilder()
        {
            _cachedMethods = new Dictionary<Type, ImplementationAndMethods>();
        }

        public IMbCacheFactory BuildFactory(ICacheFactory cacheFactory, IMbCacheKey keyBuilder)
        {
            return new MbCacheFactory(cacheFactory.Create(), keyBuilder, _cachedMethods);
        }


        public IFluentBuilder<T> ForClass<T>(params object[] ctorParameters)
        {
            return createFluentBuilder<T>(null, ctorParameters);
        }

        public IFluentBuilder<T> ForInterface<T>(object implementation)
        {
            return createFluentBuilder<T>(implementation, new object[0]);
        }

        private IFluentBuilder<T> createFluentBuilder<T>(object implementation, params object[] ctorParameters)
        {
            var type = typeof (T);
            if (_cachedMethods.ContainsKey(type))
                throw new ArgumentException("Type " + type + " is already in CacheBuilder");
            var implAndDetails = new ImplementationAndMethods(ctorParameters, implementation);
            _cachedMethods[type] = implAndDetails;
            var fluentBuilder = new FluentBuilder<T>(implAndDetails);
            return fluentBuilder;
        }
    }
}