using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models.ActionItems;
using CFMonitor.Models.MonitorItems;
using CFUtilities.Databases;
using System.Threading.Tasks;

namespace CFMonitor.Actioners
{
    /// <summary>
    /// Actions running SQL
    /// </summary>
    public class ActionerSQL : IActioner
    {
        public string Name => "Execute SQL";

        public ActionerTypes ActionerType => ActionerTypes.SQL;

        public Task ExecuteAsync(MonitorItem monitorItem, ActionItem actionItem, ActionParameters actionParameters)
        {
            var connectionStringParam = actionItem.Parameters.First(p => p.SystemValueType == SystemValueTypes.AIP_SQLConnectionString);
            var sqlParam = actionItem.Parameters.First(p => p.SystemValueType == SystemValueTypes.AIP_SQLSQL);

            //ActionSQL actionSQL = (ActionSQL)actionItem;
            OleDbDatabase database = new OleDbDatabase(connectionStringParam.Value);
            database.Open();            
            database.ExecuteNonQuery(System.Data.CommandType.Text, sqlParam.Value, null);
            database.Close();

            return Task.CompletedTask;
        }

        public bool CanExecute(ActionItem actionItem)
        {
            return actionItem.ActionerType == ActionerTypes.SQL;
        }
    }
}
