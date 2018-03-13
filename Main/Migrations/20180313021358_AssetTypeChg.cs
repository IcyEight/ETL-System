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

            migrationBuilder.DropColumn(
                name: "typeID",
                table: "AssetTypes");

            migrationBuilder.AddColumn<string>(
                name: "typeName",
                table: "Asset",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AssetTypes",
                table: "AssetTypes",
                column: "typeGUID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AssetTypes",
                table: "AssetTypes");

            migrationBuilder.DropColumn(
                name: "typeGUID",
                table: "AssetTypes");

            migrationBuilder.DropColumn(
                name: "typeName",
                table: "AssetTypes");

            migrationBuilder.DropColumn(
                name: "typeName",
                table: "Asset");

            migrationBuilder.AddColumn<string>(
                name: "typeID",
                table: "AssetTypes",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AssetTypes",
                table: "AssetTypes",
                column: "typeID");
        }
    }
}
