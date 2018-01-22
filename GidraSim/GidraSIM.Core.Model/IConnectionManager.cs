using System;
using System.Collections.Generic;

namespace GidraSIM.Core.Model
{
    public interface IConnectionManager
    {
        void Connect(IBlock block1, int outPort, IBlock block2, int inPort);
        void Connect(IProcedure procedure, IResource resource);
        void Disconnect(IBlock block1, int outPort);
        void Disconnect(IProcedure procedure, IResource resource);
        Tuple<IBlock, int> GetInput(IBlock block, int outPort);
        Tuple<IBlock, int> GetOutput(IBlock block, int inPort);
        IDictionary< Tuple<IBlock, int>,Tuple<IBlock, int>> GetAllConnections();

        void MoveTokens();
    }
}
