using ChatSocket.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChatSocket.Server.Handlers
{
    class MessageCommandHandler : MainCommandHandler
    {

        [Command("/msg", "Use esse comando para enviar mensagem. Ex: /msg Ola")]
        public CommandResult SendMessage(string message)
        {
            message = $"> {CurrentUser.Nickname} disse: " + message;

            SendMessageSocketRoom(message);

            return new CommandResult();
        }

        [Command("/msg-u", "Use esse comando para enviar mensagem a um usuario especifico. Ex: /msg-u @teste Ola")]
        public CommandResult SendMessageUser(string message)
        {

            var nickname = message.Split(" ").FirstOrDefault();

            var specificUser = Users.Where(p => $"@{p.Nickname}".Equals(nickname.Trim())).FirstOrDefault();

            if (specificUser == null)
            {
                SendMessageSocketCurrentUser("Esse apelido nao existe");
                return new CommandResult();
            }

            message = $"> {CurrentUser.Nickname} disse para {nickname.Trim()}: " + message.Substring(nickname.Length).Trim();
            SendMessageSocketRoom(message);

            return new CommandResult();
        }

        [Command("/msg-up", "Use esse comando para enviar mensagem privada a um usuario especifico. Ex: /msg-up @teste Ola")]
        public CommandResult SendPrivateMessageUser(string message)
        {

            var nickname = message.Split(" ").FirstOrDefault();

            var specificUser = Users.Where(p => $"@{p.Nickname}".Equals(nickname.Trim())).FirstOrDefault();

            if (specificUser == null)
            {
                SendMessageSocketCurrentUser("Esse apelido nao existe");
                return new CommandResult();
            }

            message = $"> {CurrentUser.Nickname} disse para {nickname.Trim()}: " + message.Substring(nickname.Length).Trim();
            SendMessageSocketUser(message, specificUser);
            return new CommandResult();
        }
    }
}
