using PP_lab1;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using ThreadsEdu;
using Buffer = PP_lab1.Buffer;

namespace ConsoleApp
{
    public class FirstThread
    {
        private Semaphore _sendSemaphore;
        private Semaphore _receiveSemaphore;
        private BitArray[] _receivedMessage;
        private BitArray[] _sendMessage;
        private BitArray[] _sendReceipt;
        private PostToSecondWT _post;

        public FirstThread(ref Semaphore sendSemaphore, ref Semaphore receiveSemaphore)
        {
            _sendSemaphore = sendSemaphore;
            _receiveSemaphore = receiveSemaphore;
        }
        public void FirstThreadMain(object obj)
        {
            //1
            _post = (PostToSecondWT)obj;
            ConsoleHelper.WriteToConsole("1 поток", "Начинаю работу.Готовлю данные для передачи.");
            _sendReceipt = new BitArray[1];
            Frame.GenerateReceipt(_sendReceipt, true);
            _post(_sendReceipt);
            _sendSemaphore.Release();
           
            //2
            _receiveSemaphore.WaitOne();
            ConsoleHelper.WriteToConsoleRequest("1 поток","", _receivedMessage);
            _post(Frame.GenerateData("Hello"));

            _sendMessage = new BitArray[2];

            _sendSemaphore.Release();
            //3
            _receiveSemaphore.WaitOne();

            ResendData(_post, _receivedMessage);

            _sendSemaphore.Release();
            //4
            _receiveSemaphore.WaitOne();

            ResendData(_post, _receivedMessage);
            ConsoleHelper.WriteToConsoleReceipt("1 поток", _receivedMessage);
            ConsoleHelper.WriteToConsoleRequest("1 поток","connect", _receivedMessage);
            _sendSemaphore.Release();
            //5
            _receiveSemaphore.WaitOne();

            Buffer buffer = new Buffer();
            ConsoleHelper.WriteToConsoleMatrixBitArray("1 поток", _receivedMessage);
            ConsoleHelper.WriteTextMessageToConsole("1 поток переданный текст: ", _receivedMessage);
            Frame.GenerateReceipt(_sendReceipt, buffer.CheckSum(_receivedMessage));
            bool check = buffer.CheckSum(_receivedMessage);
            _post(_sendReceipt);
            _sendSemaphore.Release();
            ////6
            _receiveSemaphore.WaitOne();

            if (check == false)
            {
                ConsoleHelper.WriteToConsoleMatrixBitArray("1 поток", _receivedMessage);
                ConsoleHelper.WriteTextMessageToConsole("1 поток переданный текст: ", _receivedMessage);
                Frame.GenerateReceipt(_sendReceipt, buffer.CheckSum(_receivedMessage));
                _post(_sendReceipt);
            }

            _sendSemaphore.Release();
            //7
            _receiveSemaphore.WaitOne();

            ConsoleHelper.WriteToConsoleDisconnect("1 поток","", _receivedMessage);
            ConsoleHelper.WriteToConsole("2 поток", "Заканчиваю работу");

            _sendSemaphore.Release();
  

        }
        public void ReceiveData(BitArray[] array)
        {
            _receivedMessage = array;
        }
        public void ResendData(PostToSecondWT _postTo, BitArray[] array)
        {
            if (array[0][0] == false || array == null)
            {
                ConsoleHelper.WriteToConsoleReceipt("1 поток", array);
                ConsoleHelper.WriteToConsole("1 поток", "Отправляю повторно");   
                _postTo(Frame.GenerateData("Hello"));

            }

        }

    }
}
