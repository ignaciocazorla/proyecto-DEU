using System;
using System.Collections.Generic;

namespace ProyectoDEU_API
{
    public partial class EnlaceRecurso
    {
        public Guid Id { get; set; }
        public Guid IdRecurso { get; set; }
        public string? Url { get; set; }
        public string? Nombre { get; set; }

        public virtual Recurso IdRecursoNavigation { get; set; } = null!;
    }
}
