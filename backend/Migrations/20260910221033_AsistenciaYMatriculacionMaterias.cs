using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AsistenciaYMatriculacionMaterias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cursadas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MateriaId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProfesorId = table.Column<Guid>(type: "uuid", nullable: true),
                    CicloLectivo = table.Column<int>(type: "integer", nullable: false),
                    Periodo = table.Column<string>(type: "text", nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Activa = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cursadas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cursadas_Materias_MateriaId",
                        column: x => x.MateriaId,
                        principalTable: "Materias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cursadas_Profesores_ProfesorId",
                        column: x => x.ProfesorId,
                        principalTable: "Profesores",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Clases",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CursadaId = table.Column<Guid>(type: "uuid", nullable: false),
                    Fecha = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    HorasProgramadas = table.Column<decimal>(type: "numeric", nullable: false),
                    FechaCarga = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Clases_Cursadas_CursadaId",
                        column: x => x.CursadaId,
                        principalTable: "Cursadas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CursadaHorarios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CursadaId = table.Column<Guid>(type: "uuid", nullable: false),
                    DiaSemana = table.Column<int>(type: "integer", nullable: false),
                    HoraInicio = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    HoraFin = table.Column<TimeOnly>(type: "time without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CursadaHorarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CursadaHorarios_Cursadas_CursadaId",
                        column: x => x.CursadaId,
                        principalTable: "Cursadas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Matriculas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AlumnoId = table.Column<Guid>(type: "uuid", nullable: false),
                    CursadaId = table.Column<Guid>(type: "uuid", nullable: false),
                    FechaMatriculacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Activa = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matriculas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Matriculas_Alumnos_AlumnoId",
                        column: x => x.AlumnoId,
                        principalTable: "Alumnos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Matriculas_Cursadas_CursadaId",
                        column: x => x.CursadaId,
                        principalTable: "Cursadas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AsistenciaDetalles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ClaseId = table.Column<Guid>(type: "uuid", nullable: false),
                    MatriculaId = table.Column<Guid>(type: "uuid", nullable: false),
                    Estado = table.Column<string>(type: "text", nullable: false),
                    HorasAusente = table.Column<decimal>(type: "numeric", nullable: false),
                    Observacion = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AsistenciaDetalles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AsistenciaDetalles_Clases_ClaseId",
                        column: x => x.ClaseId,
                        principalTable: "Clases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AsistenciaDetalles_Matriculas_MatriculaId",
                        column: x => x.MatriculaId,
                        principalTable: "Matriculas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AsistenciaDetalles_ClaseId_MatriculaId",
                table: "AsistenciaDetalles",
                columns: new[] { "ClaseId", "MatriculaId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AsistenciaDetalles_MatriculaId",
                table: "AsistenciaDetalles",
                column: "MatriculaId");

            migrationBuilder.CreateIndex(
                name: "IX_Clases_CursadaId_Fecha",
                table: "Clases",
                columns: new[] { "CursadaId", "Fecha" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CursadaHorarios_CursadaId",
                table: "CursadaHorarios",
                column: "CursadaId");

            migrationBuilder.CreateIndex(
                name: "IX_Cursadas_MateriaId",
                table: "Cursadas",
                column: "MateriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Cursadas_ProfesorId",
                table: "Cursadas",
                column: "ProfesorId");

            migrationBuilder.CreateIndex(
                name: "IX_Matriculas_AlumnoId_CursadaId",
                table: "Matriculas",
                columns: new[] { "AlumnoId", "CursadaId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Matriculas_CursadaId",
                table: "Matriculas",
                column: "CursadaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AsistenciaDetalles");

            migrationBuilder.DropTable(
                name: "CursadaHorarios");

            migrationBuilder.DropTable(
                name: "Clases");

            migrationBuilder.DropTable(
                name: "Matriculas");

            migrationBuilder.DropTable(
                name: "Cursadas");
        }
    }
}
