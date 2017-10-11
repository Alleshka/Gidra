using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading;

using CommonData;

namespace GidraSIM
{
    public class Imitation
    {
        DataBase_Resourses db_resources; //база данных ресурса
        Project project;
        int main_process_number; //номер моделируемого процесса
        int QuantityElements;//количество всех элементов на ПП
        public double Complexity;//сложность ПП
        public string Ling_Complexity;//лингфистическая переменная сложность ПП
        ModelingProperties parameters;//текущие параметры моделирования
        int MaxPins; // максимально возможное количество выводов у элемента
        public double hole_time;         //общее время выполнения процесса
        public double hole_time_accident_delay;         //общее время случайных задержек процесса
        List <string> accidents; //список возможных случайных событий
        public List <string> accidents_process; //список случайных событий возникших по время проектирования ПП
        public List <string> accidents_time_process; //список времени событий возникших по время проектирования ПП
        public double time_between_procedures; 
        ConvertTimeUnits convertTime;
        public List<ModelingResults> results;
        double last_time_delay;

        public Imitation(ref Project new_project, int new_process_number)
        {
            db_resources = new DataBase_Resourses();
            accidents_process = new List<string>();
            accidents_time_process = new List<string>();
            convertTime = new ConvertTimeUnits();
            results = new List<ModelingResults>();
            project = new_project;
            main_process_number = new_process_number;
            parameters = project.modelingProperties;
            MaxPins = 150;
            hole_time = 0;

            FillAccidents();
        }

        public List<ModelingResults> getResults()
        {
            return results;
        }

        public bool RunImitation()
        {
            CalculateComlexity();
            if (!CalculateTimeProcess(main_process_number))
                return false;
            return true;
        }

       private void FillAccidents()
       {
           accidents = new List<string>();
           accidents.Add("Непредвиденное отсутствие исполнителя");  //0
           accidents.Add("Необходимость корректировки документации");       //1
           accidents.Add("Человеческий фактор: ");//2
           accidents.Add("Cбой в работе технического оборудования");                 //3
           accidents.Add("Необходимость корректировки параметров ПП"); //4
       }

        //генерация случайного события
        private double GenerationAccidents(int number_accident)
        {
            double time = 0;
            Random rand = new Random();
            switch (number_accident)
            {
                case 0://нет исполнителя на месте
                    if (rand.Next(0, 100) > 90)//10%
                        time = Randomize(0, 4) ; //от 2 часов до 4 дней
                    break;
                case 1://корректируем док-ты
                    if (rand.Next(0, 100) > 60)//40%
                        time =Randomize(0.01, 2); //от 15 минут до 2 дней
                    break;
                case 2://задержка между проц
                    if (rand.Next(0, 100) > 10)//90%
                    {
                        time = Randomize(0,0.04); //от 1 минуты до 1 часа
                        time_between_procedures += time;
                    }
                    break;
                case 3://сбой в компе
                    if (rand.Next(0, 100) > 97)//3%
                        time = Randomize(0,0.006 ); //от 1 минуты до 10 минут
                    break;
                case 4://изменяем плату
                    if (rand.Next(0, 100) > 85)//15%
                        time = Randomize(0.02, 2); //от 30 минут до 2 дней
                    break;
            }
            if (time > 0)
            {
                accidents_process.Add(accidents[number_accident]);
                if (Math.Abs(last_time_delay - time) < 0.001)
                    time /= Randomize(0,10);
                accidents_time_process.Add(convertTime.DoFullFormat(time));
                last_time_delay = time;
            }
            return time;
        }

