using System;

using DownloadR.Impl.UnitTestProject.Fakes;
using Moq;

using NUnit.Framework;

namespace DownloadR.Impl.UnitTestProject.Common.Utils {
    public class ObservableTests {
        [Test]
        public void Verify_SendMessagesToObserverWorks() {
            Mock<IObserver<object>> observer = new Mock<IObserver<object>>();

            var observable = new ObservableStub();

            using var unsubscriber = observable.Subscribe(observer.Object);
            
            observable.SendCompletedMessageToObservers();
            observable.SendDataMessageToObservers(new object());
            observable.SendErrorMessageToObservers(new Exception("Test"));

            observer.Verify(x => x.OnCompleted(), Times.Once);
            observer.Verify(x => x.OnError(It.IsAny<Exception>()), Times.Once);
            observer.Verify(x => x.OnNext(It.IsAny<object>()), Times.Once);
        }

        [Test]
        public void Verify_MessagesAreNotHandled_AfterUnsubscriber_dispoe() {
            Mock<IObserver<object>> observer = new Mock<IObserver<object>>();

            var observable = new ObservableStub();

            var unsubscriber = observable.Subscribe(observer.Object);
            
            observable.SendCompletedMessageToObservers();
            observable.SendDataMessageToObservers(new object());
            observable.SendErrorMessageToObservers(new Exception("Test"));

            unsubscriber.Dispose();

            observable.SendCompletedMessageToObservers();
            observable.SendDataMessageToObservers(new object());
            observable.SendErrorMessageToObservers(new Exception("Test"));


            observer.Verify(x => x.OnCompleted(), Times.Once);
            observer.Verify(x => x.OnError(It.IsAny<Exception>()), Times.Once);
            observer.Verify(x => x.OnNext(It.IsAny<object>()), Times.Once);
        }
    }

    
}
