using CFMonitor.Interfaces;
using CFMonitor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFMonitor.Services
{
    /// <summary>
    /// Web page for displaying the status of all monitor items
    /// </summary>
    public class MonitorItemsStatusWebPage : IEntityWriter<MonitorItem>
    {
        public Task WriteAllAsync(List<MonitorItem> monitorItems)
        {
            var html = new StringBuilder("<html>" +
                            "<head>" +
                            "</head>" +
                            "<body>");


            html.Append("<table>");
            html.Append("<thead>");

            html.Append("</thead>");
            html.Append("<tbody>");
            foreach (var monitorItem in monitorItems)
            {                
                html.Append("<tr>");
                html.Append($"<td>{monitorItem.Name}</td>");

                html.Append("</tr>");
            }
            html.Append("</tbody>");
            html.Append("</table>");


            html.Append("</body></html>");
            return Task.CompletedTask;
        }
    }
}
