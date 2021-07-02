using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AuthApp.Infra.Exp.Migrations
{
    public partial class update_6 : Migration
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
                keyValue: 2,
                columns: new[] { "Auth0Id", "Email", "FirstName", "LastName" },
                values: new object[] { "auth0|5f37c3dab0e72f006eb4cb53", "authappadmin1@admin.com", "admin1", "admin1" });
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
                keyValue: 2,
                columns: new[] { "Auth0Id", "Email", "FirstName", "LastName" },
                values: new object[] { "auth0|5f36291f307dac006791986c", "authappadmin1@admin1.com", "admin", "admin" });
        }
    }
}
