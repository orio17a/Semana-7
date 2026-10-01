using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class PagoCuotaYCalificaciones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CantidadEvaluaciones",
                table: "Materias",
                type: "integer",
                nullable: false,
                defaultValue: 2);

            migrationBuilder.AddColumn<bool>(
                name: "EsPromocionable",
                table: "Materias",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<decimal>(
                name: "NotaPromocion",
                table: "Materias",
                type: "numeric",
                nullable: true,
                defaultValue: 7m);

            migrationBuilder.AddColumn<decimal>(
                name: "NotaRegularizacion",
                table: "Materias",
                type: "numeric",
                nullable: false,
                defaultValue: 4m);

            migrationBuilder.CreateTable(
                name: "EstadosCuotaAlumno",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AlumnoId = table.Column<Guid>(type: "uuid", nullable: false),
                    AlDia = table.Column<bool>(type: "boolean", nullable: false),
                    UltimoPeriodoPagado = table.Column<string>(type: "text", nullable: true),
                    FechaUltimoPago = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ImporteUltimoPago = table.Column<decimal>(type: "numeric", nullable: true),
                    Observacion = table.Column<string>(type: "text", nullable: true),
                    FechaActualizacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadosCuotaAlumno", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EstadosCuotaAlumno_Alumnos_AlumnoId",
                        column: x => x.AlumnoId,
                        principalTable: "Alumnos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Evaluaciones",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CursadaId = table.Column<Guid>(type: "uuid", nullable: false),
                    Numero = table.Column<int>(type: "integer", nullable: false),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Fecha = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Evaluaciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Evaluaciones_Cursadas_CursadaId",
                        column: x => x.CursadaId,
                        principalTable: "Cursadas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Calificaciones",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EvaluacionId = table.Column<Guid>(type: "uuid", nullable: false),
                    MatriculaId = table.Column<Guid>(type: "uuid", nullable: false),
                    Nota = table.Column<decimal>(type: "numeric", nullable: false),
                    Observacion = table.Column<string>(type: "text", nullable: true),
                    FechaCarga = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Calificaciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Calificaciones_Evaluaciones_EvaluacionId",
                        column: x => x.EvaluacionId,
                        principalTable: "Evaluaciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Calificaciones_Matriculas_MatriculaId",
                        column: x => x.MatriculaId,
                        principalTable: "Matriculas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Calificaciones_EvaluacionId_MatriculaId",
                table: "Calificaciones",
                columns: new[] { "EvaluacionId", "MatriculaId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Calificaciones_MatriculaId",
                table: "Calificaciones",
                column: "MatriculaId");

            migrationBuilder.CreateIndex(
                name: "IX_EstadosCuotaAlumno_AlumnoId",
                table: "EstadosCuotaAlumno",
                column: "AlumnoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Evaluaciones_CursadaId_Numero",
                table: "Evaluaciones",
                columns: new[] { "CursadaId", "Numero" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Calificaciones");

            migrationBuilder.DropTable(
                name: "EstadosCuotaAlumno");

            migrationBuilder.DropTable(
                name: "Evaluaciones");

            migrationBuilder.DropColumn(
                name: "CantidadEvaluaciones",
                table: "Materias");

            migrationBuilder.DropColumn(
                name: "EsPromocionable",
                table: "Materias");

            migrationBuilder.DropColumn(
                name: "NotaPromocion",
                table: "Materias");

            migrationBuilder.DropColumn(
                name: "NotaRegularizacion",
                table: "Materias");
        }
    }
}