        //время выполнения процесса
        private bool CalculateTimeProcess(int number_process)
        {
            bool stop = false;
            Process_ current_process = project.Processes[number_process]; //моделируемый процесс
            for (int i = 1; i < current_process.StructProcess.Count - 1; i++) //идем по структуре процесса с первого блока, т.к. нулевой - это Начало
            {       
                switch (current_process.StructProcess[i].Type)
                {
                    case ObjectTypes.PROCEDURE:
                        double time_procedure = CalculateTimeProcedure(current_process.StructProcess[i].number, current_process);
                        if (time_procedure == -1)
                            stop = true;
                        else
                        {
                            current_process.Procedures[current_process.StructProcess[i].number].Time_in_days = time_procedure;
                            current_process.Procedures[current_process.StructProcess[i].number].Time_in_format = convertTime.DoFullFormat(time_procedure);
                        }
                        //записываем результаты моделирование текущей процедуры
                        ModelingResults rp = new ModelingResults();
                        rp.Name = current_process.Procedures[current_process.StructProcess[i].number].Name;
                        rp.Time_in_format = current_process.Procedures[current_process.StructProcess[i].number].Time_in_format;
                        results.Add(rp);
                        break;
                    case ObjectTypes.PARALLEL_PROCESS:
                        break;
                    case ObjectTypes.SUBPROCESS:
                        CalculateTimeProcess(current_process.StructProcess[i].number);
                        //записываем результаты моделирование текущей процедуры
                        ModelingResults rs = new ModelingResults();
                        int num_sub = current_process.StructProcess[i].number;
                        int num_proc = current_process.SubProcesses[num_sub].number_in_processes;
                        rs.Name = "---" + project.Processes[num_proc].Name + "---";
                        rs.Time_in_format = "Общее время " + project.Processes[num_proc].Time_in_format;
                        results.Add(rs);
                        break;
                }
                if (stop)
                    break;
            }
            if (stop)
                return false;
            else
            {
                current_process.Time_in_days = hole_time;
                current_process.Time_in_format = convertTime.DoFullFormat(hole_time);
                current_process.Time_accidents_in_days = hole_time_accident_delay;
                current_process.Time_accidents_in_format = convertTime.DoFullFormat(hole_time_accident_delay);
                return true;                
            }
        }

        private double SeeWorker(double time, int id)
        {
            List<string> info_resource = db_resources.GetInfoRow(ResourceTypes.WORKER, id);
            //структура таблицы: 0 - имя, 1 - должность, 2 - квалификация
            Random rand = new Random();
            
            if (info_resource[2].Contains("1")) // 1 категория
                time -= time / rand.Next(1,5);//уменьшаем время, т.к. высокая категория\
            else if (info_resource[2].Contains("2")) // 2 категория
                time = time;//базовое время подсчитано для второй категории
            else if (info_resource[2].Contains("3")) // 3 категория
                time += time / rand.Next(1, 5);//базовое время подсчитано для второй категории
            else if (info_resource[2].Contains("Ведущий")) // ведущий
                time -= time / rand.Next(1, 4);//базовое время подсчитано для второй категории
            else
                time += time / rand.Next(1, 4); //инженер (техник) без категории
            return time;
        }

