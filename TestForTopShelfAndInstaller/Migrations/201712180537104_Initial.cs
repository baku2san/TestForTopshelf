namespace TestForTopShelfAndInstaller.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TableAs",
                c => new
                    {
                        TableAId = c.Int(nullable: false, identity: true),
                        ColumnA = c.Boolean(nullable: false),
                        ColumnB = c.Boolean(nullable: false),
                        ColumnC = c.Boolean(nullable: false),
                        LineName = c.String(),
                        Created = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.TableAId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TableAs");
        }
    }
}
