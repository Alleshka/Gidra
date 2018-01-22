using System.Windows;
using System.Collections.Generic;

namespace GidraSIM.GUI.Core.BlocksWPF
{
    public class EndBlockWPF : RoundBlockWPF
    {
        private const string IMG_SOURCE = "✔";


        //Входы
        private List<ProcConnectionWPF> inPuts;

        public ICollection<ProcConnectionWPF> ProcedureConnections
        {
            get => inPuts;
        }

        public EndBlockWPF(Point position) : base(position)
        {
            this.inPuts = new List<ProcConnectionWPF>();
            MakeBody(IMG_SOURCE);
        }

        protected override void UpdateConnectoins()
        {
            if (inPuts != null)
            {
                foreach (ProcConnectionWPF connection in inPuts)
                {
                    connection.Refresh();
                }
            }
        }

        /// <summary>
        /// Добавить соединение на вход
        /// </summary>
        /// <param name="connectoin"></param>
        public void AddInPutConnection(ProcConnectionWPF connectoin)
        {
            inPuts.Add(connectoin);
        }

        public override void RemoveConnection(ConnectionWPF connection)
        {
            if (connection is ProcConnectionWPF)
            {
                ProcConnectionWPF procConnection = connection as ProcConnectionWPF;

                inPuts.Remove(procConnection);
            }
        }

        public override void RemoveAllConnections()
        {
            inPuts?.ForEach(connection => connection.Remove());
            inPuts?.Clear();
        }

        public override void Remove()
        {
            // его нельзя удалить (возможно стоит врубить сюда throw исключения)
        }
    }
}
