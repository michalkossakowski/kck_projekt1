using kck_api.Database;
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

        public List<NoteModel> GetNotesByUserId(int userId)
        {
            var notes = _context.Notes.Where(n => n.AuthorId == userId).OrderBy(n => n.ModifiedDate).ToList();
            notes.Reverse();
            return notes;
        }

        public NoteModel GetNoteById(int noteId)
        {
            return _context.Notes.FirstOrDefault(n => n.Id == noteId);
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

        public List<NoteModel> GetNotesByUserIdAndMonth(int userId, DateTime date)
        {
            var notes = _context.Notes.Where(n => n.AuthorId == userId
                && n.ModifiedDate.Month == date.Month
                && n.ModifiedDate.Year == date.Year)
                .ToList();
            notes.Reverse();
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
                  .ToList();
            notes.Reverse();
            return notes;
        }

        public List<NoteModel> GetNotesByUserIdAndTitle(int userId, string title)
        {
            var notes = _context.Notes
                .Where(n => n.AuthorId == userId
                    && n.Title.Contains(title))
                .OrderBy(n => n.ModifiedDate).ToList();
            notes.Reverse();
            return notes; 
        }

        public List<NoteModel> GetNotesByUserIdAndCategory(int userId, string category)
        {
            var notes = _context.Notes.Where(n => n.AuthorId == userId
            && n.Category.Contains(category)).OrderBy(n => n.ModifiedDate).ToList();
            notes.Reverse();
            return notes;
        }

        public void EditNote(int noteId, string newTitle, string newCategory, string newContent)
        {
            var note = _context.Notes.FirstOrDefault(n => n.Id == noteId);
            note.Title = newTitle;
            note.Category = newCategory;
            note.Content = newContent;
            note.ModifiedDate = DateTime.Now;
            _context.SaveChanges();
        }
        public void RemoveNote(int noteId)
        {
            var note = _context.Notes.FirstOrDefault(n => n.Id == noteId);
            _context.Notes.Remove(note);
            _context.SaveChanges();
        }

    }
}
