using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Services;
using Baseline.Reflection;

namespace Apollo.Commands.Meta
{
    public class DescribeCommand : AuthenticatedCommand
    {
        private readonly ICommandLocator commandLocator;
        public string command { get; set; }

        public DescribeCommand(ICommandLocator commandLocator, ILoginService loginService) : base(loginService)
        {
            this.commandLocator = commandLocator;
        }

        public override async Task<CommandResult> Execute()
        {
            await Task.CompletedTask;
            var cmd = commandLocator.Locate(command);
            if(cmd==null)
                throw new ArgumentException("Command not found", nameof(command));

            var cmdType = cmd.GetType();

            var typeDescription = GetFullPropertyDescriptions(cmdType, command);

            return CommandResult.CreateSuccessResult(typeDescription);
        }


        public PropertyTreeNode GetFullPropertyDescriptions(Type type, string name)
        {
            var node = new PropertyTreeNode {type = type.Name, name = name};
            var properties = type.GetProperties();
            if (IsTerminalType(type))
            {
                node.members = new List<PropertyTreeNode>();
            }
            else if (typeof(IEnumerable).IsAssignableFrom(type))
            {
                if (type.IsConstructedGenericType && type.GetGenericTypeDefinition()
                    == typeof(List<>))
                {
                    var itemType = type.GetGenericArguments()[0]; // use this...
                    node.type = itemType.Name;
                    if (!IsTerminalType(itemType))
                    {
                        node = GetFullPropertyDescriptions(itemType, name);
                        node.array = true;
                    }
                }
                else
                {
                    node.array = true;
                    node.members = new List<PropertyTreeNode>();
                }
            }
            else
            {
                node.members = properties
                        .Where(p => p.GetAttribute<ServerOnlyAttribute>() == null)
                    .Select(p => GetFullPropertyDescriptions(p.PropertyType, p.Name)).ToList();
            }

            return node;
        }

        public bool IsTerminalType(Type type)
        {
            var terminalTypes = new[]
            {
                typeof(string),
                typeof(int),
                typeof(DateTime),
                typeof(TimeSpan),
                typeof(char),
                typeof(decimal),
                typeof(float),
                typeof(DateTime?)
            };

            return terminalTypes.Contains(type);
        }


        public override Task<bool> IsValid()
        {
            return Task.FromResult(!string.IsNullOrWhiteSpace(command));
        }

        public override object ExamplePayload()
        {
            return new { command };
        }
    }

    public class PropertyTreeNode
    {
        public string type { get; set; }
        public string name { get; set; }
        public IReadOnlyList<PropertyTreeNode> members { get; set; }
        public bool array { get; set; }
    }
}
