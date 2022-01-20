using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Moq;

using NUnit.Framework;

using ZippSafe.EcoMode;
using ZippSafe.EcoMode.Listeners;

namespace ZippSafe.Test.EcoMode
{
    [TestOf(typeof(LockerSystemNotifier))]
    public class Tests
    {
        private Mock<ILockerSystemManager> decoratedMock;
        private Mock<ILogger> loggerMock;

        private LockerSystemNotifier subject;

        private Mock<IBuildingManagementService> buildingManagementServiceMock;
        private Mock<IDatabaseService> databaseServiceMock;
        private Mock<IEmailService> emailServiceMock;

        [SetUp]
        public void Setup()
        {
            decoratedMock = new Mock<ILockerSystemManager>(MockBehavior.Strict);
            loggerMock = new Mock<ILogger>(MockBehavior.Strict);

            subject = new LockerSystemNotifier(decoratedMock.Object, loggerMock.Object);

            buildingManagementServiceMock = new Mock<IBuildingManagementService>(MockBehavior.Strict);
            databaseServiceMock = new Mock<IDatabaseService>(MockBehavior.Strict);
            emailServiceMock = new Mock<IEmailService>(MockBehavior.Strict);
        }

        [Test]
        public async Task SwitchEcoModeOff_ReactivatedCalled()
        {
            // Arrange
            var result = CreateResult(on: false,
                "16d378f1-1535-4fc1-8a9e-9e5fae31b12a");

            decoratedMock.Setup(mock => mock.SwitchEcoOff()).Returns(Task.FromResult(result));
            loggerMock.Setup(mock => mock.Info(It.IsAny<string>()));

            buildingManagementServiceMock.Setup(mock => mock.ManagerLockerStateChanges(result)).Returns(Task.CompletedTask);
            databaseServiceMock.Setup(mock => mock.SaveLockerStates(result)).Returns(Task.CompletedTask);

            // Act
            subject.Register(buildingManagementServiceMock.Object, (service, result) => service.ManagerLockerStateChanges(result));
            subject.Register(databaseServiceMock.Object, (service, result) => service.SaveLockerStates(result));

            subject.Deactivate<IDatabaseService>();

            subject.Activate<IDatabaseService>();

            await subject.SwitchEcoOff();

            // Assert
            buildingManagementServiceMock.Verify(mock => mock.ManagerLockerStateChanges(result), Times.Once);
            databaseServiceMock.Verify(mock => mock.SaveLockerStates(result), Times.Once);

            loggerMock.Verify(mock => mock.Info($"Notifying listener {nameof(IBuildingManagementService)}"), Times.Once);
            loggerMock.Verify(mock => mock.Info($"Notifying listener {nameof(IDatabaseService)}"), Times.Once);
            loggerMock.Verify(mock => mock.Info(It.IsAny<string>()), Times.Exactly(2));
        }

        [Test]
        public async Task SwitchEcoModeOn_ReactivatedCalled()
        {
            // Arrange
            var result = CreateResult(on: true,
                "d27a0ef3-f22e-49ef-8f12-a66d844443cd");

            decoratedMock.Setup(mock => mock.SwitchEcoOn()).Returns(Task.FromResult(result));
            loggerMock.Setup(mock => mock.Info(It.IsAny<string>()));

            buildingManagementServiceMock.Setup(mock => mock.ManagerLockerStateChanges(result)).Returns(Task.CompletedTask);
            databaseServiceMock.Setup(mock => mock.SaveLockerStates(result)).Returns(Task.CompletedTask);

            // Act
            subject.Register(buildingManagementServiceMock.Object, (service, result) => service.ManagerLockerStateChanges(result));
            subject.Register(databaseServiceMock.Object, (service, result) => service.SaveLockerStates(result));

            subject.Deactivate<IDatabaseService>();

            subject.Activate<IDatabaseService>();

            await subject.SwitchEcoOn();

            // Assert
            buildingManagementServiceMock.Verify(mock => mock.ManagerLockerStateChanges(result), Times.Once);
            databaseServiceMock.Verify(mock => mock.SaveLockerStates(result), Times.Once);

            loggerMock.Verify(mock => mock.Info($"Notifying listener {nameof(IBuildingManagementService)}"), Times.Once);
            loggerMock.Verify(mock => mock.Info($"Notifying listener {nameof(IDatabaseService)}"), Times.Once);
            loggerMock.Verify(mock => mock.Info(It.IsAny<string>()), Times.Exactly(2));
        }

