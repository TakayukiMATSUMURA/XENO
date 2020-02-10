using System;

namespace XENO
{
    public static class Log
    {
        public static void Output(object message)
        {
#if DEBUG
            Console.WriteLine(message);
#endif
        }
    }
}
