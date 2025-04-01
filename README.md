# cf-monitor

A system for monitoring various items and then taking an action. E.g. Check that your CRM web application is
running and the Login page is available. E.g. Run a SQL query that checks if a particular table contains any
anomaly records.

A Monitor Agent is installed on particular machines and performs the checking. Monitor Agent Manager runs 
centrally and Monitor Agents communicate with it. Monitor Agent Manager sends Monitor Agent the list of items
to check.

For each monitor item then there are various events that can occur when the item is checked. A list of conditions
can be assigned to the event and action(s) can be taken if all conditions are valid. For example, If the HTTP
check for the CRM Login page returns a non 200 status then an email can be sent.

Monitor UI
----------
UI (ASP.NET Core Blazor) for managing the list of items to monitor.

Monitor Agent
-------------
Runs silently on specific machines and performs checking of items being monitored. It communicates with Monitor
Agent Manager via TCP. Monitor Agent will get all data & files (E.g. PowerShell & SQL scripts) from Agent Manager.

Each Monitor Agent regularly sends a heartbeat to indicate that it is healthy.

Monitor Agent Manager
---------------------
Runs silently on single machine and controls agents via TCP connection. It will execute any actions as a result
of the item being checked. E.g. Send an email.

Each Monitor Agent Manager regularly sends a heartbeat to indicate that it is healthy.

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
- Ping. E.g. Ping an external server to check that there is external connectivity.
- REST endpoint. E.g. Check that a particular REST endpoint is available.
- Run process. E.g. Run a particular PowerShell script and check that the exit code is 0.
- SMTP connection.
- SOAP endpoint. E.g. Check that SOAP endpoint returns specific content.
- TCP/UDP socket. E.g. Try to connect to specific IP & port.
- SQL. E.g. Check if SQL query returns any anomaly records from table X.
- HTTP endpoint. E.g. Check that the Login page is available for a web application.

Actions
-------
The following actions can be taken if the conditions for the monitor item event is valid:
- Datadog warning.
- Email.
- Event Log entry.
- Log entry.
- Machine restart.
- Process launched. E.g. Run an .EXE or PowerShell script.
- Service restart.
- SMS message.
- SQL statement executed.
- HTTP endpoint.

TODO List
---------
- Complete data management screens.
- Change DB to EF.
- (Monitor Agent) Support download of update. E.g. Add Monitor Agent Launcher which gets updated files and
  then launches Monitor Agent.
- (Monitor Agent) Make installation as simple as possible.