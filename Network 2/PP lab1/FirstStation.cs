using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using ThreadsEdu;

namespace PP_lab1
{
    public class FirstStation
    {
        private Semaphore _sendSemaphore;
        private Semaphore _receiveSemaphore;

        private BitArray _receivedReceipt;
        private BitArray _receivedData;
        private BitArray _receivedRequest;
        private BitArray _sendMessage;
        private BitArray _sendReceipt;
        private BitArray _sendRequest;

        private PostDataToSecondWT _postData;
        private PostReceiptToSecondWt _postReceipt;
        private PostRequestToSecondWt _postRequest;

        public FirstStation(ref Semaphore sendSemaphore, ref Semaphore receiveSemaphore)
        {
            _sendSemaphore = sendSemaphore;
            _receiveSemaphore = receiveSemaphore;
        }
        public void SendDataToSecond(object obj)
        {
            
            _postData = (PostDataToSecondWT)obj;
            _receiveSemaphore.WaitOne();

            Buffer buffer = new Buffer();
            buffer.request = _receivedRequest;

            ConsoleHelper.WriteToConsole("1 поток", "Начинаю работу.Готовлю данные для передачи");
            ConsoleHelper.WriteToConsoleRequest("1 поток","connect", buffer.request);

            _sendMessage = new BitArray(64);
            _postData(Frame.GenerateData("Hello"));

            _sendSemaphore.Release();

            ConsoleHelper.WriteToConsole("1 поток", "Данные переданы");

            _receiveSemaphore.WaitOne();

            
            buffer.receipt = _receivedReceipt;
            buffer.frame = _receivedData;
            if (buffer.receipt == null)
            {
                Thread.Sleep(3000);
                if (buffer.receipt == null)
                {
                    ConsoleHelper.WriteToConsoleReceipt("1 поток", buffer.receipt);
                    ConsoleHelper.WriteToConsoleArray("1 поток получил данные", buffer.frame);
                    ConsoleHelper.WriteToConsole("1 поток", "Отправляю повторно");
                    _postData(Frame.GenerateData("Hello"));
                }
                else
                {
                    ConsoleHelper.WriteToConsoleArray("1 поток поток получил данные", buffer.frame);
                    ConsoleHelper.WriteToConsoleReceipt("1 поток", buffer.receipt);
                }
            }
            else
            {
                ConsoleHelper.WriteToConsoleReceipt("1 поток", buffer.receipt);
                ConsoleHelper.WriteToConsoleArray("1 поток", buffer.frame);
            }
            
            ConsoleHelper.WriteTextMessageToConsole(_receivedData);
            ConsoleHelper.WriteToConsoleRequest("1 поток", "", buffer.request);

            _sendSemaphore.Release();
        }
        public void SendReceiptToSecond(object obj)
        {
            _postReceipt = (PostReceiptToSecondWt)obj;
            _sendReceipt = new BitArray(1);
            Frame.GenerateReceipt(_sendReceipt, true);
            _postReceipt(_sendReceipt);
        }
        public void SendRequestToSecond(object obj)
        {
            _sendSemaphore.WaitOne();
            _postRequest = (PostRequestToSecondWt)obj;
            _sendRequest = new BitArray(1);
            Frame.GenerateReceipt(_sendRequest, true);
            _postRequest(_sendRequest);
            _sendSemaphore.Release();
        }
        public void ReceiveRequestFromSecond(BitArray array)
        {
            _receivedRequest = array;
        }
        public void ReceiveReceiptFromSecond(BitArray array)
        {
            _receivedReceipt = array;
        }
        public void ReceiveDataFromSecond(BitArray array)
        {
            _receivedData = array;
        }

    }
}
