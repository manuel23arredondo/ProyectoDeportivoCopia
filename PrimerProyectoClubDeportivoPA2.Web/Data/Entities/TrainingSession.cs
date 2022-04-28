namespace PrimerProyectoClubDeportivoPA2.Web.Data.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public class TrainingSession : IEntity
    {
        [Display(Name = "Clave")]
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} es obligatorio")]
        [MaxLength(30, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres")]
        [Display(Name = "Sesión de entrenamiento")]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0} es obligatorio")]
        [MaxLength(10, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres")]
        [Display(Name = "Capacidad")]
        public string Capacity { get; set; }

        public Schedule Schedule { get; set; }
        public Coach Coach { get; set; }
        public Sport Sport { get; set; }
        public ICollection<Agenda> Agendas { get; set; }
    } 
}
