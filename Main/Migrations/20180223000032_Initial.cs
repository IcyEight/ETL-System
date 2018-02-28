using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Main.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AssetData",
                columns: table => new
                {
                    assetID = table.Column<int>(nullable: false),
                    fieldName = table.Column<string>(nullable: false),
                    boolValue = table.Column<bool>(nullable: false),
                    dateValue = table.Column<DateTime>(nullable: false),
                    fieldType = table.Column<string>(nullable: true),
                    floatValue = table.Column<double>(nullable: false),
                    intValue = table.Column<long>(nullable: false),
                    isPrimaryKey = table.Column<bool>(nullable: false),
                    strValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetData", x => new { x.assetID, x.fieldName });
                });

            migrationBuilder.CreateTable(
                name: "AssetTypes",
                columns: table => new
                {
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetTypes", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "Reporting",
                columns: table => new
                {
                    ReportID = table.Column<int>(nullable: false),
                    DateCreate = table.Column<DateTime>(nullable: true),
                    DateModified = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reporting", x => x.ReportID);
                });

            migrationBuilder.CreateTable(
                name: "TaskQueue",
                columns: table => new
                {
                    AssetId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    alertMessage = table.Column<string>(nullable: true),
                    dateComplete = table.Column<DateTime>(nullable: true),
                    isComplete = table.Column<bool>(nullable: false),
                    resolvedBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskQueue", x => x.AssetId);
                });

            migrationBuilder.CreateTable(
                name: "Asset",
                columns: table => new
                {
                    AssetId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AssetName = table.Column<string>(nullable: true),
                    LongDescription = table.Column<string>(nullable: true),
                    ShortDescription = table.Column<string>(nullable: true),
                    assetTypeName = table.Column<string>(nullable: true),
                    isDeleted = table.Column<bool>(nullable: false),
                    isPreferredAsset = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Asset", x => x.AssetId);
                    table.ForeignKey(
                        name: "FK_Asset_AssetTypes_assetTypeName",
                        column: x => x.assetTypeName,
                        principalTable: "AssetTypes",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DataSchema",
                columns: table => new
                {
                    schemaID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    assetTypeName = table.Column<string>(nullable: true),
                    fieldName = table.Column<string>(nullable: true),
                    fieldType = table.Column<string>(nullable: true),
                    isPrimary = table.Column<bool>(nullable: false),
                    schemaName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataSchema", x => x.schemaID);
                    table.ForeignKey(
                        name: "FK_DataSchema_AssetTypes_assetTypeName",
                        column: x => x.assetTypeName,
                        principalTable: "AssetTypes",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Module",
                columns: table => new
                {
                    moduleID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    detail1 = table.Column<string>(nullable: true),
                    detail2 = table.Column<string>(nullable: true),
                    detail3 = table.Column<string>(nullable: true),
                    detail4 = table.Column<string>(nullable: true),
                    detail5 = table.Column<string>(nullable: true),
                    name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Module", x => x.moduleID);
                    table.ForeignKey(
                        name: "FK_Module_AssetTypes_name",
                        column: x => x.name,
                        principalTable: "AssetTypes",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AssetModule",
                columns: table => new
                {
                    assetID = table.Column<int>(nullable: false),
                    moduleID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetModule", x => new { x.assetID, x.moduleID });
                    table.ForeignKey(
                        name: "FK_AssetModule_Asset_assetID",
                        column: x => x.assetID,
                        principalTable: "Asset",
                        principalColumn: "AssetId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssetModule_Module_moduleID",
                        column: x => x.moduleID,
                        principalTable: "Module",
                        principalColumn: "moduleID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Asset_assetTypeName",
                table: "Asset",
                column: "assetTypeName");

            migrationBuilder.CreateIndex(
                name: "IX_AssetModule_moduleID",
                table: "AssetModule",
                column: "moduleID");

            migrationBuilder.CreateIndex(
                name: "IX_DataSchema_assetTypeName",
                table: "DataSchema",
                column: "assetTypeName");

            migrationBuilder.CreateIndex(
                name: "IX_Module_name",
                table: "Module",
                column: "name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssetData");

            migrationBuilder.DropTable(
                name: "AssetModule");

            migrationBuilder.DropTable(
                name: "DataSchema");

            migrationBuilder.DropTable(
                name: "Reporting");

            migrationBuilder.DropTable(
                name: "TaskQueue");

            migrationBuilder.DropTable(
                name: "Asset");

            migrationBuilder.DropTable(
                name: "Module");

            migrationBuilder.DropTable(
                name: "AssetTypes");
        }
    }
}
