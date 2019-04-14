using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SampleWebApp.Models
{
    [Serializable]
    public class Candidate
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(50),Required]
        public string FirstName { get; set; }
        [StringLength(50)]
        public string LastName { get; set; }
        [StringLength(50),Required]
        public string Email { get; set; }

        [StringLength(50), Required]
        public string Mobile { get; set; }

        [StringLength(200)]
        public string Resume { get; set; }
    }

    /// <summary>
    /// Under the assumption we're storing one CV  
    /// </summary>

}