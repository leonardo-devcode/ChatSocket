using ChatSocket.Server.Application;
using ChatSocket.Server.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using System.Net.Sockets;
using Xunit;

namespace ChatSocket.Server.Test
{

    public class MessageCommandHandlerTest
    {

        private ServiceManager _serviceManager;

        public MessageCommandHandlerTest()
        {
            _serviceManager = new ServiceManager();
        }

        [Fact]
        public void SendMessageUser_UserExist_ThenUseReceiveMessage()
        {
            // Arrange
            var user = new Mock<User>();
            var userReceive = new Mock<User>();
            _serviceManager.AddUser(user.Object);
            _serviceManager.AddUser(userReceive.Object);

            // Act
            _serviceManager.handleMessage(user.Object, "/msg-u @User-2 oi!");

            // Assert
            userReceive.Verify(x => x.SendMessage("> User-1 disse para @User-2: oi!"), Times.Once);
        }

        [Fact]
        public void SendMessageUser_UseNotExist_ThenReturnWarning()
        {
            // Arrange
            var user = new Mock<User>();
            _serviceManager.AddUser(user.Object);

            // Act
            _serviceManager.handleMessage(user.Object, "/msg-u @teste oi!");

            // Assert
            user.Verify(x => x.SendMessage("Esse apelido nao existe"), Times.Once);
        }

        [Fact]
        public void SendPrivateMessageUser_UserExist_ThenUseReceiveMessage()
        {
            // Arrange
            var user = new Mock<User>();
            var userReceive = new Mock<User>();
            _serviceManager.AddUser(user.Object);
            _serviceManager.AddUser(userReceive.Object);

            // Act
            _serviceManager.handleMessage(user.Object, "/msg-up @User-2 oi!");

            // Assert
            userReceive.Verify(x => x.SendMessage("> User-1 disse para @User-2: oi!"), Times.Once);
        }

        [Fact]
        public void SendPrivateMessageUser_UseNotExist_ThenReturnWarning()
        {
            // Arrange
            var user = new Mock<User>();
            _serviceManager.AddUser(user.Object);

            // Act
            _serviceManager.handleMessage(user.Object, "/msg-up @teste oi!");

            // Assert
            user.Verify(x => x.SendMessage("Esse apelido nao existe"), Times.Once);
        }

        [Fact]
        public void SendMessage_ThenUseReceiveMessage()
        {
            // Arrange
            var user = new Mock<User>();
            var userGroup = new Mock<User>();
            _serviceManager.AddUser(user.Object);
            _serviceManager.AddUser(userGroup.Object);

            // Act
            _serviceManager.handleMessage(user.Object, "/msg oi!");

            // Assert
            user.Verify(x => x.SendMessage("> User-1 disse: oi!"), Times.Never);
            userGroup.Verify(x => x.SendMessage("> User-1 disse: oi!"), Times.Once);
        }
    }
}
