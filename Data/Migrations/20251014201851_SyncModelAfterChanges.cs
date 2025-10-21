using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogApp.Migrations
{
    /// <inheritdoc />
    public partial class SyncModelAfterChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogPosts_Categorias_CategoriaId",
                table: "BlogPosts");

            migrationBuilder.AlterColumn<int>(
                name: "CategoriaId",
                table: "BlogPosts",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogPosts_Categorias_CategoriaId",
                table: "BlogPosts",
                column: "CategoriaId",
                principalTable: "Categorias",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogPosts_Categorias_CategoriaId",
                table: "BlogPosts");

            migrationBuilder.AlterColumn<int>(
                name: "CategoriaId",
                table: "BlogPosts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BlogPosts_Categorias_CategoriaId",
                table: "BlogPosts",
                column: "CategoriaId",
                principalTable: "Categorias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
