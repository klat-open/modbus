using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Code4Bugs.Utils.Event;

namespace Code4Bugs.Utils.Tests.Event
{
    [TestFixture]
    internal partial class EventBusTest
    {
        private IThreadHelper _threadHelper;

        private EventBus CreateEventBus()
        {
            var eventBus = new EventBus();
            eventBus.SetThreadHelper(_threadHelper);
            return eventBus;
        }

        [OneTimeSetUp]
        public void Setup()
        {
            _threadHelper = new DummyThreadHelper(Thread.CurrentThread.ManagedThreadId);
        }

        [Test]
        public void container_can_subscribe()
        {
            var eventBus = CreateEventBus();
            var container = new DummyContainer();

            DummyMessage message = null;
            container.SubscriberExecuted += msg => { message = msg; };

            eventBus.Register(container);
            eventBus.Post(new DummyMessage { Content = "helloworld" });
            Assert.NotNull(message);
            Assert.AreEqual(message.Content, "helloworld");
        }

        [Test]
        public void container_can_subscribe_while_posting()
        {
            var eventBus = CreateEventBus();

            new Thread(() =>
            {
                for (var i = 0; i < 500; i++)
                {
                    eventBus.Post(new DummyMessage { Content = "helloworld" });
                    Thread.Sleep(10);
                }
            }).Start();

            var container = new DummyContainer();
            DummyMessage message = null;
            container.SubscriberExecuted += msg => { message = msg; };
            eventBus.Register(container);
            eventBus.Post(new DummyMessage { Content = "helloworld" });
            Assert.NotNull(message);
            Assert.AreEqual(message.Content, "helloworld");
            Assert.DoesNotThrow(() =>
            {
                for (var i = 0; i < 500; i++)
                {
                    var newContainer = new DummyContainer();
                    newContainer.SubscriberExecuted += msg =>
                    {
                        Assert.NotNull(msg);
                        Assert.AreEqual(msg.Content, "helloworld");
                    };
                    eventBus.Register(newContainer);
                    Thread.Sleep(10);
                }
            });
        }

        [Test]
        public void action_can_subscribe()
        {
            var eventBus = CreateEventBus();
            DummyMessage message = null;
            eventBus.Register<DummyMessage>(msg => { message = msg; });
            eventBus.Post(new DummyMessage { Content = "helloworld" });
            Assert.NotNull(message);
            Assert.AreEqual(message.Content, "helloworld");
        }

        [Test]
        public void action_can_subscribe_while_posting()
        {
            var eventBus = CreateEventBus();

            new Thread(() =>
            {
                for (var i = 0; i < 500; i++)
                {
                    eventBus.Post(new DummyMessage { Content = "helloworld" });
                    Thread.Sleep(10);
                }
            }).Start();

            DummyMessage message = null;
            eventBus.Register<DummyMessage>(msg => { message = msg; });
            eventBus.Post(new DummyMessage { Content = "helloworld" });
            Assert.NotNull(message);
            Assert.AreEqual(message.Content, "helloworld");
            Assert.DoesNotThrow(() =>
            {
                for (var i = 0; i < 500; i++)
                {
                    eventBus.Register<DummyMessage>(msg =>
                    {
                        Assert.NotNull(msg);
                        Assert.AreEqual(msg.Content, "helloworld");
                    });
                    Thread.Sleep(10);
                }
            });
        }

        [Test]
        public void container_can_unsubscribe()
        {
            var eventBus = CreateEventBus();
            var container = new DummyContainer();

            DummyMessage message = null;
            container.SubscriberExecuted += msg => { message = msg; };
            eventBus.Register(container);
            eventBus.Unregister(container);
            eventBus.Post(new DummyMessage { Content = "helloworld" });
            Assert.Null(message);
        }

        [Test]
        public void action_can_unsubscribe()
        {
            var eventBus = CreateEventBus();
            DummyMessage message = null;
            Action<DummyMessage> action = msg => { message = msg; };
            eventBus.Register(action);
            eventBus.Unregister(action);
            eventBus.Post(new DummyMessage { Content = "helloworld" });
            Assert.Null(message);
        }

        [Test]
        public void threadmode_is_post__post_in_same_caller_thread_if_called_from_a_non_main_thread()
        {
            var eventBus = CreateEventBus();
            DummyMessage message = null;

            int subscriberThreadId = 0;
            eventBus.Register<DummyMessage>(msg =>
            {
                subscriberThreadId = Thread.CurrentThread.ManagedThreadId;
                message = msg;
            }, ThreadMode.Post);

            using (var waitHandle = new ManualResetEvent(false))
            {
                int postThreadId = -1;

                new Thread(() =>
                {
                    postThreadId = Thread.CurrentThread.ManagedThreadId;
                    eventBus.Post(new DummyMessage { Content = "helloworld" });
                    waitHandle.Set();
                }).Start();

                waitHandle.WaitOne(5000);

                Assert.NotNull(message);
                Assert.AreEqual(message.Content, "helloworld");
                Assert.AreEqual(subscriberThreadId, postThreadId);
            }
        }

