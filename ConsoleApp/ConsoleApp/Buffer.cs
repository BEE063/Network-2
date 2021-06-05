using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace PP_lab1
{
    public class Buffer
    {
        public BitArray frame { get; set; }
        public BitArray receipt { get; set; }
        public BitArray request { get; set; }
        public BitArray[] _frameArray;
        public Buffer()
        {

        }
    }
}
