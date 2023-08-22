using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Portfolio.Models.MainDb.NotMapped
{
    /// <summary>
    /// The entity base class represents the data that is common to the various database models. 
    /// It provides the capability to perform soft deletes and view the time of data creation.
    /// </summary>
    [NotMapped]
    public abstract class EntityBase
    {
        public int Id { get; set; }

        [Display(Name = "Date and Time Created")]
        public DateTime Created { get; set; }

        [Display(Name = "Date and Time Last Modified")]
        public DateTime LastModified { get; set; }

        [Display(Name = "Is this Soft Deleted?")]
        public bool IsDeleted { get; set; }
        
    }
}
