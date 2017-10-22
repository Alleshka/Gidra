using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;
using System.Windows.Media.Effects;
using System.Windows.Controls;
using CommonData;

namespace GidraSIM
{
    public class DrawShema
    {
        /// <summary>
        /// Поле в котором произволится отрисовка графики
        /// </summary>
        Canvas scene;

        /// <summary>
        /// Элементы которые нужно отрисовать
        /// </summary>
        List<BlockObject> blocks;

        /// <summary>
        /// Иниализация поля и элементов
        /// </summary>
        /// <param name="canva">Поле в котором произволится отрисовка графики</param>
        /// <param name="new_blocks">Элементы которые нужно отрисовать</param>
        public void ChangeCanva(Canvas canva, ref List<BlockObject> new_blocks)
        {
            scene = canva;
            blocks = new_blocks;
        }

        /// <summary>
        /// Иниализация элементов
        /// </summary>
        /// <param name="new_blocks">Элементы которые нужно отрисовать</param>
        public void ChangeImagesInTabItem(ref List<BlockObject> new_blocks)
        {
            blocks = new_blocks;
        }

        /// <summary>
        /// Создание нового элемента
        /// </summary>
        /// <param name="type"></param>
        /// <param name="point"></param>
        /// <param name="number_in_type"></param>
        /// <param name="name_subprocess"></param>
        public void CreateImage(ObjectTypes type, Point point, int number_in_type, string name_subprocess)
        {
            string Way = "";
            string Name = "";
            Image image = new Image(); //создаем новый имейдж 
            image.Height = (double)Size_Block.HEIGHT_BLOCK;
            image.Width = (double)Size_Block.WIDTH_BLOCK;

            switch (type)
            {
                case ObjectTypes.PROCEDURE: //процедурыс
                    {
                        Way = "/Image/Procedure.png";
                        Name = "Процедура" + Convert.ToString(number_in_type + 1);
                    }
                    break;
                case ObjectTypes.RESOURCE: //ресурсы
                    {
                        Way = "/Image/Resourse_blok.png";
                        Name = "Ресурс";
                    }
                    break;
                case ObjectTypes.BEGIN: //начало
                    {
                        Way = "/Image/Begin.png";
                        Name = "";
                        image.Height = image.Width = (double)Size_Block.SIZE_BLOCK_BE;//смена размеров для начала и конца
                    }
                    break;
                case ObjectTypes.END: //конец
                    {
                        Way = "/Image/End.png";
                        Name = "";
                        image.Height = image.Width = (double)Size_Block.SIZE_BLOCK_BE;//смена размеров для начала и конца
                    }
                    break;
                case ObjectTypes.SUBPROCESS: //подпроцесс
                    {
                        Way = "/Image/sub_process.png";
                        Name = Convert.ToString(name_subprocess);
                    }
                    break;
            }

            Canvas.SetLeft(image, point.X - image.Width / 2); //устанавливаем размеры канвы 
            Canvas.SetTop(image, point.Y - image.Height / 2);
            Canvas.SetZIndex(image, 1);
            image.Stretch = Stretch.Uniform; //подгоняем изображение по размеру канвы 

            image.Source = new BitmapImage(new Uri(Way, UriKind.Relative));

            TextBlock label = new TextBlock(); //создаем новый лэйбл 
            label.Text = Name;
            label.TextWrapping = TextWrapping.Wrap;
            label.Width = image.Width - 3;
            if (type == ObjectTypes.SUBPROCESS)
            {
                Canvas.SetLeft(label, point.X - image.Width / 2 + 15); //устанавливаем положение лейбла на канве 
                Canvas.SetTop(label, point.Y - image.Height / 4 + 10);
            }
            else
            {
                Canvas.SetLeft(label, point.X - image.Width / 2 + 5); 
                Canvas.SetTop(label, point.Y - image.Height / 4);
            }
            Canvas.SetZIndex(label, 2);

            StructureObject structure = new StructureObject();
            structure.Type = type;
            structure.point = point;
            structure.number = number_in_type;

            BlockObject block = new BlockObject();
            block.image = image;
            block.label = label;
            block.object_of_block = structure;

            blocks.Add(block);
        }

        //создание начала и конца  
        public void AddBeginEnd(double TabControlWidth, double TabControlHeight)
        {
            //координаты лампочкии и галочки
            int w = 50;
            Point point = new Point(w / 2, TabControlHeight / 2);
            CreateImage(ObjectTypes.BEGIN, point, -2, null);

            point = new Point(TabControlWidth - w / 2, TabControlHeight / 2);
            CreateImage(ObjectTypes.END, point, -2, null);
        }

