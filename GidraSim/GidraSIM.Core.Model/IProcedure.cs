namespace GidraSIM.Core.Model
{
    public interface IProcedure:IBlock
    {
        void AddResorce(IResource resource);
        void ClearResources();
    }
}
