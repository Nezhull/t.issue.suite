namespace T.Issue.Commons.AppFeatures
{
    public interface IAppFeatureProvider<in TKey, out TFeature> where TFeature : IAppFeature 
    {
        TFeature GetFeature(TKey feature);
    }
}
