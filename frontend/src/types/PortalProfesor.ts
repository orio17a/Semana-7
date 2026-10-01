import type { PlanillaCalificaciones } from "./Calificaciones";
import type { Cursada } from "./Academico";

export interface MiPerfilProfesor {
  profesorId:string; nombre:string; apellido:string; dni:string;
  email?:string|null; telefono?:string|null; titulo?:string|null;
}
export type ActualizarMiPerfilProfesor = Pick<MiPerfilProfesor,"nombre"|"apellido"|"email"|"telefono"|"titulo">;
export type MiCursadaProfesor = Cursada;
export type PlanillaProfesor = PlanillaCalificaciones;