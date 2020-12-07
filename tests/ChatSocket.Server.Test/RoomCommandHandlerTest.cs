using ChatSocket.Server.Application;
using ChatSocket.Server.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ChatSocket.Server.Test
{
    public class RoomCommandHandlerTest
    {

        ServiceManager _serviceManager;
        
        public RoomCommandHandlerTest()
        {
            _serviceManager = new ServiceManager();
        }

        [Fact]
        public void Exit_ThenUsersReceiveMessageExit()
        {
            // Arrange
            var user = new Mock<User>();
            var userGroup = new Mock<User>();
            _serviceManager.AddUser(user.Object);
            _serviceManager.AddUser(userGroup.Object);

            // Act
            _serviceManager.handleMessage(user.Object, "/sair");

            // Assert
            user.Verify(x => x.SendMessage("> User-1 saiu do chat"), Times.Never);
            userGroup.Verify(x => x.SendMessage("> User-1 saiu do chat"), Times.Once);
        }

        [Fact]
        public void AddRoom_WhenRoomNameExist_ThenReceiveMessageWarning()
        {
            // Arrange
            var user = new Mock<User>();
            _serviceManager.AddUser(user.Object);

            // Act
            _serviceManager.handleMessage(user.Object, "/criar-sala geral");

            // Assert
            user.Verify(x => x.SendMessage("Ja existe uma sala com esse nome"), Times.Once);
        }

        [Fact]
        public void AddRoom_WhenRoomNameIsEmpty_ThenReceiveMessageWarning()
        {
            // Arrange
            var user = new Mock<User>();
            _serviceManager.AddUser(user.Object);

            // Act
            _serviceManager.handleMessage(user.Object, "/criar-sala");

            // Assert
            user.Verify(x => x.SendMessage("O nome da sala nao pode ser vazio"), Times.Once);
        }

        [Fact]
        public void AddRoom_ThenRoomIsAddedList()
        {
            // Arrange
            var user = new Mock<User>();
            _serviceManager.AddUser(user.Object);

            // Act
            _serviceManager.handleMessage(user.Object, "/criar-sala teste");

            // Assert
            _serviceManager.handleMessage(user.Object, "/salas");
            user.Verify(x => x.SendMessage(It.Is<string>(p => p.Contains("teste"))), Times.Once);
        }

        [Fact]
        public void ChangeRoom_WhenRoomNameIsEmpty_ThenReceiveMessageWarning()
        {
            // Arrange
            var user = new Mock<User>();
            _serviceManager.AddUser(user.Object);

            // Act
            _serviceManager.handleMessage(user.Object, "/mudar-sala");

            // Assert
            user.Verify(x => x.SendMessage("O nome da sala nao pode ser vazio"), Times.Once);
        }

        [Fact]
        public void ChangeRoom_WhenRoomNameNotExist_ThenReceiveMessageWarning()
        {
            // Arrange
            var user = new Mock<User>();
            _serviceManager.AddUser(user.Object);

            // Act
            _serviceManager.handleMessage(user.Object, "/mudar-sala teste");

            // Assert
            user.Verify(x => x.SendMessage("Essa sala nao existe"), Times.Once);
        }

        [Fact]
        public void ChangeRoom_WhenChangeUserRoom_ThenAnotherRoonNotReceiveMessage()
        {
            // Arrange
            var user = new Mock<User>();
            var userAnotherGroup = new Mock<User>();
            _serviceManager.AddUser(user.Object);
            _serviceManager.AddUser(userAnotherGroup.Object);

            // Act
            _serviceManager.handleMessage(user.Object, "/criar-sala teste");
            _serviceManager.handleMessage(user.Object, "/msg oi");

            // Assert
            userAnotherGroup.Verify(x => x.SendMessage("> User-1 disse: oi"), Times.Never);
        }



    }
}
