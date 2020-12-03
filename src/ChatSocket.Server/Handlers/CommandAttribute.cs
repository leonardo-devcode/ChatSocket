using System;
using System.Collections.Generic;
using System.Text;

namespace ChatSocket.Server
{
    [AttributeUsage(AttributeTargets.Method)]
    class CommandAttribute : Attribute  
    {

        public string Name { get; private set; }
        public string Help { get; private set; }

        public CommandAttribute(string name, string help)
        {
            Name = name;
            Help = help;
        }
    }
}
