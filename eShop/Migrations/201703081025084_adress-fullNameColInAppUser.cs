namespace eShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adressfullNameColInAppUser : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Orders", "CustomerId", "dbo.AspNetUsers");
            DropIndex("dbo.Orders", new[] { "CustomerId" });
            RenameColumn(table: "dbo.Orders", name: "CustomerId", newName: "AppUser_Id");
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        CompanyId = c.Int(nullable: false),
                        FullName = c.String(nullable: false, maxLength: 20),
                        AddressLine = c.String(nullable: false),
                        City = c.String(nullable: false),
                        Country = c.String(nullable: false),
                        Postcode = c.String(nullable: false),
                        AppUser_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.AppUser_Id)
                .Index(t => t.AppUser_Id);
            
            AddColumn("dbo.Orders", "UserId", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "FullName", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.Orders", "AppUser_Id", c => c.Int());
            CreateIndex("dbo.Orders", "AppUser_Id");
            AddForeignKey("dbo.Orders", "AppUser_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "AppUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Addresses", "AppUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Orders", new[] { "AppUser_Id" });
            DropIndex("dbo.Addresses", new[] { "AppUser_Id" });
            AlterColumn("dbo.Orders", "AppUser_Id", c => c.Int(nullable: false));
            DropColumn("dbo.AspNetUsers", "FullName");
            DropColumn("dbo.Orders", "UserId");
            DropTable("dbo.Addresses");
            RenameColumn(table: "dbo.Orders", name: "AppUser_Id", newName: "CustomerId");
            CreateIndex("dbo.Orders", "CustomerId");
            AddForeignKey("dbo.Orders", "CustomerId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
