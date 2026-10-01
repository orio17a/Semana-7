import type { EstadoAsistenciaAlumno, Matricula } from "./Academico";

export interface MiPerfilAlumno {
  alumnoId:string; nombre:string; apellido:string; dni:string; email?:string|null; telefono?:string|null;
  fechaNacimiento:string; fechaIngreso:string; carreraNombre?:string|null;
}
export interface MiHorarioCursada { diaSemana:number; dia:string; horaInicio:string; horaFin:string; }
export interface MiCursadaDisponible {
  cursadaId:string; materiaId:string; materia:string; profesor?:string|null; cicloLectivo:number; periodo:string;
  fechaInicio:string; fechaFin:string; horasSemanales:number; horasTotalesPlanificadas:number;
  porcentajePromocion:number; porcentajeRegularidad:number; matriculado:boolean; matriculaId?:string|null;
  fechaMatriculacion?:string|null; puedeDesmatricularse:boolean; horarios:MiHorarioCursada[];
}
export interface MiCalificacionAlumno {
  matriculaId:string; materia:string; profesor?:string|null; cicloLectivo:number; periodo:string;
  promedio?:number|null; condicion:string;
  evaluaciones:{id:string;numero:number;nombre:string;fecha?:string|null}[];
  notas:{evaluacionId:string;nota?:number|null;observacion?:string|null}[];
}
export interface MiPagoAlumno {
  id:string; concepto:string; periodo:string; fechaPago:string; importe:number; observacion?:string|null;
}
export interface MiEstadoPagosAlumno {
  cuotaAlDia:boolean; ultimoPeriodoPagado?:string|null; anioMatricula:number; matriculaAnualPagada:boolean;
  pagos:MiPagoAlumno[];
}
export type { EstadoAsistenciaAlumno, Matricula };
