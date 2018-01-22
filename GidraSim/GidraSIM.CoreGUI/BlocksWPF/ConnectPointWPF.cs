using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GidraSIM.GUI.Core.BlocksWPF
{
    public class ConnectPointWPF : GSFigure
    {

        public ConnectPointWPF_Type ConnectType { get; private set; }

        /// <summary>
        /// Блок в котором находится эта точка
        /// </summary>
        public ProcedureWPF Owner { get; private set; }

        public int Port { get; private set; }

        public ConnectPointWPF(Point position, int port, Brush fill, ConnectPointWPF_Type connectType, ProcedureWPF owner)
        {
            this.Position = position;
            this.Port = port;
            this.ConnectType = connectType;
            this.Owner = owner;

            this.IsSelectable = true;

            // path точки 
            Path pointPath = new Path();
            pointPath.Fill = fill;
            // позиция
            Canvas.SetTop(this, Position.Y);
            Canvas.SetLeft(this, Position.X);
            // содержимое path
            pointPath.Data = new EllipseGeometry(new Point(0, 0), POINT_SIZE, POINT_SIZE);
            // добавление
            this.Children.Add(pointPath);
        }

        public override void Remove()
        {
            // его нельзя удалить (возможно стоит врубить сюда throw исключения)
        }
    }

    public enum ConnectPointWPF_Type { inPut, outPut}
}
