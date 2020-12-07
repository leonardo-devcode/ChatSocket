using ChatSocket.Server.Application;
using ChatSocket.Server.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ChatSocket.Server.Test
{
    public class HelpCommandHandlerTest
    {

        ServiceManager _serviceManager;

        public HelpCommandHandlerTest()
        {
            _serviceManager = new ServiceManager();
        }

        [Fact]
        public void SendCommandHelper_ThenReturnListOfCommands()
        {
            // Arrange
            var user = new Mock<User>();
            _serviceManager.AddUser(user.Object);

            // Act
            _serviceManager.handleMessage(user.Object, "/ajuda");

            // Assert
            user.Verify(x => x.SendMessage(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void SendCommandNotExist_ThenReturnCommandHelper()
        {
            // Arrange
            var user = new Mock<User>();
            _serviceManager.AddUser(user.Object);

            // Act
            _serviceManager.handleMessage(user.Object, "/teste");

            // Assert
            user.Verify(x => x.SendMessage("*** Esse comando nao existe, envia /ajuda para listar os comandos disponiveis"), Times.Once);
        }
    }
}
