<h3>SimpleBanking using .NET Framework.</h3>

- **SimpleBanking project**: .NET console application with Entity Framework (code first approach) for connection to MS SQL Database for SQLEXPRESS server
- **RemoteService project**: WCF service that connects to SimpleBanking project.
- **RemoteClient project**: .NET console application that connects to the RemoteService project.

<h5>Workflow:</h5>

- user starts the console application (workflow is the same for SimpleBanking.exe or RemoteClient.exe)
- types command
- user`s string is parsed to a comand, if invalid user is notified
- if the user is logged in from the current instance of the application and the session is not expired, the command is executed by the database service layer
- result is returned: database -> database service -> atm -> bank engine -> console screen

<h5>Configuration:</h5>

- minimal, just for everithing to run.

<h5>Security:</h5>

- user and pin are stored hashed.
Currently is out of scope by requirement, but at least the pin can be covered when typing it in the console application;
- user session uses cookie that is created by the current application instance and stored in the database.
This ensures user authentication by application instance.
Since cookie is stored internaly in the application, even if it is taken from the database it can not be used by the user since there is nowhere to be entered. At least untill the application is decompiled...

<h5>Testability:</h5>

- Full, since active using Dependency injection.

<h5>Known issues:</h5>

- the wcf remote service timeouts before the cookie expires. Needs configuration.

<h5>Commands examples:</h5>

Bank started.<br/>
Please type command.<br/>
Type [h] for list of available commands or [q] to quit.<br/>
**login pesho 111<br/>**
[Petur Petkov] logged in.<br/>
**get balance<br/>**
1000.00<br/>
**history<br/>**
History for [Petur Petkov]:<br/>
[Date 24/05/2020 06:35:51][Transfer from [Petur Petkov] to [Mihail Mihailov]][Amount 7.00]<br/>
[Date 23/05/2020 03:25:39][Transfer from [Petur Petkov] to [Bank Manager]][Amount 1.00]<br/>
[Date 23/05/2020 03:25:31][Transfer from [Bank Manager] to [Petur Petkov]][Amount 1.00]<br/>
[Date 23/05/2020 03:23:07][Transfer from [Bank Manager] to [Petur Petkov]][Amount 1.00]<br/>
[Date 23/05/2020 03:12:17][Transfer from [Bank Manager] to [Petur Petkov]][Amount 1.00]<br/>
[Date 23/05/2020 03:08:06][Transfer from [Petur Petkov] to [Bank Manager]][Amount 1.00]<br/>
[Date 23/05/2020 03:04:23][Transfer from [Petur Petkov] to [Bank Manager]][Amount 1.00]<br/>
[Date 23/05/2020 02:48:10][Transfer from [Petur Petkov] to [Bank Manager]][Amount 1.00]<br/>
[Date 23/05/2020 02:46:34][Transfer from [Bank Manager] to [Petur Petkov]][Amount 1.00]<br/>
[Date 23/05/2020 02:42:54][Transfer from [Bank Manager] to [Petur Petkov]][Amount 1.00]<br/>
[Date 23/05/2020 02:39:11][Transfer from [Bank Manager] to [Petur Petkov]][Amount 1.00]<br/>
[Date 23/05/2020 02:21:01][Transfer from [Petur Petkov] to [Bank Manager]][Amount 1.00]<br/>
[Date 23/05/2020 01:43:07][Transfer from [Bank Manager] to [Petur Petkov]][Amount 7.00]<br/>
[Date 23/05/2020 01:41:19][Transfer from [Petur Petkov] to [Bank Manager]][Amount 1.00]<br/>
[Date 23/05/2020 01:19:10][Transfer from [Petur Petkov] to [Bank Manager]][Amount 2.00]<br/>
[Date 23/05/2020 01:17:42][Transfer from [Bank Manager] to [Petur Petkov]][Amount 1.00]<br/>
[Date 23/05/2020 12:22:11][Transfer from [Petur Petkov] to [Bank Manager]][Amount 9.00]<br/>
[Date 23/05/2020 12:21:33][Transfer from [Bank Manager] to [Petur Petkov]][Amount 10.00]<br/>
[Date 23/05/2020 12:21:07][Transfer from [Petur Petkov] to [Bank Manager]][Amount 10.00]<br/>
[Date 23/05/2020 06:16:48][Transfer from [Bank Manager] to [Petur Petkov]][Amount 2.00]<br/>
[Date 23/05/2020 06:16:02][Transfer from [Bank Manager] to [Petur Petkov]][Amount 1.00]<br/>
[Date 23/05/2020 06:14:03][Transfer from [Bank Manager] to [Petur Petkov]][Amount 1.00]<br/>
[Date 23/05/2020 05:57:07][Transfer from [Petur Petkov] to [Petur Petkov]][Amount 1.00]<br/>
[Date 23/05/2020 05:57:06][Withdraw][Amount 1.00]<br/>
[Date 23/05/2020 05:57:06][Deposit][Amount 3.00]<br/>
[Date 23/05/2020 05:56:22][Transfer from [Petur Petkov] to [Petur Petkov]][Amount 1.00]<br/>
[Date 23/05/2020 05:56:22][Withdraw][Amount 1.00]<br/>
[Date 23/05/2020 05:56:22][Deposit][Amount 3.00]<br/>
[Date 23/05/2020 05:55:39][Transfer from [Bank Manager] to [Petur Petkov]][Amount 1.00]<br/>
[Date 23/05/2020 05:53:59][Transfer from [Bank Manager] to [Petur Petkov]][Amount 1.00]<br/>
[Date 23/05/2020 04:49:05][Transfer from [Bank Manager] to [Petur Petkov]][Amount 1.00]<br/>
[Date 23/05/2020 04:48:16][Transfer from [Bank Manager] to [Petur Petkov]][Amount 1.00]<br/>
[Date 23/05/2020 04:02:17][Deposit][Amount 1000.00]<br/>
**deposit 1<br/>**
Deposit successful.<br/>
**withdraw 7<br/>**
Withdrawn 7.00<br/>
**transfer 6 to misho<br/>**
Transfter successful.<br/>
**get balance<br/>**
988.00<br/>
**history<br/>**
History for [Petur Petkov]:<br/>
[Date 24/05/2020 08:17:18][Transfer from [Petur Petkov] to [Mihail Mihailov]][Amount 6.00]<br/>
[Date 24/05/2020 08:15:48][Withdraw][Amount 7.00]<br/>
[Date 24/05/2020 08:15:43][Deposit][Amount 1.00]<br/>
[Date 24/05/2020 06:35:51][Transfer from [Petur Petkov] to [Mihail Mihailov]][Amount 7.00]<br/>
[Date 23/05/2020 03:25:39][Transfer from [Petur Petkov] to [Bank Manager]][Amount 1.00]<br/>
[Date 23/05/2020 03:25:31][Transfer from [Bank Manager] to [Petur Petkov]][Amount 1.00]<br/>
[Date 23/05/2020 03:23:07][Transfer from [Bank Manager] to [Petur Petkov]][Amount 1.00]<br/>
[Date 23/05/2020 03:12:17][Transfer from [Bank Manager] to [Petur Petkov]][Amount 1.00]<br/>
[Date 23/05/2020 03:08:06][Transfer from [Petur Petkov] to [Bank Manager]][Amount 1.00]<br/>
[Date 23/05/2020 03:04:23][Transfer from [Petur Petkov] to [Bank Manager]][Amount 1.00]<br/>
[Date 23/05/2020 02:48:10][Transfer from [Petur Petkov] to [Bank Manager]][Amount 1.00]<br/>
[Date 23/05/2020 02:46:34][Transfer from [Bank Manager] to [Petur Petkov]][Amount 1.00]<br/>
[Date 23/05/2020 02:42:54][Transfer from [Bank Manager] to [Petur Petkov]][Amount 1.00]<br/>
[Date 23/05/2020 02:39:11][Transfer from [Bank Manager] to [Petur Petkov]][Amount 1.00]<br/>
[Date 23/05/2020 02:21:01][Transfer from [Petur Petkov] to [Bank Manager]][Amount 1.00]<br/>
[Date 23/05/2020 01:43:07][Transfer from [Bank Manager] to [Petur Petkov]][Amount 7.00]<br/>
[Date 23/05/2020 01:41:19][Transfer from [Petur Petkov] to [Bank Manager]][Amount 1.00]<br/>
[Date 23/05/2020 01:19:10][Transfer from [Petur Petkov] to [Bank Manager]][Amount 2.00]<br/>
[Date 23/05/2020 01:17:42][Transfer from [Bank Manager] to [Petur Petkov]][Amount 1.00]<br/>
[Date 23/05/2020 12:22:11][Transfer from [Petur Petkov] to [Bank Manager]][Amount 9.00]<br/>
[Date 23/05/2020 12:21:33][Transfer from [Bank Manager] to [Petur Petkov]][Amount 10.00]<br/>
[Date 23/05/2020 12:21:07][Transfer from [Petur Petkov] to [Bank Manager]][Amount 10.00]<br/>
[Date 23/05/2020 06:16:48][Transfer from [Bank Manager] to [Petur Petkov]][Amount 2.00]<br/>
[Date 23/05/2020 06:16:02][Transfer from [Bank Manager] to [Petur Petkov]][Amount 1.00]<br/>
[Date 23/05/2020 06:14:03][Transfer from [Bank Manager] to [Petur Petkov]][Amount 1.00]<br/>
[Date 23/05/2020 05:57:07][Transfer from [Petur Petkov] to [Petur Petkov]][Amount 1.00]<br/>
[Date 23/05/2020 05:57:06][Withdraw][Amount 1.00]<br/>
[Date 23/05/2020 05:57:06][Deposit][Amount 3.00]<br/>
[Date 23/05/2020 05:56:22][Transfer from [Petur Petkov] to [Petur Petkov]][Amount 1.00]<br/>
[Date 23/05/2020 05:56:22][Withdraw][Amount 1.00]<br/>
[Date 23/05/2020 05:56:22][Deposit][Amount 3.00]<br/>
[Date 23/05/2020 05:55:39][Transfer from [Bank Manager] to [Petur Petkov]][Amount 1.00]<br/>
[Date 23/05/2020 05:53:59][Transfer from [Bank Manager] to [Petur Petkov]][Amount 1.00]<br/>
[Date 23/05/2020 04:49:05][Transfer from [Bank Manager] to [Petur Petkov]][Amount 1.00]<br/>
[Date 23/05/2020 04:48:16][Transfer from [Bank Manager] to [Petur Petkov]][Amount 1.00]<br/>
[Date 23/05/2020 04:02:17][Deposit][Amount 1000.00]<br/>