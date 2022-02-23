using System;
using System.IO;

namespace MNK.Unpacker
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("MONARK LPKG Unpacker");
            Console.WriteLine("(c) 2022 Ekey (h4x0r) / v{0}\n", Utils.iGetApplicationVersion());
            Console.ResetColor();

            if (args.Length != 2)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("[Usage]");
                Console.WriteLine("    MNK.Unpacker <m_File> <m_Directory>\n");
                Console.WriteLine("    m_File - Source of LPKG archive file");
                Console.WriteLine("    m_Directory - Destination directory\n");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("[Examples]");
                Console.WriteLine("    MNK.Unpacker E:\\Games\\MONARK\\data.lpkg D:\\Unpacked");
                Console.ResetColor();
                return;
            }

            String m_LpkgFile = args[0];
            String m_Output = Utils.iCheckArgumentsPath(args[1]);

            if (!File.Exists(m_LpkgFile))
            {
                Utils.iSetError("[ERROR]: Input LPKG file -> " + m_LpkgFile + " <- does not exist");
                return;
            }

            LpkgUnpack.iDoIt(m_LpkgFile, m_Output);
        }
    }
}
