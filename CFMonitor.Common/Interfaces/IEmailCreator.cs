using CFMonitor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFMonitor.Interfaces
{
    public interface IEmailCreator
    {
        string Name { get; }

        List<string> GetRecipientEmails(List<SystemValue> systemValues);

        /// <summary>
        /// Returns email subject
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        string GetSubject(List<SystemValue> systemValues);

        /// <summary>
        /// Returns email body
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        string GetBody(List<SystemValue> systemValues);
    }
}