        //учет ресурсов при КД   
        private bool SeeResourcesForDocs(int number, ref double time, Process_ current_process)
        {//разработка КД ведется не с помощью самой сапр, а либо вручную, либо с помощью PDM системы, поэтому кад система не обязательна в ресурсах
            bool exist_worker = false;
            bool exist_technical = false;
            List<string> info_resource = new List<string>();
            //есть ли исполнитель для создания электрической схемы
            for (int i = 0; i < current_process.Procedures[number].Resources.Count; i++)
            {
                Resource current_resource = current_process.Resources[current_process.Procedures[number].Resources[i]];
                if (current_resource.Type == ResourceTypes.WORKER)
                {
                    exist_worker = true;
                    time = SeeWorker(time, current_resource.id);
                }
                else if (current_resource.Type == ResourceTypes.TECHNICAL_SUPPORT)
                {
                    exist_technical = true;
                    //структура таблицы: 0 - имя, 1 - частота, 2 - объесм память процесора, 3 - диагональ монитора, 4 - объем памяти видеокарты
                    info_resource = db_resources.GetInfoRow(ResourceTypes.TECHNICAL_SUPPORT, current_resource.id);
                    double frequency = Convert.ToDouble(info_resource[1]);//частота
                    double memory_proc = Convert.ToDouble(info_resource[2]);//объем памяти процессора
                    double memory_video = Convert.ToDouble(info_resource[4]);//объем памяти ведеокарты
                    //базовые параметры: 
                    double base_frequency = 1.5;//частота
                    double base_memory_proc = 2;//объем памяти процессора
                    double base_memory_video = 1024;//объем памяти ведеокарты
                    //выражения полученные аналитическим способом
                    time += (base_frequency - frequency) / 1000; //порядок влияния на время
                    time += (base_memory_proc - memory_proc) / 10000;
                    time += (base_memory_video - memory_video) / 100000;
                    //диагональ влияет только на качество выполняемой исполнотелем работы, которое не считается в данной работе
                }
                else if (current_resource.Type == ResourceTypes.METHODOLOGICAL_SUPPORT)
                {
                    //методологическое обеспечение увеличивает качество и чуть-чуть уменьшает время
                    Random rand = new Random();
                    time -= 0.01 * rand.NextDouble();//от 0 до 15 минут
                }
            }
            
            if (!exist_technical)
                MessageBox.Show("Отсутствует ресурс типа \"Техническое обеспечение\" у процедуры создания электрической схемы. Провести моделирование не возможно", "Ошибка!");
            if (!exist_worker)
                MessageBox.Show("Отсутствует ресурс типа \"Исполнитель\" у процедуры создания электрической схемы. Провести моделирование не возможно", "Ошибка!");
            if (!exist_worker && !exist_technical)
                return false;
            else
                return true;
        }


