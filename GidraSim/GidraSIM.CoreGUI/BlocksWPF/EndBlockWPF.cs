using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Collections.Generic;

namespace GidraSIM.GUI.Core.BlocksWPF
{
    public class EndBlockWPF : RoundBlockWPF
    {
        private const string IMG_SOURCE = "/Image/End.png";


        //Входы
        private List<ProcConnectionWPF> inPuts;

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
    }
}
