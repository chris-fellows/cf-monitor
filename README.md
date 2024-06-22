# cf-monitor

A system for monitoring various items and then taking an action. E.g. Monitoring that request a particular
web page returns an HTTP 200 status and if not then sending an email or creating a log entry.

For each monitor item then there are various events that can occur when the item is checked. A list of conditions
can be assigned to the event and action(s) can be taken if all conditions are valid. In the above example then 
there would be events 'HTTP status returned' & 'HTTP status not returned'. If event 'HTTP status returned' occurs
then the condition 'Status not 200' would be evaluated and if the condition was true then an email could be sent.

Monitor Manager
---------------
UI for managing the list of items to monitor.

Monitor Agent Service
---------------------
Checks the list of items to be monitored.

Monitor Items
-------------
The following items can be monitored:
- Active process. E.g. Check that a particular process is running.
- CPU.
- DHCP.
- Disk space.
- DNS.
- File size.
- Folder size.
- LDAP.
- Local file. E.g. Check that overnight process has created a log file for current day and it contains "COMPLETED SUCCESSFULLY".
- NTP time and machine time are within tolerance.
- Memory.
- Ping.
- Process.
- Registry setting.
- REST endpoint.
- Run process. E.g. Run a particular PowerShell script and check the exit code value.
- Windows service.
- SMTP connection.
- SOAP endpoint.
- TCP/UDP socket.
- SQL. E.g. Query that checks if various tables contain issues that need to be addressed by Support Team.
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