 //учет ресурсов при создании электрической схемы
        private bool SeeResourcesForCreateShema(int number, ref double time, Process_ current_process)
        {
            bool exist_worker=false;
            bool exist_cad_system=false;
            bool exist_technical = false;
            List<string> info_resource = new List<string>();
            //есть ли исполнитель для создания электрической схемы
            for (int i = 0; i < current_process.Procedures[number].Resources.Count; i++)
            {
                Resource current_resource = current_process.Resources[current_process.Procedures[number].Resources[i]];
                if (current_resource.Type == ResourceTypes.WORKER)
                {
                    exist_worker = true;
                    time = SeeWorker(time, current_resource.id);
                }
                else if (current_resource.Type == ResourceTypes.CAD_SYSTEM)
                {
                    exist_cad_system = true;
                }
                else if (current_resource.Type == ResourceTypes.TECHNICAL_SUPPORT)
                {
                    exist_technical = true;
                     //структура таблицы: 0 - имя, 1 - частота, 2 - объесм память процесора, 3 - диагональ монитора, 4 - объем памяти видеокарты
                    info_resource = db_resources.GetInfoRow(ResourceTypes.TECHNICAL_SUPPORT, current_resource.id);
                    double frequency = Convert.ToDouble(info_resource[1]);//частота
                    double memory_proc = Convert.ToDouble(info_resource[2]);//объем памяти процессора
                    double memory_video = Convert.ToDouble(info_resource[4]);//объем памяти ведеокарты
                //базовые параметры: 
                    double base_frequency = 1.5;//частота
                    double base_memory_proc = 2;//объем памяти процессора
                    double base_memory_video = 1024;//объем памяти ведеокарты
                //выражения полученные аналитическим способом
                    time += (base_frequency - frequency) / 1000; //порядок влияния на время
                    time += (base_memory_proc - memory_proc) / 10000;
                    time += (base_memory_video - memory_video) / 100000;
                //диагональ влияет только на качество выполняемой исполнотелем работы, которое не считается в данной работе
                }
                else if (current_resource.Type == ResourceTypes.METHODOLOGICAL_SUPPORT)
                {
                    //методологическое обеспечение увеличивает качество и чуть-чуть уменьшает время
                    Random rand = new Random();
                    time -= 0.01*rand.NextDouble();//от 0 до 15 минут
                }
            }

            if (!exist_cad_system)
                MessageBox.Show("Отсутствует ресурс типа \"САПР\" у процедуры создания электрической схемы. Провести моделирование не возможно", "Ошибка!");  
            if (!exist_technical)
                MessageBox.Show("Отсутствует ресурс типа \"Техническое обеспечение\" у процедуры создания электрической схемы. Провести моделирование не возможно", "Ошибка!"); 
            if (!exist_worker)
                MessageBox.Show("Отсутствует ресурс типа \"Исполнитель\" у процедуры создания электрической схемы. Провести моделирование не возможно", "Ошибка!");
            
            if (!exist_worker && !exist_technical && !exist_cad_system)
                return false;
            else
                return true;
        }

//учет ресурсов при компоновки
        private bool SeeResourcesForAlgorithm(int number, ref double time, Process_ current_process)
        {
            bool exist_worker = false;
            bool exist_cad_system = false;
            bool exist_technical = false;
            List<string> info_resource = new List<string>();
            //есть ли исполнитель для создания электрической схемы
            for (int i = 0; i < current_process.Procedures[number].Resources.Count; i++)
            {
                Resource current_resource = current_process.Resources[current_process.Procedures[number].Resources[i]];
                if (current_resource.Type == ResourceTypes.WORKER)
                {
                    exist_worker = true;
                    //временная задержка для анализа полученного результата
                    Random rand = new Random();
                        time += 0.02*rand.NextDouble(); //30(0,02 дня) минут - максимальное время для анализа
                }
                else if (current_resource.Type == ResourceTypes.CAD_SYSTEM)
                {
                    exist_cad_system = true;
                }
                else if (current_resource.Type == ResourceTypes.TECHNICAL_SUPPORT)
                {
                    exist_technical = true;
                    //структура таблицы: 0 - имя, 1 - частота, 2 - объесм память процесора, 3 - диагональ монитора, 4 - объем памяти видеокарты
                    info_resource = db_resources.GetInfoRow(ResourceTypes.TECHNICAL_SUPPORT, current_resource.id);
                    double frequency = Convert.ToDouble(info_resource[1]);//частота
                    double memory_proc = Convert.ToDouble(info_resource[2]);//объем памяти процессора
                    double memory_video = Convert.ToDouble(info_resource[4]);//объем памяти ведеокарты
                    //базовые параметры: 
                    double base_frequency = 1.5;//частота
                    double base_memory_proc = 2;//объем памяти процессора
                    double base_memory_video = 1024;//объем памяти ведеокарты
                    //выражения полученные аналитическим способом
                    time += (base_frequency - frequency) / 1000; //порядок влияния на время
                    time += (base_memory_proc - memory_proc) / 10000;
                    time += (base_memory_video - memory_video) / 100000;
                    //диагональ влияет только на качество выполняемой исполнотелем работы, которое не считается в данной работе
                }
            }

            if (!exist_cad_system)
                MessageBox.Show("Отсутствует ресурс типа \"САПР\" у процедуры создания электрической схемы. Провести моделирование не возможно", "Ошибка!");
            if (!exist_technical)
                MessageBox.Show("Отсутствует ресурс типа \"Техническое обеспечение\" у процедуры создания электрической схемы. Провести моделирование не возможно", "Ошибка!");
            if (!exist_worker)
                MessageBox.Show("Отсутствует ресурс типа \"Исполнитель\" у процедуры создания электрической схемы. Провести моделирование не возможно", "Ошибка!");
            if (!exist_worker && !exist_technical && !exist_cad_system)
                return false;
            else
                return true;
        }

