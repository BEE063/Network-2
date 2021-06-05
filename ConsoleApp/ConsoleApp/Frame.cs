using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace PP_lab1
{
    public class Frame
    {
        public static BitArray GenerateData(string message)
        {
            byte[] bytes = System.Text.Encoding.Unicode.GetBytes(message);
            BitArray array = new BitArray(bytes);
            return array;
        }

        private static byte[] BitArrayToByteArray(BitArray bits)
        {
            byte[] ret = new byte[(bits.Length - 1) / 8 + 1];
            bits.CopyTo(ret, 0);
            return ret;
        }

        public static void GenerateReceipt(BitArray array, bool isValid)
        {
            if (isValid == true)
            {
                array[0] = true;
            }
            else
            {
                array[0] = false;
            }
        }
        public static void GenerateRequest(BitArray array, bool isValid)
        {
            if (isValid == true)
            {
                array[0] = true;
            }
            else
            {
                array[0] = false;
            }
        }

    }
}

