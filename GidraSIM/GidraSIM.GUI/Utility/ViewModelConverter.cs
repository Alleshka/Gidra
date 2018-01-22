using System;
using System.Collections.Generic;
using System.Windows.Controls;
using GidraSIM.Core.Model;
using GidraSIM.GUI.Core.BlocksWPF;

namespace GidraSIM.GUI.Utility
{
    public class ViewModelConverter : IViewModelConverter
    {
        public ViewModelConverter()
        {

        }

        public void Map(UIElementCollection uIElementCollection, Process process)
        {
            Dictionary<ProcedureWPF, IBlock> procedures = new Dictionary<ProcedureWPF, IBlock>();
            Dictionary<ResourceWPF, IResource> resources = new Dictionary<ResourceWPF, IResource>();
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
                            block = (connection.EndBlock as ProcedureWPF).BlockModel;

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
                            block = (connection.StartBlock as ProcedureWPF).BlockModel;
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
                            block = (connection.StartBlock as ProcedureWPF).BlockModel;
                            procedures.Add(connection.StartBlock as ProcedureWPF, block);
                            process.Blocks.Add(block);
                        }
                        //если в первый раз такое встречаем
                        if (!(procedures.ContainsKey(connection.EndBlock as ProcedureWPF)))
                        {
                            block = (connection.EndBlock as ProcedureWPF).BlockModel;
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
                    ProcedureWPF procedure;
                    ResourceWPF resourceWPF;
                    if (connection.StartBlock is ProcedureWPF)
                    {
                        procedure = connection.StartBlock as ProcedureWPF;
                        resourceWPF = connection.EndBlock as ResourceWPF;
                    }
                    else
                    {
                        procedure = connection.EndBlock as ProcedureWPF;
                        resourceWPF = connection.StartBlock as ResourceWPF;
                    }
                    IBlock block = null;

                    //если в первый раз такое встречаем
                    if (!(procedures.ContainsKey(procedure)))
                    {
                        block = procedure.BlockModel;
                        procedures.Add(procedure, block);
                        process.Blocks.Add(block);
                    }
                    else
                        block = procedures[procedure];

                    IResource resource = null; ;
                    //если в первый раз такое встречаем
                    if (!(resources.ContainsKey(resourceWPF)))
                    {
                        resource = this.ConvertWpfResourcekToModel(resourceWPF);
                        resources.Add(resourceWPF, resource);
                        process.Resources.Add(resource);
                    }
                    else
                        resource = resources[resourceWPF];

                    if (block is IProcedure)
                        (block as IProcedure).AddResorce(resource);
                    else
                        throw new ArgumentException("Ресурсы поддерживает только блоки типа IProcedure");
                }
                
            }
        }

        private IResource ConvertWpfResourcekToModel(ResourceWPF resourceWPF)
        {
            if (resourceWPF == null)
                throw new ArgumentNullException();
            return (resourceWPF as ResourceWPF).ResourceModel;
        }
    }
}
