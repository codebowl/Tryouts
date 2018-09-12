using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    /// <summary>
    /// AutoResetEvents games
    /// </summary>
    public class Sample4
    {
        AutoResetEvent resetEvent = new AutoResetEvent(false);
        public static void Do()
        {
            Sample4 sample4 = new Sample4();
            Task.Factory.StartNew(() => sample4.FirstAction());
            Task.Factory.StartNew(() => sample4.SecondAction());
            Task.Factory.StartNew(() => sample4.ThirdAction());

            Console.ReadLine();
        }

        public void FirstAction()
        {
            while (true)
            {
                Console.WriteLine("A start");
                Thread.Sleep(4000);
                Console.WriteLine("A done");
                resetEvent.Set();
                //resetEvent.Reset();
                Thread.Sleep(2000);
            }
        }

        public void SecondAction()
        {
            while (true)
            {
                Console.WriteLine("B start");
                resetEvent.WaitOne();
                Console.WriteLine("B done");
                Thread.Sleep(2000);
            }
        }

        public void ThirdAction()
        {
            while (true)
            {
                Console.WriteLine("C start");
                resetEvent.WaitOne();
                Console.WriteLine("C done");
                Thread.Sleep(2000);
            }
        }
    }
}