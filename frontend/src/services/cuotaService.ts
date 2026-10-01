import { apiFetch } from "../auth/authService";
import type { EstadoCuota } from "../types/Calificaciones";
const API="http://localhost:5270/api/Cuotas";
async function parse(r:Response){if(!r.ok)throw new Error((await r.text())||"Error en cuotas");return r.json()}
export async function getCuotas():Promise<EstadoCuota[]>{return parse(await apiFetch(API))}
export async function guardarCuota(alumnoId:string,data:{alDia:boolean;ultimoPeriodoPagado?:string|null;fechaUltimoPago?:string|null;importeUltimoPago?:number|null;observacion?:string|null}):Promise<EstadoCuota>{return parse(await apiFetch(`${API}/${alumnoId}`,{method:"PUT",headers:{"Content-Type":"application/json"},body:JSON.stringify(data)}))}
export async function registrarPago(data:{alumnoId:string;concepto:"CUOTA_MENSUAL"|"MATRICULA_ANUAL";periodo:string;fechaPago:string;importe:number;observacion?:string|null}):Promise<EstadoCuota>{return parse(await apiFetch(`${API}/pagos`,{method:"POST",headers:{"Content-Type":"application/json"},body:JSON.stringify(data)}))}
