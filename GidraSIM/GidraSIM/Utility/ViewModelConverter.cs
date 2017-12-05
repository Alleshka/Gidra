using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using GidraSIM.Model;
using GidraSIM.Model.Procedures;
using GidraSIM.BlocksWPF;

namespace GidraSIM.Utility
{
    public class ViewModelConverter : IViewModelConverter
    {
        public void Map(UIElementCollection uIElementCollection, Process process)
        {
            Dictionary<ProcedureWPF, FixedTimeBlock> procedures = new Dictionary<ProcedureWPF, FixedTimeBlock>();
            foreach (var element in uIElementCollection)
            {
                if(element is ProcConnectionWPF)
                {
                    var connection = (element as ProcConnectionWPF);
                    //TODO типа можно здесь всё обработать

                    if(connection.StartBlock is StartBlockWPF && connection.EndBlock is EndBlockWPF)
                    {
                        throw new Exception("Нельзя просто соединить начало с концом!");
                    }
                    //обработка стартового блока
                    if(connection.StartBlock  is StartBlockWPF)
                    {
                        //если в первый раз такое встречаем
                        if (!(procedures.ContainsKey(connection.EndBlock as ProcedureWPF)))
                        {
                            //для примера 10 единиц
                            FixedTimeBlock block = new FixedTimeBlock(process.Collector, 10);
                            procedures.Add(connection.EndBlock as ProcedureWPF, block);
                            process.Blocks.Add(block);
                            process.StartBlock = block;
                        }
                        //соединение будет когда-то потом
                    }
                    else if(connection.EndBlock is EndBlockWPF)
                    {
                        //если в первый раз такое встречаем
                        if (!(procedures.ContainsKey(connection.StartBlock as ProcedureWPF)))
                        {
                            //для примера 10 единиц
                            FixedTimeBlock block = new FixedTimeBlock(process.Collector, 10);
                            procedures.Add(connection.StartBlock as ProcedureWPF, block);
                            process.Blocks.Add(block);
                            process.EndBlock = block;
                        }
                        else
                        {
                            FixedTimeBlock block = procedures[connection.StartBlock as ProcedureWPF];
                            process.Blocks.Add(block);
                            process.EndBlock = block;
                        }
                        //соединение будет когда-то потом
                    }
                    //обработка
                    else 
                    {

                        //если в первый раз такое встречаем
                        if (!(procedures.ContainsKey(connection.StartBlock as ProcedureWPF)))
                        {
                            //для примера 10 единиц
                            FixedTimeBlock block = new FixedTimeBlock(process.Collector, 10);
                            procedures.Add(connection.StartBlock as ProcedureWPF, block);
                            process.Blocks.Add(block);
                        }
                        //если в первый раз такое встречаем
                        if (!(procedures.ContainsKey(connection.EndBlock as ProcedureWPF)))
                        {
                            //для примера 10 единиц
                            FixedTimeBlock block = new FixedTimeBlock(process.Collector, 10);
                            procedures.Add(connection.EndBlock as ProcedureWPF, block);
                            process.Blocks.Add(block);
                        }

                        //к счастью там только один вход и выход
                        process.Connections.Connect(procedures[connection.StartBlock as ProcedureWPF], 0,
                            procedures[connection.EndBlock as ProcedureWPF], 0);

                    }
                }
                else if (element is ResConnectionWPF)
                {
                    var connection = (element as ResConnectionWPF);
                    //TODO типа можно здесь всё обработать

                }
                
            }
        }
    }
}
