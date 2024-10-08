using System;

public class BookModel
{
	public BookModel()
	{
		static int LastId;

		int Id;
		string Title;
		string Author;
		int PublishDate;

		public Book(string title, string author, int publishDate)
		{
			this.Id = ++LastId;
			this.Title = title;
			this.Author = author;
			this.PublishDate = publishDate;
		}

        public static void Save()
        {
            using (StreamWriter writer = new StreamWriter("database/BooksDb.csv"))
            {
                writer.WriteLine($"{Id},{Title},{Author},{PublishDate}");
            }
        }

    }
}
