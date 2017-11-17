using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Diagnostics;
using System.Windows.Forms;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace GidraSIM.BlocksWPF
{
    /// <summary>
    /// Обший родительский класс для блоков и их соединений
    /// </summary>
    public abstract class GSFigure : Canvas, ISelectable
    {
        protected static Brush stroke = Brushes.Black;

        private bool isSelectable;

        // параметры тени выделенной фигуры
        private Color shadowColor = Colors.Purple;
        private const int SHADOW_BLUR_RADIUS = 10;

        /// <summary>
        /// Можно ли выделять фигуру
        /// </summary>
        public bool IsSelectable
        {
            get
            {
                return isSelectable;
            }

            set
            {
                isSelectable = value;
            }
        }

        /// <summary>
        /// Выделена ли фигура
        /// </summary>
        public bool IsSelected { get; private set; }

        public void Select()
        {
            if (IsSelectable)
            {
                DropShadowEffect shadow = new DropShadowEffect();
                shadow.Color = shadowColor;
                shadow.BlurRadius = SHADOW_BLUR_RADIUS;

                foreach(UIElement body in this.Children)
                {
                    if (!(body is TextBlock))
                    {
                        body.Effect = shadow;
                    }
                }

                IsSelected = true;
            }
        }

        public void UnSelect()
        {
            if (IsSelectable)
            {
                foreach (UIElement body in this.Children)
                {
                    if(!(body is TextBlock))
                    {
                        body.Effect = null;
                    } 
                }

                IsSelected = false;
            }
        }
    }
}
