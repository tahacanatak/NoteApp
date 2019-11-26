using NoteApp.API.Dtos;
using NoteApp.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoteApp.API.Extensions
{
    //extension classlar static olmali
    public static class NoteExtensions
    {
        public static GetNoteDto ToGetNoteDto(this Note note)
        {
            return new GetNoteDto
            {
                Id = note.Id,
                Title = note.Title,
                Content = note.Content,
                CreatedTime = note.CreatedTime,
                ModifiedTime = note.ModifiedTime
            };
        }
    }
}