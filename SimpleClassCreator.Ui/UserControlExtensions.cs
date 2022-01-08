using System;
using System.Windows;
using System.Windows.Controls;

namespace SimpleClassCreator.Ui
{
    public static class UserControlExtensions
    {
        public static void ShowErrorMessage(Exception ex) => ShowWarningMessage(ex.Message);

        public static void ShowWarningMessage(string message) => MessageBox.Show(message);

        public static void ShowAsErrorMessage(this Exception ex) => ShowWarningMessage(ex.Message);

        public static void ShowAsWarningMessage(this string message) => ShowWarningMessage(message);

        public static bool IsTextInvalid(
            this TextBoxWithDefaultControl target, 
            string message) => IsTextInvalid(target.TextBox, message);

        public static bool IsTextInvalid(this TextBox target, string message)
        {
            var invalid = string.IsNullOrWhiteSpace(target.Text);

            if (invalid)
                ShowWarningMessage(message);

            return invalid;
        }

        public static bool IsCheckedAndEnabled(this CheckBox target) => 
            target.IsEnabled && IsChecked(target);

        public static bool IsChecked(this CheckBox target) => target.IsChecked.GetValueOrDefault();
    }
}
