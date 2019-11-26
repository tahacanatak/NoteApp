using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoteApp.API.Dtos
{
    //viewmodelin api icin olani olarak dusunebilirsin

    public class GetNoteDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime? CreatedTime { get; set; }

        public DateTime? ModifiedTime { get; set; }




    }
}