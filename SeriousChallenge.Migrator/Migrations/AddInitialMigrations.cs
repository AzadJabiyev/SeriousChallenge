using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Text;

namespace SeriousChallenge.Migrator.Migrations
{
    [Migration(1)]
    public class AddInitialMigration : Migration
    {
        public override void Down()
        {
            Delete.Table("StockSymbol");
        }

        public override void Up()
        {
            Create.Table("StockSymbol")
                .WithColumn("Name").AsString().NotNullable().PrimaryKey()
                .WithColumn("Price").AsFloat()
                .WithColumn("InfoDate").AsDateTime().PrimaryKey();
        }
    }
}
