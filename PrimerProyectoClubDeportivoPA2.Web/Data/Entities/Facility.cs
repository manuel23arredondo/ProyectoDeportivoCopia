namespace PrimerProyectoClubDeportivoPA2.Web.Data.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public class Facility : IEntity
    {
        [Display(Name = "Clave")]
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} es obligatorio")]
        [MaxLength(12, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres")]      
        [Display(Name = "Código")]
        public string Code { get; set; }

        [Required(ErrorMessage = "{0} es obligatorio.")]
        [MaxLength(25, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres")]      
        [Display(Name = "Nombre de Instalación")]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0} es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres")]    
        [Display(Name = "Descripción")]
        public string Description { get; set; }

        [Display(Name = "Imagen")]
        public string ImageUrl { get; set; }

        public ICollection<Schedule> Schedules { get; set; }
    }
}
