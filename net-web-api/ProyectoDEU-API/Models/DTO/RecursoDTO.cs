namespace ProyectoDEU_API.Models.DTO
{
    public class RecursoDTO
    {
        public string Titulo { get; set; } = null!;
        public string Texto { get; set; } = null!;
        public Guid? IdCurso { get; set; }

        public virtual ICollection<EnlaceRecursoDTO> Enlaces { get; set; }
    }

    public class RecursoModificadoDTO: RecursoDTO
    {
        public virtual ICollection<Guid> EnlacesBaja { get; set; }

    }

    public class EnlaceRecursoDTO
    {
        public string? Url { get; set; }
        public string? Nombre { get; set; }
    }
}
