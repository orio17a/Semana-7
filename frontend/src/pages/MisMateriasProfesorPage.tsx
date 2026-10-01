import { useEffect, useState } from "react";
import { getMisCursadasProfesor } from "../services/portalProfesorService";
import type { MiCursadaProfesor } from "../types/PortalProfesor";

function fecha(value:string){return new Date(value).toLocaleDateString();}

export default function MisMateriasProfesorPage(){
 const [cursadas,setCursadas]=useState<MiCursadaProfesor[]>([]),[error,setError]=useState(""),[loading,setLoading]=useState(true);
 useEffect(()=>{getMisCursadasProfesor().then(setCursadas).catch(e=>setError(e instanceof Error?e.message:"Error")).finally(()=>setLoading(false))},[]);
 return <div><div className="page-header"><h1>Mis materias</h1><p>Comisiones a tu cargo y sus horarios.</p></div>{error&&<div className="alert-error">{error}</div>}
 {loading&&<div className="card">Cargando materias...</div>}{!loading&&!error&&cursadas.length===0&&<div className="card">No tenés cursadas asignadas.</div>}
 <div className="course-enrollment-grid">{cursadas.map(c=><article className="card enrollment-card" key={c.id}><div className="enrollment-head"><div><h2>{c.materiaNombre}</h2><p>{c.cicloLectivo} · {c.periodo}</p></div><span className={`status-pill ${c.activa?"status-promociona":"status-inactive"}`}>{c.activa?"ACTIVA":"FINALIZADA"}</span></div><div className="enrollment-details"><p><strong>Alumnos inscriptos:</strong> {c.cantidadAlumnos}</p><p><strong>Período:</strong> {fecha(c.fechaInicio)} al {fecha(c.fechaFin)}</p><p><strong>Carga:</strong> {c.horasSemanales} hs semanales</p></div><div className="schedule-chips">{c.horarios.map((h,i)=><span key={`${h.diaSemana}-${i}`}>{["","Lunes","Martes","Miércoles","Jueves","Viernes","Sábado","Domingo"][h.diaSemana]} {h.horaInicio}–{h.horaFin}</span>)}</div></article>)}</div></div>;
}