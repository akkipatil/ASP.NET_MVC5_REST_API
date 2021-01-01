This project exposes REST API using ASP.NET Web API to perform CRUD operations on SQL Server.
We Consume this API using MVC 5 application and perform Db operation through actions.
We are also writing Operation Log in a file "ProjectPath\MyLogs" and DB table name "OperationLog" 

	1. Database Name: UserManagement
	2. Table Names:	 
		1. AdminUsers 
		2. Users
		3. OperationLog
			

1. Please change "connectionString" in Web.config

2. You can restore data through Migrations.
	Type this Command to restore Migration in 'Package Manager Console': Update-Database -Verbose

3. If Restore fails due to some reason follow the steps below.
		1. Delete all migration from the "Migrations" folder.
		2. In "Package Manager Console"
		
			1. Add-Migration 'InitialTableCreation'
			2. Update-Database -Verbose
			3. Add-Migration 'AddDataInTables'
			4. Add the following command in "public override void Up(){} of the created transaction"
			
					Sql("Insert into AdminUsers(AdminUserName,AdminPassword) Values('Akshay', '" + System.Web.Helpers.Crypto.HashPassword("admin@123") + "');");
					Sql("Insert into AdminUsers(AdminUserName,AdminPassword) Values('Neeraj', '" + System.Web.Helpers.Crypto.HashPassword("admin@123") + "');");
					Sql("Insert into AdminUsers(AdminUserName,AdminPassword) Values('Jolly', '" + System.Web.Helpers.Crypto.HashPassword("admin@123") + "');");


					Sql("Insert into Users(First_Name,Last_Name,Email_Address,Password,StatusId, AdminUserId)" +
						"Values('Some UserName 1','Some Surname 1','user1@123','" + System.Web.Helpers.Crypto.HashPassword("admin@123") + "',1,1)");
					Sql("Insert into Users(First_Name,Last_Name,Email_Address,Password,StatusId, AdminUserId)" +
						"Values('Some UserName 2','Some Surname 2','user2@123','" + System.Web.Helpers.Crypto.HashPassword("admin@123") + "',4,1)");
					Sql("Insert into Users(First_Name,Last_Name,Email_Address,Password,StatusId, AdminUserId)" +
						"Values('Some UserName 3','Some Surname 3','user3@123','" + System.Web.Helpers.Crypto.HashPassword("admin@123") + "3',1,2)");
					Sql("Insert into Users(First_Name,Last_Name,Email_Address,Password,StatusId, AdminUserId)" +
						"Values('Some UserName 4','Some Surname 4','user4@123','" + System.Web.Helpers.Crypto.HashPassword("admin@123") + "',3,2)");
					Sql("Insert into Users(First_Name,Last_Name,Email_Address,Password,StatusId, AdminUserId)" +
						"Values('Some UserName 5','Some Surname 5','user5@123','" + System.Web.Helpers.Crypto.HashPassword("admin@123") + "',2,3)");
					Sql("Insert into Users(First_Name,Last_Name,Email_Address,Password,StatusId, AdminUserId)" +
						"Values('Some UserName 6','Some Surname 6','user6@123','" + System.Web.Helpers.Crypto.HashPassword("admin@123") + "',1,3)");
						
			5. Update-Database -Verbose
			
4. Below are 3 Admin users' credantials added in DB to perform Login in our application.
	1. user: Akshay	pass:admin@123
	2. user: Neeraj	pass:admin@123
	3. user: Jolly	pass:admin@123	

5. Each User have the following attr
	1. First Name
	2. Last Name
	3. Email Address
	4. Password
	5. Status: Active/Inactive/Locked/Unlocked.

6. Below are operations we can perform.
	1. View all users assigned to a particular Admin.
	2. Admin users can perform CRUD operations on all users assigned to that user.
	3. Admin can view the latest 200 records logged within the application.
	
7. Passwords is stored in DB using Hashing for security purpose.
