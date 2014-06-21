namespace Employees.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPermission : DbMigration
    {
        public override void Up()
        {
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserGroupPermission", "UserGroupEntity_UserGroupId", "dbo.UserGroup");
            DropForeignKey("dbo.UserGroupPermission", "PermissionKeyEntity_PermissionKeyId", "dbo.PermissionKey");
            DropIndex("dbo.UserGroupPermission", new[] { "UserGroupEntity_UserGroupId" });
            DropIndex("dbo.UserGroupPermission", new[] { "PermissionKeyEntity_PermissionKeyId" });
            DropTable("dbo.UserGroupPermission");
            DropTable("dbo.PermissionKey");
        }
    }
}
