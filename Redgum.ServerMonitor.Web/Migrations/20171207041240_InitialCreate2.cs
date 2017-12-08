using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Redgum.ServerMonitor.Web.Migrations
{
    public partial class InitialCreate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServerData_Servers_ServerDataModelId",
                table: "ServerData");

            migrationBuilder.DropForeignKey(
                name: "FK_ServerDataAgregates_Servers_ServerDataModelId",
                table: "ServerDataAgregates");

            migrationBuilder.RenameColumn(
                name: "ServerDataModelId",
                table: "ServerDataAgregates",
                newName: "ServerId");

            migrationBuilder.RenameIndex(
                name: "IX_ServerDataAgregates_ServerDataModelId",
                table: "ServerDataAgregates",
                newName: "IX_ServerDataAgregates_ServerId");

            migrationBuilder.RenameColumn(
                name: "ServerDataModelId",
                table: "ServerData",
                newName: "ServerId");

            migrationBuilder.RenameIndex(
                name: "IX_ServerData_ServerDataModelId",
                table: "ServerData",
                newName: "IX_ServerData_ServerId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServerData_Servers_ServerId",
                table: "ServerData",
                column: "ServerId",
                principalTable: "Servers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ServerDataAgregates_Servers_ServerId",
                table: "ServerDataAgregates",
                column: "ServerId",
                principalTable: "Servers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServerData_Servers_ServerId",
                table: "ServerData");

            migrationBuilder.DropForeignKey(
                name: "FK_ServerDataAgregates_Servers_ServerId",
                table: "ServerDataAgregates");

            migrationBuilder.RenameColumn(
                name: "ServerId",
                table: "ServerDataAgregates",
                newName: "ServerDataModelId");

            migrationBuilder.RenameIndex(
                name: "IX_ServerDataAgregates_ServerId",
                table: "ServerDataAgregates",
                newName: "IX_ServerDataAgregates_ServerDataModelId");

            migrationBuilder.RenameColumn(
                name: "ServerId",
                table: "ServerData",
                newName: "ServerDataModelId");

            migrationBuilder.RenameIndex(
                name: "IX_ServerData_ServerId",
                table: "ServerData",
                newName: "IX_ServerData_ServerDataModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServerData_Servers_ServerDataModelId",
                table: "ServerData",
                column: "ServerDataModelId",
                principalTable: "Servers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ServerDataAgregates_Servers_ServerDataModelId",
                table: "ServerDataAgregates",
                column: "ServerDataModelId",
                principalTable: "Servers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
