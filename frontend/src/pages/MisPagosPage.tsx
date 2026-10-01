import { useEffect, useState } from "react";
import { getMisPagos } from "../services/portalAlumnoService";
import type { MiEstadoPagosAlumno } from "../types/PortalAlumno";

const dinero=(value:number)=>new Intl.NumberFormat("es-AR",{style:"currency",currency:"ARS"}).format(value);
const concepto=(value:string)=>value==="MATRICULA_ANUAL"?"Matrícula anual":"Cuota mensual";

export default function MisPagosPage(){
 const [estado,setEstado]=useState<MiEstadoPagosAlumno|null>(null),[error,setError]=useState("");
 useEffect(()=>{getMisPagos().then(setEstado).catch(e=>setError(e instanceof Error?e.message:"Error"))},[]);
 return <div><div className="page-header"><h1>Mis pagos</h1><p>Estado de cuotas y pagos registrados en tu cuenta.</p></div>{error&&<div className="alert-error">{error}</div>}
 {!estado&&!error&&<div className="card">Cargando pagos...</div>}
 {estado&&<><div className="attendance-cards"><section className="card"><span>Cuota mensual</span><h2>{estado.cuotaAlDia?"Al día":"Con deuda"}</h2><p>Último período registrado: {estado.ultimoPeriodoPagado??"Sin pagos cargados"}</p></section><section className="card"><span>Matrícula anual {estado.anioMatricula}</span><h2>{estado.matriculaAnualPagada?"Pagada":"Pendiente"}</h2></section></div><section className="card table-container"><h2>Historial de pagos</h2>{estado.pagos.length===0?<p>Todavía no hay pagos registrados.</p>:<table><thead><tr><th>Concepto</th><th>Período</th><th>Fecha</th><th>Importe</th><th>Observación</th></tr></thead><tbody>{estado.pagos.map(p=><tr key={p.id}><td>{concepto(p.concepto)}</td><td>{p.periodo}</td><td>{new Date(p.fechaPago).toLocaleDateString()}</td><td>{dinero(p.importe)}</td><td>{p.observacion??"-"}</td></tr>)}</tbody></table>}</section></>}
 </div>;
}