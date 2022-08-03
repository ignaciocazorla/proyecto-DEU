using System;
using System.Collections.Generic;

namespace ProyectoDEU_API
{
    public partial class Recurso
    {
        public Recurso()
        {
            Enlaces = new HashSet<EnlaceRecurso>();
            Pregunta = new HashSet<Pregunta>();
        }

        public Guid Id { get; set; }
        public string Titulo { get; set; } = null!;
        public string Texto { get; set; } = null!;
        public Guid? IdCurso { get; set; }

        public virtual ICollection<EnlaceRecurso> Enlaces { get; set; }
        public virtual ICollection<Pregunta> Pregunta { get; set; }
        public virtual Curso? Curso { get; set; }
    }
}
