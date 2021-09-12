using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SimpleClassCreator.Lib;
using SimpleClassCreator.Lib.DataAccess;
using SimpleClassCreator.Lib.Models;
using SimpleClassCreator.Lib.Services;
using B = SimpleClassCreator.Ui.UserControlBase;

namespace SimpleClassCreator.Ui
{
    /// <summary>
    ///     Interaction logic for QueryToClassControl.xaml
    /// </summary>
    public partial class QueryToClassControl : UserControl
    {
        private INameFormatService _svcClass;
        private IQueryToClassService _svcQueryToClass;
        private IGeneralDatabaseQueries _generalRepo;

        private static string DefaultPath => AppDomain.CurrentDomain.BaseDirectory;

        private List<ResultWindow> ResultWindows { get; }
        private ConnectionManager VerifiedConnections { get; }

        private ConnectionManager.Connection CurrentConnection
        {
            get
            {
                if (CbConnectionString.SelectedIndex > -1)
                    return (ConnectionManager.Connection) CbConnectionString.SelectedItem;

                var obj = new ConnectionManager.Connection();
                obj.Verified = false;
                obj.ConnectionString = CbConnectionString.Text;

                return obj;
            }
        }

        // Empty constructor Required by WPF
        public QueryToClassControl()
        {
            InitializeComponent();

            SetPathAsDefault();

            ResultWindows = new List<ResultWindow>();

            VerifiedConnections = new ConnectionManager();

            CbConnectionString_Refresh();

            TxtClassEntityName.TextBox.TextChanged += TxtClassEntityName_TextChanged;
            TxtClassEntityName.TextBox.MouseDown += TxtClassEntityName_MouseDown;
            TxtClassEntityName.DefaultButton_UnregisterDefaultEvent();
            TxtClassEntityName.DefaultButton.Click += BtnClassEntityNameDefault_Click;
        }

        public void Dependencies(
            INameFormatService classService, 
            IQueryToClassService queryToClassService,
            IGeneralDatabaseQueries repository)
        {
            _svcClass = classService;
            _svcQueryToClass = queryToClassService;
            _generalRepo = repository;
        }

        private void CbConnectionString_Refresh()
        {
            CbConnectionString.ItemsSource =
                new ObservableCollection<ConnectionManager.Connection>(VerifiedConnections.Connections);
        }

        private void BtnConnectionStringTest_Click(object sender, RoutedEventArgs e)
        {
            TestConnectionString();
        }

        private bool TestConnectionString(bool showMessageOnFailureOnly = false)
        {
            var con = CurrentConnection;

            var obj = _generalRepo.TestConnectionString(con.ConnectionString);

            con.Verified = obj.Success;

            VerifiedConnections.UpdateConnection(con);

            CbConnectionString_Refresh();

            var showMessage = true;

            if (showMessageOnFailureOnly)
                showMessage = !obj.Success;

            if (showMessage)
                B.Warning(obj.Success ? "Connected Successfully" : "Connection Failed. Returned error: " + obj.Message);

            return obj.Success;
        }

        private void TxtSource_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (RbSourceTypeTableName.IsChecked != true) return;

            FormatTableName(TxtSource);

            TxtClassEntityName.Text = GetDefaultClassName();
        }

        private void FormatTableName(TextBox target)
        {
            var strName = target.Text;

            if (string.IsNullOrWhiteSpace(strName))
                return;

            target.Text = _svcClass.FormatTableQuery(strName);
        }

        private void BtnClassEntityNameDefault_Click(object sender, RoutedEventArgs e)
        {
            TxtClassEntityName.Text = GetDefaultClassName();

            if (string.IsNullOrWhiteSpace(TxtFileName.Text))
                TxtFileName.Text = TxtClassEntityName.Text + ".cs";
        }

        private string GetDefaultClassName(bool includeExtension = false)
        {
            string strName;

            if (RbSourceTypeTableName.IsChecked == true)
            {
                var tbl = _svcClass.ParseTableName(TxtSource.Text);

                strName = _svcClass.GetClassName(tbl);
            }
            else
            {
                var i = 0;

                var strDir = GetPath();
                var strExt = includeExtension ? ".cs" : string.Empty;

                strName = "Class_" + i + strExt; //Must prime the string

                while (FileExist(strDir, strName))
                {
                    i++;

                    strName = "Class_" + i + strExt;
                }
            }

            return strName;
        }

        private bool FileExist(string path, string fileName)
        {
            return File.Exists(Path.Combine(path, fileName));
        }

        private string GetPath()
        {
            var strPath = TxtPath.Text;

            if (!Directory.Exists(strPath))
                strPath = SetPathAsDefault();

            return strPath;
        }

        private string SetPathAsDefault()
        {
            TxtPath.Text = DefaultPath;

            return TxtPath.Text;
        }

