import { apiFetch } from "../auth/authService";
import type { GuardarCalificacionesDto, PlanillaCalificaciones } from "../types/Calificaciones";
import type { ActualizarMiPerfilProfesor, MiCursadaProfesor, MiPerfilProfesor } from "../types/PortalProfesor";

async function json<T>(response:Response):Promise<T>{if(!response.ok)throw new Error((await response.text())||"Error en la operación");return response.json()}
export const getMiPerfilProfesor=()=>apiFetch("/PortalProfesor/mis-datos").then(json<MiPerfilProfesor>);
export const actualizarMiPerfilProfesor=(data:ActualizarMiPerfilProfesor)=>apiFetch("/PortalProfesor/mis-datos",{method:"PUT",body:JSON.stringify(data)}).then(json<MiPerfilProfesor>);
export const getMisCursadasProfesor=()=>apiFetch("/PortalProfesor/cursadas").then(json<MiCursadaProfesor[]>);
export const getPlanillaProfesor=(id:string)=>apiFetch(`/PortalProfesor/cursadas/${id}/calificaciones`).then(json<PlanillaCalificaciones>);
export const generarEvaluacionesProfesor=(id:string)=>apiFetch(`/PortalProfesor/cursadas/${id}/evaluaciones`,{method:"POST"}).then(json<PlanillaCalificaciones>);
export const guardarCalificacionesProfesor=(data:GuardarCalificacionesDto)=>apiFetch("/PortalProfesor/calificaciones",{method:"POST",body:JSON.stringify(data)}).then(json<PlanillaCalificaciones>);