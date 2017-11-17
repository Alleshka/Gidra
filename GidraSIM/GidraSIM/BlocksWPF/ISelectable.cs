﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GidraSIM.BlocksWPF
{
    /// <summary>
    /// Определяет функционал выделения фигуры
    /// </summary>
    public interface ISelectable
    {
        /// <summary>
        /// Можно ли выделять фигуру
        /// </summary>
        bool IsSelectable { get; set; }

        /// <summary>
        /// Выделить фигуру
        /// </summary>
        void Select();

        /// <summary>
        /// Снять выделение
        /// </summary>
        void UnSelect();
    }
}
