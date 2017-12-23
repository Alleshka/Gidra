using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using GidraSIM.Core.Model;
using GidraSIM.Core.Model.Procedures;
using GidraSIM.Core.Model.Resources;
using GidraSIM.GUI.Core.BlocksWPF;
using GidraSIM.GUI.Core.BlocksWPF.ViewModels.Procedures;
using GidraSIM.GUI.Core.BlocksWPF.ViewModels.Resources;

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
                            block = this.ConvertWpfBlockToModel(connection.StartBlock as ProcedureWPF, process.Collector);
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
                        block = this.ConvertWpfBlockToModel(procedure, process.Collector);
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

                    (block as IProcedure).AddResorce(resource);
                }
                
            }
        }

        private IBlock ConvertWpfBlockToModel(ProcedureWPF procedureWPF, ITokensCollector collector)
        {
            if(procedureWPF is FixedTimeBlockViewModel)
            {
                return new FixedTimeBlock(collector, 10);
            }
            else if(procedureWPF is QualityCheckProcedureViewModel)
            {
                return new QualityCheckProcedure(collector);
            }
            else if (procedureWPF is SchemaCreationProcedureViewModel)
            {
                return new SchemaCreationProcedure(collector);
            }
            else if (procedureWPF is ArrangementProcedureViewModel)
            {
                return new ArrangementProcedure(collector);
            }
            else if (procedureWPF is ClientCoordinationPrrocedureViewModel)
            {
                return new ClientCoordinationPrrocedure(collector);
            }
            else if (procedureWPF is DocumentationCoordinationProcedureViewModel)
            {
                return new DocumentationCoordinationProcedure(collector);
            }
            else if (procedureWPF is ElectricalSchemeSimulationViewModel)
            {
                return new ElectricalSchemeSimulation(collector);
            }
            else if (procedureWPF is FormingDocumentationProcedureViewModel)
            {
                return new FormingDocumentationProcedure(collector);
            }
            else if (procedureWPF is PaperworkProcedureViewModel)
            {
                return new PaperworkProcedure(collector);
            }
            else if (procedureWPF is QualityCheckProcedureViewModel)
            {
                return new QualityCheckProcedure(collector);
            }
            else if (procedureWPF is SampleTestingProcedureViewModel)
            {
                return new SampleTestingProcedure(collector);
            }
            else if (procedureWPF is TracingProcedureViewModel)
            {
                return new TracingProcedure(collector);
            }

            else if (procedureWPF == null)
            {
                throw new NullReferenceException("Для процедуры WPF не указан прототип");
            }
            else
            {
                throw new NotImplementedException("Даный тип процедуры пока нельзя ковертировать");
            }
        }

        private IResource ConvertWpfResourcekToModel(ResourceWPF resourceWPF)
        {
            if (resourceWPF == null)
                throw new ArgumentNullException();
            else if( resourceWPF is CadResourceViewModel)
            {
                return (resourceWPF as CadResourceViewModel).Model;
            }
            else if( resourceWPF is WorkerResourceViewModel)
            {
                return (resourceWPF as WorkerResourceViewModel).Model;
            }
            else if(resourceWPF is TechincalSupportResourceViewModel)
            {
                return (resourceWPF as TechincalSupportResourceViewModel).Model;
            }
            else if (resourceWPF is MethodolgicalSupportResourceViewModel)
            {
                return (resourceWPF as MethodolgicalSupportResourceViewModel).Model;
            }
            else
            {
                throw new NotImplementedException("Данный тип ресурса пока не поддерживается конвертером");
            }
        }
    }
}
