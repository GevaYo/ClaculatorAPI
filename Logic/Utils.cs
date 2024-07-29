using NLog;
using System.Diagnostics;

namespace ClaculatorAPI.Logic
{
    public static class Utils
    {
        public static int requestCounter = 0;
        public static Stopwatch timer;
        public static TimeSpan timeSpan;
        static readonly Dictionary<string, int> r_ValidOperations
                    = new Dictionary<string, int>
                    {
                        { "plus", 2 },
                        { "minus", 2 },
                        { "times", 2 },
                        { "divide", 2 },
                        { "pow", 2 },
                        { "abs", 1 },
                        { "fact", 1 }
                    };
        public static Dictionary<string, int> ValidOperations { get { return r_ValidOperations; } }
        public static int calculate(string i_Operation, List<int> i_Arguments)
        {
            int result = 0;
            int x, y;
            x = i_Arguments[0];
            y = i_Arguments.Count > 1 ? i_Arguments[1] : 0;

            switch (i_Operation)
            {
                case "plus":
                    result = x + y; break;
                case "minus":
                    result = x - y; break;
                case "times":
                    result = x * y; break;
                case "divide":
                    if(y == 0) { throw new Exception("Error while performing operation Divide: division by 0"); }
                    result = x / y; break;
                case "pow":
                    result = (int)Math.Pow(x,y); break;
                case "abs":
                    result = Math.Abs(x); break;
                case "fact":
                    result = factorial(x); ; break;
                default:
                    break;
            }

            return result;
        }

        public static int factorial(int i_Number)
        {
            if(i_Number < 0)
            {
                throw new Exception("Error while performing operation Factorial: not supported for the negative number");
            }
            return (i_Number == 1 || i_Number == 0) ? 1 : i_Number * factorial(i_Number - 1);
        }

        public static string getLoggerCurrentLevel(string loggerName)
        {
            var name = "";
            switch(loggerName) 
            {
                case "request-logger":
                    name = LogManager.Configuration.Variables["request-logger"].ToString().ToUpper();
                    break;
                case "stack-logger":
                    name = LogManager.Configuration.Variables["stack-logger"].ToString().ToUpper();
                    break;
                case "independent-logger":
                    name = LogManager.Configuration.Variables["independent-logger"].ToString().ToUpper();
                    break;
                default:
                    throw new Exception($"Error! this logger name: {loggerName} doesn't exist!");
            }
            return name;
        }

        public static string setLoggerLevel(string loggerName, string level)
        {
            if (level != "INFO" && level != "DEBUG" && level != "ERROR")
            {
                throw new Exception($"Error! this logger level: {level} doesn't exist!");
            }
            else
            {
                switch (loggerName)
                {
                    case "request-logger":
                        LogManager.Configuration.Variables["request-logger"] = level;
                        break;
                    case "stack-logger":
                        LogManager.Configuration.Variables["stack-logger"] = level;
                        break;
                    case "independent-logger":
                        LogManager.Configuration.Variables["independent-logger"] = level;
                        break;
                    default:
                        throw new Exception($"Error! this logger name: {loggerName} doesn't exist!");
                }
                LogManager.ReconfigExistingLoggers();
            }

            return loggerName;
        }
    }

}






