namespace PrimerProyectoClubDeportivoPA2.Web.Data.Entities
{
    using System.ComponentModel.DataAnnotations;
    public class Admin : IEntity
    {
        [Display(Name = "Clave")]
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} es obligatorio")]
        [MaxLength(8, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres")]
        [Display(Name = "Salario")]
        public string Salary { get; set; }

        public User User { get; set; }
    }
}
