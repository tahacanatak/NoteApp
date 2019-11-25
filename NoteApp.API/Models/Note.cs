using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NoteApp.API.Models
{
    public class Note
    {
        public int Id { get; set; }

        [Required]
        [ForeignKey("Author")]
        public string AuthorId { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        public string Content { get; set; }

        [Required]
        public DateTime? CreatedTime { get; set; }

        public DateTime? ModifiedTime { get; set; }




        public ApplicationUser Author { get; set; }

    }
}