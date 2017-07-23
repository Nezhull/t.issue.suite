
namespace T.Issue.Commons.AppFeatures
{
    public interface IAppFeatureService<in TKey>
    {
        bool IsEnabled(TKey key);
        bool IsAllEnabled(params TKey[] keys);
        bool IsAnyEnabled(params TKey[] keys);
    }
}
