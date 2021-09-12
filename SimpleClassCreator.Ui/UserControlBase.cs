using System;
using System.Windows;

namespace SimpleClassCreator.Ui
{
    public static class UserControlBase
    {
        public static void Error(Exception ex)
        {
            Warning(ex.Message);
        }

        public static void Warning(string message)
        {
            MessageBox.Show(message);
        }
    }
}
