using System;
using System.Windows;
using System.Windows.Controls;

namespace SimpleClassCreator.Ui
{
    public static class UserControlBase
    {
        public static void Error(Exception ex) => Warning(ex.Message);

        public static void Warning(string message) => MessageBox.Show(message);

        public static bool IsTextInvalid(
            TextBoxWithDefaultControl target, 
            string message) => IsTextInvalid(target.TextBox, message);

        public static bool IsTextInvalid(TextBox target, string message)
        {
            var invalid = string.IsNullOrWhiteSpace(target.Text);

            if (invalid)
                Warning(message);

            return invalid;
        }

        public static bool IsCheckedAndEnabled(CheckBox target) => 
            target.IsEnabled && IsChecked(target);

        public static bool IsChecked(CheckBox target) => target.IsChecked.GetValueOrDefault();
    }
}
