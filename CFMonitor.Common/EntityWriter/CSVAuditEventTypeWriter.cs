using CFMonitor.EntityWriter;
using CFMonitor.Models;
using CFUtilities.CSV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFMonitor.EntityWriter
{
    public class CSVAuditEventTypeWriter : IEntityWriter<AuditEventType>
    {
        private readonly CSVWriter<AuditEventType> _csvWriter = new CSVWriter<AuditEventType>();

        public CSVAuditEventTypeWriter(string file, Char delimiter, Encoding encoding)
        {
            _csvWriter.Delimiter = delimiter;
            _csvWriter.Encoding = encoding;
            _csvWriter.File = file;
        }

        public void Write(IEnumerable<AuditEventType> auditEventTypes)
        {
            _csvWriter.AddColumn<string>("Id", i => i.Id, value => value.ToString());
            _csvWriter.AddColumn<string>("Name", i => i.Name, value => value.ToString());

            _csvWriter.Write(auditEventTypes);
        }
    }
}
