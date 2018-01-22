using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace GidraSIM.Core.Model
{
    [DataContract(IsReference =true)]
    public class ConnectionManager : IConnectionManager
    {
        [DataMember(EmitDefaultValue = false)]
        Dictionary<Tuple<IBlock, int>, Tuple<IBlock, int>> connections = new Dictionary<Tuple<IBlock, int>, Tuple<IBlock, int>>();
        [DataMember(EmitDefaultValue = false)]
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


        public override bool Equals(object obj)
        {
            var temp = obj as ConnectionManager;

            if (temp.connections.Count != this.connections.Count)
                return false;
            if (temp.resorcesConnections.Count != this.resorcesConnections.Count)
                return false;

            // FIXME - затратно, хорошо бы переделать по умному
            var connect1 = temp.connections.ToList();
            var connect2 = this.connections.ToList();

            var res1 = temp.resorcesConnections.ToList();
            var res2 = this.resorcesConnections.ToList();

            for (int i = 0; i < connect1.Count; i++)
            {
                if (/*(connect1[i].Key != connect2[i].Key)&&*/(!Equals(connect1[i].Key, connect2[i].Key)))
                    return false;
                if (!Equals(connect1[i].Value, connect2[i].Value))
                    return false;
            }

            for (int i = 0; i < res1.Count; i++)
            {
                if (/*(res1[i].Item1 != res2[i].Item1)&&*/(!Equals(res1[i].Item1, res2[i].Item1)))
                    return false;
                if (/*(res1[i].Item2 != res2[i].Item2)&&*/(!Equals(res1[i].Item2, res2[i].Item2)))
                    return false;
            }

            return true;
        }

        
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