        [Test]
        public async Task SwitchEcoModeOff_DeactivatedNotCalled()
        {
            // Arrange
            var result = CreateResult(on: false,
                "47e9529a-62c1-400c-859c-3371ed8abceb");

            decoratedMock.Setup(mock => mock.SwitchEcoOff()).Returns(Task.FromResult(result));
            loggerMock.Setup(mock => mock.Info(It.IsAny<string>()));

            buildingManagementServiceMock.Setup(mock => mock.ManagerLockerStateChanges(result)).Returns(Task.CompletedTask);
            databaseServiceMock.Setup(mock => mock.SaveLockerStates(result)).Returns(Task.CompletedTask);

            // Act
            subject.Register(buildingManagementServiceMock.Object, (service, result) => service.ManagerLockerStateChanges(result));
            subject.Register(databaseServiceMock.Object, (service, result) => service.SaveLockerStates(result));

            subject.Deactivate<IDatabaseService>();

            await subject.SwitchEcoOff();

            // Assert
            buildingManagementServiceMock.Verify(mock => mock.ManagerLockerStateChanges(result), Times.Once);
            databaseServiceMock.Verify(mock => mock.SaveLockerStates(result), Times.Never);

            loggerMock.Verify(mock => mock.Info($"Notifying listener {nameof(IBuildingManagementService)}"), Times.Once);
            loggerMock.Verify(mock => mock.Info(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task SwitchEcoModeOn_DeactivatedNotCalled()
        {
            // Arrange
            var result = CreateResult(on: true,
                "8587cd96-5656-4ddf-ae24-2bc5fd52ef57");

            decoratedMock.Setup(mock => mock.SwitchEcoOn()).Returns(Task.FromResult(result));
            loggerMock.Setup(mock => mock.Info(It.IsAny<string>()));

            buildingManagementServiceMock.Setup(mock => mock.ManagerLockerStateChanges(result)).Returns(Task.CompletedTask);
            databaseServiceMock.Setup(mock => mock.SaveLockerStates(result)).Returns(Task.CompletedTask);

            // Act
            subject.Register(buildingManagementServiceMock.Object, (service, result) => service.ManagerLockerStateChanges(result));
            subject.Register(databaseServiceMock.Object, (service, result) => service.SaveLockerStates(result));

            subject.Deactivate<IDatabaseService>();

            await subject.SwitchEcoOn();

            // Assert
            buildingManagementServiceMock.Verify(mock => mock.ManagerLockerStateChanges(result), Times.Once);
            databaseServiceMock.Verify(mock => mock.SaveLockerStates(result), Times.Never);

            loggerMock.Verify(mock => mock.Info($"Notifying listener {nameof(IBuildingManagementService)}"), Times.Once);
            loggerMock.Verify(mock => mock.Info(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task SwitchEcoModeOff_DeregisteredNotCalled()
        {
            // Arrange
            var result = CreateResult(on: false,
                "0d412355-6883-452b-ae39-e633567f01bb");

            decoratedMock.Setup(mock => mock.SwitchEcoOff()).Returns(Task.FromResult(result));
            loggerMock.Setup(mock => mock.Info(It.IsAny<string>()));

            buildingManagementServiceMock.Setup(mock => mock.ManagerLockerStateChanges(result)).Returns(Task.CompletedTask);
            databaseServiceMock.Setup(mock => mock.SaveLockerStates(result)).Returns(Task.CompletedTask);

            // Act
            subject.Register(buildingManagementServiceMock.Object, (service, result) => service.ManagerLockerStateChanges(result));
            subject.Register(databaseServiceMock.Object, (service, result) => service.SaveLockerStates(result));

            subject.Deregister<IDatabaseService>();

            await subject.SwitchEcoOff();

            // Assert
            buildingManagementServiceMock.Verify(mock => mock.ManagerLockerStateChanges(result), Times.Once);
            databaseServiceMock.Verify(mock => mock.SaveLockerStates(result), Times.Never);

            loggerMock.Verify(mock => mock.Info($"Notifying listener {nameof(IBuildingManagementService)}"), Times.Once);
            loggerMock.Verify(mock => mock.Info(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task SwitchEcoModeOn_DeregisteredNotCalled()
        {
            // Arrange
            var result = CreateResult(on: true,
                "e46c8d73-4ca5-4989-a4b7-bf48899786c3");

            decoratedMock.Setup(mock => mock.SwitchEcoOn()).Returns(Task.FromResult(result));
            loggerMock.Setup(mock => mock.Info(It.IsAny<string>()));

            buildingManagementServiceMock.Setup(mock => mock.ManagerLockerStateChanges(result)).Returns(Task.CompletedTask);
            databaseServiceMock.Setup(mock => mock.SaveLockerStates(result)).Returns(Task.CompletedTask);

            // Act
            subject.Register(buildingManagementServiceMock.Object, (service, result) => service.ManagerLockerStateChanges(result));
            subject.Register(databaseServiceMock.Object, (service, result) => service.SaveLockerStates(result));

            subject.Deregister<IDatabaseService>();

            await subject.SwitchEcoOn();

            // Assert
            buildingManagementServiceMock.Verify(mock => mock.ManagerLockerStateChanges(result), Times.Once);
            databaseServiceMock.Verify(mock => mock.SaveLockerStates(result), Times.Never);

            loggerMock.Verify(mock => mock.Info($"Notifying listener {nameof(IBuildingManagementService)}"), Times.Once);
            loggerMock.Verify(mock => mock.Info(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task SwitchEcoModeOff_AllSubscribersCalled()
        {
            // Arrange
            var result = CreateResult(on: false, 
                "4da5877f-4b0b-45f8-a271-3ff6afe57bc5",
                "138db831-5f6f-4dc6-8cc4-b53d9e47f3a8");

            decoratedMock.Setup(mock => mock.SwitchEcoOff()).Returns(Task.FromResult(result));
            loggerMock.Setup(mock => mock.Info(It.IsAny<string>()));

            buildingManagementServiceMock.Setup(mock => mock.ManagerLockerStateChanges(result)).Returns(Task.CompletedTask);
            databaseServiceMock.Setup(mock => mock.SaveLockerStates(result)).Returns(Task.CompletedTask);
            emailServiceMock.Setup(mock => mock.SendEmail(result)).Returns(Task.CompletedTask);

            // Act
            subject.Register(buildingManagementServiceMock.Object, (service, result) => service.ManagerLockerStateChanges(result));
            subject.Register(databaseServiceMock.Object, (service, result) => service.SaveLockerStates(result));
            subject.Register(emailServiceMock.Object, (service, result) => service.SendEmail(result));

            await subject.SwitchEcoOff();

            // Assert
            buildingManagementServiceMock.Verify(mock => mock.ManagerLockerStateChanges(result), Times.Once);
            databaseServiceMock.Verify(mock => mock.SaveLockerStates(result), Times.Once);
            emailServiceMock.Verify(mock => mock.SendEmail(result), Times.Once);

            loggerMock.Verify(mock => mock.Info($"Notifying listener {nameof(IBuildingManagementService)}"), Times.Once);
            loggerMock.Verify(mock => mock.Info($"Notifying listener {nameof(IDatabaseService)}"), Times.Once);
            loggerMock.Verify(mock => mock.Info($"Notifying listener {nameof(IEmailService)}"), Times.Once);
            loggerMock.Verify(mock => mock.Info(It.IsAny<string>()), Times.Exactly(3));
        }

        [Test]
        public async Task SwitchEcoModeOn_AllSubscribersCalled()
        {
            // Arrange
            var result = CreateResult(on: true, 
                "3a923f73-7935-486d-a82b-b15a1e38d393",
                "39c776d2-daa1-45e4-aeb4-68cbbd146098");

            decoratedMock.Setup(mock => mock.SwitchEcoOn()).Returns(Task.FromResult(result));
            loggerMock.Setup(mock => mock.Info(It.IsAny<string>()));

            buildingManagementServiceMock.Setup(mock => mock.ManagerLockerStateChanges(result)).Returns(Task.CompletedTask);
            databaseServiceMock.Setup(mock => mock.SaveLockerStates(result)).Returns(Task.CompletedTask);
            emailServiceMock.Setup(mock => mock.SendEmail(result)).Returns(Task.CompletedTask);

            // Act
            subject.Register(buildingManagementServiceMock.Object, (service, result) => service.ManagerLockerStateChanges(result));
            subject.Register(databaseServiceMock.Object, (service, result) => service.SaveLockerStates(result));
            subject.Register(emailServiceMock.Object, (service, result) => service.SendEmail(result));

            await subject.SwitchEcoOn();

            // Assert
            buildingManagementServiceMock.Verify(mock => mock.ManagerLockerStateChanges(result), Times.Once);
            databaseServiceMock.Verify(mock => mock.SaveLockerStates(result), Times.Once);
            emailServiceMock.Verify(mock => mock.SendEmail(result), Times.Once);

            loggerMock.Verify(mock => mock.Info($"Notifying listener {nameof(IBuildingManagementService)}"), Times.Once);
            loggerMock.Verify(mock => mock.Info($"Notifying listener {nameof(IDatabaseService)}"), Times.Once);
            loggerMock.Verify(mock => mock.Info($"Notifying listener {nameof(IEmailService)}"), Times.Once);
            loggerMock.Verify(mock => mock.Info(It.IsAny<string>()), Times.Exactly(3));
        }

        [Test]
        public async Task SwitchEcoModeOn_Exception_AllSubscribersCalled()
        {
            // Arrange
            var result = CreateResult(on: true,
                "3a923f73-7935-486d-a82b-b15a1e38d393",
                "39c776d2-daa1-45e4-aeb4-68cbbd146098");

            var exception = new Exception("This was expected");

            decoratedMock.Setup(mock => mock.SwitchEcoOn()).Returns(Task.FromResult(result));
            loggerMock.Setup(mock => mock.Info(It.IsAny<string>()));
            loggerMock.Setup(mock => mock.Error($"Error during notification of listener {nameof(IDatabaseService)}", exception));

            buildingManagementServiceMock.Setup(mock => mock.ManagerLockerStateChanges(result)).Returns(Task.CompletedTask);
            databaseServiceMock.Setup(mock => mock.SaveLockerStates(result)).Returns(Task.FromException(exception));
            emailServiceMock.Setup(mock => mock.SendEmail(result)).Returns(Task.CompletedTask);
            
            // Act
            subject.Register(buildingManagementServiceMock.Object, (service, result) => service.ManagerLockerStateChanges(result));
            subject.Register(databaseServiceMock.Object, (service, result) => service.SaveLockerStates(result));
            subject.Register(emailServiceMock.Object, (service, result) => service.SendEmail(result));

            await subject.SwitchEcoOn();

            // Assert
            buildingManagementServiceMock.Verify(mock => mock.ManagerLockerStateChanges(result), Times.Once);
            databaseServiceMock.Verify(mock => mock.SaveLockerStates(result), Times.Once);
            emailServiceMock.Verify(mock => mock.SendEmail(result), Times.Once);

            loggerMock.Verify(mock => mock.Info($"Notifying listener {nameof(IBuildingManagementService)}"), Times.Once);
            loggerMock.Verify(mock => mock.Info($"Notifying listener {nameof(IDatabaseService)}"), Times.Once);
            loggerMock.Verify(mock => mock.Info($"Notifying listener {nameof(IEmailService)}"), Times.Once);
            loggerMock.Verify(mock => mock.Info(It.IsAny<string>()), Times.Exactly(3));

            loggerMock.Verify(mock => mock.Error($"Error during notification of listener {nameof(IDatabaseService)}", exception), Times.Once);
        }

        [Test]
        public async Task SwitchEcoModeOff_Exception_AllSubscribersCalled()
        {
            // Arrange
            var result = CreateResult(on: false,
                "3a923f73-7935-486d-a82b-b15a1e38d393",
                "39c776d2-daa1-45e4-aeb4-68cbbd146098");

            var exception = new Exception("This was expected");

            decoratedMock.Setup(mock => mock.SwitchEcoOff()).Returns(Task.FromResult(result));
            loggerMock.Setup(mock => mock.Info(It.IsAny<string>()));
            loggerMock.Setup(mock => mock.Error($"Error during notification of listener {nameof(IDatabaseService)}", exception));

            buildingManagementServiceMock.Setup(mock => mock.ManagerLockerStateChanges(result)).Returns(Task.CompletedTask);
            databaseServiceMock.Setup(mock => mock.SaveLockerStates(result)).Returns(Task.FromException(exception));
            emailServiceMock.Setup(mock => mock.SendEmail(result)).Returns(Task.CompletedTask);

            // Act
            subject.Register(buildingManagementServiceMock.Object, (service, result) => service.ManagerLockerStateChanges(result));
            subject.Register(databaseServiceMock.Object, (service, result) => service.SaveLockerStates(result));
            subject.Register(emailServiceMock.Object, (service, result) => service.SendEmail(result));

            await subject.SwitchEcoOff();

            // Assert
            buildingManagementServiceMock.Verify(mock => mock.ManagerLockerStateChanges(result), Times.Once);
            databaseServiceMock.Verify(mock => mock.SaveLockerStates(result), Times.Once);
            emailServiceMock.Verify(mock => mock.SendEmail(result), Times.Once);

            loggerMock.Verify(mock => mock.Info($"Notifying listener {nameof(IBuildingManagementService)}"), Times.Once);
            loggerMock.Verify(mock => mock.Info($"Notifying listener {nameof(IDatabaseService)}"), Times.Once);
            loggerMock.Verify(mock => mock.Info($"Notifying listener {nameof(IEmailService)}"), Times.Once);
            loggerMock.Verify(mock => mock.Info(It.IsAny<string>()), Times.Exactly(3));

            loggerMock.Verify(mock => mock.Error($"Error during notification of listener {nameof(IDatabaseService)}", exception), Times.Once);
        }

        private static IEnumerable<LockerState> CreateResult(bool on, params string[] lockerIds)
        {
            return lockerIds.Select(lockerId => new LockerState
            {
                LockerId = Guid.Parse(lockerId),
                RunsInEco = on,
            });
        }
    }
}