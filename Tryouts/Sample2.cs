using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    /// <summary>
    /// reset events games
    /// </summary>
    public class Sample2
    {
        static AutoResetEvent resetEvent = new AutoResetEvent(false);

        public static void Do()
        {
            Task.Factory.StartNew(() => { GetDataFromServer(1); });
            Task.Factory.StartNew(() => { GetDataFromServer(2); });
            Task.Factory.StartNew(() => { GetDataFromServer(3); });

            string input = null;
            while (input != "x")
            {
                Console.WriteLine("Hit enter");
                input = Console.ReadLine();
                resetEvent.Set();
            }
        }

        static void GetDataFromServer(int serverNumber)
        {
            while (true)
            {
                Console.WriteLine(DateTime.Now.ToLongTimeString() + " I get first data from server" + serverNumber);
                resetEvent.WaitOne();
            }
        }
    }
}