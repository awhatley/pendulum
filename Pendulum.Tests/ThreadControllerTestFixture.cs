using System;
using System.Threading;

using NUnit.Framework;

namespace Pendulum
{
    [TestFixture]
    public class ThreadControllerTestFixture
    {
        [Test]
        public void Add_DoesNotStartTasks()
        {
            var token = "Token";
            using(var controller = new ThreadController())
            {
                controller.Add(c => { token = "Changed"; });

                Thread.Sleep(50);
                Assert.That(token, Is.EqualTo("Token"));

                controller.Stop();
            }
        }

        [Test]
        public void Start_StartsAllPendingTasks()
        {
            long[] token = { 0 };
            using(var controller = new ThreadController())
            {
                controller.Add(c => Interlocked.Increment(ref token[0]));
                controller.Add(c => Interlocked.Increment(ref token[0]));
                controller.Add(c => Interlocked.Increment(ref token[0]));

                Assert.That(Interlocked.Read(ref token[0]), Is.EqualTo(0));

                controller.Start();
                Thread.Sleep(50);

                Assert.That(Interlocked.Read(ref token[0]), Is.EqualTo(3));

                controller.Stop();
            }
        }

        [Test]
        public void StartAction_StartsTaskImmediately()
        {
            var token = "Token";
            using(var controller = new ThreadController())
            {
                controller.Start(c => { token = "Changed"; });

                Thread.Sleep(50);
                Assert.That(token, Is.EqualTo("Changed"));

                controller.Stop();
            }
        }

        [Test]
        public void Pause_PausesWaitingTasks()
        {
            long[] token = { 0 };
            using(var controller = new ThreadController())
            {
                controller.Add(c => { Thread.Sleep(50); c.Wait(); Interlocked.Increment(ref token[0]); });
                controller.Add(c => { Thread.Sleep(50); c.Wait(); Interlocked.Increment(ref token[0]); });
                controller.Add(c => { Thread.Sleep(50); c.Wait(); Interlocked.Increment(ref token[0]); });

                controller.Start();
                controller.Pause();

                Thread.Sleep(100);
                Assert.That(Interlocked.Read(ref token[0]), Is.EqualTo(0));

                controller.Stop();
            }
        }

        [Test]
        public void Resume_ResumesWaitingTasks()
        {
            long[] token = { 0 };
            using(var controller = new ThreadController())
            {
                controller.Add(c => { Thread.Sleep(10); c.Wait(); Interlocked.Increment(ref token[0]); });
                controller.Add(c => { Thread.Sleep(10); c.Wait(); Interlocked.Increment(ref token[0]); });
                controller.Add(c => { Thread.Sleep(10); c.Wait(); Interlocked.Increment(ref token[0]); });

                controller.Start();
                controller.Pause();

                Assert.That(Interlocked.Read(ref token[0]), Is.EqualTo(0));

                controller.Resume();
                
                Thread.Sleep(50);
                Assert.That(Interlocked.Read(ref token[0]), Is.EqualTo(3));

                controller.Stop();
            }
        }

        [Test]
        public void Wait_WithTimeout_BlocksExecution()
        {
            var fail = false;
            using(var controller = new ThreadController())
            {
                controller.Add(c =>
                {
                    c.Wait(TimeSpan.FromMilliseconds(100));
                    fail = true;
                });

                controller.Add(c => c.Wait());

                controller.Start();
                Thread.Sleep(50);
                controller.Stop();

                Assert.That(fail, Is.False);
            }
        }

        [Test]
        public void Wait_ThrowsOperationCanceledException_OnStop()
        {
            var fail = true;
            using(var controller = new ThreadController())
            {
                controller.Add(c =>
                    {
                        try
                        {
                            Thread.Sleep(100);
                            c.Wait();
                        }
                        catch(OperationCanceledException)
                        {
                            fail = false;
                        }
                    });

                controller.Start();
                controller.Stop();

                Assert.That(fail, Is.False, "Expected OperationCanceledException");
            }
        }

        [Test]
        public void Wait_OnNonTaskThread_BlocksForActivity()
        {
            var fail = true;
            using(var controller = new ThreadController())
            {
                controller.Start();
                controller.Pause();

                Action func = () =>
                {
                    controller.Wait(TimeSpan.FromMilliseconds(50));
                    fail = true;
                };

                fail = false;
                var result = func.BeginInvoke(null, null);
                controller.Stop();
                result.AsyncWaitHandle.WaitOne();

                Assert.That(fail, Is.False, "Task did not wait properly.");
            }
        }

        [Test]
        public void Stop_DoesNotPropagate_OperationCanceledExceptions()
        {
            using(var controller = new ThreadController())
            {
                controller.Add(c => { Thread.Sleep(50); c.Wait(); });
                controller.Add(c => { Thread.Sleep(50); c.Wait(); });
                controller.Add(c => { Thread.Sleep(50); throw new OperationCanceledException(); });

                controller.Start();
                controller.Stop();
            }
        }

        [Test]
        public void Stop_PropagatesExceptions()
        {
            using(var controller = new ThreadController())
            {
                controller.Add(c => { throw new InvalidOperationException(); });
                controller.Add(c => { Thread.Sleep(50); c.Wait(); throw new ArgumentOutOfRangeException(); });
                controller.Add(c => { throw new ArgumentNullException(); });

                controller.Start();

                try
                {
                    controller.Stop();
                    Assert.Fail("Expected AggregateException.");
                }

                catch(AggregateException)
                {
                }
            }
        }
    }
}