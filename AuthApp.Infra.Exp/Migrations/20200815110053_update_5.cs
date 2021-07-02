using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AuthApp.Infra.Exp.Migrations
{
    public partial class update_5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:uuid-ossp", ",,");

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .OldAnnotation("Npgsql:PostgresExtension:uuid-ossp", ",,");

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
        }
    }
}
