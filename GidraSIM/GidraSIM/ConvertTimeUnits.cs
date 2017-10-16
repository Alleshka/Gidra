using System;

namespace GidraSIM
{
    class ConvertTimeUnits
    {
        //перевод в дни
        public double ConvertToDays(int index, double time)
        {
            double days = 0;
            switch (index)
            {
                case 0: //секунды
                    days = ((time / 60) / 60) / 24; //86400 секунд в сутках
                    break;
                case 1: //минуты
                    days = (time / 60) / 24;
                    break;
                case 2: //часы
                    days = time / 24;
                    break;
                case 3: //дни
                    days = time;
                    break;
                case 4: //месяцы
                    days = time * 30;
                    break;
            }
            return days;
        }

        //перевод в полную запись
        private int[] ConvertFromDaysToResult(double time)
        {
            int[] result = new int[5]; //0 - месяцы, 1 - дни, 2 - часы, 3 - минуты, 4 - секунды
            //45.436 = 1 месяц 15 дней 10 часов 27 минут 50 секунд
            while (time >= 30) //
            {
                time-=30;
                result[0]++;
            }
            while (time>=1)
            {
                time--;
                result[1]++;
            }
            time=time*24;//получаем часы
            while (time>=1)
            {
                time--;
                result[2]++;
            }
            time=time*60;//получаем минуты
            while (time>=1)
            {
                time--;
                result[3]++;
            }
            time=time*60;//получаем секунды
            while (time>=1)
            {
                time--;
                result[4]++;
            }
            return result;
        }

        public string DoFullFormat(double days)
        {
            ConvertTimeUnits convert = new ConvertTimeUnits();
            int[] hole_time = convert.ConvertFromDaysToResult(days);
            string full_time = null;
            if (hole_time[0] > 0)
                full_time += Convert.ToString(hole_time[0]) + " мес. ";
            if (hole_time[1] > 0)
                full_time += Convert.ToString(hole_time[1]) + " дн. ";
            if (hole_time[2] > 0)
                full_time += Convert.ToString(hole_time[2]) + " ч. ";
            if (hole_time[3] > 0)
                full_time += Convert.ToString(hole_time[3]) + " мин. ";
            if (hole_time[4] > 0)
                full_time += Convert.ToString(hole_time[4]) + " сек. ";

            return full_time;
        }
    }
}
