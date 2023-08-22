using Portfolio.Models.MainDb.NotMapped;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Portfolio.Models.MainDb
{
    /// <summary>
    /// This RelatedFile is a model representation of the database entity that holds the name and path of a file.
    /// It is used in conjuction with other models to provide access to the files that they have stored.
    /// 
    /// Author: Devon X. Dalrymple
    /// Version: 2022-12-08
    /// </summary>
    public class RelatedFile : EntityBase
    {
        /// <summary>
        /// The relative file path that is used to locate the file on the system.
        /// </summary>
        [MaxLength(255)]
        [Display(Name = "File Path")]
        public String FilePath { get; set; }

        /// <summary>
        /// The true name of file, so that the file downloads have the correct name and the path can be obfuscated for security reasons.
        /// </summary>
        [MaxLength(255)]
        [Display(Name = "File Name")]
        public String FileName { get; set; }

    }
}
