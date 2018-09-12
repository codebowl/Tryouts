using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    /// <summary>
    /// Semaphore implementaed by hand
    /// </summary>
    class Sample1
    {
        Semaphore semaphoreOld = new Semaphore(3, 3);
        Semaphorex semaphore = new Semaphorex(3, 3);

        public static void Do()
        {
            Sample1 p = new Sample1();
            Task.Factory.StartNew(() => p.semaphore.WaitOne());
            Task.Factory.StartNew(() => p.semaphore.WaitOne());
            Task.Factory.StartNew(() => p.semaphore.WaitOne());
            Task.Factory.StartNew(() => p.semaphore.WaitOne());
            Task.Factory.StartNew(() => p.semaphore.WaitOne());
            Task.Factory.StartNew(() => p.FireIn(5000, () => p.semaphore.Release()));
            Task.Factory.StartNew(() => p.FireIn(8000, () => p.semaphore.Release()));



            Console.ReadLine();
            //p.Event += P_Event;
            //p.NumberEvent += P_NumberEvent;
            //p.Event(p, new Sample1.DataClass { Value = 9 });
        }

        private void FireIn(int ms, Action action)
        {
            new Timer(state => action(), null, ms, Timeout.Infinite);
        }

        private static int P_NumberEvent(string value)
        {
            return int.Parse(value);
        }

        private static void P_Event(object sender, DataClass e)
        {
            var result = (sender as Sample1).NumberEvent(e.Value.ToString());
            Console.WriteLine("got data {0}", result);
        }

        public delegate int GetNumber(string value);

        public event GetNumber NumberEvent;

        public event EventHandler<DataClass> Event;

        public class Semaphorex
        {
            private readonly int _maximumCount;
            private readonly object _locker = new object();
            private long _counter;

            public Semaphorex(int initialCount, int maximumCount)
            {
                _counter = initialCount;
                _maximumCount = maximumCount;
            }

            public void WaitOne(int timeout = Timeout.Infinite)
            {
                lock (_locker)
                {
                    if (_counter == 0)
                    {
                        Console.WriteLine(Thread.CurrentThread.ManagedThreadId + " blocked");
                        Monitor.Wait(_locker, timeout);
                        Console.WriteLine(Thread.CurrentThread.ManagedThreadId + " released");

                    }

                    _counter--;
                }
            }

            //public void WaitOne(int timeout = Timeout.Infinite)
            //{
            //    var value = Interlocked.Read(ref _counter);
            //    if (value == 0)
            //    {
            //        Console.WriteLine(Thread.CurrentThread.ManagedThreadId + " blocked");
            //        Monitor.Wait(_locker, timeout);
            //        Console.WriteLine(Thread.CurrentThread.ManagedThreadId + " released");
            //    }

            //    Interlocked.Decrement(ref _counter);
            //}

            public void Release() => Release(1);

            public void Release(int count)
            {
                lock (_locker)
                {
                    _counter += count;

                    if (_counter > _maximumCount)
                        throw new Exception("something");

                    Monitor.Pulse(_locker);
                }
            }

            //public void Release(int count)
            //{
            //    Interlocked.Add(ref _counter, count);

            //    if (_counter > _maximumCount)
            //        throw new Exception("something");

            //    Monitor.Pulse(_locker);
            //}
        }

        public class Semaphorex2
        {
            private readonly int _maximumCount;
            private readonly object _locker = new object();
            private int _counter;
            readonly AutoResetEvent _resetEvent = new AutoResetEvent(false);

            public Semaphorex2(int initialCount, int maximumCount)
            {
                _counter = initialCount;
                _maximumCount = maximumCount;
            }

            public void WaitOne(int timeout = Timeout.Infinite)
            {
                lock (_locker)
                {
                    if (_counter == 0)
                    {
                        _resetEvent.WaitOne(timeout);
                        Console.WriteLine("released");
                    }

                    _counter--;
                }
            }

            public void Release() => Release(1);

            public void Release(int count)
            {
                lock (_locker)
                {
                    if (_counter + count > _maximumCount) throw new Exception("cannot release more than the maximum");

                    _counter += count;

                    if (_counter > 0)
                        _resetEvent.Set();
                }
            }
        }

        internal class DataClass
        {
            public int Value { get; set; }
        }
    }
}