        //вычисление времени выполнения одной процедуры, все вычисления - в днях
        public double CalculateTimeProcedure(int number, Process_ current_process)
        {
            double time = 0, time_acc=0;
            Randomize(2,4);
            if (!current_process.Procedures[number].is_common)
                return current_process.Procedures[number].Time_in_days;
            switch (current_process.Procedures[number].common_type)
            {
                case "Создание электрической схемы":
                    //базовое время, зависящее от сложности объекта проектирования  // порядок часы/дни
                    time = Complexity; // для 2ой категории, для компьютера с частотой 1,5. объемом памяти 2 Гб, диагонать 15 
                    if (!SeeResourcesForCreateShema(number, ref time, current_process))//учитываем ресурсы
                        return -1;//ошибка, далее не моделируем
                    else
                    {
                        //считаем случ события
                        time_acc += GenerationAccidents(0);
                        time_acc += GenerationAccidents(2);
                        time_acc += GenerationAccidents(3);
                        time_acc += GenerationAccidents(4);
                    }
                    break;
                case "Моделирование электрической схемы":
                    //базовое время, зависящее от сложности объекта проектирования  // порядок часы
                    time = Complexity / 10; // для 2ой категории, для компьютера с частотой 1,5. объемом памяти 2 Гб, диагонать 15 
                    if (!SeeResourcesForCreateShema(number, ref time, current_process))//учитываем ресурсы
                        return -1;//ошибка, далее не моделируем
                    else
                    {
                        //считаем случ события
                        time_acc += GenerationAccidents(0);
                        time_acc += GenerationAccidents(2);
                        time_acc += GenerationAccidents(3);
                        time_acc += GenerationAccidents(4);
                    }
                    break;
                case "Компоновка":
                    // среднее время компоновки ПП 3 минуты  // порядок минуты
                    time = Complexity / 100;//базовое время
                    if (!SeeResourcesForAlgorithm(number, ref time, current_process))
                        return -1;//ошибка, далее не моделируем
                    else
                    {
                        //считаем случ события
                        time_acc += GenerationAccidents(0);
                        time_acc += GenerationAccidents(2);
                        time_acc += GenerationAccidents(3);
                    }
                    break;
                case "Размещение":
                    // среднее время размещения ПП 3 минуты  // порядок минуты
                    time = Complexity / 100;//базовое время
                    if (!SeeResourcesForAlgorithm(number, ref time, current_process))
                        return -1;//ошибка, далее не моделируем
                    else
                    {
                        //считаем случ события
                        time_acc += GenerationAccidents(0);
                        time_acc += GenerationAccidents(2);
                        time_acc += GenerationAccidents(3);
                        time_acc += GenerationAccidents(4);
                    }
                    break;
                case "Трассировка":
                    // среднее время трассировки ПП 3 минуты  // порядок минуты
                    time = Complexity / 100;//базовое время
                    if (!SeeResourcesForAlgorithm(number, ref time, current_process))
                        return -1;//ошибка, далее не моделируем
                    else
                    {
                        Random rand = new Random();
                        if (Complexity> 0.4)  //сложность больше средней - может возникнуть вероятность необходимости ручной трассировки
                        {
                            double percent =rand.Next((int)(Complexity * 100/6), (int)(Complexity * 100/2));//что нужно трассировать вручную
                            double sred_pers = 20;// среднюю плату (20%) средней квалификации человек будет разводить 1 день
                            double loc_time = percent / sred_pers;
                            SeeResourcesForAlgorithm(number, ref time, current_process);
                        }
                        //считаем случ события
                        time_acc += GenerationAccidents(0);
                        time_acc += GenerationAccidents(2);
                        time_acc += GenerationAccidents(3);
                        time_acc += GenerationAccidents(4);
                    }
                    break;
                case "Оформление документации":
                    Random rand1 = new Random();//случайное число, т.к. сроки размыты // порядок дни
                    time = rand1.Next(1, 30);//базовое время зависит от сроков сдачи проекта разрабатываемого объекта, которое в данной работе не учитывается
                    if (!SeeResourcesForDocs(number, ref time, current_process))
                        return -1;//ошибка, далее не моделируем
                    else
                    {
                        //считаем случ события
                        time_acc += GenerationAccidents(0);
                        time_acc += GenerationAccidents(1);
                        time_acc += GenerationAccidents(2);
                        time_acc += GenerationAccidents(3);
                    }
                    break;
                case "Согласование документации с контролирующими службами":
                    Random rand2 = new Random();//случайное число, т.к. сроки размыты // порядок дни/месяцы
                    time = rand2.Next(7, 60);//базовое время зависит от сроков сдачи проекта разрабатываемого объекта, которое в данной работе не учитывается
                    //не зависит ни от кого, только от проверяющих служм
                    //считаем случ события
                    time_acc += GenerationAccidents(1);
                    time_acc += GenerationAccidents(2);
                    time_acc += GenerationAccidents(4);
                    break;
                case "Согласование документации с заказчиком":
                    Random rand3 = new Random();//случайное число // порядок дни
                    time = rand3.Next(1, 30);//базовое время зависит от согласия заказчика
                    time_acc += GenerationAccidents(1);
                    time_acc += GenerationAccidents(2);
                    time_acc += GenerationAccidents(4);
                    // только от требований заказчика
                    break;
                case "Тестирование опытного образца объекта":
                    // порядок дни/месяцы
                    time = Complexity * 100;//базовое время зависит от сложности и проверяющего оборудования(КИА), но данный ремурс в системе не учитывается
                    time_acc += GenerationAccidents(0);
                    time_acc += GenerationAccidents(1);
                    time_acc += GenerationAccidents(2);
                    time_acc += GenerationAccidents(4);
                    break;
            }
            hole_time += time;  //записываем в общее время
            hole_time += time_acc;//записываем в общее время случ соб
            hole_time_accident_delay += time_acc;
            //записываем время в структуру
            return time;
        }