        [Test]
        public void threadmode_is_post__post_in_same_caller_thread_if_called_from_main_thread()
        {
            var eventBus = CreateEventBus();
            DummyMessage message = null;

            using (var waitHandle = new ManualResetEvent(false))
            {
                int subscriberThreadId = 0;
                eventBus.Register<DummyMessage>(msg =>
                {
                    subscriberThreadId = Thread.CurrentThread.ManagedThreadId;
                    message = msg;
                    waitHandle.Set();
                }, ThreadMode.Post);

                int postThreadId = Thread.CurrentThread.ManagedThreadId;
                eventBus.Post(new DummyMessage { Content = "helloworld" });
                waitHandle.WaitOne(5000);

                Assert.NotNull(message);
                Assert.AreEqual(message.Content, "helloworld");
                Assert.AreEqual(subscriberThreadId, postThreadId);
            }
        }

        [Test]
        public void threadmode_is_thread__post_in_same_caller_thread_if_called_from_a_non_main_thread()
        {
            var eventBus = CreateEventBus();
            DummyMessage message = null;

            int subscriberThreadId = 0;
            eventBus.Register<DummyMessage>(msg =>
            {
                subscriberThreadId = Thread.CurrentThread.ManagedThreadId;
                message = msg;
            }, ThreadMode.Thread);

            using (var waitHandle = new ManualResetEvent(false))
            {
                int postThreadId = -1;

                new Thread(() =>
                {
                    postThreadId = Thread.CurrentThread.ManagedThreadId;
                    eventBus.Post(new DummyMessage { Content = "helloworld" });
                    waitHandle.Set();
                }).Start();

                waitHandle.WaitOne(5000);

                Assert.NotNull(message);
                Assert.AreEqual(message.Content, "helloworld");
                Assert.AreEqual(subscriberThreadId, postThreadId);
            }
        }

        [Test]
        public void threadmode_is_thread__post_in_background_thread_if_called_from_main_thread()
        {
            var eventBus = CreateEventBus();
            DummyMessage message = null;

            using (var waitHandle = new ManualResetEvent(false))
            {
                int subscriberThreadId = 0;
                eventBus.Register<DummyMessage>(msg =>
                {
                    subscriberThreadId = Thread.CurrentThread.ManagedThreadId;
                    message = msg;
                    waitHandle.Set();
                }, ThreadMode.Thread);

                int postThreadId = Thread.CurrentThread.ManagedThreadId;
                eventBus.Post(new DummyMessage { Content = "helloworld" });
                waitHandle.WaitOne(5000);

                Assert.NotNull(message);
                Assert.AreEqual(message.Content, "helloworld");
                Assert.AreNotEqual(subscriberThreadId, postThreadId);
            }
        }

        [Test]
        public void threadmode_is_async__post_in_background_thread_if_called_from_a_non_main_thread()
        {
            var eventBus = CreateEventBus();
            DummyMessage message = null;

            using (var waitHandle1 = new ManualResetEvent(false))
            using (var waitHandle2 = new ManualResetEvent(false))
            {
                int subscriberThreadId = 0;
                eventBus.Register<DummyMessage>(msg =>
                {
                    subscriberThreadId = Thread.CurrentThread.ManagedThreadId;
                    message = msg;
                    waitHandle1.Set();
                }, ThreadMode.Async);

                int postThreadId = -1;

                new Thread(() =>
                {
                    postThreadId = Thread.CurrentThread.ManagedThreadId;
                    eventBus.Post(new DummyMessage { Content = "helloworld" });
                    waitHandle2.Set();
                }).Start();

                WaitHandle.WaitAll(new WaitHandle[] { waitHandle1, waitHandle2 });

                Assert.NotNull(message);
                Assert.AreEqual(message.Content, "helloworld");
                Assert.AreNotEqual(subscriberThreadId, postThreadId);
            }
        }

        [Test]
        public void threadmode_is_async__post_in_background_thread_if_called_from_main_thread()
        {
            var eventBus = CreateEventBus();
            DummyMessage message = null;

            using (var waitHandle = new ManualResetEvent(false))
            {
                int subscriberThreadId = 0;
                eventBus.Register<DummyMessage>(msg =>
                {
                    subscriberThreadId = Thread.CurrentThread.ManagedThreadId;
                    message = msg;
                    waitHandle.Set();
                }, ThreadMode.Async);

                int postThreadId = Thread.CurrentThread.ManagedThreadId;
                eventBus.Post(new DummyMessage { Content = "helloworld" });
                waitHandle.WaitOne(5000);

                Assert.NotNull(message);
                Assert.AreEqual(message.Content, "helloworld");
                Assert.AreNotEqual(subscriberThreadId, postThreadId);
            }
        }

