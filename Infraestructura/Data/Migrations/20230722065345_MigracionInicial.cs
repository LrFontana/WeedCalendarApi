using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infraestructura.Data.Migrations
{
    /// <inheritdoc />
    public partial class MigracionInicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TblUsuario",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreUsuario = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Mail = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblUsuario", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TblPlantas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombrePlanta = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    VariedadPlanta = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    EstapaCrecimento = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notas = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaSiembre = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaCosecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblPlantas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TblPlantas_TblUsuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "TblUsuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TblSistemaRiego",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CantidadAgua = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    TipoSistema = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    ProporcionNutrientes = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    PlantasId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblSistemaRiego", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TblSistemaRiego_TblPlantas_PlantasId",
                        column: x => x.PlantasId,
                        principalTable: "TblPlantas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TblSistemaRiego_TblUsuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "TblUsuario",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TblNotificaciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TipoDeNotificacion = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Mensaje = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FechaHora = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    PlantasId = table.Column<int>(type: "int", nullable: false),
                    SistemaRiegoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblNotificaciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TblNotificaciones_TblPlantas_PlantasId",
                        column: x => x.PlantasId,
                        principalTable: "TblPlantas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TblNotificaciones_TblSistemaRiego_SistemaRiegoId",
                        column: x => x.SistemaRiegoId,
                        principalTable: "TblSistemaRiego",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TblNotificaciones_TblUsuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "TblUsuario",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TblNotificaciones_PlantasId",
                table: "TblNotificaciones",
                column: "PlantasId");

            migrationBuilder.CreateIndex(
                name: "IX_TblNotificaciones_SistemaRiegoId",
                table: "TblNotificaciones",
                column: "SistemaRiegoId");

            migrationBuilder.CreateIndex(
                name: "IX_TblNotificaciones_UsuarioId",
                table: "TblNotificaciones",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_TblPlantas_UsuarioId",
                table: "TblPlantas",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_TblSistemaRiego_PlantasId",
                table: "TblSistemaRiego",
                column: "PlantasId");

            migrationBuilder.CreateIndex(
                name: "IX_TblSistemaRiego_UsuarioId",
                table: "TblSistemaRiego",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TblNotificaciones");

            migrationBuilder.DropTable(
                name: "TblSistemaRiego");

            migrationBuilder.DropTable(
                name: "TblPlantas");

            migrationBuilder.DropTable(
                name: "TblUsuario");
        }
    }
}