        //вычисление сложности
        public bool CalculateComlexity()
        {
            QuantityElements = 0;
            double NormPinsChips = 0;//нормированное значение количества выводов у микросхем
            double NormPinsOther = 0;//нормированное значение количества выводов у других элементов

            for (int i = 0; i < 4; i++)//считаем общее число элементов
                QuantityElements += parameters.elements[i].quantity_elements;

            //задаем изначальную сложность ОП
            if (QuantityElements > 0 && QuantityElements <= 100)
                Complexity = 0.2;
            else if (QuantityElements > 100 && QuantityElements <= 500)
                Complexity = 0.4;
            else if (QuantityElements > 500 && QuantityElements <= 1000)
                Complexity = 0.55;
            else if (QuantityElements > 1000)
                Complexity = 0.7;

            //коэффициент заполнения ПП
            double Kzap = parameters.elements_square / parameters.board_square;
            if (Kzap < 0.4 || Kzap > 0.85)//ай яй яй
                return false;// подсчет сложности ОП завершился неудачно
            else//учитываем коэффициент заполнения в сложности ПП
                Complexity += Math.Abs(0.625 - Kzap);

            Complexity += (double)parameters.layers / 100.0;//учитываем число слоев ПП

            //Вычисляем долю каждой группы элементов в общем числе элементов ПП
            double[] PersentElementsInCommon = new double[4];
            for (int i = 0; i < 4; i++)
                PersentElementsInCommon[i] = (double)parameters.elements[i].quantity_elements / QuantityElements;

            
            NormPinsChips = CalculateAveragePins(2);//считаем количество ножек у микросхем
            NormPinsOther = CalculateAveragePins(3);//считаем количество ножек у других элементов
            
            //назначаем коэффициенты сложности для каждого типа элементов
            double[] kof = new double[4];

            //коэффициент сложности микросхем и прочих элементов умножаем на нормированное значение количества контактов и на их долю в общем числе элементов
            kof[0] = convert(2, 1, MaxPins, 0, 1) * PersentElementsInCommon[0];//двухполюсники имеют 2 вывода
            kof[1] = convert(3, 1, MaxPins, 0, 1) * PersentElementsInCommon[1];//трехполюсники имеют 3 вывода
            kof[2] = NormPinsChips * PersentElementsInCommon[2];//микросхемы
            kof[3] = NormPinsOther * PersentElementsInCommon[3];//другие элементы
            //Далее учитываем сложность всех элементов в общей сложности ПП
            for (int i = 0; i < 4; i++)
                Complexity += kof[i];
            ConvertComplexity();
            return true;// подсчет сложности ОП завершился успешно
        }

