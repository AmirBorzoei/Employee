namespace Employees.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Employee",
                c => new
                    {
                        EmployeeId = c.Long(nullable: false, identity: true),
                        PersonallyCode = c.String(),
                        NationalCode = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        FatherName = c.String(),
                        FamilyCount = c.Int(nullable: false),
                        Age = c.Int(nullable: false),
                        WorkHistory = c.Int(nullable: false),
                        IsMarried = c.Boolean(nullable: false),
                        CreateUserId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.EmployeeId);
            
            CreateTable(
                "dbo.PermissionKey",
                c => new
                    {
                        PermissionKeyId = c.Long(nullable: false, identity: true),
                        TreeId = c.Long(nullable: false),
                        TreeParentId = c.Long(nullable: false),
                        PermissionKeyName = c.String(),
                        PermissionKeyLabel = c.String(),
                    })
                .PrimaryKey(t => t.PermissionKeyId);
            
            CreateTable(
                "dbo.UserGroup",
                c => new
                    {
                        UserGroupId = c.Long(nullable: false, identity: true),
                        UserGroupName = c.String(),
                    })
                .PrimaryKey(t => t.UserGroupId);
            
            CreateTable(
                "dbo.UserGroupPermission",
                c => new
                    {
                        UserGroupPermissionId = c.Long(nullable: false, identity: true),
                        PermissionAccessType = c.Int(nullable: false),
                        PermissionKeyEntity_PermissionKeyId = c.Long(),
                        UserGroupEntity_UserGroupId = c.Long(),
                    })
                .PrimaryKey(t => t.UserGroupPermissionId)
                .ForeignKey("dbo.PermissionKey", t => t.PermissionKeyEntity_PermissionKeyId)
                .ForeignKey("dbo.UserGroup", t => t.UserGroupEntity_UserGroupId)
                .Index(t => t.PermissionKeyEntity_PermissionKeyId)
                .Index(t => t.UserGroupEntity_UserGroupId);
            
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
            DropForeignKey("dbo.UserGroupPermission", "UserGroupEntity_UserGroupId", "dbo.UserGroup");
            DropForeignKey("dbo.UserGroupPermission", "PermissionKeyEntity_PermissionKeyId", "dbo.PermissionKey");
            DropIndex("dbo.UserGroupUser", new[] { "UserGroupId" });
            DropIndex("dbo.UserGroupUser", new[] { "UserId" });
            DropIndex("dbo.UserGroupPermission", new[] { "UserGroupEntity_UserGroupId" });
            DropIndex("dbo.UserGroupPermission", new[] { "PermissionKeyEntity_PermissionKeyId" });
            DropTable("dbo.UserGroupUser");
            DropTable("dbo.User");
            DropTable("dbo.UserGroupPermission");
            DropTable("dbo.UserGroup");
            DropTable("dbo.PermissionKey");
            DropTable("dbo.Employee");
        }
    }
}
