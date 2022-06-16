# Installation

* Have [Microsoft SQL Server Manage Studio](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver16) (version 18 or 19)

### Download the release project from GitHub

Which you can download [here](https://github.com/AlumniTracker/AlumniTrackerSite/tree/Release)
1. Open File Explorer, go into the C:/ drive, and if one does not already exist, make a SQL folder.  
2. Download the project from Github, place it inside the new SQL Folder  
3. Extract from Zip File (right click, then Extract All)  

### Create Database:

1. Right click Databases on the left side and click the 'Restore Database...' Option  
2. Under 'Source', change from 'Database:' to 'Device', then click the 3 dots on the right hand side.  
3. Make sure 'Backup media type' is 'File' and then click  'Add'.  
4. Locate the AlumniTracker.bak file, which is inside the project downloaded from GitHub.  
Location inside project `UnzippedFileName/AlumniTrackerSite/AlumniTracker.bak`
5. Click 'OK', then click 'OK' again on the page underneath.  

### Create Database Account:

1. Click the plus next to the 'Security' folder in the Folder Toolbar on the left  
(Inside Microsoft SQL Server Manage Studio)
2. Right Click Logins, and Press 'New Login...'
    * Make a Login Name and Password, which will be used by the program later.
    * Ensure that Authentication is on SQL Server Authentication  
    (password will not be available otherwise)
    * Turn off 'User must change password at next login', and 'Enforce password expiration'  
    * Change Default Database to AlumniTracker
3. Go into Server Roles in the top left of the menu, and Ensure the only role is 'public'
4. Go into User Mapping, find and click AlumniTracker for database
    * Add 'db_datareader' and 'db_datawriter' to roles.
    * Ensure 'db-owner' is not checked.
5. Press ok to create the login.

Done with SQL Server, you can close Microsoft SQL Server Management Studio if you wish.  

### Configuration:
1. inside the downloaded project, open the'appsettings.json' file
    * change the string to the right of `"conn" :` to have your new SQL login Username and Password
2. Open Settings.Json, which wil be here:  
`UnzippedFileName/AlumniTrackerSite/AlumniTrackerSite/Data/Settings.Json`
    * Change the Email to to the one you want the site to send emails with.  
    *This may only work with gmail accounts. To set up a gmail account:  
        * go to 'manage your google account' by clicking on the top right image.
        * click on security on the left toolbar
        * find 'Signing in to google
        * make sure 2-Step Verification is on
        * Click on App passwords
        * Under 'Select app' select other and give it a custom name
        * Select device should be Windows Computer
        * Set the password in settings.json to the newly generated password

Finished! go ahead and run the program.
`UnzippedFileName/AlumniTrackerSite/bin/Release/net6.0/AlumniTrackerSite.exe`   
The Default Admin Account is  
`UserName: CCFoundation@Centralia.eduPassword: wv7_TGhQUCsA8dt`


