﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GidraSIM.Core.Model.Resources;

namespace GidraSIM.Core.Model.Procedures
{

    /// <summary>
    /// case "Оформление документации":
    /// </summary>
    public class PaperworkProcedure : Procedure
    {
        private double prevTime = -1;
        public PaperworkProcedure(ITokensCollector collector) : base(1, 1, collector)
        {

        }

        public override void Update(double globalTime)
        {
            // Смотрим, есть ли что на входе
            if (inputQueue[0].Count > 0)
            {
                Random rand = new Random();

                var token = inputQueue[0].Peek();

                var worker = resources.Find(res => res is WorkerResource) as WorkerResource;
                var comp = resources.Find(res => res is TechincalSupportResource) as TechincalSupportResource;

                // Если какого-то ресурса не хватает
                if ((worker == null) || (comp == null))
                {
                    StringBuilder errorList = new StringBuilder("Ошибка: ");
                    if (worker == null) errorList.Append(Environment.NewLine + "Отсутствует ресурс \"Исполнитель\"");
                    if (comp == null) errorList.Append(Environment.NewLine + "Отсутствует ресурс \"Техническое обеспечение\"");
                    throw new ArgumentNullException(errorList.ToString());
                }

                int resourceCount = 0;

                if (token.Progress < 0.01)
                {
                    token.ProcessedByBlock = this;
                    token.ProcessStartTime = globalTime;
                    // Блокируем ресурсы

                    if (worker.TryGetResource())
                    {
                        resourceCount++;
                    }
                    else worker.ReleaseResource(); // <-- ???

                    if (comp.TryGetResource())
                    {
                        resourceCount++;
                    }
                    else comp.ReleaseResource();
                }
                else
                {
                    resourceCount = 2;
                }

                //общее время, которое должно бытьл затрачено на процедуру
                double time = token.Complexity;

                #region WorckerImpact
                // Влияние рабочего на скорость работы
                switch (worker.WorkerQualification)
                {
                    case WorkerResource.Qualification.LeadCategory:
                        time -= time / rand.Next(1, 4);
                        break;
                    case WorkerResource.Qualification.FirstCategory:
                        time -= time / rand.Next(1, 5);//уменьшаем время, т.к. высокая категория\
                        break;
                    case WorkerResource.Qualification.SecondCategory:
                        //базовое время подсчитано для второй категории
                        break;
                    case WorkerResource.Qualification.ThirdCategory:
                        time += time / rand.Next(1, 5);
                        break;
                    case WorkerResource.Qualification.NoCategory:
                        time += time / rand.Next(1, 4);
                        break;
                }
                #endregion

                // Влияние ПК на скорость работы
                #region PCImpact
                {
                    double frequency = comp.Frequency;
                    double memory_proc = comp.Ram;
                    double memory_video = comp.Vram;

                    //базовые параметры: 
                    double base_frequency = 1.5;//частота
                    double base_memory_proc = 2;//объем памяти процессора
                    double base_memory_video = 1;//объем памяти ведеокарты

                    time += (base_frequency - frequency) / 1000; //порядок влияния на время
                    time += (base_memory_proc - memory_proc) / 10000;
                    time += (base_memory_video - memory_video) / 100000;
                    //диагональ влияет только на качество выполняемой исполнотелем работы, которое не считается в данной работе
                }
                #endregion

                #region MetodImpact
                // Влияние методички (необязательный ресурс)
                var metod = resources.Find(res => res is MethodolgicalSupportResource) as MethodolgicalSupportResource;
                // методологическое обеспечение увеличивает качество и чуть-чуть уменьшает время
                if ((metod!=null)&&(metod.TryGetResource()))
                {
                    time -= 0.01 * rand.NextDouble(); //от 0 до 15 минут
                }
                #endregion

                // Если все ресурсы взяли, то выполняем задачу
                if (resourceCount == 2)
                {
                    // Обновляем прогресс задачи
                    token.Progress += (globalTime - prevTime) / time;
                }

                // Задача выполнена
                if(token.Progress >= 0.99)
                {
                    inputQueue[0].Dequeue();
                    collector.Collect(token);

                    outputs[0] = new Token(globalTime, token.Complexity) { Parent = this };

                    // Освобождаем ресурсы
                    worker.ReleaseResource();
                    comp.ReleaseResource();
                    if(metod!=null) metod.ReleaseResource();
                }

                prevTime = globalTime;
            }
        }
    }
}