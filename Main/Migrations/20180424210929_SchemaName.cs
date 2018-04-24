using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Main.Migrations
{
    public partial class SchemaName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Reporting",
                table: "Reporting");

            migrationBuilder.DropColumn(
                name: "ReportID",
                table: "Reporting");

            migrationBuilder.DropColumn(
                name: "DateCreate",
                table: "Reporting");

            migrationBuilder.DropColumn(
                name: "DateModified",
                table: "Reporting");

            migrationBuilder.DropColumn(
                name: "schemaID",
                table: "AssetData");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Reporting",
                newName: "strValue");

            migrationBuilder.AddColumn<string>(
                name: "fieldName",
                table: "Reporting",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "schemaName",
                table: "AssetData",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reporting",
                table: "Reporting",
                column: "fieldName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Reporting",
                table: "Reporting");

            migrationBuilder.DropColumn(
                name: "fieldName",
                table: "Reporting");

            migrationBuilder.DropColumn(
                name: "schemaName",
                table: "AssetData");

            migrationBuilder.RenameColumn(
                name: "strValue",
                table: "Reporting",
                newName: "Name");

            migrationBuilder.AddColumn<int>(
                name: "ReportID",
                table: "Reporting",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreate",
                table: "Reporting",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateModified",
                table: "Reporting",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "schemaID",
                table: "AssetData",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reporting",
                table: "Reporting",
                column: "ReportID");
        }
    }
}
