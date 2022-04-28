namespace PrimerProyectoClubDeportivoPA2.Web.Data.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public class Coach : IEntity
    {
        [Display(Name = "Clave")]
        public int Id { get; set; }
        
        [Required(ErrorMessage = "{0} es obligatorio")]
        [MaxLength(8, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres")]
        [Display(Name = "Salario")]
        public string Salary { get; set; }

        [Required(ErrorMessage = "{0} es obligatorio")]
        [MaxLength(30, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres")]
        [Display(Name = "Especialidad")]
        public string Expertise { get; set; }

        [Display(Name = "Imagen")]
        public string ImageUrl { get; set; }

        public User User { get; set; }
        public ICollection<TrainingSession> TrainingSessions { get; set; }
        public ICollection<AdditionalSkill> AdditionalSkills { get; set; }
    }
}
