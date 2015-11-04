using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using ApiAlumnos.Models;

namespace ApiAlumnos.Controllers
{
    public class AlumnosController : ApiController
    {
        private cursosEntities db;

        public AlumnosController()
        {
            db=new cursosEntities();
        }

        public IQueryable<Alumno> Get()
        {
            return db.Alumno;
        }

        [ResponseType(typeof (Alumno))]
        public IHttpActionResult Get(String id)
        {
            var a = db.Alumno.Find(id);
            if (a == null)
                return NotFound();
            return Ok(a);
        }

        public ICollection<Alumno> GetByCurso(int idCurso)
        {
            var c = db.Curso.Include("Alumno").FirstOrDefault(o => o.idCurso == idCurso);

            if (c == null)
                return null;
            return c.Alumno.ToList();

        }
        public ICollection<Alumno> GetByProfe(int idProfesor)
        {
            var c = db.ProfesorCurso.Where(o => o.idProfesor == idProfesor)
                .Select(o=>o.idCurso);
            var al = db.Curso.Where(o => c.Contains(o.idCurso)).Select(o=>o.Alumno);
            var l=new List<Alumno>();
            foreach (var a in al)
            {
                l.AddRange(a);
            }

            
            return l;

        }

        [ResponseType(typeof (Alumno))]
        public IHttpActionResult Post(Alumno alumno)
        {
            db.Alumno.Add(alumno);

            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                return BadRequest("Error en el alta");
            }

            return Created("ApiAlumnos", alumno);


        }

        [ResponseType(typeof (Alumno))]
        public IHttpActionResult Put(Alumno al)
        {
            db.Entry(al).State=EntityState.Modified;

            //var alu = db.Alumno.Find(al.dni);
            //if (alu == null)
            //    return NotFound();

            //alu.nombre = al.nombre;

            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            return Ok(al);
        }

        [ResponseType(typeof(void))]
        public IHttpActionResult Delete(string id)
        {
            

            var alu = db.Alumno.Find(id);
            if (alu == null)
                return NotFound();
            db.Alumno.Remove(alu);
            

            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
