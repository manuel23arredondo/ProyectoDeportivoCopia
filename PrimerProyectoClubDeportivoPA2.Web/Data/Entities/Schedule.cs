namespace PrimerProyectoClubDeportivoPA2.Web.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public class Schedule : IEntity
    {
        [Display(Name = "Clave")]
        public int Id { get; set; }      

        [Required(ErrorMessage = "{0} es obligatorio")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString ="{0:yyyy-MM-dd H:mm}", ApplyFormatInEditMode = true)]
        [Display(Name = "Hora de inicio")]
        public DateTime StartingHour { get; set; }

        [Required(ErrorMessage = "{0} es obligatorio")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd H:mm}", ApplyFormatInEditMode = true)]
        [Display(Name = "Hora de término")]
        public DateTime FinishingHour { get; set; }

        public WeekDay WeekDay { get; set; }
        public Facility Facility { get; set; }
        public ICollection<TrainingSession> TrainingSessions { get; set; }
    }
}
