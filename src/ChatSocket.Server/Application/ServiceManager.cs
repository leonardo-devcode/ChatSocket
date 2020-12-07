using ChatSocket.Server.Exceptions;
using ChatSocket.Server.Handlers;
using ChatSocket.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;

namespace ChatSocket.Server.Application
{
    public class ServiceManager
    {

        private int _id;
        public int GetNextId => _id++;

        List<User> _users;
        List<Command> _commands;
        List<Room> _rooms;

        public ServiceManager()
        {
            _users = new List<User>();
            _commands = new List<Command>();
            _rooms = new List<Room>();
            _id = 1;

            // Cria um grupo padrão
            _rooms.Add(new Room { Name = "geral" });

            RegisterCommand<NicknameCommandHandler>();
            RegisterCommand<RoomCommandHandler>();
            RegisterCommand<MessageCommandHandler>();
        }

        private void RegisterCommand<T>() where T : MainCommandHandler, new()
        {
            foreach (var m in typeof(T).GetMethods())
            {
                var attr = (CommandAttribute)m.GetCustomAttribute(typeof(CommandAttribute));
                if (attr != null)
                {
                    _commands.Add(new Command
                    {
                        Name = attr.Name,
                        Help = attr.Help,
                        Handler = (string param, User user, List<User> users, List<Room> rooms) => {
                            var obj = new T
                            {
                                CurrentUser = user,
                                Users = users,
                                Rooms = rooms
                            };
                            if (m.GetParameters().Length == 0) return (CommandResult)m.Invoke(obj, new object[] { });
                            return (CommandResult)m.Invoke(obj, new object[] { param });
                        }
                    });
                }
            }
        }

        private CommandResult ExecCommand(string message, User user)
        {
            var nameCommand = message.Split(" ").FirstOrDefault();

            if (string.IsNullOrEmpty(message)) throw new CommandNotFoundException();

            var command = _commands.Where(c => c.Name == nameCommand).FirstOrDefault();

            if (command == null) throw new CommandNotFoundException();

            var param = message.Substring(nameCommand.Length);

            return command.Handler.Invoke(param.Trim(), user, _users, _rooms);
        }

        private void HandleCommandReturned(CommandResult result, User user)
        {
            // TODO: Se a aplicar for crescer mais, o ideal seria usar um pattern para resolver o problema dos if's

            // verifica se é para fechar o chat do usuario
            if (result.Closed)
            {
                var stream = user.Client?.GetStream();
                stream?.Close();
                _users = _users.Where(p => p.Nickname != user.Nickname).ToList();
            }

            // verifica se é para criar uma nova sala
            if (result.CreatedRoom)
            {
                var room = new Room { Name = result.RoomName };
                _rooms.Add(room);
                _users.FirstOrDefault(p => p.Nickname == user.Nickname).Room = room;
            }

            // verifica se é para trocar o usuario de sala sala
            if (result.ChangedRoom)
            {
                var room = _rooms.FirstOrDefault(p => p.Name == result.RoomName);

                if (room == null)
                {
                    user.SendMessage("Essa sala nao existe");
                    return;
                }

                _users.FirstOrDefault(p => p.Nickname == user.Nickname).Room = room;
            }
        }

        private void SendHelp(User user)
        {
            var message = new StringBuilder();
            message.Append("Esses sao os comandos que vc pode usar\n");
            foreach (var command in _commands)
            {
                message.Append($"-> {command.Name} {command.Help}\n");
            }
            user.SendMessage(message.ToString());
        }


        public User AddUser(User user)
        {
            user.Nickname = $"User-{GetNextId}";
            user.Room = _rooms.FirstOrDefault(p => p.Name == "geral");
            _users.Add(user);
            return user;
        }

        public void handleMessage(User user, string message)
        {
            var isCommandHelp = message.Equals("/ajuda");
            if (isCommandHelp)
            {
                SendHelp(user);
                return;
            }

            try
            {
                var result = ExecCommand(message, user);
                HandleCommandReturned(result, user);
            }
            catch (CommandNotFoundException)
            {
                user.SendMessage("*** Esse comando nao existe, envia /ajuda para listar os comandos disponiveis");
            }
        }

    }
}
