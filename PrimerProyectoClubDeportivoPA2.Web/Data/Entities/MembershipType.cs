namespace PrimerProyectoClubDeportivoPA2.Web.Data.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public class MembershipType : IEntity
    {
        [Display(Name = "Clave")]
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} es obligatorio")]
        [MaxLength(25, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres")]
        [Display(Name = "Nombre de Membresía")]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0} es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres")]
        [Display(Name = "Descripción")]
        public string Description { get; set; }

        [Required(ErrorMessage = "{0} es obligatorio")]
        [MaxLength(7, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres")]
        [Display(Name = "Costo")]
        public string Cost { get; set; }

        public ICollection<Permit> Permits { get; set; }
        public ICollection<Member> Members { get; set; }
    }
}
