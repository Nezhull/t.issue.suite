using System;
using System.Collections.Generic;
using System.Linq;
using T.Issue.Commons.Utils;

namespace T.Issue.Commons.AppFeatures
{
    public class AppFeatureService<TKey> : IAppFeatureService<TKey>
    {
        private readonly IAppFeatureProvider<TKey, IAppFeature> featureProvider;

        public AppFeatureService(IAppFeatureProvider<TKey, IAppFeature> featureProvider)
        {
            Assert.NotNull(featureProvider);

            this.featureProvider = featureProvider;
        }

        public bool IsEnabled(TKey key)
        {
            Assert.NotNull(key);

            IAppFeature feature = featureProvider.GetFeature(key);
            return feature != null && feature.IsEnabled();
        }

        public bool IsAllEnabled(params TKey[] keys)
        {
            Assert.IsNotEmpty(keys);

            IList<IAppFeature> features = GetFeatures(keys);
            return features.All(f => f.IsEnabled());
        }

        public bool IsAnyEnabled(params TKey[] keys)
        {
            Assert.IsNotEmpty(keys);

            IList<IAppFeature> features = GetFeatures(keys);
            return features.Any(f => f.IsEnabled());
        }

        private IList<IAppFeature> GetFeatures(TKey[] keys)
        {
            return keys.Select(k => featureProvider.GetFeature(k)).Where(f => f != null).ToList();
        }
    }
}
