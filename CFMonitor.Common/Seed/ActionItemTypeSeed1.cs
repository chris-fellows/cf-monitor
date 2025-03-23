using CFMonitor.EntityReader;
using CFMonitor.Enums;
using CFMonitor.Models;
using System.Drawing;

namespace CFMonitor.Seed
{
    public class ActionItemTypeSeed1 : IEntityReader<ActionItemType>
    {
        public IEnumerable<ActionItemType> Read()
        {
            var list = new List<ActionItemType>()
            {
                new ActionItemType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Console",
                    ActionerType = ActionerTypes.Console,
                    Color = Color.Blue.ToArgb().ToString(),
                    ImageSource = "action_item_type.png",
                },
                new ActionItemType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Datadog warning",
                    ActionerType = ActionerTypes.DatadogWarning,
                    Color = Color.Blue.ToArgb().ToString(),
                       ImageSource = "action_item_type.png",
                },
                new ActionItemType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Email",
                    ActionerType = ActionerTypes.Email,
                    Color = Color.Blue.ToArgb().ToString(),
                     ImageSource = "action_item_type.png",
                },
                new ActionItemType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Event Log",
                    ActionerType = ActionerTypes.EventLog,
                    Color = Color.Blue.ToArgb().ToString(),
                  ImageSource = "action_item_type.png",
                },
                new ActionItemType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Machine restart",
                    ActionerType = ActionerTypes.MachineRestart,
                    Color = Color.Blue.ToArgb().ToString(),
                        ImageSource = "action_item_type.png",
                },
                new ActionItemType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Process",
                    ActionerType = ActionerTypes.Process,
                    Color = Color.Blue.ToArgb().ToString(),
                        ImageSource = "action_item_type.png",
                },
                new ActionItemType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "SMS",
                    ActionerType = ActionerTypes.SMS,
                    Color = Color.Blue.ToArgb().ToString(),
                        ImageSource = "action_item_type.png",
                },
                new ActionItemType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "SQL",
                    ActionerType = ActionerTypes.SQL,
                    Color = Color.Blue.ToArgb().ToString(),
                        ImageSource = "action_item_type.png",
                },
                new ActionItemType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "URL",
                    ActionerType = ActionerTypes.URL,
                    Color = Color.Blue.ToArgb().ToString(),
                        ImageSource = "action_item_type.png",
                },
            };

            return list;
        }
    }
}
