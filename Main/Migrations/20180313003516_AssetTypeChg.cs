using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Main.Migrations
{
    public partial class AssetTypeChg : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AssetTypes",
                table: "AssetTypes");
            migrationBuilder.AddColumn<int>(
                name: "typeGUID",
                table: "AssetTypes",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<string>(
                name: "typeName",
                table: "AssetTypes",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AssetTypes",
                table: "AssetTypes",
                column: "typeGUID");

            migrationBuilder.DropColumn(
                name: "typeID",
                table: "AssetTypes");

            migrationBuilder.RenameColumn(
                name: "typeID",
                table: "Asset",
                newName: "typeName");


        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AssetTypes",
                table: "AssetTypes");
            migrationBuilder.RenameColumn(
                name: "typeName",
                table: "Asset",
                newName: "typeID");

            migrationBuilder.AddColumn<string>(
                name: "typeID",
                table: "AssetTypes",
                nullable: false,
                defaultValue: "");

            migrationBuilder.DropColumn(
                name: "typeGUID",
                table: "AssetTypes");

            migrationBuilder.DropColumn(
                name: "typeName",
                table: "AssetTypes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AssetTypes",
                table: "AssetTypes",
                column: "typeID");
        }
    }
}
