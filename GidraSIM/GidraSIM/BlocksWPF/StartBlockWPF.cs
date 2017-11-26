using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Collections.Generic;

namespace GidraSIM.BlocksWPF
{
    public class StartBlockWPF : RoundBlockWPF
    {
        private const string IMG_SOURCE = "/Image/Begin.png";


        // Выходы
        private List<ProcConnectionWPF> outPuts;

        

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
    }
}
