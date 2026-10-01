using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class PortalPorRolesYPagos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProfesorId",
                table: "Usuarios",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PagosAlumnos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AlumnoId = table.Column<Guid>(type: "uuid", nullable: false),
                    Concepto = table.Column<string>(type: "text", nullable: false),
                    Periodo = table.Column<string>(type: "text", nullable: false),
                    FechaPago = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Importe = table.Column<decimal>(type: "numeric", nullable: false),
                    Observacion = table.Column<string>(type: "text", nullable: true),
                    FechaRegistro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PagosAlumnos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PagosAlumnos_Alumnos_AlumnoId",
                        column: x => x.AlumnoId,
                        principalTable: "Alumnos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_ProfesorId",
                table: "Usuarios",
                column: "ProfesorId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PagosAlumnos_AlumnoId_Concepto_Periodo",
                table: "PagosAlumnos",
                columns: new[] { "AlumnoId", "Concepto", "Periodo" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Profesores_ProfesorId",
                table: "Usuarios",
                column: "ProfesorId",
                principalTable: "Profesores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.Sql("""
                INSERT INTO "PagosAlumnos" ("Id", "AlumnoId", "Concepto", "Periodo", "FechaPago", "Importe", "Observacion", "FechaRegistro")
                SELECT md5(pago."AlumnoId"::text || ':CUOTA_MENSUAL:' || pago."Periodo")::uuid, pago."AlumnoId", 'CUOTA_MENSUAL', pago."Periodo", pago."FechaUltimoPago", pago."ImporteUltimoPago", pago."Observacion", NOW()
                FROM (
                    SELECT estado."AlumnoId", estado."FechaUltimoPago", estado."ImporteUltimoPago", estado."Observacion",
                        CASE
                            WHEN estado."UltimoPeriodoPagado" ~ '^[0-9]{4}-(0[1-9]|1[0-2])$' THEN estado."UltimoPeriodoPagado"
                            WHEN estado."UltimoPeriodoPagado" ~ '^(0[1-9]|1[0-2])/[0-9]{4}$' THEN to_char(to_date(estado."UltimoPeriodoPagado", 'MM/YYYY'), 'YYYY-MM')
                            ELSE NULL
                        END AS "Periodo"
                    FROM "EstadosCuotaAlumno" estado
                ) pago
                WHERE pago."Periodo" IS NOT NULL
                    AND pago."FechaUltimoPago" IS NOT NULL
                    AND pago."ImporteUltimoPago" IS NOT NULL;
                """);

            migrationBuilder.Sql("""
                DELETE FROM "RolPermisos" permiso
                USING "Roles" rol
                WHERE permiso."RolId" = rol."Id"
                    AND upper(rol."Nombre") = 'ALUMNO'
                    AND permiso."Pantalla" IN ('mi_asistencia', 'auto_matriculacion');

                INSERT INTO "RolPermisos" ("RolId", "Pantalla")
                SELECT rol."Id", pantalla."Clave"
                FROM "Roles" rol
                CROSS JOIN (VALUES ('mis_datos'), ('mis_cursadas'), ('mis_calificaciones'), ('mis_pagos')) AS pantalla("Clave")
                WHERE upper(rol."Nombre") = 'ALUMNO' AND NOT rol."EsSistema"
                ON CONFLICT ("RolId", "Pantalla") DO NOTHING;

                INSERT INTO "RolPermisos" ("RolId", "Pantalla")
                SELECT rol."Id", pantalla."Clave"
                FROM "Roles" rol
                CROSS JOIN (VALUES ('profesor_datos'), ('profesor_cursadas'), ('profesor_calificaciones')) AS pantalla("Clave")
                WHERE upper(rol."Nombre") = 'PROFESOR' AND NOT rol."EsSistema"
                ON CONFLICT ("RolId", "Pantalla") DO NOTHING;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Profesores_ProfesorId",
                table: "Usuarios");

            migrationBuilder.DropTable(
                name: "PagosAlumnos");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_ProfesorId",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "ProfesorId",
                table: "Usuarios");
        }
    }
}
