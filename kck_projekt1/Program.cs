using kck_projekt1.View;
using Spectre.Console;
using System.Text;

namespace kck_projekt1
{
    public class Program
    {
        public static FigletFont font = FigletFont.Load("ANSI Shadow.flf");

        static void Main(string[] args)
        {
            System.Console.OutputEncoding = Encoding.UTF8;
            System.Console.InputEncoding = Encoding.UTF8;

            var menuView = new MenuView();
            menuView.MainMenu();
        }
    }
}