        private void CbSaveFileOnGeneration_Checked(object sender, RoutedEventArgs e)
        {
            ToggleSaveFileOnGenerationDependentControls(CbSaveFileOnGeneration.IsChecked.GetValueOrDefault());
        }

        private void ToggleSaveFileOnGenerationDependentControls(bool isEnabled)
        {
            CbReplaceExistingFiles.IsEnabled = isEnabled;

            BtnPathDefault.IsEnabled = isEnabled;
            BtnFileNameDefault.IsEnabled = isEnabled;

            TxtFileName.IsEnabled = isEnabled;
            TxtPath.IsEnabled = isEnabled;

            LblFileName.IsEnabled = isEnabled;
            LblPath.IsEnabled = isEnabled;
        }

        private void btnPathDefault_Click(object sender, RoutedEventArgs e)
        {
            SetPathAsDefault();
        }

        private void btnFileNameDefault_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtPath.Text))
                SetPathAsDefault();

            TxtFileName.Text = GetDefaultClassName(true);
        }

        private void TxtClassEntityName_TextChanged(object sender, TextChangedEventArgs e)
        {
            SetFileName();
        }

        private void SetFileName()
        {
            //On startup this control is null and so it throws an exception
            if (TxtFileName == null) return;

            TxtFileName.Text = TxtClassEntityName.Text + ".cs";
        }

        private void txtFileName_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (FileExist(GetPath(), TxtFileName.Text))
                    TxtFileName.Text = GetDefaultClassName(true) + ".cs";
            }
            catch
            {
                //Trap Exception
            }
        }

        private void BtnGenerate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var obj = GetCodeGeneratorParameters();

                if (obj == null) return;

                var win = new ResultWindow(_svcQueryToClass.GenerateClass(obj));

                win.Show();

                ResultWindows.Add(win);
            }
            catch (Exception ex)
            {
                B.Error(ex);
            }
        }

        private SourceTypeEnum GetSourceType()
        {
            return RbSourceTypeQuery.IsChecked == true ? SourceTypeEnum.Query : SourceTypeEnum.TableName;
        }

        private ClassParameters CommonValidation()
        {
            var obj = new ClassParameters();

            var con = CurrentConnection;

            if (!con.Verified && !TestConnectionString(true))
                return null;

            obj.ConnectionString = CurrentConnection.ConnectionString;

            obj.SourceType = GetSourceType();

            if (IsTextInvalid(TxtSource, obj.SourceType + " cannot be empty."))
                return null;

            obj.ClassSource = TxtSource.Text;
            obj.SaveAsFile = CbSaveFileOnGeneration.IsChecked.GetValueOrDefault();

            if (obj.SaveAsFile)
            {
                var s = "If saving file on generation, then {0} cannot be empty.";

                if (IsTextInvalid(TxtPath, string.Format(s, "Path")))
                    return null;

                if (IsTextInvalid(TxtFileName, string.Format(s, "File name")))
                    return null;
            }

            obj.Filepath = TxtPath.Text;
            obj.Filename = TxtFileName.Text;

            return obj;
        }

        private ClassParameters GetCodeGeneratorParameters()
        {
            var obj = CommonValidation();

            //Check if the common validation failed
            if (obj == null) return null;

            obj.LanguageType = CodeType.CSharp;
            obj.ReplaceExisting = GetCheckBoxState(CbReplaceExistingFiles);
            obj.Namespace = TxtNamespaceName.Text;

            if (IsTextInvalid(TxtClassEntityName, "Class name cannot be empty."))
                return null;

            obj.TableQuery = _svcClass.ParseTableName(TxtSource.Text);
            obj.ClassName = TxtClassEntityName.Text;

            return obj;
        }

        private bool IsTextInvalid(TextBoxWithDefaultControl target, string message)
        {
            return IsTextInvalid(target.TextBox, message);
        }

        private bool IsTextInvalid(TextBox target, string message)
        {
            var invalid = string.IsNullOrWhiteSpace(target.Text);

            if (invalid)
                B.Warning(message);

            return invalid;
        }

        private bool GetCheckBoxState(CheckBox target)
        {
            return target.IsEnabled && target.IsChecked.GetValueOrDefault();
        }

        public void CloseResultWindows()
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

        private void CbConnectionString_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                TestConnectionString();
        }

        private void TxtClassEntityName_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TxtClassEntityName.TextBox.SelectAll();
        }

        private void BtnGenerateGridViewColumns_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var obj = CommonValidation();

                if (obj == null) return;

                var win = new ResultWindow(_svcQueryToClass.GenerateGridViewColumns(obj));

                win.Show();

                ResultWindows.Add(win);
            }
            catch (Exception ex)
            {
                B.Error(ex);
            }
        }
    }
}