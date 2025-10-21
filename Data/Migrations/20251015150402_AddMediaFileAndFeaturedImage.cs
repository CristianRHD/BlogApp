using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogApp.Migrations
{
    /// <inheritdoc />
    public partial class AddMediaFileAndFeaturedImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogPosts_MediaFile_FeaturedImageId",
                table: "BlogPosts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MediaFile",
                table: "MediaFile");

            migrationBuilder.RenameTable(
                name: "MediaFile",
                newName: "MediaFiles");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MediaFiles",
                table: "MediaFiles",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogPosts_MediaFiles_FeaturedImageId",
                table: "BlogPosts",
                column: "FeaturedImageId",
                principalTable: "MediaFiles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogPosts_MediaFiles_FeaturedImageId",
                table: "BlogPosts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MediaFiles",
                table: "MediaFiles");

            migrationBuilder.RenameTable(
                name: "MediaFiles",
                newName: "MediaFile");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MediaFile",
                table: "MediaFile",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogPosts_MediaFile_FeaturedImageId",
                table: "BlogPosts",
                column: "FeaturedImageId",
                principalTable: "MediaFile",
                principalColumn: "Id");
        }
    }
}
