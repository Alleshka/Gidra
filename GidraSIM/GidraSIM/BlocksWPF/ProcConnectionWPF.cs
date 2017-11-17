using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Windows.Shapes;
using System.Windows.Media;

namespace GidraSIM.BlocksWPF
{
    /// <summary>
    /// Соединение между процессами.
    /// Имеет направление.
    /// </summary>
    public class ProcConnectionWPF : ConnectionWPF
    {
        private PathFigure bezierLinePF = null;
        private PathFigure arrowPF = null;

        private const int ARROW_HEIGHT = 10;
        private const int ARROW_WIDTH = 15;

        public ProcConnectionWPF(BlockWPF startBlock, BlockWPF endBlock) : base(startBlock, endBlock)
        {
            // startBlock неможет быть EndBlockWPF или ResourceWPF
            if ((startBlock is EndBlockWPF) || (startBlock is ResourceWPF))
            {
                throw new ArgumentException("Неверное значение", "startBlock");
            }
            // endBloc неможет быть StartBlockWPF или ResourceWPF
            if ((endBlock is StartBlockWPF) || (startBlock is ResourceWPF))
            {
                throw new ArgumentException("Неверное значение", "endBlock");
            }

            MakeLine();

            // установить ZIndex
            //SetZIngex();
            SetZIndex(this, 1);
        }

        protected override void MakeLine()
        {
            Point startPoint = startBlock.RightPosition;
            Point endPoint = endBlock.LeftPosition;

            if (bezierLinePF == null)
            {
                // path тела линии
                Path linePath = new Path();
                linePath.Stroke = stroke;
                // содержимое path
                
                // кривая
                List<PathSegment> bezierLine = MakeBezierLine(startPoint, endPoint);
                bezierLinePF = new PathFigure(startPoint, bezierLine, false);
                // стрелка 
                List<PathSegment> arrow = MakeArrow(startPoint, endPoint);
                arrowPF = new PathFigure(new Point(endPoint.X - ARROW_WIDTH, endPoint.Y - ARROW_HEIGHT / 2.0), arrow, false);
                
                PathGeometry lineGeometry = new PathGeometry(new List<PathFigure>() { bezierLinePF , arrowPF });
                linePath.Data = lineGeometry;

                // добавление
                this.Children.Add(linePath);
            }
            else
            {
                Refresh();
            }
        }

        private List<PathSegment> MakeBezierLine(Point startPoint, Point endPoint)
        {
            List<PathSegment> result = new List<PathSegment>();

            if (startPoint.X <= endPoint.X)
            {
                result.Add(
                    new BezierSegment(
                        new Point(endPoint.X, startPoint.Y),
                        new Point(startPoint.X, endPoint.Y),
                        new Point(endPoint.X, endPoint.Y),
                        true));
            }
            else
            {
                // TODO: Сделать нормальную кривую
                result.Add(
                    new BezierSegment(
                        new Point(endPoint.X, startPoint.Y),
                        new Point(startPoint.X, endPoint.Y),
                        new Point(endPoint.X, endPoint.Y),
                        true));
            }

            return result;
        }

        private List<PathSegment> MakeArrow(Point startPoint, Point endPoint)
        {
            List<PathSegment> result = new List<PathSegment>();

            result.Add(new LineSegment(new Point(endPoint.X, endPoint.Y), true));
            result.Add(new LineSegment(new Point(endPoint.X - ARROW_WIDTH, endPoint.Y + ARROW_HEIGHT/2.0), true));

            return result;
        }

        public override void Refresh()
        {
            if (bezierLinePF != null)
            {
                Point startPoint = startBlock.RightPosition;
                Point endPoint = endBlock.LeftPosition;

                // кривая
                bezierLinePF.StartPoint = startPoint;
                bezierLinePF.Segments = new PathSegmentCollection(MakeBezierLine(startPoint, endPoint));
                // стрелка 
                arrowPF.StartPoint = new Point(endPoint.X - ARROW_WIDTH, endPoint.Y - ARROW_HEIGHT / 2.0);
                arrowPF.Segments = new PathSegmentCollection(MakeArrow(startPoint, endPoint));
            }
        }
    }
}
