using ChatSocket.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChatSocket.Server.Handlers
{
    class RoomCommandHandler : MainCommandHandler
    {

        [Command("/sair", "Use esse comando para sair do chat. Ex: /sair")]
        public CommandResult Exit(string message)
        {
            message = $"> {CurrentUser.Nickname} saiu do chat";
            SendMessageSocketRoom(message);
            return new CommandResult
            {
                Closed = true
            };
        }

        [Command("/criar-sala", "Use esse comando para criar uma sala. Ex: /criar-sala Legais")]
        public CommandResult AddRoom(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                SendMessageSocketCurrentUser("O nome da sala nao pode ser vazio");
                return new CommandResult();
            }

            if (Rooms.Any(p => p.Name.Equals(message)))
            {
                SendMessageSocketCurrentUser("Ja existe uma sala com esse nome");
                return new CommandResult();
            }

            return new CommandResult
            {
                CreatedRoom = true,
                RoomName = message
            };
        }

        [Command("/mudar-sala", "Use esse comando para criar uma sala. Ex: /mudar-sala Legais")]
        public CommandResult ChangeRoom(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                SendMessageSocketCurrentUser("O nome da sala nao pode ser vazio");
                return new CommandResult();
            }

            return new CommandResult
            {
                ChangedRoom = true,
                RoomName = message
            };
        }

        [Command("/salas", "Use esse comando para listar as salas disponiveis. Ex: /salas Legais")]
        public CommandResult FindRooms(string message)
        {

            var messageHelp = new StringBuilder();
            messageHelp.Append("Essas sao as salas disponiveis\n");
            foreach (var room in Rooms)
            {
                var usersTotal = Users.Count(p => p.Room.Name.Equals(room.Name));
                messageHelp.Append($"{room.Name} - possui {usersTotal} usuarios\n");
            }
            SendMessageSocketCurrentUser(messageHelp.ToString());

            return new CommandResult();
        }
    }
}
