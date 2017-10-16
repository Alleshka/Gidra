using System.Collections.Generic;
using System.Windows.Controls;

namespace GidraSIM
{
    public class Messages
    {
        private List<string> Errors;  //список возможных ошибок
        private List<string> SystemMessages;  //список системных сообщений
        Label LabelError;
        Label LabelMessage;
        TabControl tabControl;

        public Messages(ref Label labelMessage, ref Label labelError, ref TabControl new_tabControl)
        {
            LabelError = labelError;
            LabelMessage = labelMessage;
            tabControl = new_tabControl;

            //сообщения об ошибках, возникающих после сборки проекта---------------------------------------------------------------------------
            Errors = new List<string>();
            Errors.Add("Всего ошибок: "); //0
            Errors.Add("Отсутствует соединение между блоками"); //1
            Errors.Add("Отсутствуют ресурсы у процедуры "); //2
            Errors.Add("У блока Начало больше, чем одна связь"); //3
            Errors.Add("У блока Конец больше, чем одна связь"); //4
            Errors.Add("Некорректно соединен блок: "); //5
            Errors.Add("Дублирование ресурса: "); //6
            Errors.Add("Попытка открытия второго проекта"); //7
            Errors.Add("Ошибка при построении процесса");//8
            Errors.Add("Слишком близкое размещение блоков");//9
            Errors.Add("Попытка размещения блока за границей поля");//10
            Errors.Add("Попытка соединения блоков Начало и Конец");//11
            Errors.Add("Попытка соединения двух ресурсов");//12
            Errors.Add("Попытка соединения блока Начало с ресурсом");//13
            Errors.Add("Попытка соединения блока Конец с ресурсом");//14
            Errors.Add("Попытка удаления блока Начало");//15
            Errors.Add("Попытка удаления блока Конец");//16
            Errors.Add("Для данного действия нужно создать(открыть) проект, а затем процесс");//17   
            Errors.Add("Попытка создания процесса без создания(открытия) проекта");//18
            Errors.Add("Возникновение ошибок в ходе моделирования");//19


            //сообщения о состоянии программы, подсказки и тд....----------------------------------------------------------------------------------
            SystemMessages = new List<string>();
            SystemMessages.Add("Идет построение процеса...");//0
            SystemMessages.Add("Построение процесса завершено успешно");//1
            SystemMessages.Add("Выполняется процесс моделирования...");//2
            SystemMessages.Add("Моделирование завершено");//3
            SystemMessages.Add("Проект сохранен");//4
            SystemMessages.Add("Идет загрузка процесса...");//5
            SystemMessages.Add("Проект открыт успешно");//6
        }

        //вывод ошибки по номеру---------------------------------------------------------------------------------------------------------
        public void ShowError(int number)
        {
            TabItem tabItem = tabControl.Items[0] as TabItem;  //активируем вкладку ошибок
            tabItem.IsSelected = true;
            LabelError.Content += Errors[number] + "\n";
        }

        //вывод сщщбщения по номеру---------------------------------------------------------------------------------------------------------
        public void ShowMessage(int number)
        {
            TabItem tabItem = tabControl.Items[1] as TabItem;  //активируем вкладку сообщений
            tabItem.IsSelected = true;
            LabelMessage.Content = SystemMessages[number] ;
        }


    }
}
