using ChatSocket.Server.Application;
using ChatSocket.Server.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ChatSocket.Server.Test
{
    public class NicknameCommandHandlerTest
    {

        ServiceManager _serviceManager;

        public NicknameCommandHandlerTest()
        {
            _serviceManager = new ServiceManager();
        }

        [Fact]
        public void ChangeNickName_WhenHasNickname_ThenNotReceiveMessageWarning()
        {
            // Arrange
            var user = new Mock<User>();
            _serviceManager.AddUser(user.Object);

            // Act
            _serviceManager.handleMessage(user.Object, "/apelido teste");

            // Assert
            user.Verify(x => x.SendMessage(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void ChangeNickName_WhenEmpty_ThenReceiveMessageWarning()
        {
            // Arrange
            var user = new Mock<User>();
            _serviceManager.AddUser(user.Object);

            // Act
            _serviceManager.handleMessage(user.Object, "/apelido");

            // Assert
            user.Verify(x => x.SendMessage("O apelido nao pode ser vazio"), Times.Once);
        }
    }
}
