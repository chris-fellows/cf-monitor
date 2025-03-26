# cf-monitor

A system for monitoring various items and then taking an action. E.g. Check that your CRM web application is
running and the Login page is available.

A Monitor Agent is installed on particular machines. Monitor Agent Manager runs centrally and Monitor Agents
communicate with it.

For each monitor item then there are various events that can occur when the item is checked. A list of conditions
can be assigned to the event and action(s) can be taken if all conditions are valid. In the above example then 
there would be events 'HTTP status returned' & 'HTTP status not returned'. If event 'HTTP status returned' occurs
then the condition 'Status not 200' would be evaluated and if the condition was true then an email could be sent.

Monitor UI
----------
UI (ASP.NET Core Blazor) for managing the list of items to monitor.

Monitor Agent
-------------
Runs silently on specific machines and performs checking of items being monitored. It communicates with Monitor
Agent Manager via TCP. Monitor Agent will get all data & files (E.g. PowerShell & SQL scripts) from Agent Manager.

Monitor Agent Manager
---------------------
Runs silently on single machine and controls agents via TCP connection. It will execute any actions as a result
of items being checks. E.g. Send an email.

Monitor Items
-------------
The following items can be monitored:
- Active process. E.g. Check that process MyProcess.exe is running.
- CPU.
- DHCP.
- Disk space. E.g. Check that available disk space is more than 500 GB.
- DNS. E.g. Check that host MyServer can be resolved locally.
- File size.
- Folder size. E.g. Check that the size of folder X is less than 10GB.
- LDAP.
- Local file & content. E.g. Check that overnight process has created a log file for current day and it contains "COMPLETED SUCCESSFULLY".
- Local time within tolerance (HTTP time/NIST time/NTP time)
- Memory.
- Ping.
- REST endpoint.
- Run process. E.g. Run a particular PowerShell script and check that the exit code is 0.
- SMTP connection.
- SOAP endpoint. E.g. Check that SOAP endpoint returns specific content.
- TCP/UDP socket.
- SQL. E.g. Query that returns all anomaly records from a particular table.
- HTTP endpoint. E.g. Check that the Login page is available for a web application.

Actions
-------
The following actions can be taken if the conditions for the monitor item event is valid:
- Datadog warning.
- Email.
- Event Log entry.
- Log entry.
- Machine restart.
- Process launched.
- Service restart.
- SMS message.
- SQL statement executed.
- HTTP endpoint.

TODO List
---------
- Complete data management screens.
- (Monitor Agent) Support download of update. E.g. Add Monitor Agent Launcher which gets updated files and
  then launches Monitor Agent.