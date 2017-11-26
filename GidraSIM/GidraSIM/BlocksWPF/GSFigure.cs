﻿using System;
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
        public const int POINT_SIZE = 4; // возможно стоит вывести все константы в одно место


        /// <summary>
        /// Координата верхнего левого угла блока
        /// </summary>
        public Point Position { get; protected set; }

        /// <summary>
        /// Координата центра блока
        /// </summary>
        public virtual Point MidPosition
        {
            get
            {
                return new Point(
                    Position.X + this.ActualWidth / 2,
                    Position.Y + this.ActualHeight / 2);
            }
        }

        /// <summary>
        /// Координата центра левой стороны блока
        /// </summary>
        public virtual Point LeftPosition
        {
            get
            {
                return new Point(
                    Position.X,
                    Position.Y + this.ActualHeight / 2);
            }
        }

        /// <summary>
        /// Координата центра правой стороны блока
        /// </summary>
        public virtual Point RightPosition
        {
            get
            {
                return new Point(
                    Position.X + this.ActualWidth,
                    Position.Y + this.ActualHeight / 2);
            }
        }

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

                this.Effect = shadow;

                IsSelected = true;
            }
        }

        public void UnSelect()
        {
            if (IsSelectable)
            {
                this.Effect = null;

                IsSelected = false;
            }
        }
    }
}