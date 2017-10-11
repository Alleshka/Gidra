using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using CommonData;  //пространство структур системы

namespace GidraSIM
{
    public class Find
    {
//поиск блока
        public int WhatBlock(List<BlockObject> images_in_tabItem, Point point)  //ищем, на какой блок нажали
        {
            Point pointCurr = new Point();
            for (int i = 0; i < images_in_tabItem.Count; i++) //ищем по списку блоков в вкладке
            {
                pointCurr = images_in_tabItem[i].object_of_block.point;
                if ((point.X > pointCurr.X - (double)Size_Block.WIDTH_BLOCK / 2) && (point.X < pointCurr.X + (double)Size_Block.WIDTH_BLOCK / 2))
                    if ((point.Y > pointCurr.Y - (double)Size_Block.HEIGHT_BLOCK / 2) && (point.Y < pointCurr.Y + (double)Size_Block.HEIGHT_BLOCK / 2))
                        return i;
            }
            return -1;  //если не нашел, на какой блок нажали
        }

//поиск связи
        public int WhatConnect(List<Connection_Line> lines, Point point)  //ищем, на какую связь нажали
        {
            for (int i = 0; i < lines.Count; i++) //ищем по всем линиям
                if (PointOnLine(lines[i], point))
                   return i;
            return -1;  //если не нашел, на какую связь нажали
        }

//пренадлежит ли точка линии
        public bool PointOnLine(Connection_Line line, Point point)
        {
            double k1 = (line.object_line.Y2 - line.object_line.Y1) * point.X;
            double k2 = (line.object_line.X2 - line.object_line.X1) * point.Y;
            double k3 = line.object_line.X2 * line.object_line.Y1 - line.object_line.Y2 * line.object_line.X1;
            double k4 = Math.Pow((line.object_line.Y2 - line.object_line.Y1),2);
            double k5 = Math.Pow((line.object_line.X2 - line.object_line.X1),2);
            double D = Math.Abs(k1 - k2 + k3) / Math.Sqrt(k4 + k5);

            if (D <= 25)
                if (((line.object_line.X1 < point.X) && (point.X < line.object_line.X2)) ||        //проверка границ прямой
                    ((line.object_line.X2 < point.X) && (point.X < line.object_line.X1)))
                    return true;
            return false;
        }
    }
}
