import { useEffect, useState } from "react";
import { getMisCalificaciones } from "../services/portalAlumnoService";
import type { MiCalificacionAlumno } from "../types/PortalAlumno";

const condicion=(value:string)=>({PROMOCIONA:"Promociona",REGULAR:"Regular",NO_REGULARIZA:"No regulariza",EN_CURSO:"En curso"}[value]??value);

export default function MisCalificacionesPage(){
 const [cursadas,setCursadas]=useState<MiCalificacionAlumno[]>([]),[error,setError]=useState(""),[loading,setLoading]=useState(true);
 useEffect(()=>{getMisCalificaciones().then(setCursadas).catch(e=>setError(e instanceof Error?e.message:"Error")).finally(()=>setLoading(false))},[]);
 return <div><div className="page-header"><h1>Mis notas</h1><p>Calificaciones registradas en tus materias.</p></div>{error&&<div className="alert-error">{error}</div>}
 {loading&&<div className="card">Cargando calificaciones...</div>}
 {!loading&&!error&&cursadas.length===0&&<div className="card">Todavía no tenés materias con calificaciones.</div>}
 <div className="attendance-cards">{cursadas.map(c=><section className="card" key={c.matriculaId}><div className="grade-header"><div><h2>{c.materia}</h2><p>{c.cicloLectivo} · {c.periodo}{c.profesor?` · ${c.profesor}`:""}</p></div><div><strong>{c.promedio??"-"}</strong><small className="table-sub">{condicion(c.condicion)}</small></div></div>{c.evaluaciones.length===0?<p>Las evaluaciones todavía no fueron publicadas.</p>:<div className="table-container"><table><thead><tr><th>Evaluación</th><th>Nota</th><th>Observación</th></tr></thead><tbody>{c.evaluaciones.map((e,i)=><tr key={e.id}><td>{e.nombre}</td><td>{c.notas[i]?.nota??"Pendiente"}</td><td>{c.notas[i]?.observacion??"-"}</td></tr>)}</tbody></table></div>}</section>)}</div></div>;
}