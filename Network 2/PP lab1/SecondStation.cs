using System;
using System.Collections;
using System.Threading;
using ThreadsEdu;

namespace PP_lab1
{
    public class SecondStation
    {
        private Semaphore _sendSemaphore;
        private Semaphore _receiveSemaphore;
        private BitArray _receivedMessageFromFirst;
        private BitArray _receivedReceiptFromFirst;
        private BitArray _receivedRequestFromFirst;
        private BitArray _sendReceipt;
        private BitArray _sendRequest;
        private PostReceiptToFirstWt _postReceiptToFirst;
        private PostDataToFirstWt _postDataToFirst;
        private PostRequestToFirstWt _postRequestToFirst;
        private BitArray _sendData;
        private bool _dataIsReceived = false;


        public SecondStation(ref Semaphore sendSemaphore, ref Semaphore receiveSemaphore)
        {
            _sendSemaphore = sendSemaphore;
            _receiveSemaphore = receiveSemaphore;
        }
        public void SendReceiptToFirst(Object obj)
        {
            _postReceiptToFirst = (PostReceiptToFirstWt)obj;
            _receiveSemaphore.WaitOne();

            _sendReceipt = new BitArray(1);

            Buffer buffer = new Buffer();
            buffer.frame = _receivedMessageFromFirst;
            buffer.request = _receivedReceiptFromFirst;

            if (buffer.frame != null && _dataIsReceived==true)
            {
                ConsoleHelper.WriteToConsoleReceipt("2 поток", _receivedRequestFromFirst);
                ConsoleHelper.WriteToConsoleArray("2 поток получил данные", buffer.frame);
                ConsoleHelper.WriteTextMessageToConsole(buffer.frame);
                Frame.GenerateReceipt(_sendReceipt, true);
                _dataIsReceived = false;
            }
            else
            {
                Frame.GenerateReceipt(_sendReceipt, false);
            }
            ConsoleHelper.WriteToConsole("2 поток", "Готовлю данные для передачи");
            ConsoleHelper.WriteToConsole("2 поток", "Данные переданы");

            _postReceiptToFirst(_sendReceipt);

            
            _sendSemaphore.Release();
            _receiveSemaphore.WaitOne();
            if (_dataIsReceived == true)
            {
                ConsoleHelper.WriteToConsoleArray("2 поток получил данные", buffer.frame);
                ConsoleHelper.WriteTextMessageToConsole(buffer.frame);
                Frame.GenerateReceipt(_sendReceipt, true);
                _sendSemaphore.Release();
            }
            ConsoleHelper.WriteToConsoleRequest("2 поток", "", buffer.request);
            
        }
        public void SendDataToFirst(Object obj)
        {
            _postDataToFirst = (PostDataToFirstWt)obj;
            _sendData = new BitArray(64);
            _postDataToFirst(Frame.GenerateData("World"));
            
        }
        public void SendRequestToFirst(object obj)
        {
            _postRequestToFirst = (PostRequestToFirstWt)obj;
            _sendRequest = new BitArray(1);
            Frame.GenerateReceipt(_sendRequest, true);
            _postRequestToFirst(_sendRequest);
            _sendSemaphore.Release();
        }
        public void ReceivedRequestFromFirst(BitArray array)
        {
            _receivedRequestFromFirst = array;
        }
        public void ReceiveDataFromFirst(BitArray array)
        {
            _receivedMessageFromFirst = array;
            _dataIsReceived = true;
        }
        public void ReceiveReceiptFromFirst(BitArray array)
        {
            _receivedReceiptFromFirst = array;
        }
    }
}
