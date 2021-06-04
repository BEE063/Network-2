using PP_lab1;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ThreadsEdu
{
    public delegate void PostReceiptToFirstWt(BitArray message);
    public delegate void PostDataToSecondWT(BitArray message);
    public delegate void PostDataToFirstWt(BitArray message);
    public delegate void PostReceiptToSecondWt(BitArray message);
    public delegate void PostRequestToSecondWt(BitArray message);
    public delegate void PostRequestToFirstWt(BitArray message);
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleHelper.WriteToConsole("Главный поток", "");
            Semaphore firstReceiveSemaphore = new Semaphore(0, 1);
            Semaphore secondReceiveSemaphore = new Semaphore(0, 1);

            FirstStation firstStation = new FirstStation(ref secondReceiveSemaphore, ref firstReceiveSemaphore);
            SecondStation secondStation = new SecondStation(ref firstReceiveSemaphore, ref secondReceiveSemaphore);

            Thread threadFifth = new Thread(new ParameterizedThreadStart(firstStation.SendRequestToSecond));
            Thread threadSixth = new Thread(new ParameterizedThreadStart(secondStation.SendRequestToFirst));
            Thread threadFirst = new Thread(new ParameterizedThreadStart(firstStation.SendDataToSecond));
            Thread threadSecond = new Thread(new ParameterizedThreadStart(secondStation.SendReceiptToFirst));
            Thread threadThird = new Thread(new ParameterizedThreadStart(secondStation.SendDataToFirst));
            Thread threadFourth = new Thread(new ParameterizedThreadStart(firstStation.SendReceiptToSecond));
            


            PostReceiptToFirstWt postReciptToFirstWt = new PostReceiptToFirstWt(firstStation.ReceiveReceiptFromSecond);
            PostDataToSecondWT postDataToSecondWt = new PostDataToSecondWT(secondStation.ReceiveDataFromFirst);
            PostDataToFirstWt postDataToFirstWt = new PostDataToFirstWt(firstStation.ReceiveDataFromSecond);
            PostReceiptToSecondWt postReceiptToSecondWt = new PostReceiptToSecondWt(secondStation.ReceiveReceiptFromFirst);
            PostRequestToFirstWt postRequestToFirstWt = new PostRequestToFirstWt(firstStation.ReceiveRequestFromSecond);
            PostRequestToSecondWt postRequestToSecondWt = new PostRequestToSecondWt(secondStation.ReceivedRequestFromFirst);

            threadFifth.Start(postRequestToSecondWt);
            threadSixth.Start(postRequestToFirstWt);
            threadFirst.Start(postDataToSecondWt);
            threadSecond.Start(postReciptToFirstWt);
            threadThird.Start(postDataToFirstWt);
            threadFourth.Start(postReceiptToSecondWt);

            Console.ReadLine();

        }
    }
    

}