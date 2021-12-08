using System;

namespace CRYPTOPALS
{
    class Program
    {
        static void Main(string[] args)
        {
            bool ok;

            Console.WriteLine("Runnning Set01 Challenge01...");
            ok = Set01.Challenge01.Run();
            Console.WriteLine("> {0}\n\n", ok ? "PASS" : "FAIL");

            Console.WriteLine("Runnning Set01 Challenge02...");
            ok = Set01.Challenge02.Run();
            Console.WriteLine("> {0}\n\n", ok ? "PASS" : "FAIL");

            Console.WriteLine("Runnning Set01 Challenge03...");
            ok = Set01.Challenge03.Run();
            Console.WriteLine("> {0}\n\n", ok ? "PASS" : "FAIL");

            Console.WriteLine("Runnning Set01 Challenge04...");
            ok = Set01.Challenge04.Run();
            Console.WriteLine("> {0}\n\n", ok ? "PASS" : "FAIL");

            Console.WriteLine("Runnning Set01 Challenge05...");
            ok = Set01.Challenge05.Run();
            Console.WriteLine("> {0}\n\n", ok ? "PASS" : "FAIL");

            Console.WriteLine("Runnning Set01 Challenge06...");
            ok = Set01.Challenge06.Run();
            Console.WriteLine("> {0}\n\n", ok ? "PASS" : "FAIL");

            Console.WriteLine("Runnning Set01 Challenge07...");
            ok = Set01.Challenge07.Run();
            Console.WriteLine("> {0}\n\n", ok ? "PASS" : "FAIL");

            Console.WriteLine("Runnning Set01 Challenge08...");
            ok = Set01.Challenge08.Run();
            Console.WriteLine("> {0}\n\n", ok ? "PASS" : "FAIL");

            ////////////////////////////////////////////////////////////////////////

            Console.WriteLine("Runnning Set02 Challenge01...");
            ok = Set02.Challenge09.Run();
            Console.WriteLine("> {0}\n\n", ok ? "PASS" : "FAIL");

            Console.WriteLine("Runnning Set02 Challenge02...");
            ok = Set02.Challenge10.Run();
            Console.WriteLine("> {0}\n\n", ok ? "PASS" : "FAIL");
        }
    }
}
