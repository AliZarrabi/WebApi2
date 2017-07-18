using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MVCCore.Migrations
{
    public partial class MessageAddUserID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "AliMsg",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "UserID",
                table: "AliMsg",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AliMsg_UserID",
                table: "AliMsg",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_AliMsg_User_UserID",
                table: "AliMsg",
                column: "UserID",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AliMsg_User_UserID",
                table: "AliMsg");

            migrationBuilder.DropIndex(
                name: "IX_AliMsg_UserID",
                table: "AliMsg");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "AliMsg");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "AliMsg",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
