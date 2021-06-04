using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace PP_lab1
{
    public static class ConsoleHelper
    {
        public static object LockObject = new Object();
        public static void WriteTextMessageToConsole(BitArray array)
        {
            lock (LockObject)
            {
                byte[] bytesBack = BitArrayToByteArray(array);
                string textBack = System.Text.Encoding.Unicode.GetString(bytesBack);
                Console.WriteLine("Переданный текст: " + textBack);
            }
        }

        private static byte[] BitArrayToByteArray(BitArray array)
        {
            byte[] ret = new byte[(array.Length - 1) / 8 + 1];
            array.CopyTo(ret, 0);
            return ret;
        }

        public static void WriteToConsole(string info, string write)
        {
            lock (LockObject)
            {
                Console.WriteLine(info + " : " + write);
            }

        }
        public static void WriteToConsoleReceipt(string info, BitArray array)
        {
            lock (LockObject)
            {
                Console.Write(info + " : ");
                if (array[0] == true)
                    Console.WriteLine("Данные пришли");
                else
                    Console.WriteLine("Данные не пришли");
            }
        }
        public static void WriteToConsoleRequest(string info, string type, BitArray array)
        {
            lock (LockObject)
            {
                if (type == "connect")
                {
                    Console.Write(info + " : ");
                    if (array[0] == true)
                        Console.WriteLine("Другой поток готов принимать данные");
                    else
                        Console.WriteLine("Другой поток не готов принимать данные");
                }
                else
                {
                    Console.Write(info + " : ");
                    if (array[0] == true)
                        Console.WriteLine("Поток завершил работу и готов разорвать соединение");
                    else
                        Console.WriteLine("Поток не завершил работу и готов разорвать соединение");
                }
            }
        }
        public static void WriteToConsoleArray(string info, BitArray array)
        {
            lock (LockObject)
            {
                Console.Write(info + " : ");
                for (int i = 0; i < array.Length; i++)
                {
                    if (array[i] == true)
                        Console.Write("1");
                    else
                        Console.Write("0");

                }

                Console.WriteLine();

            }

        }
    }
}
