using System;
using System.Collections.Generic;

namespace ProyectoDEU_API
{
    public partial class Estudiante
    {
        public Estudiante()
        {
            Cursos = new HashSet<Curso>();
        }

        public Guid Id { get; set; }
        public string Nombre { get; set; } = null!;

        public virtual ICollection<Curso> Cursos { get; set; }
    }
}
