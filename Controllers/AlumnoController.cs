using CRUD_Alumnos.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace CRUD_Alumnos.Controllers
{
    public class AlumnoController : Controller
    {
        // GET: Alumno
        public ActionResult Index()
        {
            //int edad = 30;
            //string sql = @"select a.id as Id, a.Nombres, a.Apellidos,a.Edad,a.Sexo,a.FechaRegistro,c.Nombre as NombreCiudad
            //               from Alumno A inner join Ciudad c on a.IdCiudad = c.Id where a.edad > @edad"; //Segunda opcion
       
            using (AlumnosContext db = new AlumnosContext())
            {
                var data = from a in db.Alumno //Primera opcion LinQ
                           join c in db.Ciudad on a.IdCiudad equals c.Id
                           where a.Edad > 18
                           select new AlumnoCE()
                           {
                               Id = a.Id,
                               Nombres = a.Nombres,
                               Apellidos = a.Apellidos,
                               Edad = a.Edad,
                               Sexo = a.Sexo,
                               NombreCiudad = c.Nombre,
                               FechaRegistro = a.FechaRegistro
                           };


                return View(data.ToList()); // Primera Opcion
               // return View(db.Database.SqlQuery<AlumnoCE>(sql, new SqlParameter("@edad",edad)).ToList()); //Segunda opcion
            }

            //    List<Alumno> lista = db.Alumno.Where(a => a.Edad > 18).ToList();

        }

        public ActionResult Agregar()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Agregar(Alumno a)
        {
            if (!ModelState.IsValid)
                return View();

            using(AlumnosContext db = new AlumnosContext())
            {
                try
                {
                    a.FechaRegistro = DateTime.Now;

                    db.Alumno.Add(a);
                    db.SaveChanges();
                    return RedirectToAction("Index");

                }
                catch (Exception ex)
                {

                    ModelState.AddModelError("","Error al agrega el alumno - " +  ex.Message);
                    return View();
                }

            }

        }
        public ActionResult Agregar2()
        {
            return View();
        }

        public ActionResult ListaCiudades()
        {
            using (var db = new AlumnosContext())
            {
                return PartialView(db.Ciudad.ToList());
            }
        }
        public ActionResult Editar(int id)
        {
            try
            {
                using (var db = new AlumnosContext())
                {
                    //Alumno al = db.Alumno.Where(a => a.Id == id).FirstOrDefault();
                    Alumno al2 = db.Alumno.Find(id);
                    return View(al2);

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(Alumno a)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View();

                using (var db = new AlumnosContext())
                {
                    //Alumno al = db.Alumno.Where(a => a.Id == id).FirstOrDefault();
                    Alumno al2 = db.Alumno.Find(a.Id);
                    al2.Nombres = a.Nombres;
                    al2.Apellidos = a.Apellidos;
                    al2.Edad = a.Edad;
                    al2.Sexo = a.Sexo;

                    db.SaveChanges();

                    return RedirectToAction("Index");
                 }
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        public ActionResult Detalles(int id)
        {

            try
            {
                using (var db = new AlumnosContext())
                {
                    Alumno al2 = db.Alumno.Find(id);
                    return View(al2);

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public ActionResult EliminarAlumno(int id)
        {

            try
            {
                using (var db = new AlumnosContext())
                {
                    Alumno al2 = db.Alumno.Find(id);
                    db.Alumno.Remove(al2);
                    db.SaveChanges();
                    return RedirectToAction("Index");

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public static string NombreCiudad(int IdCiudad)
        {
            using (var db = new AlumnosContext())
            {
                return db.Ciudad.Find(IdCiudad).Nombre;
            }
        }


    }
}