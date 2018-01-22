namespace GidraSIM.Core.Model
{
    public interface IBlock
    {
        void AddToken(Token token, int inputNumber);
        //bool AllInputesFilled();

        int OutputQuantity { get;}
        int InputQuantity { get;}
        // void Connect(int outputNumber, IBlock block, int blockInputNumber);
        void Update(ModelingTime modelingTime);
        Token GetOutputToken(int port);
        void ClearOutputs();
        void CleaInputs();
        string Description { get; }
    }
}
