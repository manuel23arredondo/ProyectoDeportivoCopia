namespace PrimerProyectoClubDeportivoPA2.Web.Data.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public class Sport : IEntity
    {
        [Display(Name = "Clave")]
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} es obligatorio")]
        [MaxLength(25, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres")]
        [Display(Name = "Deporte")]
        public string Name { get; set; }

        public ICollection<TrainingSession> TrainingSessions { get; set; }
        public ICollection<AdditionalSkill> AdditionalSkills { get; set; }
        public ICollection<Permit> Permits { get; set; }
    }
}
