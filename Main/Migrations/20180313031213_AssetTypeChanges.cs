using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Main.Migrations
{
    public partial class AssetTypeChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "typeGUID",
                table: "AssetTypes",
                newName: "typeID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "typeID",
                table: "AssetTypes",
                newName: "typeGUID");
        }
    }
}
