namespace Vidly.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDataInTables : DbMigration
    {
        public override void Up()
        {
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
        }
        
        public override void Down()
        {
        }
    }
}
