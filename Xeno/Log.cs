using System;
using System.Diagnostics;

namespace XENO
{
    public static class Log
    {
        [Conditional("DEBUG")]
        public static void Output(object message)
        {
            Console.WriteLine(message);
        }
    }
}
