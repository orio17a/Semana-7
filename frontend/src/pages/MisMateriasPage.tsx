import { useEffect, useState } from "react";
import { getMisMaterias } from "../services/portalAlumnoService";
import type { MiCursadaDisponible } from "../types/PortalAlumno";

function fecha(value:string){return new Date(value).toLocaleDateString();}

export default function MisMateriasPage(){
 const [materias,setMaterias]=useState<MiCursadaDisponible[]>([]),[error,setError]=useState(""),[loading,setLoading]=useState(true);
 useEffect(()=>{getMisMaterias().then(setMaterias).catch(e=>setError(e instanceof Error?e.message:"Error")).finally(()=>setLoading(false))},[]);
 return <div><div className="page-header"><h1>Mis materias</h1><p>Materias en las que estás inscripto, con profesor y horarios.</p></div>{error&&<div className="alert-error">{error}</div>}
 {loading&&<div className="card">Cargando materias...</div>}
 {!loading&&!error&&materias.length===0&&<div className="card">No tenés materias inscriptas.</div>}
 <div className="course-enrollment-grid">{materias.map(c=><article className="card enrollment-card enrolled" key={c.matriculaId}><div className="enrollment-head"><div><h2>{c.materia}</h2><p>{c.cicloLectivo} · {c.periodo}</p></div><span className="status-pill status-promociona">INSCRIPTO</span></div><div className="enrollment-details"><p><strong>Profesor:</strong> {c.profesor??"A confirmar"}</p><p><strong>Período:</strong> {fecha(c.fechaInicio)} al {fecha(c.fechaFin)}</p><p><strong>Carga horaria:</strong> {c.horasSemanales} hs semanales</p></div><div className="schedule-chips">{c.horarios.map((h,i)=><span key={`${h.diaSemana}-${i}`}>{h.dia} {h.horaInicio}–{h.horaFin}</span>)}</div></article>)}</div></div>;
}