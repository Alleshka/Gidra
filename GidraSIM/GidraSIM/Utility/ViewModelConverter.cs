using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using GidraSIM.Core.Model;
using GidraSIM.Core.Model.Procedures;
using GidraSIM.GUI.Core.BlocksWPF;

namespace GidraSIM.Utility
{
    public class ViewModelConverter : IViewModelConverter
    {
        public ViewModelConverter()
        {

        }

        public void Map(UIElementCollection uIElementCollection, Process process)
        {
            Dictionary<ProcedureWPF, IBlock> procedures = new Dictionary<ProcedureWPF, IBlock>();
            foreach (var element in uIElementCollection)
            {
                //смотрим все соединения процедур
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
                        IBlock block;

                        //если в первый раз встерчаем блок
                        if (!(procedures.ContainsKey(connection.EndBlock as ProcedureWPF)))
                        {
                            //первого нет, второй значимый
                            block = this.ConvertWpfBlockToModel(connection.EndBlock as ProcedureWPF, process.Collector);

                            //добавляем его в список
                            procedures.Add(connection.EndBlock as ProcedureWPF, block);

                            //добавляем его в список всех блоков процесса
                            process.Blocks.Add(block);
                        }
                        //иначе просто берём из базы
                        else
                            block = procedures[connection.EndBlock as ProcedureWPF];

                        process.StartBlock = block;
                        //соединение будет когда-то потом
                    }
                    //обработка конечного блока
                    else if(connection.EndBlock is EndBlockWPF)
                    {
                        IBlock block = null;

                        //если в первый раз такое встречаем
                        if (!(procedures.ContainsKey(connection.StartBlock as ProcedureWPF)))
                        {
                            block = this.ConvertWpfBlockToModel(connection.EndBlock as ProcedureWPF, process.Collector);
                            procedures.Add(connection.StartBlock as ProcedureWPF, block);
                            process.Blocks.Add(block);
                        }
                        else
                        {
                            block = procedures[connection.StartBlock as ProcedureWPF];
                        }
                        process.EndBlock = block;
                        //соединение будет когда-то потом
                    }
                    //обработка всех остальных блоков
                    else 
                    {
                        IBlock block = null;

                        //если в первый раз такое встречаем
                        if (!(procedures.ContainsKey(connection.StartBlock as ProcedureWPF)))
                        {
                            block = this.ConvertWpfBlockToModel(connection.StartBlock as ProcedureWPF, process.Collector);
                            procedures.Add(connection.StartBlock as ProcedureWPF, block);
                            process.Blocks.Add(block);
                        }
                        //если в первый раз такое встречаем
                        if (!(procedures.ContainsKey(connection.EndBlock as ProcedureWPF)))
                        {
                            block = this.ConvertWpfBlockToModel(connection.EndBlock as ProcedureWPF, process.Collector);
                            procedures.Add(connection.EndBlock as ProcedureWPF, block);
                            process.Blocks.Add(block);
                        }
                        

                        //к счастью там только один вход и выход
                        process.Connections.Connect(procedures[connection.StartBlock as ProcedureWPF], connection.StartPort,
                            procedures[connection.EndBlock as ProcedureWPF], connection.EndPort);

                    }
                }
                //сотрим соединения ресурсов
                else if (element is ResConnectionWPF)
                {
                    var connection = (element as ResConnectionWPF);
                    //TODO типа можно здесь всё обработать

                }
                
            }
        }

        private IBlock ConvertWpfBlockToModel(ProcedureWPF procedureWPF, ITokensCollector collector)
        {
            if(procedureWPF.ProcedurePrototype is FixedTimeBlock)
            {
                return new FixedTimeBlock(collector, 10);
            }
            else if(procedureWPF.ProcedurePrototype is QualityCheckProcedure)
            {
                return new QualityCheckProcedure(collector);
            }
            else if(procedureWPF.ProcedurePrototype == null)
            {
                throw new NullReferenceException("Для процедуры WPF не указан прототип");
            }
            else
            {
                throw new NotImplementedException("Даный тип процедуры пока нельзя ковертировать");
            }
        }
    }
}
