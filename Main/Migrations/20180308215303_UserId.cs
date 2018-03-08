using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Main.Migrations
{
    public partial class UserId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "assignee",
                table: "TaskQueue",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Owner",
                table: "Asset",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "assignee",
                table: "TaskQueue");

            migrationBuilder.DropColumn(
                name: "Owner",
                table: "Asset");
        }
    }
}
