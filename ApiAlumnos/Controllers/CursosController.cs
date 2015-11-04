using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using ApiAlumnos.Models;

namespace ApiAlumnos.Controllers
{
    public class CursosController : ApiController
    {
        private cursosEntities db;

        public CursosController()
        {
            db=new cursosEntities();
        }

        public IQueryable<Curso> Get()
        {
            return db.Curso;
        }

        [ResponseType(typeof(Curso))]
        public IHttpActionResult GetPorId(int id)
        {
            var data = db.Curso.Find(id);

            if (data == null)
                return NotFound();

            return Ok(data);

        }

    }
}
