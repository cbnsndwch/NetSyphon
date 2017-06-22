using PowerArgs;

namespace NetSyphon.Domain
{
    public class Program
    {
        [HelpHook, ArgShortcut("h"), ArgDescription("Displays this help information")]
        public bool Help { get; set; }

        //[ArgActionMethod, ArgDescription("Adds the two operands")]
        //public void Add(string args)
        //{
        //    Console.WriteLine(args.Value1 + args.Value2);
        //}

        //[ArgActionMethod, ArgDescription("Subtracts the two operands")]
        //public void Subtract(TwoOperandArgs args)
        //{
        //    Console.WriteLine(args.Value1 - args.Value2);
        //}

        public void Main()
        {
            
        }
    }
}
