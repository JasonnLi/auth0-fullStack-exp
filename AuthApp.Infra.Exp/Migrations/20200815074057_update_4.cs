using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AuthApp.Infra.Exp.Migrations
{
    public partial class update_4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Customer",
                keyColumn: "Id",
                keyValue: 1,
                column: "PublicId",
                value: new Guid("bbdee09c-089b-4d30-bece-44df5923716c"));

            migrationBuilder.UpdateData(
                table: "Customer",
                keyColumn: "Id",
                keyValue: 2,
                column: "PublicId",
                value: new Guid("6fb600c1-9011-4fd7-9234-881379716440"));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "Auth0Id",
                value: "auth0|5f36283c307dac0067919854");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Customer",
                keyColumn: "Id",
                keyValue: 1,
                column: "PublicId",
                value: new Guid("bbdee09c-089b-4d30-bece-44df5923716c"));

            migrationBuilder.UpdateData(
                table: "Customer",
                keyColumn: "Id",
                keyValue: 2,
                column: "PublicId",
                value: new Guid("6fb600c1-9011-4fd7-9234-881379716440"));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "Auth0Id",
                value: "auth0|507f1f77bcf86cd799439020");
        }
    }
}
