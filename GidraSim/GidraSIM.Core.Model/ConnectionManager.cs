using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GidraSIM.Core.Model
{
    public class ConnectionManager : IConnectionManager
    {
        Dictionary<Tuple<IBlock, int>, Tuple<IBlock, int>> connections = new Dictionary<Tuple<IBlock, int>, Tuple<IBlock, int>>();
        HashSet<Tuple<IProcedure, IResource>> resorcesConnections = new HashSet<Tuple<IProcedure, IResource>>();

        //static ConnectionManager instance = null;
        public ConnectionManager()
        {
            
        }

        /*public static ConnectionManager GetInstance()
        {
            if (instance == null)
                instance = new ConnectionManager();
            return instance;
        }*/

        public void Connect(IBlock block1, int outPort, IBlock block2, int inPort)
        {
            connections.Add(new Tuple<IBlock, int>(block1, outPort), new Tuple<IBlock, int>(block2, inPort));
        }

        public void Connect(IProcedure procedure, IResource resource)
        {
            resorcesConnections.Add(new Tuple<IProcedure, IResource>(procedure, resource));
        }

        public IDictionary<Tuple<IBlock, int>, Tuple<IBlock, int>> GetAllConnections()
        {
            return connections;
        }

        public Tuple<IBlock, int> GetInput(IBlock block, int outPort)
        {
            return connections[new Tuple<IBlock, int>(block, outPort)];
        }

        public Tuple<IBlock, int> GetOutput(IBlock block, int inPort)
        {
            return connections.FirstOrDefault(x => x.Value == new Tuple<IBlock, int>(block, inPort)).Key;
        }

        public void Disconnect(IBlock block1, int outPort)
        {
            connections.Remove(new Tuple<IBlock, int>(block1, outPort));
        }

        public void Disconnect(IProcedure procedure, IResource resource)
        {
            resorcesConnections.Remove(new Tuple<IProcedure, IResource>(procedure, resource));
        }

        public void MoveTokens()
        {
            foreach(var connection in connections)
            {
                var outputBlock = connection.Key.Item1;
                var outputPort = connection.Key.Item2;
                var outputToken = outputBlock.GetOutputToken(outputPort);
                if( outputToken != null)
                {
                    var inputBlock = connection.Value.Item1;
                    var inPort = connection.Value.Item2;
                    inputBlock.AddToken(outputToken, inPort);
                }
            }
        }
    }
}
