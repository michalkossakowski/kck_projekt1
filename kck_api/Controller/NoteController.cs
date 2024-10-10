using kck_api.Database;

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

    }
}
