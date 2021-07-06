using SimpleClassCreator;
using SimpleClassCreator.DataAccess;
using SimpleClassCreator.Models;
using SimpleClassCreator.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SimpleClassCreatorUI
{
    /// <summary>
    /// Interaction logic for QueryToClassWindow.xaml
    /// </summary>
    public partial class QueryToClassWindow
        : Window
    {
        private const string DEFAULT_FIELD_PREFIX = "m_";
        private const string DEFAULT_NAMESPACE = "Namespace1";

        private readonly IClassService _svcClass;
        private readonly IQueryToClassService _svcQueryToClass;
        private readonly IGeneralDatabaseQueries _generalRepo;

        private string DefaultPath => AppDomain.CurrentDomain.BaseDirectory;

        private List<ResultWindow> ResultWindows { get; set; }
        private ConnectionManager VerifiedConnections { get; set; }

        private ConnectionManager.Connection CurrentConnection
        {
            get 
            {
                if (cbConnectionString.SelectedIndex > -1) 
                    return (ConnectionManager.Connection)cbConnectionString.SelectedItem;

                var obj = new ConnectionManager.Connection();
                obj.Verified = false;
                obj.ConnectionString = cbConnectionString.Text;

                return obj; 
            }
        }

        public QueryToClassWindow(IClassService classService, IQueryToClassService queryToClassService, IGeneralDatabaseQueries repository)
        {
            InitializeComponent();

            _svcClass = classService;
            _svcQueryToClass = queryToClassService;
            _generalRepo = repository;

            SetPathAsDefault();

            ResultWindows = new List<ResultWindow>();

            VerifiedConnections = new ConnectionManager();

            cbConnectionString_Refresh();
        }

        private void cbConnectionString_Refresh()
        {
            cbConnectionString.ItemsSource = new ObservableCollection<ConnectionManager.Connection>(VerifiedConnections.Connections);
        }

        private void btnConnectionStringTest_Click(object sender, RoutedEventArgs e)
        {
            TestConnectionString();
        }

        private bool TestConnectionString(bool showMessageOnFailureOnly = false)
        {
            var con = CurrentConnection;

            var obj = _generalRepo.TestConnectionString(con.ConnectionString);

            con.Verified = obj.Success;

            VerifiedConnections.UpdateConnection(con);

            cbConnectionString_Refresh();

            bool showMessage = true;

            if (showMessageOnFailureOnly)
                showMessage = !obj.Success;

            if (showMessage)
                Warning(obj.Success ? "Connected Successfully" : "Connection Failed. Returned error: " + obj.Message);

            return obj.Success;
        }

        private void txtSource_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (rbSourceTypeTableName.IsChecked == true)
            {
                FormatTableName(txtSource);

                SetClassNameToDefault();
            }
        }

        private void FormatTableName(TextBox target)
        {
            string strName = target.Text;

            if (string.IsNullOrWhiteSpace(strName))
                return;

            if (strName.Contains(" ") && !strName.StartsWith("[") && !strName.EndsWith("]"))
                target.Text = "[" + strName + "]";
        }

        private void SetClassNameToDefault()
        {
            txtClassName.Text = GetDefaultClassName();
        }

        private void cbBuildClassProperties_Checked(object sender, RoutedEventArgs e)
        {
            ToggleClassPropertyDependentControls((bool)cbBuildClassProperties.IsChecked);
        }

        private void ToggleClassPropertyDependentControls(bool isEnabled)
        {
            cbFieldPrefix.IsEnabled = isEnabled;
            cbFieldPrefix.IsChecked = false;

            txtFieldPrefix.IsEnabled = isEnabled;
            
            btnMemberPrefixDefault.IsEnabled = isEnabled;
        }

        private void cbIncludeNamespace_Checked(object sender, RoutedEventArgs e)
        {
            ToggleNamespaceDependentControls((bool)cbIncludeNamespace.IsChecked);
        }

        private void ToggleNamespaceDependentControls(bool isEnabled)
        {
            txtNamespace.IsEnabled = isEnabled;

            btnNamespaceDefault.IsEnabled = isEnabled;
        }

        private void btnMemberPrefixDefault_Click(object sender, RoutedEventArgs e)
        {
            txtFieldPrefix.Text = DEFAULT_FIELD_PREFIX;
        }

        private void btnNamespaceDefault_Click(object sender, RoutedEventArgs e)
        {
            txtNamespace.Text = DEFAULT_NAMESPACE;
        }

        private void btnClassNameDefault_Click(object sender, RoutedEventArgs e)
        {
            txtClassName.Text = GetDefaultClassName();

            if (string.IsNullOrWhiteSpace(txtFileName.Text))
                txtFileName.Text = txtClassName.Text + "." + GetClassFileExtension();
        }

        private string GetDefaultClassName(bool includeExtension = false)
        { 
            string strName;

            if (rbSourceTypeTableName.IsChecked == true)
            {
                var tbl = _svcClass.ParseTableName(txtSource.Text);

                strName = tbl.Table;
            }
            else
            {
                int i = 0;

                string strDir = GetPath();
                string strExt = includeExtension ? "." + GetClassFileExtension() : string.Empty;

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
            return System.IO.File.Exists(System.IO.Path.Combine(path, fileName));
        }

        private string GetPath()
        { 
            string strPath = txtPath.Text;

            if (!System.IO.Directory.Exists(strPath))
                strPath = SetPathAsDefault();

            return strPath;
        }

        private string SetPathAsDefault()
        {
            txtPath.Text = DefaultPath;

            return txtPath.Text;
        }

        private string GetClassFileExtension()
        {
            return rbGenerateAsCS.IsChecked == true ? "cs" : "vb";
        }

        private void cbSaveFileOnGeneration_Checked(object sender, RoutedEventArgs e)
        {
            ToggleSaveFileOnGenerationDependentControls((bool)cbSaveFileOnGeneration.IsChecked);
        }

        private void ToggleSaveFileOnGenerationDependentControls(bool isEnabled)
        {
            cbReplaceExistingFiles.IsEnabled = isEnabled;

            btnPathDefault.IsEnabled = isEnabled;
            btnFileNameDefault.IsEnabled = isEnabled;
            
            txtFileName.IsEnabled = isEnabled;
            txtPath.IsEnabled = isEnabled;
            
            lblFileName.IsEnabled = isEnabled;
            lblPath.IsEnabled = isEnabled;
        }

        private void btnPathDefault_Click(object sender, RoutedEventArgs e)
        {
            SetPathAsDefault();
        }

        private void btnFileNameDefault_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrWhiteSpace(txtPath.Text))
                SetPathAsDefault();

            txtFileName.Text = GetDefaultClassName(true);
        }

        private void txtClassName_TextChanged(object sender, TextChangedEventArgs e)
        {
            SetFileName();
        }

        private void SetFileName()
        {
            //On startup this control is null and so it throws an exception
            if (txtFileName == null) return;

            txtFileName.Text = txtClassName.Text + "." + GetClassFileExtension();
        }

        private void txtFileName_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (FileExist(GetPath(), txtFileName.Text))
                    txtFileName.Text = GetDefaultClassName(true) + "." + GetClassFileExtension();
            }
            catch
            {
                //Trap Exception
            }
        }

        private void rbGenerateAsCS_Checked(object sender, RoutedEventArgs e)
        {
            SetFileName();
        }

        private void rbGenerateAsVB_Checked(object sender, RoutedEventArgs e)
        {
            bool isChecked = rbGenerateAsVB.IsChecked == true;

            cbBuildClassProperties.IsChecked = isChecked;
            cbBuildClassProperties.IsEnabled = !isChecked;

            if (isChecked)
                SetFileName();
        }

        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var obj = GetCodeGeneratorParameters();

                if (obj == null) return;

                var win = new ResultWindow(_svcQueryToClass.GenerateClass(obj).ToString());
                
                win.Show();

                ResultWindows.Add(win);
            }
            catch (Exception ex)
            {
                Error(ex);
            }
        }

        private SourceTypeEnum GetSourceType()
        {
            return (rbSourceTypeQuery.IsChecked == true) ? SourceTypeEnum.Query : SourceTypeEnum.TableName;
        }

        private CodeType GetCodeType()
        {
            return (rbGenerateAsCS.IsChecked == true) ? CodeType.CSharp : CodeType.VBNet;
        }

        private ClassParameters CommonValidation()
        {
            var obj = new ClassParameters();

            var con = CurrentConnection;

            if (!con.Verified && !TestConnectionString(true))
                return null;

            obj.ConnectionString = CurrentConnection.ConnectionString;

            obj.SourceType = GetSourceType();

            if (IsTextInvalid(txtSource, obj.SourceType.ToString() + " cannot be empty."))
                return null;

            obj.ClassSource = txtSource.Text;
            obj.SaveAsFile = cbSaveFileOnGeneration.IsChecked.Value;

            if (obj.SaveAsFile)
            {
                string s = "If saving file on generation, then {0} cannot be empty.";

                if (IsTextInvalid(txtPath, string.Format(s, "Path")))
                    return null;

                if (IsTextInvalid(txtFileName, string.Format(s, "File name")))
                    return null;
            }

            obj.Filepath = txtPath.Text;
            obj.Filename = txtFileName.Text;

            return obj;
        }

        private ClassParameters GetCodeGeneratorParameters()
        {
            var obj = CommonValidation();

            //Check if the common validation failed
            if (obj == null) return null;

            obj.LanguageType = GetCodeType();
            obj.IncludeSerializableAttribute = GetCheckBoxState(cbIncludeSerializableAttribute);
            obj.BuildOutClassProperties = cbBuildClassProperties.IsChecked.Value;
            obj.IncludeFieldPrefix = GetCheckBoxState(cbFieldPrefix);
            obj.IncludeNamespace = GetCheckBoxState(cbIncludeNamespace);
            obj.ReplaceExisting = GetCheckBoxState(cbReplaceExistingFiles);

            if (obj.IncludeNamespace)
            {
                if (IsTextInvalid(txtNamespace, "If including a namespace, it cannot be empty."))
                    return null;
            }

            obj.Namespace = txtNamespace.Text;

            if (obj.IncludeFieldPrefix)
            {
                if (IsTextInvalid(txtFieldPrefix, "If including a member prefix, it cannot be empty."))
                    return null;
            }

            obj.MemberPrefix = txtFieldPrefix.Text;

            if (IsTextInvalid(txtClassName, "Class name cannot be empty."))
                return null;

            obj.TableQuery = _svcClass.ParseTableName(txtSource.Text);
            obj.ClassName = txtClassName.Text;

            return obj;
        }

        private bool IsTextInvalid(TextBox target, string message)
        {
            bool invalid = string.IsNullOrWhiteSpace(target.Text);

            if (invalid)
                Warning(message);

            return invalid;
        }

        private bool GetCheckBoxState(CheckBox target)
        {
            return target.IsEnabled ? (bool)target.IsChecked : false;
        }

        private void Error(Exception ex)
        {
            Warning(ex.Message);
        }

        private void Warning(string message)
        {
            MessageBox.Show(message);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            
            foreach (var obj in ResultWindows)
            {
                try
                {
                    if (obj != null)
                        obj.Close();
                }
                catch
                { 
                    //Trap
                }
            }
        }

        private void cbConnectionString_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                TestConnectionString();
        }

        private void txtClassName_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtClassName.SelectAll();
        }

        private void btnGenerateGridViewColumns_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var obj = CommonValidation();

                if (obj == null) return;

                var win = new ResultWindow(_svcQueryToClass.GenerateGridViewColumns(obj).ToString());
                
                win.Show();

                ResultWindows.Add(win);
            }
            catch (Exception ex)
            {
                Error(ex);
            }
        }

        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {
            using (var obj = new AboutSimpleClassCreator())
            {
                obj.ShowDialog();
            }
        }
    }
}
