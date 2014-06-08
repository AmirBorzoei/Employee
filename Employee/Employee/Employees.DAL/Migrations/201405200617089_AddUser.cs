namespace Employees.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUser : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserGroup",
                c => new
                    {
                        UserGroupId = c.Long(nullable: false, identity: true),
                        UserGroupName = c.String(),
                    })
                .PrimaryKey(t => t.UserGroupId);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        UserId = c.Long(nullable: false, identity: true),
                        UserName = c.String(),
                        Password = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.UserGroupUser",
                c => new
                    {
                        UserId = c.Long(nullable: false),
                        UserGroupId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.UserGroupId })
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.UserGroup", t => t.UserGroupId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.UserGroupId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserGroupUser", "UserGroupId", "dbo.UserGroup");
            DropForeignKey("dbo.UserGroupUser", "UserId", "dbo.User");
            DropIndex("dbo.UserGroupUser", new[] { "UserGroupId" });
            DropIndex("dbo.UserGroupUser", new[] { "UserId" });
            DropTable("dbo.UserGroupUser");
            DropTable("dbo.User");
            DropTable("dbo.UserGroup");
        }
    }
}
