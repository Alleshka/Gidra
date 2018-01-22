using System.Windows;
using System.Collections.Generic;
using System;
using GidraSIM.Core.Model;
using System.Runtime.Serialization;

namespace GidraSIM.GUI.Core.BlocksWPF
{
    [DataContract]
    public class ProcedureWPF : SquareBlockWPF
    {
        public const int POINT_MARGIN = 7;

        public int InputCount { get; set; }
        public int OutputCount { get; set; }

        //Входы
        private List<ProcConnectionWPF> inputs;

        // Выходы
        private List<ProcConnectionWPF> outputs;

        // Соединения с ресурсами
        private List<ResConnectionWPF> resputs;

        public IBlock BlockModel {  get; private set; }

        // константы для определения высоты блока

        public ProcedureWPF(Point position,IBlock block) : base(position, block.Description)
        {
            this.InputCount = block.InputQuantity;
            this.OutputCount = block.OutputQuantity;
            this.BlockModel = block;

            this.outputs = new List<ProcConnectionWPF>();
            this.inputs = new List<ProcConnectionWPF>();
            this.resputs = new List<ResConnectionWPF>();

            // проверка корректности inputCount и outputCount (TODO: переписать через исключения)
            if (this.InputCount < 1)
                throw new ArgumentOutOfRangeException("Число входов должно быть от 1 до 10");
            if (this.OutputCount < 1)
                throw new ArgumentOutOfRangeException("Число выходов должно быть от 1 до 10");
            if (this.InputCount > 10)
                throw new ArgumentOutOfRangeException("Число входов должно быть от 1 до 10");
            if (this.OutputCount > 10)
                throw new ArgumentOutOfRangeException("Число выходов должно быть от 1 до 10");

            // перерасчёт высоты блока
            int maxCount = Math.Max(this.InputCount, this.OutputCount);
            if(DEFAULT_HEIGHT < (2*maxCount*POINT_MARGIN + 2*RADIUS))
            {
                SetHeight(2 * maxCount * POINT_MARGIN + 2 * RADIUS);
            }

            // TODO: переписать код рисования точек
            // точки входа
            //MakePoint(inPointFill, DEFAULT_HEIGHT / 2, 0);
            double x = 0;
            double y = (GetHeight() / 2.0) - POINT_MARGIN * (this.InputCount - 1);
            
            for (int i = 0; i < this.InputCount; i++)
            {
                this.Children.Add(new ConnectPointWPF(
                    new Point(x, y),
                    i,
                    InPointFill,
                    ConnectPointWPF_Type.inPut,
                    this));

                y += 2.0 * POINT_MARGIN;
            }

            // точка выхода
            //MakePoint(outPointFill, DEFAULT_HEIGHT / 2, DEFAULT_WIDTH);
            x = DEFAULT_WIDTH;
            y = (GetHeight() / 2.0) - POINT_MARGIN * (this.OutputCount - 1);
            for (int i = 0; i < this.OutputCount; i++)
            {
                this.Children.Add(new ConnectPointWPF(
                    new Point(x, y),
                    i,
                    OutPointFill,
                    ConnectPointWPF_Type.outPut,
                    this));

                y += 2.0 * POINT_MARGIN;
            }
        }

        protected virtual void SetHeight(double height)
        {
            this.bodyGeometry.Rect = new Rect(new Size(DEFAULT_WIDTH, height));
        }

        protected virtual double GetHeight()
        {
            return this.bodyGeometry.Rect.Height;
        }

        protected override void UpdateConnectoins()
        {
            if (inputs != null)
            {
                foreach (ProcConnectionWPF connection in inputs)
                {
                    connection.Refresh();
                }
            }
            if (outputs != null)
            {
                foreach (ProcConnectionWPF connection in outputs)
                {
                    connection.Refresh();
                }
            }
            if (resputs != null)
            {
                foreach (ResConnectionWPF connection in resputs)
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
            inputs.Add(connectoin);
        }

        /// <summary>
        /// Добавить соединение на выход
        /////// </summary>
        /// <param name="connectoin"></param>
        public void AddOutPutConnection(ProcConnectionWPF connectoin)
        {
            outputs.Add(connectoin);
        }

        /// <summary>
        /// Добавить соединение с ресурсом
        /// </summary>
        /// <param name="connectoin"></param>
        public void AddResPutConnection(ResConnectionWPF connectoin)
        {
            resputs.Add(connectoin);
        }
        

        public override void RemoveConnection(ConnectionWPF connection)
        {
            if(connection is ProcConnectionWPF)
            {
                ProcConnectionWPF procConnection = connection as ProcConnectionWPF;

                inputs.Remove(procConnection);
                outputs.Remove(procConnection);
            }
            else if (connection is ResConnectionWPF)
            {
                ResConnectionWPF resConnection = connection as ResConnectionWPF;

                resputs.Remove(resConnection);
            }
        }

        public override void RemoveAllConnections()
        {
            if(inputs != null)
            {
                while (inputs.Count != 0)
                {
                    inputs[0].Remove();
                }
            }

            if (outputs != null)
            {
                while (outputs.Count != 0)
                {
                    outputs[0].Remove();
                }
            }

            if (resputs != null)
            {
                while (resputs.Count != 0)
                {
                    resputs[0].Remove();
                }
            }
        }
    }
}