using Microsoft.AspNet.Identity;
using NoteApp.API.Dtos;
using NoteApp.API.Extensions;
using NoteApp.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NoteApp.API.Controllers
{
    [Authorize]
    public class NotesController : ApiController
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public IHttpActionResult GetNotes()
        {
            var userId = User.Identity.GetUserId(); //login olmuş kullanıcının idsi
            var user = db.Users.Find(userId);

            return Ok(user.Notes.Select(x => new GetNoteDto
            {
                Id = x.Id,
                Title = x.Title,
                Content = x.Content,
                CreatedTime = x.CreatedTime,
                ModifiedTime = x.ModifiedTime
            }).ToList());
        }


        public IHttpActionResult GetNote(int id)
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);

            return Ok(user.Notes
                .Where(x => x.Id == id)
                .Select(x => new GetNoteDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Content = x.Content,
                    CreatedTime = x.CreatedTime,
                    ModifiedTime = x.ModifiedTime
                })
                .FirstOrDefault());
        }

        public IHttpActionResult PostNote(PostNoteDto dto)
        {
            if (ModelState.IsValid)
            {
                var note = new Note
                {
                    Title = dto.Title,
                    Content = dto.Content,
                    CreatedTime = DateTime.Now,
                    ModifiedTime = DateTime.Now,
                    AuthorId = User.Identity.GetUserId()
                };
                db.Notes.Add(note);
                db.SaveChanges();

                return Ok(note.ToGetNoteDto());
            }
            return BadRequest(ModelState);
        }


        //PUT: api/Notes/PutNote/id
        public IHttpActionResult PutNote(int id, PutNoteDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest();
            }
            var note = db.Notes.Find(id);

            if (note == null)
            {
                return NotFound();
            }

            if (note.AuthorId != User.Identity.GetUserId())
            {
                return Unauthorized();
            }

            if (ModelState.IsValid)
            {
                note.Title = dto.Title;
                note.Content = dto.Content;
                note.ModifiedTime = DateTime.Now;

                db.SaveChanges();

                return Ok(note.ToGetNoteDto());
            }

            return BadRequest(ModelState);
        }




        public IHttpActionResult DeleteNote(int id)
        {

            var note = db.Notes.Find(id);

            if (note == null)
            {
                return NotFound();
            }

            if (note.AuthorId != User.Identity.GetUserId())
            {
                return Unauthorized();
            }

            db.Notes.Remove(note);
            db.SaveChanges();

            return Ok(note.ToGetNoteDto());
        }
    }
}