        [Test]
        public void threadmode_is_main__post_in_main_thread_if_called_from_main_thread()
        {
            var eventBus = CreateEventBus();
            DummyMessage message = null;

            int subscriberThreadId = 0;
            eventBus.Register<DummyMessage>(msg =>
            {
                subscriberThreadId = Thread.CurrentThread.ManagedThreadId;
                message = msg;
            }, ThreadMode.Main);

            int postThreadId = Thread.CurrentThread.ManagedThreadId;
            eventBus.Post(new DummyMessage { Content = "helloworld" });

            Assert.NotNull(message);
            Assert.AreEqual(message.Content, "helloworld");
            Assert.AreEqual(subscriberThreadId, postThreadId);
        }

        [Test]
        public void threadmode_is_main__post_in_main_thread_if_called_from_background_thread()
        {
            using (var waitInitHandle = new ManualResetEvent(false))
            {
                var eventBus = CreateEventBus();
                var form = new Form();
                int postThreadId = -1;
                form.Load += (sender, args) =>
                {
                    postThreadId = Thread.CurrentThread.ManagedThreadId;
                    waitInitHandle.Set();
                };
                var uiThread = new Thread(() => Application.Run(form));
                uiThread.SetApartmentState(ApartmentState.STA);
                uiThread.Start();
                waitInitHandle.WaitOne(5000);

                DummyMessage message = null;

                int subscriberThreadId = 0;
                eventBus.Register<DummyMessage>(msg =>
                {
                    subscriberThreadId = Thread.CurrentThread.ManagedThreadId;
                    message = msg;
                }, ThreadMode.Main);

                using (var waitHandle = new ManualResetEvent(false))
                {
                    new Thread(() =>
                    {
                        eventBus.Post(new DummyMessage { Content = "helloworld" });
                        waitHandle.Set();
                    }).Start();

                    waitHandle.WaitOne(5000);

                    Assert.NotNull(message);
                    Assert.AreEqual(message.Content, "helloworld");
                    Assert.AreEqual(subscriberThreadId, postThreadId);
                }
            }
        }

        [Test]
        public void threadmode_is_mainorder__post_in_main_thread_if_called_from_background_thread()
        {
            using (var waitInitHandle = new ManualResetEvent(false))
            {
                var eventBus = CreateEventBus();
                var form = new Form();
                int postThreadId = -1;
                form.Load += (sender, args) =>
                {
                    postThreadId = Thread.CurrentThread.ManagedThreadId;
                    waitInitHandle.Set();
                };
                var uiThread = new Thread(() => Application.Run(form));
                uiThread.SetApartmentState(ApartmentState.STA);
                uiThread.Start();
                waitInitHandle.WaitOne(5000);

                DummyMessage message = null;

                using (var waitSubscriber = new ManualResetEvent(false))
                {
                    int subscriberThreadId = 0;
                    eventBus.Register<DummyMessage>(msg =>
                    {
                        subscriberThreadId = Thread.CurrentThread.ManagedThreadId;
                        message = msg;
                        waitSubscriber.Set();
                    }, ThreadMode.MainOrder);

                    using (var waitHandle = new ManualResetEvent(false))
                    {
                        new Thread(() =>
                        {
                            eventBus.Post(new DummyMessage { Content = "helloworld" });
                            waitHandle.Set();
                        }).Start();
                        waitSubscriber.WaitOne(1000);
                        waitHandle.WaitOne(5000);

                        Assert.NotNull(message);
                        Assert.AreEqual(message.Content, "helloworld");
                        Assert.AreEqual(subscriberThreadId, postThreadId);
                    }
                }
            }
        }

        [Test]
        public void subscriber_can_receive_from_stack_message_if_it_intents()
        {
            var eventBus = CreateEventBus();
            DummyMessage message = null;
            eventBus.Post(new DummyMessage { Content = "helloworld" }, true);
            eventBus.Register<DummyMessage>(msg => { message = msg; }, true);
            Assert.NotNull(message);
            Assert.AreEqual(message.Content, "helloworld");
        }

        [Test]
        public void subscriber_cannot_receive_from_stack_message_if_it_doesnt_intents()
        {
            var eventBus = CreateEventBus();
            DummyMessage message = null;
            eventBus.Post(new DummyMessage { Content = "helloworld" }, true);
            eventBus.Register<DummyMessage>(msg => { message = msg; }, false);
            Assert.Null(message);
        }

        [Test]
        public void stacked_message_will_be_removed_after_passed_to_subscriber()
        {
            var eventBus = CreateEventBus();
            DummyMessage message = null;
            eventBus.Post(new DummyMessage { Content = "helloworld" }, true);
            eventBus.Register<DummyMessage>(msg => { message = msg; }, true);
            Assert.NotNull(message);
            Assert.AreEqual(message.Content, "helloworld");

            var messages = (IList<object>)eventBus.GetType().GetField("_messageStacked", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(eventBus);
            Assert.Zero(messages.Count);
        }
    }
}