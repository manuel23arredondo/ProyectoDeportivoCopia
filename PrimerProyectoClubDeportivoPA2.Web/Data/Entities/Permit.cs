namespace PrimerProyectoClubDeportivoPA2.Web.Data.Entities
{
    using System.ComponentModel.DataAnnotations;
    public class Permit : IEntity
    {
        [Display(Name = "Clave")]
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} es obligatorio")]
        [MaxLength(25, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres")]
        [Display(Name = "Permiso")]
        public string Name { get; set; }

        public Sport Sport { get; set; }
        public MembershipType MembershipType { get; set; }
    }
}
