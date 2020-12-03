using ChatSocket.Server.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatSocket.Server.Handlers
{
    class NicknameCommandHandler : MainCommandHandler
    {

        [Command("/apelido", "Use esse comando para trocar seu apelido. Ex: /apelido kamikaze")]
        public CommandResult ChangeNickName(string message)
        {

            if (string.IsNullOrEmpty(message))
            {
                SendMessageSocketCurrentUser("O apelido nao pode ser vazio");
                return new CommandResult();
            }

            CurrentUser.Nickname = message;

            return new CommandResult();
        }

    }
}
