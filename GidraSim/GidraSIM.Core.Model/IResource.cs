namespace GidraSIM.Core.Model
{
    public interface IResource
    {
        bool TryGetResource();
        bool TryUseResource(ModelingTime time);
        void ReleaseResource();
        string Description { get; set; }
    }
}
