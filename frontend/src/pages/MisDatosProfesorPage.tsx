import { useEffect, useState } from "react";
import { actualizarMiPerfilProfesor, getMiPerfilProfesor } from "../services/portalProfesorService";
import type { MiPerfilProfesor } from "../types/PortalProfesor";

export default function MisDatosProfesorPage(){
 const [perfil,setPerfil]=useState<MiPerfilProfesor|null>(null),[error,setError]=useState(""),[ok,setOk]=useState(""),[saving,setSaving]=useState(false);
 useEffect(()=>{getMiPerfilProfesor().then(setPerfil).catch(e=>setError(e instanceof Error?e.message:"Error"))},[]);
 function change(k:keyof MiPerfilProfesor,v:string){if(perfil)setPerfil({...perfil,[k]:v})}
 async function save(e:React.FormEvent){e.preventDefault();if(!perfil)return;try{setSaving(true);setError("");setOk("");setPerfil(await actualizarMiPerfilProfesor({nombre:perfil.nombre,apellido:perfil.apellido,email:perfil.email,telefono:perfil.telefono,titulo:perfil.titulo}));setOk("Datos actualizados.")}catch(e){setError(e instanceof Error?e.message:"Error")}finally{setSaving(false)}}
 return <div><div className="page-header"><h1>Mis datos</h1><p>Consultá y actualizá tu información de contacto.</p></div>{error&&<div className="alert-error">{error}</div>}{ok&&<div className="alert-success">{ok}</div>}
 {!perfil&&!error&&<div className="card">Cargando perfil...</div>}
 {perfil&&<form className="card" onSubmit={save}><div className="form-grid"><div className="form-group"><label>Nombre</label><input value={perfil.nombre} onChange={e=>change("nombre",e.target.value)} required/></div><div className="form-group"><label>Apellido</label><input value={perfil.apellido} onChange={e=>change("apellido",e.target.value)} required/></div><div className="form-group"><label>DNI</label><input value={perfil.dni} readOnly/></div><div className="form-group"><label>Correo</label><input type="email" value={perfil.email??""} onChange={e=>change("email",e.target.value)}/></div><div className="form-group"><label>Teléfono</label><input value={perfil.telefono??""} onChange={e=>change("telefono",e.target.value)}/></div><div className="form-group"><label>Título</label><input value={perfil.titulo??""} onChange={e=>change("titulo",e.target.value)}/></div></div><div className="form-actions"><button className="btn btn-primary" disabled={saving}>{saving?"Guardando...":"Guardar cambios"}</button></div></form>}
 </div>;
}