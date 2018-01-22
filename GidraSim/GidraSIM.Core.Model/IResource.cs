namespace GidraSIM.Core.Model
{
    public interface IResource
    {
        bool TryGetResource();
        void ReleaseResource();
        string Description { get; set; }
    }
}
