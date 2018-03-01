using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Main.Migrations
{
    public partial class DataAPI : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Asset_AssetTypes_assetTypeName",
                table: "Asset");

            migrationBuilder.DropForeignKey(
                name: "FK_AssetModule_Asset_assetID",
                table: "AssetModule");

            migrationBuilder.DropForeignKey(
                name: "FK_AssetModule_Module_moduleID",
                table: "AssetModule");

            migrationBuilder.DropForeignKey(
                name: "FK_DataSchema_AssetTypes_assetTypeName",
                table: "DataSchema");

            migrationBuilder.DropForeignKey(
                name: "FK_Module_AssetTypes_name",
                table: "Module");

            migrationBuilder.DropIndex(
                name: "IX_Module_name",
                table: "Module");

            migrationBuilder.DropIndex(
                name: "IX_DataSchema_assetTypeName",
                table: "DataSchema");

            migrationBuilder.DropIndex(
                name: "IX_AssetModule_moduleID",
                table: "AssetModule");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AssetData",
                table: "AssetData");

            migrationBuilder.DropIndex(
                name: "IX_Asset_assetTypeName",
                table: "Asset");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Module",
                newName: "typeID");

            migrationBuilder.RenameColumn(
                name: "assetTypeName",
                table: "DataSchema",
                newName: "assetTypeID");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "AssetTypes",
                newName: "typeID");

            migrationBuilder.RenameColumn(
                name: "assetTypeName",
                table: "Asset",
                newName: "typeID");

            migrationBuilder.AlterColumn<string>(
                name: "typeID",
                table: "Module",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "moduleName",
                table: "Module",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "assetTypeID",
                table: "DataSchema",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "dataEntryID",
                table: "AssetData",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "typeID",
                table: "Asset",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AssetData",
                table: "AssetData",
                columns: new[] { "assetID", "dataEntryID", "fieldName" });

            migrationBuilder.AddForeignKey(
                name: "FK_AssetData_Asset_assetID",
                table: "AssetData",
                column: "assetID",
                principalTable: "Asset",
                principalColumn: "AssetId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssetData_Asset_assetID",
                table: "AssetData");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AssetData",
                table: "AssetData");

            migrationBuilder.DropColumn(
                name: "moduleName",
                table: "Module");

            migrationBuilder.DropColumn(
                name: "dataEntryID",
                table: "AssetData");

            migrationBuilder.RenameColumn(
                name: "typeID",
                table: "Module",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "assetTypeID",
                table: "DataSchema",
                newName: "assetTypeName");

            migrationBuilder.RenameColumn(
                name: "typeID",
                table: "AssetTypes",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "typeID",
                table: "Asset",
                newName: "assetTypeName");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "Module",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "assetTypeName",
                table: "DataSchema",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "assetTypeName",
                table: "Asset",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AssetData",
                table: "AssetData",
                columns: new[] { "assetID", "fieldName" });

            migrationBuilder.CreateIndex(
                name: "IX_Module_name",
                table: "Module",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "IX_DataSchema_assetTypeName",
                table: "DataSchema",
                column: "assetTypeName");

            migrationBuilder.CreateIndex(
                name: "IX_AssetModule_moduleID",
                table: "AssetModule",
                column: "moduleID");

            migrationBuilder.CreateIndex(
                name: "IX_Asset_assetTypeName",
                table: "Asset",
                column: "assetTypeName");

            migrationBuilder.AddForeignKey(
                name: "FK_Asset_AssetTypes_assetTypeName",
                table: "Asset",
                column: "assetTypeName",
                principalTable: "AssetTypes",
                principalColumn: "Name",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AssetModule_Asset_assetID",
                table: "AssetModule",
                column: "assetID",
                principalTable: "Asset",
                principalColumn: "AssetId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AssetModule_Module_moduleID",
                table: "AssetModule",
                column: "moduleID",
                principalTable: "Module",
                principalColumn: "moduleID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DataSchema_AssetTypes_assetTypeName",
                table: "DataSchema",
                column: "assetTypeName",
                principalTable: "AssetTypes",
                principalColumn: "Name",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Module_AssetTypes_name",
                table: "Module",
                column: "name",
                principalTable: "AssetTypes",
                principalColumn: "Name",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
