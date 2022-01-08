using System.Collections.Generic;

namespace SimpleClassCreator.Ui.Helpers
{
    public class ResultWindowManager
    {
        private List<ResultWindow> ResultWindows { get; } = new List<ResultWindow>();

        public void Add(ResultWindow resultWindow) => ResultWindows.Add(resultWindow);

        public void CloseAll()
        {
            if (ResultWindows == null) return;

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
