using PP_lab1;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using ThreadsEdu;

namespace ConsoleApp
{
    public class FirstThread
    {
        private Semaphore _sendSemaphore;
        private Semaphore _receiveSemaphore;
        private BitArray _receivedMessage;
        private BitArray _sendMessage;
        private BitArray _sendReceipt;
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
            _sendReceipt = new BitArray(1);
            Frame.GenerateReceipt(_sendReceipt, true);
            _post(_sendReceipt);
            _sendSemaphore.Release();
           
            //2
            _receiveSemaphore.WaitOne();
            ConsoleHelper.WriteToConsoleRequest("1 поток","", _receivedMessage);
            _post(Frame.GenerateData("Hello"));

            _sendMessage = new BitArray(56);

            _sendSemaphore.Release();
            //3
            _receiveSemaphore.WaitOne();
            ConsoleHelper.WriteToConsoleRequest("1 поток", "connect", _receivedMessage);
            Frame.GenerateReceipt(_sendReceipt, true);
            _post(_sendReceipt);
            _sendSemaphore.Release();
            //4
            _receiveSemaphore.WaitOne();

            _sendSemaphore.Release();
            //5
            _receiveSemaphore.WaitOne();
            ConsoleHelper.WriteToConsole("1 поток", "Данные полученны");
            ConsoleHelper.WriteToConsoleArray("1 поток", _receivedMessage);
            ConsoleHelper.WriteTextMessageToConsole(_receivedMessage);
            
            Frame.GenerateRequest(_sendReceipt, true);
            _post(_sendReceipt);
            _sendSemaphore.Release();
            //6
            _receiveSemaphore.WaitOne();

            ConsoleHelper.WriteToConsoleDisconect("1 поток", "", _receivedMessage);
            ConsoleHelper.WriteToConsole("1 поток", "Заканчиваю работу");
            _sendSemaphore.Release();

        }
        public void ReceiveData(BitArray array)
        {
            _receivedMessage = array;
        }

    }
}
