CREATE TABLE "Carreras" (
    "Id" uuid NOT NULL,
    "Nombre" text NOT NULL,
    "Descripcion" text,
    "DuracionAnios" integer NOT NULL,
    "Activa" boolean NOT NULL,
    "FechaCreacion" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_Carreras" PRIMARY KEY ("Id")
);


CREATE TABLE "Profesores" (
    "Id" uuid NOT NULL,
    "Nombre" text NOT NULL,
    "Apellido" text NOT NULL,
    "Dni" text NOT NULL,
    "Email" text,
    "Telefono" text,
    "Titulo" text,
    "Activo" boolean NOT NULL,
    "FechaCreacion" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_Profesores" PRIMARY KEY ("Id")
);


CREATE TABLE "Roles" (
    "Id" uuid NOT NULL,
    "Nombre" text NOT NULL,
    "Descripcion" text,
    "Activo" boolean NOT NULL,
    "EsSistema" boolean NOT NULL,
    "FechaCreacion" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_Roles" PRIMARY KEY ("Id")
);


CREATE TABLE "Alumnos" (
    "Id" uuid NOT NULL,
    "Nombre" text NOT NULL,
    "Apellido" text NOT NULL,
    "Dni" text NOT NULL,
    "Email" text,
    "Telefono" text,
    "FechaNacimiento" timestamp with time zone NOT NULL,
    "FechaIngreso" timestamp with time zone NOT NULL,
    "Activo" boolean NOT NULL,
    "CarreraId" uuid,
    CONSTRAINT "PK_Alumnos" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Alumnos_Carreras_CarreraId" FOREIGN KEY ("CarreraId") REFERENCES "Carreras" ("Id")
);


CREATE TABLE "Materias" (
    "Id" uuid NOT NULL,
    "Nombre" text NOT NULL,
    "Codigo" text NOT NULL,
    "Anio" integer NOT NULL,
    "Activa" boolean NOT NULL,
    "CantidadEvaluaciones" integer NOT NULL DEFAULT 2,
    "EsPromocionable" boolean NOT NULL DEFAULT TRUE,
    "NotaPromocion" numeric DEFAULT 7.0,
    "NotaRegularizacion" numeric NOT NULL DEFAULT 4.0,
    "CarreraId" uuid NOT NULL,
    "ProfesorId" uuid,
    CONSTRAINT "PK_Materias" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Materias_Carreras_CarreraId" FOREIGN KEY ("CarreraId") REFERENCES "Carreras" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_Materias_Profesores_ProfesorId" FOREIGN KEY ("ProfesorId") REFERENCES "Profesores" ("Id")
);


CREATE TABLE "RolPermisos" (
    "RolId" uuid NOT NULL,
    "Pantalla" text NOT NULL,
    CONSTRAINT "PK_RolPermisos" PRIMARY KEY ("RolId", "Pantalla"),
    CONSTRAINT "FK_RolPermisos_Roles_RolId" FOREIGN KEY ("RolId") REFERENCES "Roles" ("Id") ON DELETE CASCADE
);


CREATE TABLE "EstadosCuotaAlumno" (
    "Id" uuid NOT NULL,
    "AlumnoId" uuid NOT NULL,
    "AlDia" boolean NOT NULL,
    "UltimoPeriodoPagado" text,
    "FechaUltimoPago" timestamp with time zone,
    "ImporteUltimoPago" numeric,
    "Observacion" text,
    "FechaActualizacion" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_EstadosCuotaAlumno" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_EstadosCuotaAlumno_Alumnos_AlumnoId" FOREIGN KEY ("AlumnoId") REFERENCES "Alumnos" ("Id") ON DELETE CASCADE
);


CREATE TABLE "Usuarios" (
    "Id" uuid NOT NULL,
    "NombreUsuario" text NOT NULL,
    "Nombre" text NOT NULL,
    "Apellido" text NOT NULL,
    "PasswordHash" text NOT NULL,
    "Activo" boolean NOT NULL,
    "FechaCreacion" timestamp with time zone NOT NULL,
    "RolId" uuid NOT NULL,
    "AlumnoId" uuid,
    CONSTRAINT "PK_Usuarios" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Usuarios_Alumnos_AlumnoId" FOREIGN KEY ("AlumnoId") REFERENCES "Alumnos" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_Usuarios_Roles_RolId" FOREIGN KEY ("RolId") REFERENCES "Roles" ("Id") ON DELETE RESTRICT
);


CREATE TABLE "Cursadas" (
    "Id" uuid NOT NULL,
    "MateriaId" uuid NOT NULL,
    "ProfesorId" uuid,
    "CicloLectivo" integer NOT NULL,
    "Periodo" text NOT NULL,
    "FechaInicio" timestamp with time zone NOT NULL,
    "FechaFin" timestamp with time zone NOT NULL,
    "Activa" boolean NOT NULL,
    "PorcentajePromocion" numeric NOT NULL,
    "PorcentajeRegularidad" numeric NOT NULL,
    CONSTRAINT "PK_Cursadas" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Cursadas_Materias_MateriaId" FOREIGN KEY ("MateriaId") REFERENCES "Materias" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_Cursadas_Profesores_ProfesorId" FOREIGN KEY ("ProfesorId") REFERENCES "Profesores" ("Id")
);


CREATE TABLE "Clases" (
    "Id" uuid NOT NULL,
    "CursadaId" uuid NOT NULL,
    "Fecha" timestamp with time zone NOT NULL,
    "HorasProgramadas" numeric NOT NULL,
    "FechaCarga" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_Clases" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Clases_Cursadas_CursadaId" FOREIGN KEY ("CursadaId") REFERENCES "Cursadas" ("Id") ON DELETE CASCADE
);


