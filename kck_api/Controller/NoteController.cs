using kck_api.Database;
using static Azure.Core.HttpHeader;

namespace kck_api.Controller
{
    public class NoteController
    {
        private static NoteController _instance;

        private readonly ApplicationDbContext _context;

        public NoteController()
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
            return _context.Notes.Where(n => n.AuthorId == userId).ToList();
        }

        public List<NoteModel> GetLatestNotesByUserId(int userId,int count)
        {    
            var notes = _context.Notes.Where(n => n.AuthorId == userId).ToList();
            return notes.TakeLast(count).ToList();
        }

        public NoteModel GetNoteById(int noteId)
        {
            return _context.Notes.FirstOrDefault(n => n.Id == noteId);
        }

        public void RemoveNote(int noteId)
        {
            var note = _context.Notes.FirstOrDefault(n => n.Id == noteId);
            _context.Notes.Remove(note);
            _context.SaveChanges();
        }

    }
}
