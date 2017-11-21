using RangedNumbers;
using System;

namespace ExampleUsageCore.CSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Define a variable that holds a number between zero and ten
            var foo = 7;

            foo += 5;

            Console.WriteLine(foo); // 12

            RangedInt dpp = foo;

            Console.WriteLine(dpp.Value);

            var bar = new RangedInt(7, 0, 10);

            bar += 5;

            Console.WriteLine(bar.Value); // 10

            RangedInt xyz = (7, 0, 10);

            xyz += 5;

            Console.WriteLine(xyz.Value); // 10

            var a = xyz < 5;
            var b = xyz > 5;
            var c = xyz <= 5;
            var d = xyz >= 5;
            var e = xyz == 5;
            var f = xyz != 5;

            sbyte bb = 8;

            RangedInt rib = bb;

            Console.WriteLine(rib);

            Console.ReadKey(true);
        }
    }
}
