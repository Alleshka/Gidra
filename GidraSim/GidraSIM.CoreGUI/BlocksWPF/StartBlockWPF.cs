using System.Windows;
using System.Collections.Generic;

namespace GidraSIM.GUI.Core.BlocksWPF
{
    public class StartBlockWPF : RoundBlockWPF
    {
        private const string IMG_SOURCE = "💡";//костыль с пробелом


        // Выходы
        private List<ProcConnectionWPF> outPuts;

        public ICollection<ProcConnectionWPF> ProcedureConnections
        {
            get => outPuts;
        }

        public StartBlockWPF(Point position) : base (position)
        {
            this.outPuts = new List<ProcConnectionWPF>();
            MakeBody(IMG_SOURCE);
        }

        protected override void UpdateConnectoins()
        {
            if(outPuts != null)
            {
                foreach (ProcConnectionWPF connection in outPuts)
                {
                    connection.Refresh();
                }
            }
        }

        /// <summary>
        /// Добавить соединение на выход
        /// </summary>
        /// <param name="connectoin"></param>
        public void AddOutPutConnection(ProcConnectionWPF connectoin)
        {
            outPuts.Add(connectoin);
        }

        public override void RemoveConnection(ConnectionWPF connection)
        {
            if (connection is ProcConnectionWPF)
            {
                ProcConnectionWPF procConnection = connection as ProcConnectionWPF;

                outPuts.Remove(procConnection);
            }
        }

        public override void RemoveAllConnections()
        {
            outPuts?.ForEach(connection => connection.Remove());
            outPuts?.Clear();
        }

        public override void Remove()
        {
            // его нельзя удалить (возможно стоит врубить сюда throw исключения)
        }
    }
}