        private void ConvertComplexity()
        {
            double y1, y2, x = Complexity*100;
            y1 = 1 / (1 + Math.Pow((x - 10) / 12, 12));
            y2 = 1 / (1 + Math.Pow((x - 30) / 12, 12));
            if (y1 > y2)
            {
                Ling_Complexity = "Очень легкая";
                return ;
            }
            else
            {
                y1 = y2;
                y2 = 1 / (1 + Math.Pow((x - 50) / 12, 12));
            }
            if (y1 > y2)
            {
                Ling_Complexity = "Легкая";
                return;
            }
            else
            {
                y1 = y2;
                y2 = 1 / (1 + Math.Pow((x - 70) / 12, 12));
            }
            if (y1 > y2)
            {
                Ling_Complexity = "Средняя";
                return;
            }
            else
            {
                y1 = y2;
                y2 = 1 / (1 + Math.Pow((x - 90) / 12, 12));
            }
            if (y1 > y2)
            {
                Ling_Complexity = "Сложная";
                return;
            }
            else
            {
                Ling_Complexity = "Очень сложная";
                return;
            }
 
        }

        private double CalculateAveragePins(int number)
        {
            if (parameters.elements[number].quantity_elements > 0)
            {
                double NormPins=0;//результат
                if (parameters.elements[number].quantity_elements > 1)
                {
                    Random rand = new Random();
                    int[] Pins = new int[parameters.elements[number].quantity_elements];
                    Pins[0] = parameters.elements[number].quantity_pins_min;//назначаем первым двум элементам минимальное и максимальное количество 
                    Pins[1] = parameters.elements[number].quantity_pins_max;//выводов, так как они точно должны быть
                    //Половине числа элементов назначается наиболее часто встречающееся количество выводов
                    for (int i = 2; i < parameters.elements[number].quantity_elements / 2+2; i++)
                        Pins[i] = parameters.elements[number].quantity_pins_often;
                    //Второй половине назначается случайное количество контактов из диапазона от минимального до максимального количества ножек
                    for (int i = parameters.elements[number].quantity_elements / 2; i < parameters.elements[number].quantity_elements; i++)
                        Pins[i] = rand.Next(parameters.elements[number].quantity_pins_min, parameters.elements[number].quantity_pins_max);
                    //Вычисляем среднее значение количества контактов
                    double MediumPins = 0;
                    for (int i = 0; i < parameters.elements[number].quantity_elements; i++)
                        MediumPins += Pins[i];
                    MediumPins /= parameters.elements[number].quantity_elements;
                    //переводим в диапазон от 0 до 1 
                    NormPins = convert(MediumPins, 1, MaxPins, 0, 1);
                    return NormPins;
                }
                else
                    return convert(parameters.elements[number].quantity_pins_often, 1, MaxPins, 0, 1);
            }
            else return 0;//если компонентов нет
        }
//перевод из одного диапазона в другой
        private double convert(double value, double From1, double From2, double To1, double To2)
        {
            return (value - From1) / (From2 - From1) * (To2 - To1) + To1;
        }

//рандом
        private double Randomize(double first, double last)
        {
            Thread.Sleep(20);
            Random rand = new Random();
            double val = rand.Next(0,1000);
            return convert(val, 0, 1000, first, last);
        }
    }
}
