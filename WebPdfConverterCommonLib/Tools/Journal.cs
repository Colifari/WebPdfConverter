namespace WebPdfConverterCommonLib.Tools
{
    /// <summary>
    /// Handles logging
    /// </summary>
    public static class Journal
    {
        static NLog.Logger nLogger = NLog.LogManager.GetCurrentClassLogger();

        public static void Warn(string message)
        {
            nLogger.Warn(message);
        }

        /// <summary>
        /// Warning
        /// </summary>
        /// <param name="message">message text</param>
        /// <param name="argument">Arg</param>
        public static void Warn(string message, string argument)
        {
            nLogger.Warn(message, argument);
        }

        /// <summary>
        /// Warning
        /// </summary>
        /// <param name="message">message text</param>
        /// <param name="argument1">Arg 1</param>
        public static void Warn<TArgument>(string message, TArgument argument)
        {
            nLogger.Warn(message, argument);
        }

        /// <summary>
        /// Warning
        /// </summary>
        /// <param name="message">message text</param>
        /// <param name="argument1">Arg 1</param>
        /// <param name="argument2">Arg 2/param>
        public static void Warn<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2)
        {
            nLogger.Warn(message, argument1, argument2);
        }

        /// <summary>
        /// Warning
        /// </summary>
        /// <param name="message">message text</param>
        /// <param name="argument1">Arg 1</param>
        /// <param name="argument2">Arg 2/param>
        /// <param name="argument3">Arg 3/param>
        public static void Warn<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            nLogger.Warn(message, argument1, argument2, argument3);
        }

        /// <summary>
        /// Information
        /// </summary>
        /// <param name="message"></param>
        public static void Info(string message)
        {
            nLogger.Info(message);
        }

        /// <summary>
        /// Information
        /// </summary>
        /// <param name="message">message text</param>
        /// <param name="argument">Arg</param>
        public static void Info(string message, string argument)
        {
            nLogger.Info(message, argument);
        }

        /// <summary>
        /// Information
        /// </summary>
        /// <param name="message">message text</param>
        /// <param name="argument1">Arg 1</param>
        public static void Info<TArgument>(string message, TArgument argument)
        {
            nLogger.Info(message, argument);
        }

        /// <summary>
        /// Information
        /// </summary>
        /// <param name="message">message text</param>
        /// <param name="argument1">Arg 1</param>
        /// <param name="argument2">Arg 2/param>
        public static void Info<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2)
        {
            nLogger.Info(message, argument1, argument2);
        }

        /// <summary>
        /// Information
        /// </summary>
        /// <param name="message">message text</param>
        /// <param name="argument1">Arg 1</param>
        /// <param name="argument2">Arg 2/param>
        /// <param name="argument3">Arg 3/param>
        public static void Info<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            nLogger.Info(message, argument1, argument2, argument3);
        }



       
        /// <summary>
        /// Error
        /// </summary>
        /// <param name="message"></param>
        public static void Error(string message)
        {
            nLogger.Error(message);
        }

        /// <summary>
        /// Error
        /// </summary>
        /// <param name="message">message text</param>
        /// <param name="argument">Arg</param>
        public static void Error(string message, string argument)
        {
            nLogger.Error(message, argument);
        }

        /// <summary>
        /// Error
        /// </summary>
        /// <param name="message">message text</param>
        /// <param name="argument1">Arg 1</param>
        public static void Error<TArgument>(string message, TArgument argument)
        {
            nLogger.Error(message, argument);
        }

        /// <summary>
        /// Error
        /// </summary>
        /// <param name="message">message text</param>
        /// <param name="argument1">Arg 1</param>
        /// <param name="argument2">Arg 2/param>
        public static void Error<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2)
        {
            nLogger.Error(message, argument1, argument2);
        }

        /// <summary>
        /// Error
        /// </summary>
        /// <param name="message">message text</param>
        /// <param name="argument1">Arg 1</param>
        /// <param name="argument2">Arg 2/param>
        /// <param name="argument3">Arg 3/param>
        public static void Error<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            nLogger.Error(message, argument1, argument2, argument3);
        }

        /// <summary>
        /// Error
        /// </summary>
        /// <param name="e"></param>
        /// <param name="msg"></param>
        public static void Error(Exception e, string msg)
        {
            nLogger.Error(e, msg);
        }

        /// <summary>
        /// Error
        /// </summary>
        /// <param name="e"></param>
        public static void Error(Exception e)
        {
            nLogger.Error(e);
        }
    }
}
