using System;
using System.Collections.Generic;

namespace ProyectoDEU_API
{
    public partial class Curso
    {
        public Curso()
        {
            Quizzes = new HashSet<Quiz>();
            Estudiantes = new HashSet<Estudiante>();
            Recursos = new HashSet<Recurso>();
        }

        public Guid Id { get; set; }
        public string Nombre { get; set; } = null!;
        public Guid? IdDocente { get; set; }

        public virtual Docente? Docente{ get; set; }
        public virtual ICollection<Quiz> Quizzes { get; set; }
        public virtual ICollection<Estudiante> Estudiantes { get; set; }
        public virtual ICollection<Recurso> Recursos { get; set; }
    }
}
