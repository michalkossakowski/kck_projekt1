﻿using kck_api.Database;
using Microsoft.EntityFrameworkCore;
using static Azure.Core.HttpHeader;

namespace kck_api.Controller
{
    public class NoteController
    {
        private static NoteController _instance;

        private readonly ApplicationDbContext _context;

        private NoteController()
        {
            _context = ApplicationDbContext.GetInstance();
        }

        public static NoteController GetInstance()
        {
            if (_instance == null)
                _instance = new NoteController();

            return _instance;
        }

        public void AddNote(NoteModel note)
        {
            _context.Notes.Add(note);
            _context.SaveChanges();
        }

        public async Task<bool> AddNoteAsync(NoteModel note)
        {
            try
            {
                await _context.Notes.AddAsync(note);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public List<NoteModel> GetNotesByUserId(int userId)
        {
            var notes = _context.Notes.Where(n => n.AuthorId == userId).OrderByDescending(n => n.ModifiedDate).ToList();
           
            return notes;
        }

        public async Task<List<NoteModel>> GetNotesByUserIdAsync(int userId)
        {
            try
            {
                return await _context.Notes.Where(n => n.AuthorId == userId)
                                           .OrderByDescending(n => n.ModifiedDate)
                                           .ToListAsync();
            }
            catch (Exception)
            {
                return new List<NoteModel>();
            }
        }

        public NoteModel GetNoteById(int noteId)
        {
            return _context.Notes.FirstOrDefault(n => n.Id == noteId);
        }

        public async Task<NoteModel?> GetNoteByIdAsync(int noteId)
        {
            try
            {
                return await _context.Notes.FirstOrDefaultAsync(n => n.Id == noteId);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<NoteModel> GetLatestNotesByUserId(int userId, int count)
        {
            var notes = _context.Notes
                .Where(n => n.AuthorId == userId)
                .OrderBy(n => n.ModifiedDate)
                .ToList();
            notes = notes.TakeLast(count).ToList(); 
            notes.Reverse();

            return notes;
        }

        public async Task<List<NoteModel>> GetLatestNotesByUserIdAsync(int userId, int count)
        {
            var notes = await _context.Notes
                .Where(n => n.AuthorId == userId)
                .OrderByDescending(n => n.ModifiedDate)
                .ToListAsync();
            notes = notes.Take(count).ToList();

            return notes;
        }

        public List<NoteModel> GetNotesByUserIdAndMonth(int userId, DateTime date)
        {
            var notes = _context.Notes.Where(n => n.AuthorId == userId
                && n.ModifiedDate.Month == date.Month
                && n.ModifiedDate.Year == date.Year)
                .OrderByDescending(n => n.ModifiedDate)
                .ToList();

            return notes;
        }

        public async Task<List<NoteModel>> GetNotesByUserIdAndMonthAsync(int userId, DateTime date)
        {
            var notes = await _context.Notes.Where(n => n.AuthorId == userId
                && n.ModifiedDate.Month == date.Month
                && n.ModifiedDate.Year == date.Year)
                .OrderByDescending(n => n.ModifiedDate)
                .ToListAsync();

            return notes;
        }

        public List<NoteModel> GetNotesByUserIdAndDay(int userId, DateTime date)
        {
            var notes = _context.Notes
                  .OrderBy(n => n.ModifiedDate)
                  .Where(n => n.AuthorId == userId
                        && n.ModifiedDate.Month == date.Month
                        && n.ModifiedDate.Year == date.Year
                        && n.ModifiedDate.Day == date.Day)
                  .OrderByDescending(n => n.ModifiedDate)
                  .ToList();

            return notes;
        }

        public async Task<List<NoteModel>> GetNotesByUserIdAndDayAsync(int userId, DateTime date)
        {
            var notes = await _context.Notes
                  .OrderBy(n => n.ModifiedDate)
                  .Where(n => n.AuthorId == userId
                        && n.ModifiedDate.Month == date.Month
                        && n.ModifiedDate.Year == date.Year
                        && n.ModifiedDate.Day == date.Day)
                  .OrderByDescending(n => n.ModifiedDate)
                  .ToListAsync();

            return notes;
        }

        public List<NoteModel> GetNotesByUserIdAndTitle(int userId, string title)
        {
            var notes = _context.Notes
                .Where(n => n.AuthorId == userId
                    && n.Title.Contains(title))
                .OrderByDescending(n => n.ModifiedDate)
                .ToList();

            return notes; 
        }

        public async Task<List<NoteModel>> GetNotesByUserIdAndTitleAsync(int userId, string title)
        {
            var notes = await _context.Notes
                .Where(n => n.AuthorId == userId
                    && n.Title.Contains(title))
                .OrderByDescending(n => n.ModifiedDate)
                .ToListAsync();

            return notes;
        }

        public List<NoteModel> GetNotesByUserIdAndCategory(int userId, int category)
        {
            var notes = _context.Notes.Where(n => n.AuthorId == userId
            && n.CategoryId == category)
            .OrderByDescending(n => n.ModifiedDate)
            .ToList();

            return notes;
        }

        public async Task<List<NoteModel>> GetNotesByUserIdAndCategoryAsync(int userId, int category)
        {
            var notes = await _context.Notes.Where(n => n.AuthorId == userId
            && n.CategoryId==category)
            .OrderByDescending(n => n.ModifiedDate)
            .ToListAsync();

            return notes;
        }

        public void EditNote(int noteId, string newTitle, int newCategory, string newContent)
        {
            var note = _context.Notes.FirstOrDefault(n => n.Id == noteId);
            note.Title = newTitle;
            note.CategoryId = newCategory;
            note.Content = newContent;
            note.ModifiedDate = DateTime.Now;

            _context.SaveChanges();
        }

        public async Task<bool> EditNoteAsync(int noteId, string newTitle, int newCategory, string newContent)
        {
            try
            {
                var note = await _context.Notes.FirstOrDefaultAsync(n => n.Id == noteId);
                if (note != null)
                {
                    note.Title = newTitle;
                    note.CategoryId = newCategory;
                    note.Content = newContent;
                    note.ModifiedDate = DateTime.Now;
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void RemoveNote(int noteId)
        {
            var note = _context.Notes.FirstOrDefault(n => n.Id == noteId);
            _context.Notes.Remove(note);

            _context.SaveChanges();
        }

        public async Task<bool> RemoveNoteAsync(int noteId)
        {
            try
            {
                var note = await _context.Notes.FirstOrDefaultAsync(n => n.Id == noteId);
                if (note != null)
                {
                    _context.Notes.Remove(note);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool IsUserHasAnyNotes(int userId)
        {
            return _context.Notes.Where(n => n.AuthorId == userId).Any();
        }
    }
}
