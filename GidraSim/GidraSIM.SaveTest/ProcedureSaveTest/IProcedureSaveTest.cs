namespace GidraSIM.SaveTest.ProcedureSaveTest
{
    interface IProcedureSaveTest
    {
        /// <summary>
        /// Просто создание процедуры
        /// </summary>
        void CreateEmptyProcedure();

        /// <summary>
        /// Создал процедуру и добавил токен + необходимые ресурсы
        /// </summary>
        void CreateProcedure();

        /// <summary>
        /// Создал процедуру с нужными данными и прокрутил update
        /// </summary>
        void CreateAndUpdateProcedure();

        /// <summary>
        /// Создание через интерфейс
        /// </summary>
        void CreateEmptyProcedureAsIBlock();
    }
}
