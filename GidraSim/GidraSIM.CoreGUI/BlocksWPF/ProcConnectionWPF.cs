using System;
using System.Windows;
using System.Collections.Generic;
using System.Windows.Shapes;
using System.Windows.Media;

namespace GidraSIM.GUI.Core.BlocksWPF
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

        private const int R = 10;
        private const int dX = 20;
        private const int dY = 80;

        /// <summary>
        /// Позиция начала относительно координат блока
        /// </summary>
        private Point relativeStartPosition;

        /// <summary>
        /// Позиция конца относительно координат блока
        /// </summary>
        private Point relativeEndPosition;

        public Point RelateStart => relativeStartPosition;
        public Point RelateEnd => relativeEndPosition;

        public int StartPort { get; private set; }

        public int EndPort { get; private set; }

        public ProcConnectionWPF(
            BlockWPF startBlock, 
            BlockWPF endBlock, 
            Point relativeStartPosition, 
            Point relativeEndPosition,
            int startPort = 0,
            int endPort = 0) : base(startBlock, endBlock)
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

            this.relativeStartPosition = relativeStartPosition;
            this.relativeEndPosition = relativeEndPosition;

            this.StartPort = startPort;
            this.EndPort = endPort;

            MakeLine();
            
            SetZIndex(this, 1);
        }

        protected override void MakeLine()
        {
            Point startPoint = startBlock.Position + (Vector)relativeStartPosition;
            Point endPoint = endBlock.Position + (Vector)relativeEndPosition;

            if (bezierLinePF == null)
            {
                // path тела линии
                Path linePath = new Path();
                linePath.Stroke = Stroke;
                linePath.StrokeThickness = THICKNESS;
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
                double minY = Math.Min(startPoint.Y, endPoint.Y);

                result.Add(
                    new LineSegment(
                        new Point(startPoint.X + dX - R, startPoint.Y), 
                        true));
                result.Add(
                    new ArcSegment(
                        new Point(startPoint.X + dX, startPoint.Y - R),
                        new Size(R, R),
                        0,
                        false,
                        SweepDirection.Counterclockwise,
                        true));
                result.Add(
                    new LineSegment(
                        new Point(startPoint.X + dX , minY - dY + R),
                        true));
                result.Add(
                    new ArcSegment(
                        new Point(startPoint.X + dX - R, minY - dY),
                        new Size(R, R),
                        0,
                        false,
                        SweepDirection.Counterclockwise,
                        true));
                result.Add(
                    new LineSegment(
                        new Point(endPoint.X - dX + R, minY - dY),
                        true));
                result.Add(
                    new ArcSegment(
                        new Point(endPoint.X - dX, minY - dY + R),
                        new Size(R, R),
                        0,
                        false,
                        SweepDirection.Counterclockwise,
                        true));
                result.Add(
                    new LineSegment(
                        new Point(endPoint.X - dX, endPoint.Y - R),
                        true));
                result.Add(
                    new ArcSegment(
                        new Point(endPoint.X - dX + R, endPoint.Y),
                        new Size(R, R),
                        0,
                        false,
                        SweepDirection.Counterclockwise,
                        true));
                result.Add(
                    new LineSegment(
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
                Point startPoint = startBlock.Position + (Vector)relativeStartPosition;
                Point endPoint = endBlock.Position + (Vector)relativeEndPosition;

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
