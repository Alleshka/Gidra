using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Collections.Generic;
using System;

namespace GidraSIM.BlocksWPF
{
    public class ProcedureWPF : BlockWPF
    {
        //Входы
        private List<ProcConnectionWPF> inPuts;

        // Выходы
        private List<ProcConnectionWPF> outPuts;

        // Соединения с ресурсами
        private List<ResConnectionWPF> resPuts;

        public ProcedureWPF(Point position, string processName) : base(position, processName)
        {
            this.outPuts = new List<ProcConnectionWPF>();
            this.inPuts = new List<ProcConnectionWPF>();
            this.resPuts = new List<ResConnectionWPF>();

            // точка входа
            MakePoint(inPointFill, HEIGHT / 2, 0);

            // точка выхода
            MakePoint(outPointFill, HEIGHT / 2, WIDTH);

            // установить ZIndex
            //SetZIngex();          
        }

        protected virtual void MakePoint(Brush fill, double setTop, double setLeft)
        {
            // path точки 
            Path pointPath = new Path();
            pointPath.Fill = fill;
            // позиция
            Canvas.SetTop(pointPath, setTop);
            Canvas.SetLeft(pointPath, setLeft);
            // содержимое path
            pointPath.Data = new EllipseGeometry(new Point(0, 0), POINT_SIZE, POINT_SIZE);
            // добавление
            this.Children.Add(pointPath);
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
            if (outPuts != null)
            {
                foreach (ProcConnectionWPF connection in outPuts)
                {
                    connection.Refresh();
                }
            }
            if (resPuts != null)
            {
                foreach (ResConnectionWPF connection in resPuts)
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

        /// <summary>
        /// Добавить соединение на выход
        /// </summary>
        /// <param name="connectoin"></param>
        public void AddOutPutConnection(ProcConnectionWPF connectoin)
        {
            outPuts.Add(connectoin);
        }

        /// <summary>
        /// Добавить соединение с ресурсом
        /// </summary>
        /// <param name="connectoin"></param>
        public void AddResPutConnection(ResConnectionWPF connectoin)
        {
            resPuts.Add(connectoin);
        }
    }
}
