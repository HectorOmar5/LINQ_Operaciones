using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Security.Cryptography;
using System.Diagnostics;
using System.Runtime.ConstrainedExecution;

namespace LINQ
{
    internal class OperacionesLINQ
    {
        private static List<Alumno> _listAlumnos = new List<Alumno>();
        public static List<Estado> _listEstados = new List<Estado>();
        private static List<Estatus> _listEstatus = new List<Estatus>();

        private static List<ItemISR> _listISR = new List<ItemISR>();


        public static void CargarList()
        {
            StreamReader sr = new StreamReader(@"C:\Users\Tichs\Desktop\Archivos Skype\Linq - Serializar\Alumnos.json"); //leer el archivo que contiene los datos JSON
            string jsonString = sr.ReadToEnd(); //Luego se inicializa el "jsonString", que son los datos dentro del archivo Alumnos.json
            _listAlumnos = JsonConvert.DeserializeObject<List<Alumno>>(jsonString); //Creamos una instancia de la clase Alumnos para almacenar el valor devuelto por la función
            sr.Close();

            sr = new StreamReader(@"C:\Users\Tichs\Desktop\Archivos Skype\Linq - Serializar\Estados.json");
            _listEstados = JsonConvert.DeserializeObject<List<Estado>>(sr.ReadToEnd());
            sr.Close();

            sr = new StreamReader(@"C:\Users\Tichs\Desktop\Archivos Skype\Linq - Serializar\Estatus.json");
            _listEstatus = JsonConvert.DeserializeObject<List<Estatus>>(sr.ReadToEnd());
            sr.Close();

            string[] ArchivosSeparados = new string[5];
            
            string[] TextoArchivo = File.ReadAllLines(@"C:\Users\Tichs\source\C#\TablaISR.csv");
            foreach (string line in TextoArchivo)
            {
                ItemISR objitemISR = new ItemISR();
                ArchivosSeparados = line.Split(',');
                objitemISR.LimInf = decimal.Parse(ArchivosSeparados[0]);
                objitemISR.LimSup = decimal.Parse(ArchivosSeparados[1]);
                objitemISR.CuotaFija= decimal.Parse(ArchivosSeparados[2]);
                objitemISR.PorExced = decimal.Parse(ArchivosSeparados[3]);
                objitemISR.Subsidio = decimal.Parse(ArchivosSeparados[4]);

                _listISR.Add(objitemISR); //Agregar objeto a la lista
            }
        }
        public static void Consultas()
        {
            //7.2.1.1. De la lista de estados, obtener el estado que tiene el id = 5
            var listestado = from Estado in _listEstados
                             where Estado.id == 5
                             select Estado;
            Console.WriteLine("Estado id estado 5");
            foreach (Estado estado in listestado.ToList())
            {
                Console.WriteLine($"{estado.id}, {estado.nombre}");
            }
            Console.WriteLine("\n");
            /* 7.2.1.2. De la lista de alumnos obtener a los alumnos cuyo idEstado
            29 y 13, Ordenado por nombre */
            var listalumno = from Alumno in _listAlumnos
                             where Alumno.IdEstado == 13 | Alumno.IdEstado == 29
                             orderby Alumno.nombre
                             select Alumno;
            Console.WriteLine("Alumnos con id 29 y 13");
            foreach (var alumno in listalumno.ToList())
            {
                Console.WriteLine($"{alumno.id}, {alumno.nombre}");
            }
            Console.WriteLine("\n");
            /* 7.2.1.3. De la lista de alumnos obtener los alumnos que son IdEstado 
             19 y 20 y además de que estén en el estatus 4 o 5 */
            var listainnerAlumnos = from Alumno in _listAlumnos
                                    join Estatus in _listEstatus on Alumno.IdEstatus equals Estatus.id
                                    where (Alumno.IdEstatus == 4 || Alumno.IdEstatus == 5) && (Alumno.IdEstado == 19 || Alumno.IdEstado == 20)
                                    select new { nombreAlumno = Alumno.nombre, nombreEstatus = Estatus.nombre };
            Console.WriteLine("Alumnos con id 19 y 20, estatus 4 y 5");
            foreach (var alumnos in listainnerAlumnos.ToList())
            {
                Console.WriteLine($"Nombre Alumno: {alumnos.nombreAlumno}\n Estatus: {alumnos.nombreEstatus}");
            }
            Console.WriteLine("\n");
            /* 7.2.1.4.Obtener una lista de los alumnos que tienen calificación aprobatoria, 
             considerando esta como 6 o mayor, ordenado por calificación del mayor al menor */
            var alumlist = from Alumno in _listAlumnos
                           where Alumno.calificacion >= 6
                           orderby Alumno.calificacion descending
                           select Alumno;
            Console.WriteLine("Calificacion aprobatoria 6 y ordenados de mayor a menor");
            foreach (var alumno in alumlist.ToList())
            {
                Console.WriteLine($"Nombre Alumno: {alumno.nombre} *** Calificacion: {alumno.calificacion}");
            }
            Console.WriteLine("\n");
            /* 7.2.1.5. Obtener la calificación promedio de los alumnos */
            Console.WriteLine("Calificacion promedio de los alumnos");
            var alum = _listAlumnos.Average(x => x.calificacion);
            Console.WriteLine("El promedio de los alumnos es: " + alum);


            Console.WriteLine("\n");
            /* 7.2.1.6. En caso de que ningún alumno tenga 10, sumarles un punto de calificación, 
             y en caso de que todos estén entre 6 y 7 sumarles dos puntos. */

            bool todos10 = _listAlumnos.All(alumno=>alumno.calificacion >= 10);
            bool todos6y7 = _listAlumnos.All(alumno => alumno.calificacion >= 6 && alumno.calificacion <= 7);

            if (todos10) _listAlumnos.Select(alumno => alumno.calificacion + 1);
            else if (todos6y7) _listAlumnos.Select(alumno => alumno.calificacion + 2);
            else Console.WriteLine("Ninguna cumple la condicion: " + todos10);

            Console.WriteLine("\n");
            /* 7.2.1.7. Mostar en la consola los siguientes datos, de aquellos alumnos que estén en: 
                        • Estatus 3:
                        • idAlumnos,
                        • nombreAlumno,
                        • nombre del Estado al que pertenece */
            var alu = from Alumno in _listAlumnos
                      join Estatus in _listEstatus on Alumno.IdEstatus equals Estatus.id
                      join Estado in _listEstados on Alumno.IdEstado equals Estado.id
                      where Alumno.IdEstatus == 3
                      select new { idAlumno = Alumno.id, nombreAlumno = Alumno.nombre, nombreEstatus = Estatus.id, estadoNombre = Estado.nombre };
            Console.WriteLine("Alumnos que estan en Estatus 3, idAlumnos, NombreAlumno y nombre del estado al que pertenecen");
            foreach (var alumno2 in alu)
            {
                Console.WriteLine($"Id: {alumno2.idAlumno} *** Alumno: {alumno2.nombreAlumno} *** Estatus: {alumno2.nombreEstatus} Estado: {alumno2.estadoNombre}");
            }

            Console.WriteLine("\n");
            /* 7.2.1.8.Mostar en la consola los siguientes datos, de aquellos alumnos que estén 
             • en estatus 2, ordenado por nombre del Alumno:
            • idAlumnos
            • nombreAlumno
            • nombre del Estatus en que se encuentran */
            var alus = from Alumno in _listAlumnos
                       join Estatus in _listEstatus on Alumno.IdEstatus equals Estatus.id
                       where Alumno.IdEstatus == 3
                       orderby Alumno.nombre
                       select new { id = Alumno.id, nombreAlumno = Alumno.nombre };
            Console.WriteLine("Alumnos en estatus 3 (no hay 2), idAlumno, NombreAlumno \n ordenados por nombre del alumno");
            foreach (var alumno3 in alus)
            {
                Console.WriteLine($"Id: {alumno3.id} *** {alumno3.nombreAlumno}");
            }

            Console.WriteLine("\n");
            /* 7.2.1.9.Mostar en la consola los siguientes datos, de aquellos alumnos cuyo estatus 
             sea mayor a 2, ordenado por nombre del estatus:
            • idAlumnos,
            • nombreAlumno,
            • nombre del Estado al que pertenece
            • nombre del Estatus en que se encuentran */
            var alums = from Alumno in _listAlumnos
                        join Estatus in _listEstatus on Alumno.IdEstatus equals Estatus.id
                        join Estado in _listEstados on Alumno.IdEstado equals Estado.id
                        where Alumno.IdEstatus > 3
                        select new { id = Alumno.id, nombreAlumno = Alumno.nombre, nombreEstado = Estado.nombre, nombreEstatus = Estatus.nombre };
            Console.WriteLine("Estatus mayor a 3(no hay 2), idAlumnos, nombreAlumno, nombreEstado y nombreEstatus");
            foreach (var alumno4 in alums)
            {
                Console.WriteLine($"id:{alumno4.id} ** Nombre:{alumno4.nombreAlumno} ** Estado:{alumno4.nombreEstado} ** Estatus:{alumno4.nombreEstatus}");
            }

            Console.WriteLine("\n");
            /* 7.2.1.10.Calcular el impuesto para un sueldo mensual de 22,000, y mostrarlo en la consola:
               • La búsqueda en la tablaISR de los parámetros correspondientes para 
                el cálculo del ISR deben de ser a través de LINQ */

            decimal sueldoMensaul = 22000;
            decimal sueldoQuincenal = sueldoMensaul / 2;

            ItemISR iSR = _listISR.Find(isr => sueldoQuincenal > isr.LimInf && sueldoQuincenal < isr.LimSup);
            decimal resultadoISR = (sueldoQuincenal - iSR.LimInf) * (iSR.PorExced / 100);
            decimal impuesto = resultadoISR + iSR.CuotaFija + iSR.Subsidio;

            Console.WriteLine("Limite Inferior:" + iSR.LimInf);
            Console.WriteLine("Limite Superior:" + iSR.LimSup);
            Console.WriteLine("Cuota Fija:" + iSR.CuotaFija);
            Console.WriteLine("Porcentaje Excedente:" + iSR.PorExced);
            Console.WriteLine("Impuesto:" + impuesto);


            Console.ReadKey();

        }
    }
}
