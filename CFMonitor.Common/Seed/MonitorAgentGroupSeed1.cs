using CFMonitor.EntityReader;
using CFMonitor.Models;
using System.Drawing;

namespace CFMonitor.Seed
{
    public class MonitorAgentGroupSeed1 : IEntityReader<MonitorAgentGroup>
    {
        public IEnumerable<MonitorAgentGroup> Read()
        {
            var list = new List<MonitorAgentGroup>()
            {
                new MonitorAgentGroup()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Group 1",
                    Color = Color.Green.ToArgb().ToString(),
                    ImageSource = "monitor_item_group.png",
                },
                new MonitorAgentGroup()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Group 2",
                    Color = Color.Green.ToArgb().ToString(),
                    ImageSource = "monitor_item_group.png",
                },
            };

            return list;
        }
    }
}
