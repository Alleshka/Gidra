﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Collections.Generic;
using System;

namespace GidraSIM.GUI.Core.BlocksWPF
{
    public class ProcedureWPF : SquareBlockWPF
    {
        public const int POINT_MARGIN = 7;

        //Входы
        private List<ProcConnectionWPF> inputs;

        // Выходы
        private List<ProcConnectionWPF> outputs;

        // Соединения с ресурсами
        private List<ResConnectionWPF> resputs;


        // константы для определения высоты блока

        public ProcedureWPF(Point position, string processName, int inputCount, int outputCount) : base(position, processName)
        {
            this.outputs = new List<ProcConnectionWPF>();
            this.inputs = new List<ProcConnectionWPF>();
            this.resputs = new List<ResConnectionWPF>();

            // проверка корректности inputCount и outputCount (TODO: переписать через исключения)
            if (inputCount < 1) inputCount = 1;
            if (outputCount < 1) outputCount = 1;
            if (inputCount > 10) inputCount = 10;
            if (outputCount > 10) outputCount = 10;

            // перерасчёт высоты блока
            int maxCount = Math.Max(inputCount, outputCount);
            if(DEFAULT_HEIGHT < (2*maxCount*POINT_MARGIN + 2*RADIUS))
            {
                SetHeight(2 * maxCount * POINT_MARGIN + 2 * RADIUS);
            }

            // TODO: переписать код рисования точек
            // точки входа
            //MakePoint(inPointFill, DEFAULT_HEIGHT / 2, 0);
            double x = 0;
            double y = (GetHeight() / 2.0) - POINT_MARGIN * (inputCount - 1);
            
            for (int i = 0; i < inputCount; i++)
            {
                this.Children.Add(new ConnectPointWPF(
                    new Point(x, y),
                    i,
                    inPointFill,
                    ConnectPointWPF_Type.inPut,
                    this));

                y += 2.0 * POINT_MARGIN;
            }

            // точка выхода
            //MakePoint(outPointFill, DEFAULT_HEIGHT / 2, DEFAULT_WIDTH);
            x = DEFAULT_WIDTH;
            y = (GetHeight() / 2.0) - POINT_MARGIN * (outputCount - 1);
            for (int i = 0; i < outputCount; i++)
            {
                this.Children.Add(new ConnectPointWPF(
                    new Point(x, y),
                    i,
                    outPointFill,
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
        /// </summary>
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