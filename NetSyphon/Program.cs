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
                args = new[] { "-h" };
                //args = new[] { "job", "D:\\test.json" };
                //args = new[] { "new", "D:\\test.json", "D:\\out.json" };

                Args.InvokeAction<Entry>(args);

                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception ocurred: {e.Message}");
                Console.WriteLine($"StackTrace: {e.StackTrace}");
            }

        }
    }
}
