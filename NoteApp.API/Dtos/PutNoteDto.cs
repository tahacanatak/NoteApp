using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NoteApp.API.Dtos
{
    public class PutNoteDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Başlık alanı zorunludur.")]
        [StringLength(100, ErrorMessage = "Başlık {2} karakteri geçemez")]
        public string Title { get; set; }

        public string Content { get; set; }
    }
}