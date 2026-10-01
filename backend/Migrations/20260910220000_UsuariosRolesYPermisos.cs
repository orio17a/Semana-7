using System;
using backend.Data;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations;

[DbContext(typeof(AppDbContext))]
[Migration("20260910220000_UsuariosRolesYPermisos")]
public partial class UsuariosRolesYPermisos : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Roles",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Nombre = table.Column<string>(type: "text", nullable: false),
                Descripcion = table.Column<string>(type: "text", nullable: true),
                Activo = table.Column<bool>(type: "boolean", nullable: false),
                EsSistema = table.Column<bool>(type: "boolean", nullable: false),
                FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_Roles", x => x.Id));

        migrationBuilder.CreateTable(
            name: "RolPermisos",
            columns: table => new
            {
                RolId = table.Column<Guid>(type: "uuid", nullable: false),
                Pantalla = table.Column<string>(type: "text", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_RolPermisos", x => new { x.RolId, x.Pantalla });
                table.ForeignKey("FK_RolPermisos_Roles_RolId", x => x.RolId, "Roles", "Id", onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Usuarios",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                NombreUsuario = table.Column<string>(type: "text", nullable: false),
                Nombre = table.Column<string>(type: "text", nullable: false),
                Apellido = table.Column<string>(type: "text", nullable: false),
                PasswordHash = table.Column<string>(type: "text", nullable: false),
                Activo = table.Column<bool>(type: "boolean", nullable: false),
                FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                RolId = table.Column<Guid>(type: "uuid", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Usuarios", x => x.Id);
                table.ForeignKey("FK_Usuarios_Roles_RolId", x => x.RolId, "Roles", "Id", onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateIndex("IX_Roles_Nombre", "Roles", "Nombre", unique: true);
        migrationBuilder.CreateIndex("IX_Usuarios_NombreUsuario", "Usuarios", "NombreUsuario", unique: true);
        migrationBuilder.CreateIndex("IX_Usuarios_RolId", "Usuarios", "RolId");

        migrationBuilder.Sql("""
            INSERT INTO "Roles" ("Id", "Nombre", "Descripcion", "Activo", "EsSistema", "FechaCreacion")
            VALUES (
                '10000000-0000-0000-0000-000000000001'::uuid,
                'ADMIN',
                'Administrador del sistema',
                TRUE,
                TRUE,
                TIMESTAMPTZ '2026-09-10 21:00:00+00'
            );

            INSERT INTO "RolPermisos" ("RolId", "Pantalla") VALUES
                ('10000000-0000-0000-0000-000000000001'::uuid, 'alumnos'),
                ('10000000-0000-0000-0000-000000000001'::uuid, 'carreras'),
                ('10000000-0000-0000-0000-000000000001'::uuid, 'profesores'),
                ('10000000-0000-0000-0000-000000000001'::uuid, 'materias'),
                ('10000000-0000-0000-0000-000000000001'::uuid, 'usuarios'),
                ('10000000-0000-0000-0000-000000000001'::uuid, 'roles');

            INSERT INTO "Usuarios" ("Id", "NombreUsuario", "Nombre", "Apellido", "PasswordHash", "Activo", "FechaCreacion", "RolId")
            VALUES (
                '20000000-0000-0000-0000-000000000001'::uuid,
                'admin',
                'Administrador',
                'Local',
                'SW5zdGl0dXRvTG9jYWxBZG1pbjIwMjY=.eO0Tm3AEw52HgWVNI5niIb1gOQG+ev/M1qXfQDF1tok=',
                TRUE,
                TIMESTAMPTZ '2026-09-10 21:00:00+00',
                '10000000-0000-0000-0000-000000000001'::uuid
            );
        """);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "RolPermisos");
        migrationBuilder.DropTable(name: "Usuarios");
        migrationBuilder.DropTable(name: "Roles");
    }
}
