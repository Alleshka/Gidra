﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GidraSIM.Core.Model.Resources;

namespace GidraSIM.Core.Model.Procedures
{
    public class SchemaCreationProcedure : Procedure
    {

        public SchemaCreationProcedure (ITokensCollector collector) : base(1, 1, collector)
        {

            
        }

        public override void Update(ModelingTime modelingTime)
        {
            //првоеряем, есть ли вообще что-то на входе
            if (inputQueue[0].Count > 0)
            {
                Random rand = new Random();
                //смотрим на первыйтокен
                var token = inputQueue[0].Peek();

                var worker = resources.Find(res => res is WorkerResource)  as WorkerResource;
                var cad = resources.Find(res => res is CadResource) as CadResource;
                var computer = resources.Find(res => res is TechincalSupportResource) as TechincalSupportResource;

                if (worker == null || cad == null || computer == null)
                    throw new ArgumentNullException("SchemaCreationProcedure - не присустствуют все ресурсы");

                int resourceCount = 0;
                

                //токен в первый раз?
                if (token.Progress < 0.01)
                {
                    token.ProcessedByBlock = this;
                    token.ProcessStartTime = modelingTime.Now;
                    //блокируем ресурсы для него

                    //пробуем взять рабочего
                    if (worker.TryGetResource())
                    {
                        resourceCount++;
                    }
                    else
                    {
                        worker.ReleaseResource();
                    }

                    //пробуем взять CAD
                    if (cad.TryGetResource())
                    {
                        resourceCount++;
                    }
                    else
                    {
                        cad.ReleaseResource();
                    }

                    //пробеум взять методичку
                    if (computer.TryGetResource())
                    {
                        resourceCount++;
                    }
                    else
                    {
                        computer.ReleaseResource();
                    }

                }
                //токен тут уже был, ресурсы уже заблочены
                else
                {
                    //поэтому сразу знаем, что все ресурсы есть
                    resourceCount = 3;
                }

                //общее время, которое должно бытьл затрачено на процедуру
                double time = token.Complexity;

                //влияние ПК на скорость работы
                double frequency = computer.Frequency;
                double memory_proc = computer.Ram;
                double memory_video = computer.Vram;
                                                                                                                            //базовые параметры: 
                double base_frequency = 1.5;//частота
                double base_memory_proc = 2;//объем памяти процессора
                double base_memory_video = 1;//объем памяти ведеокарты
                                                //выражения полученные аналитическим способом
                time += (base_frequency - computer.Frequency) / 1000; //порядок влияния на время
                time += (base_memory_proc - computer.Ram) / 10000;
                time += (base_memory_video - computer.Vram) / 10000; //TODO ээ, нулевое влияние времени в случае если всё также????

                //влияние рабочего на скорость работы
                switch(worker.WorkerQualification)
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

                //влияение методичики (необязательный ресурс)
                var methodSupport = resources.Find(res => res is MethodolgicalSupportResource) as MethodolgicalSupportResource;
                //если есть методичка, то время немного экономится
                if((methodSupport!=null)&&(methodSupport.TryGetResource()))
                {
                    time -= 0.01 * rand.NextDouble(); //от 0 до 15 минут
                }


                //если все ресурсы взяли, то выполняем задачу
                if (resourceCount == 3)
                {
                    //обновляем прогресс задачи
                    token.Progress += modelingTime.Delta/time; //делим общее время на dt
                }

                //задача выполнена
                if (token.Progress >= 0.99)
                {
                    inputQueue[0].Dequeue();
                    collector.Collect(token);

                    outputs[0] = new Token(modelingTime.Now, token.Complexity) { Parent = this };

                    //освобождаем все ресурсы
                    worker.ReleaseResource();
                    cad.ReleaseResource();
                    computer.ReleaseResource();
                    if (methodSupport != null) methodSupport.ReleaseResource();
                }

            }
        }
    }
}