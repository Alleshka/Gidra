using System.Windows.Controls;

namespace GidraSIM.GUI.Core.BlocksWPF
{
    /// <summary>
    /// Абстрактный класс соединений между блоками
    /// </summary>
    public abstract class ConnectionWPF : GSFigure
    {
        // Константные параметры блока
        protected const int THICKNESS = 2;
        protected const int ZINDEX = 1;

        // блоки, к которым соединяются
        protected BlockWPF startBlock;
        protected int startBlockPort;
        protected BlockWPF endBlock;
        protected int endBlockPort;

        public ConnectionWPF(BlockWPF startBlock, BlockWPF endBlock)
        {
            this.startBlock = startBlock;
            this.endBlock = endBlock;
        }

        protected abstract void MakeLine();

        public abstract void Refresh();

        public BlockWPF StartBlock
        {
            get => startBlock;
        }

        public BlockWPF EndBlock
        {
            get => endBlock;
        }

        public void ClearConnections()
        {
            // удалить ссылку в начальном блоке
            startBlock.RemoveConnection(this);

            //  удалить ссылку в конечном блоке
            endBlock.RemoveConnection(this);
        }

        /// <summary>
        ///  удаляет текущий блок из родительского канваса
        /// </summary>
        public override void Remove()
        {
            Canvas parrent = FindVisualParent<Canvas>(this.Parent);

            this.ClearConnections();

            parrent.Children.Remove(this);
        }
    }
}
