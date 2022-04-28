namespace PrimerProyectoClubDeportivoPA2.Web.Data.Entities
{
    using Microsoft.AspNetCore.Identity;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class User : IdentityUser
    {
        [Required(ErrorMessage = "{0} es obligatorio")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres")]
        [Display(Name = "Nombre")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "{0} es obligatorio")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres")]
        [Display(Name = "Apellidos")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "{0} es obligatorio")]
        [MaxLength(15, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Display(Name = "Número de celular")]
        public override string PhoneNumber { get; set; }

        [Display(Name = "Nombre Completo")]
        public string FullName => $"{LastName} {FirstName}";

        [Required(ErrorMessage = "{0} es obligatorio")]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Fecha de nacimiento")]
        public DateTime BirhtDate { get; set; }

        [Required(ErrorMessage = "{0} es obligatorio")]
        [Display(Name = "Matrícula")]
        public int EnrollmentNumber { get; set; }
    }
}
