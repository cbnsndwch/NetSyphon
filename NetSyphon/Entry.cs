using System;
using NetSyphon.Domain;
using PowerArgs;

namespace NetSyphon
{
    internal class Entry
    {
        private static void Main(string[] args)
        {
            try
            {
                Args.InvokeMain<Program>(args);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception ocurred: {e.Message}");
                Console.WriteLine($"StackTrace: {e.StackTrace}");
            }

        }
    }
}
