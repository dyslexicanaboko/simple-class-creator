using System.Collections.Generic;
using System.Linq;

namespace SimpleClassCreator.Ui.Helpers
{
    public class ResultWindowManager
    {
        private List<ResultWindow> ResultWindows { get; } = new List<ResultWindow>();

        public void Add(ResultWindow resultWindow) => ResultWindows.Add(resultWindow);

        public void Show(string title, string contents)
        {
            var win = new ResultWindow(title, contents);

            win.Show();

            Add(win);
        }

        public void CloseAll()
        {
            if (ResultWindows == null) return;

            if (!ResultWindows.Any()) return;

            foreach (var obj in ResultWindows)
            {
                try
                {
                    obj?.Close();
                }
                catch
                {
                    //Trap
                }
            }
        }
    }
}
