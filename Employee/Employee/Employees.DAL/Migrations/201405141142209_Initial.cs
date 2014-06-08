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
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Employee");
        }
    }
}
