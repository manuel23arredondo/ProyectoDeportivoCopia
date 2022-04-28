namespace PrimerProyectoClubDeportivoPA2.Web.Data.Entities
{
    using System.ComponentModel.DataAnnotations;
    public class AdditionalSkill : IEntity
    {
        [Display(Name = "Clave")]
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} es obligatorio")]
        [MaxLength(30, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres")]
        [Display(Name = "Habilidad adicional")]
        public string Name { get; set; }

        public Coach Coach { get; set; }
        public Sport Sport { get; set; }
    }
}
