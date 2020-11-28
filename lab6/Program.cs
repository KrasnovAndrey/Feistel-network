using System;
using System.IO;

namespace lab6
{
    class Program
    {
        public const byte k = 0x55; //01010101
        public const int iter = 3;
        static void Main(string[] args)
        {
            while (true)
            {
                char menu = 'a';
                while (menu != '1' && menu != '2' && menu != '3')
                {
                    Console.WriteLine("1. Encryption \n2. Decryption \n3. Exit");
                    Console.Write(">>");
                    menu = Console.ReadKey().KeyChar;
                    if (menu != '0' && menu != '1')
                    {
                        Console.WriteLine("\nEncorrect command. Choose 1, 2 or 3;");
                        Console.WriteLine(menu);
                    }
                }
                if (menu == '3')
                    return;
                Console.Clear();
                if (menu == '1')
                    Console.WriteLine("Path to file for encryption:");
                else
                    Console.WriteLine("Path to file for decryption:");


                string pathForOpen = Console.ReadLine();
                if (menu == '1')
                    Console.WriteLine("Name of file to save encryption text:");
                else
                    Console.WriteLine("Name of file to save decryption text:");
                string pathForClose = pathForOpen.Substring(0, pathForOpen.LastIndexOf('\\') + 1) + Console.ReadLine();

                if (menu == '1')
                    Encryption(pathForOpen, pathForClose);
                else
                    Decryption(pathForOpen, pathForClose);
                Console.Clear();
            }
        }

        static byte EncrFunc(byte x, byte key)
        {
            byte result = (byte)((x << 3) ^ (x << 4) ^ key);
            return result;
        }


        static void Encryption(string pathF, string pathS)
        {
            FileStream fileOpen = new FileStream(pathF, FileMode.Open, FileAccess.Read);
            FileStream fileClose = new FileStream(pathS, FileMode.Create, FileAccess.Write);
            byte[] buf = new byte[2];
            byte li = 0;
            byte ri = 0;
            byte li_1 = 0;
            byte ri_1 = 0;
            long lenght = fileOpen.Length / 2;
            long end = fileOpen.Length % 2;
            fileOpen.Position = 0;
            fileClose.Position = 0;
            int counter = 0;
            while (counter < lenght)
            {
                for (int i = 0; i < 2; i++)
                {
                    buf[i] = (byte)fileOpen.ReadByte();
                    if (i == 0)
                        li_1 = buf[i];
                    else
                        ri_1 = buf[i];
                }
                for (int i = 0; i < iter; ++i)
                {
                    li = ri_1;
                    ri = (byte)(li_1 ^ EncrFunc(ri_1, k));
                    li_1 = li;
                    ri_1 = ri;
                }
                buf[0] = li_1;
                buf[1] = ri_1;
                for (int i = 0; i < 2; i++)
                {
                    fileClose.WriteByte(buf[i]);
                }
                counter++;
            }
            if (end != 0)
            {
                buf[0] = (byte)fileOpen.ReadByte();
                buf[1] = 0;
                li_1 = buf[0];
                ri_1 = buf[1];
                for (int i = 0; i < iter; ++i)
                {
                    li = ri_1;
                    ri = (byte)(li_1 ^ EncrFunc(ri_1, k));
                    li_1 = li;
                    ri_1 = ri;
                }
                buf[0] = li_1;
                buf[1] = ri_1;
                for (int i = 0; i < 2; i++)
                {
                    fileClose.WriteByte(buf[i]);
                }
            }

            fileOpen.Dispose();
            fileOpen.Close();
            fileClose.Dispose();
            fileClose.Close();
        }

        static void Decryption(string pathF, string pathS)
        {
            FileStream fileOpen = new FileStream(pathF, FileMode.Open, FileAccess.Read);
            FileStream fileClose = new FileStream(pathS, FileMode.Create, FileAccess.Write);
            byte[] buf = new byte[2];
            byte li = 0;
            byte ri = 0;
            byte li_1 = 0;
            byte ri_1 = 0;
            long lenght = fileOpen.Length / 2;
            long end = fileOpen.Length % 2;
            fileOpen.Position = 0;
            fileClose.Position = 0;
            int counter = 0;
            while (counter != (lenght - end))
            {
                for (int i = 0; i < 2; i++)
                {
                    buf[i] = (byte)fileOpen.ReadByte();
                    if (i == 0)
                        li = buf[i];
                    else
                        ri = buf[i];
                }
                for (int i = 0; i < iter; ++i)
                {
                    ri_1 = li;
                    li_1 = (byte)(ri ^ EncrFunc(li, k));
                    li = li_1;
                    ri = ri_1;
                }
                buf[0] = li;
                buf[1] = ri;
                for (int i = 0; i < 2; i++)
                {
                    fileClose.WriteByte(buf[i]);
                }
                counter++;
            }
            if (end != 0)
            {
                buf[0] = (byte)fileOpen.ReadByte();
                buf[1] = 0;
                li = buf[0];
                ri = buf[1];
                for (int i = 0; i < iter; ++i)
                {
                    ri_1 = li;
                    li_1 = (byte)(ri ^ EncrFunc(li, k));
                    li = li_1;
                    ri = ri_1;
                }
                buf[0] = li;
                buf[1] = ri;
                for (int i = 0; i < 2; i++)
                {
                    fileClose.WriteByte(buf[i]);
                }
            }

            fileOpen.Dispose();
            fileOpen.Close();
            fileClose.Dispose();
            fileClose.Close();
        }
    }
}
