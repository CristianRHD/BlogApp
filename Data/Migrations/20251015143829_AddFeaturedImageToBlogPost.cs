using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogApp.Migrations
{
    /// <inheritdoc />
    public partial class AddFeaturedImageToBlogPost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogPosts_AspNetUsers_UsuarioId",
                table: "BlogPosts");

            migrationBuilder.DropIndex(
                name: "IX_BlogPosts_UsuarioId",
                table: "BlogPosts");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "BlogPosts");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "BlogPosts",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "FeaturedImageId",
                table: "BlogPosts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MediaFile",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UploadDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaFile", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlogPosts_FeaturedImageId",
                table: "BlogPosts",
                column: "FeaturedImageId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogPosts_UserId",
                table: "BlogPosts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogPosts_AspNetUsers_UserId",
                table: "BlogPosts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BlogPosts_MediaFile_FeaturedImageId",
                table: "BlogPosts",
                column: "FeaturedImageId",
                principalTable: "MediaFile",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogPosts_AspNetUsers_UserId",
                table: "BlogPosts");

            migrationBuilder.DropForeignKey(
                name: "FK_BlogPosts_MediaFile_FeaturedImageId",
                table: "BlogPosts");

            migrationBuilder.DropTable(
                name: "MediaFile");

            migrationBuilder.DropIndex(
                name: "IX_BlogPosts_FeaturedImageId",
                table: "BlogPosts");

            migrationBuilder.DropIndex(
                name: "IX_BlogPosts_UserId",
                table: "BlogPosts");

            migrationBuilder.DropColumn(
                name: "FeaturedImageId",
                table: "BlogPosts");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "BlogPosts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "UsuarioId",
                table: "BlogPosts",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BlogPosts_UsuarioId",
                table: "BlogPosts",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogPosts_AspNetUsers_UsuarioId",
                table: "BlogPosts",
                column: "UsuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
