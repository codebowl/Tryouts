using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Sample6
    {
        private ReaderWriterLockSlimx _readerWriterLock = new ReaderWriterLockSlimx();
        private Random random = new Random((int)DateTime.Now.Ticks);

        public static void Do()
        {
            Sample6 sample6 = new Sample6();
            Task.Factory.StartNew(() => sample6.Read());
            Task.Factory.StartNew(() => sample6.Read());
            Task.Factory.StartNew(() => sample6.Read());
            Task.Factory.StartNew(() => sample6.Read());
            Task.Factory.StartNew(() => sample6.Read());
            Task.Factory.StartNew(() => sample6.Read());
            Task.Factory.StartNew(() => sample6.Write());
            Task.Factory.StartNew(() => sample6.Write());

            Console.ReadLine();
        }

        public void Read()
        {
            while (true)
            {
                _readerWriterLock.EnterReadLock();
                Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} is start reading");
                Thread.Sleep(100);
                Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} is done reading");
                _readerWriterLock.ExitReadLock();

                Thread.Sleep(random.Next(500, 1000));
            }
        }

        public void Write()
        {
            while (true)
            {
                _readerWriterLock.EnterWriteLock();
                Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} is start writing".ToUpper());
                Thread.Sleep(3000);
                Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} is done writing".ToUpper());
                _readerWriterLock.ExitWriteLock();
                Thread.Sleep(random.Next(4000, 10000));
            }
        }

        class ReaderWriterLockSlimx
        {
            private int _readers = 0;
            readonly ManualResetEvent _resetEventForReaders = new ManualResetEvent(true);
            readonly AutoResetEvent _resetEventForWriters = new AutoResetEvent(true);

            public void EnterReadLock()
            {
                _resetEventForWriters.Reset();
                _resetEventForReaders.WaitOne();
                Interlocked.Increment(ref _readers);
            }

            public void ExitReadLock()
            {
                var value = Interlocked.Decrement(ref _readers);
                if (value == 0)
                {
                    _resetEventForWriters.Set();
                }
            }

            public void EnterWriteLock()
            {
                _resetEventForWriters.WaitOne();
                Console.WriteLine(">>\t currently readers count: " + _readers);
                _resetEventForReaders.Reset();
            }

            public void ExitWriteLock()
            {
                _resetEventForReaders.Set();
            }
        }
    }
}