        //добавляем блок
        public void AddBlock(ObjectTypes type, Point point, int number_in_type, string name_subprocess)
        {
            CreateImage(type, point, number_in_type, name_subprocess);
        }

        //создание линии
        public Line DrawLine(Point point1, Point point2)
        {
            Line line = new Line();
            line.X1 = point1.X;
            line.Y1 = point1.Y;
            line.X2 = point2.X;
            line.Y2 = point2.Y;
            line.Stroke = System.Windows.Media.Brushes.Purple;
            line.StrokeThickness = 2;
            Canvas.SetZIndex(line, 0);

            return line;
        }

        //снять выделение блока
        public void DropShadowBlock(int number)
        {
            if (number != -1)
                if (blocks.Count > 0)
                    if (number < blocks.Count)
                        blocks[number].image.Effect = null;
        }

        //выделлить блок
        public void SetShadowBlock(int number, int previous_block)
        {
            DropShadowEffect shadow = new DropShadowEffect();
            shadow.Color = Colors.Purple;
            shadow.BlurRadius = 10;
            shadow.Opacity = 1;

            blocks[number].image.Effect = shadow;
            if (number != previous_block)
                DropShadowBlock(previous_block);
        }

        //снять выделение связи
        public void DropShadowLine(ref List<Connection_Line> line, int number)
        {
            if (number != -1)
                if (line.Count > 0)
                    line[number].object_line.Effect = null;
        }

        //выделлить связь
        public void SetShadowLine(ref List<Connection_Line> line, int number, int previous_line)
        {
            DropShadowEffect shadow = new DropShadowEffect();
            shadow.Color = Colors.Purple;
            shadow.BlurRadius = 14;
            shadow.Opacity = 3;

            line[number].object_line.Effect = shadow;
            if (number != previous_line)
                DropShadowLine(ref line, previous_line);
        }

        //убрать блок с поля
        public void ClearBlock(int number_to_delete)
        {
            blocks[number_to_delete].image.Source = null;
            blocks[number_to_delete].label.Text = null;
        }
        //переключение лампочки
        public void SetLight(bool light)//true - горит, false - не горит
        {
            if (light)
                blocks[0].image.Source = new BitmapImage(new Uri("/Image/begin_model.png", UriKind.Relative));
            else
                blocks[0].image.Source = new BitmapImage(new Uri("/Image/Begin.png", UriKind.Relative));
        }

        //переключение галочки
        public void SetOk(bool ok)//true - горит, false - не горит
        {
            if (ok)
                blocks[1].image.Source = new BitmapImage(new Uri("/Image/end_good.png", UriKind.Relative));
            else
                blocks[1].image.Source = new BitmapImage(new Uri("/Image/end_bad.png", UriKind.Relative));
        }

        //поставить картиночку на ресурс
        public void SetResourceImage(ResourceTypes new_type, int new_id, int number_in_images)
        {
            List<string> infoResource = new List<string>();
            if (new_type != ResourceTypes.NO_TYPE)  //если ресурс назначен
            {
                DataBase_Resourses dataBase = new DataBase_Resourses();
                infoResource = dataBase.GetInfoRow(new_type, new_id);
            }

            string way = "/Image/Resourse_blok.png";

            switch (new_type)
            {
                case ResourceTypes.WORKER: way = "/Image/Resourse_blok_worker.png";
                    break;
                case ResourceTypes.CAD_SYSTEM: way = "/Image/Resourse_blok_SAPR.png";
                    break;
                case ResourceTypes.TECHNICAL_SUPPORT: way = "/Image/Resourse_blok_technical.png";
                    break;
                case ResourceTypes.METHODOLOGICAL_SUPPORT: way = "/Image/Resourse_blok_metodolog.png";
                    break;
                case ResourceTypes.NO_TYPE: way = "/Image/Resourse_blok.png";
                    break;
            }
            blocks[number_in_images].image.Source = new BitmapImage(new Uri(way, UriKind.Relative));
            if (new_type == ResourceTypes.NO_TYPE)
                blocks[number_in_images].label.Text = "Ресурс";  //если ресурс не назначен
            else
                blocks[number_in_images].label.Text = infoResource[0];  //если ресурс назначен
        }
    }
}
