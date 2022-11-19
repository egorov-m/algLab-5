namespace algLab_5.Models.Utils
{
    /// <summary> Установщик идентификатора </summary>
    public static class IdentifierSetter
    {
        private static int _currentIdentifier = 0;

        /// <summary> Получить идентификатор </summary>
        public static int GetId() => _currentIdentifier++;
    }
}
