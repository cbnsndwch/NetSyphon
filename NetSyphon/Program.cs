using System;
using PowerArgs;

namespace NetSyphon
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                Args.InvokeMain<Entry>(args);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception ocurred: {e.Message}");
                Console.WriteLine($"StackTrace: {e.StackTrace}");
            }

        }
    }
}