CREATE TABLE "CursadaHorarios" (
    "Id" uuid NOT NULL,
    "CursadaId" uuid NOT NULL,
    "DiaSemana" integer NOT NULL,
    "HoraInicio" time without time zone NOT NULL,
    "HoraFin" time without time zone NOT NULL,
    CONSTRAINT "PK_CursadaHorarios" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_CursadaHorarios_Cursadas_CursadaId" FOREIGN KEY ("CursadaId") REFERENCES "Cursadas" ("Id") ON DELETE CASCADE
);


CREATE TABLE "Evaluaciones" (
    "Id" uuid NOT NULL,
    "CursadaId" uuid NOT NULL,
    "Numero" integer NOT NULL,
    "Nombre" text NOT NULL,
    "Fecha" timestamp with time zone,
    CONSTRAINT "PK_Evaluaciones" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Evaluaciones_Cursadas_CursadaId" FOREIGN KEY ("CursadaId") REFERENCES "Cursadas" ("Id") ON DELETE CASCADE
);


CREATE TABLE "Matriculas" (
    "Id" uuid NOT NULL,
    "AlumnoId" uuid NOT NULL,
    "CursadaId" uuid NOT NULL,
    "FechaMatriculacion" timestamp with time zone NOT NULL,
    "Activa" boolean NOT NULL,
    CONSTRAINT "PK_Matriculas" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Matriculas_Alumnos_AlumnoId" FOREIGN KEY ("AlumnoId") REFERENCES "Alumnos" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_Matriculas_Cursadas_CursadaId" FOREIGN KEY ("CursadaId") REFERENCES "Cursadas" ("Id") ON DELETE CASCADE
);


CREATE TABLE "AsistenciaDetalles" (
    "Id" uuid NOT NULL,
    "ClaseId" uuid NOT NULL,
    "MatriculaId" uuid NOT NULL,
    "Estado" text NOT NULL,
    "HorasAusente" numeric NOT NULL,
    "Observacion" text,
    CONSTRAINT "PK_AsistenciaDetalles" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_AsistenciaDetalles_Clases_ClaseId" FOREIGN KEY ("ClaseId") REFERENCES "Clases" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_AsistenciaDetalles_Matriculas_MatriculaId" FOREIGN KEY ("MatriculaId") REFERENCES "Matriculas" ("Id") ON DELETE RESTRICT
);


CREATE TABLE "Calificaciones" (
    "Id" uuid NOT NULL,
    "EvaluacionId" uuid NOT NULL,
    "MatriculaId" uuid NOT NULL,
    "Nota" numeric NOT NULL,
    "Observacion" text,
    "FechaCarga" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_Calificaciones" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Calificaciones_Evaluaciones_EvaluacionId" FOREIGN KEY ("EvaluacionId") REFERENCES "Evaluaciones" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_Calificaciones_Matriculas_MatriculaId" FOREIGN KEY ("MatriculaId") REFERENCES "Matriculas" ("Id") ON DELETE RESTRICT
);


CREATE INDEX "IX_Alumnos_CarreraId" ON "Alumnos" ("CarreraId");


CREATE UNIQUE INDEX "IX_AsistenciaDetalles_ClaseId_MatriculaId" ON "AsistenciaDetalles" ("ClaseId", "MatriculaId");


CREATE INDEX "IX_AsistenciaDetalles_MatriculaId" ON "AsistenciaDetalles" ("MatriculaId");


CREATE UNIQUE INDEX "IX_Calificaciones_EvaluacionId_MatriculaId" ON "Calificaciones" ("EvaluacionId", "MatriculaId");


CREATE INDEX "IX_Calificaciones_MatriculaId" ON "Calificaciones" ("MatriculaId");


CREATE UNIQUE INDEX "IX_Clases_CursadaId_Fecha" ON "Clases" ("CursadaId", "Fecha");


CREATE INDEX "IX_CursadaHorarios_CursadaId" ON "CursadaHorarios" ("CursadaId");


CREATE INDEX "IX_Cursadas_MateriaId" ON "Cursadas" ("MateriaId");


CREATE INDEX "IX_Cursadas_ProfesorId" ON "Cursadas" ("ProfesorId");


CREATE UNIQUE INDEX "IX_EstadosCuotaAlumno_AlumnoId" ON "EstadosCuotaAlumno" ("AlumnoId");


CREATE UNIQUE INDEX "IX_Evaluaciones_CursadaId_Numero" ON "Evaluaciones" ("CursadaId", "Numero");


CREATE INDEX "IX_Materias_CarreraId" ON "Materias" ("CarreraId");


CREATE INDEX "IX_Materias_ProfesorId" ON "Materias" ("ProfesorId");


CREATE UNIQUE INDEX "IX_Matriculas_AlumnoId_CursadaId" ON "Matriculas" ("AlumnoId", "CursadaId");


CREATE INDEX "IX_Matriculas_CursadaId" ON "Matriculas" ("CursadaId");


CREATE UNIQUE INDEX "IX_Roles_Nombre" ON "Roles" ("Nombre");


CREATE UNIQUE INDEX "IX_Usuarios_AlumnoId" ON "Usuarios" ("AlumnoId");


CREATE UNIQUE INDEX "IX_Usuarios_NombreUsuario" ON "Usuarios" ("NombreUsuario");


CREATE INDEX "IX_Usuarios_RolId" ON "Usuarios" ("RolId");


