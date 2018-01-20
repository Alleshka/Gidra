using GidraSim.Model.Processes;

namespace GidraSIM.DataLayer
{
    public interface IProcedureRepository
    {
        Procedure Create(Procedure newProcedure);

        void Delete(int id);

        Procedure Update(Procedure updateProcedure); 



    }
}
