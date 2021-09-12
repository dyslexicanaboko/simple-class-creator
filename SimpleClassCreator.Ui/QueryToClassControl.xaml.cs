using SimpleClassCreator.Lib;
using SimpleClassCreator.Lib.DataAccess;
using SimpleClassCreator.Lib.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
        private readonly Dictionary<ClassServices, CheckBox> _serviceToCheckBoxMap;
        private readonly CheckBoxGroup _classCheckBoxGroup;

        private static string DefaultPath => AppDomain.CurrentDomain.BaseDirectory;

        private List<ResultWindow> ResultWindows { get; }
        
        // Empty constructor Required by WPF
        public QueryToClassControl()
        {
            InitializeComponent();

            SetPathAsDefault();

            ResultWindows = new List<ResultWindow>();

            VerifiedConnections = new ConnectionManager();

            CbConnectionString_Refresh();
            
            TxtNamespaceName.ApplyDefault();
            
            TxtClassEntityName.TextBox.TextChanged += TxtClassEntityName_TextChanged;
            TxtClassEntityName.TextBox.MouseDown += TxtClassEntityName_MouseDown;
            TxtClassEntityName.DefaultButton_UnregisterDefaultEvent();
            TxtClassEntityName.DefaultButton.Click += BtnClassEntityNameDefault_Click;

            TxtClassModelName.DefaultButton_UnregisterDefaultEvent();
            TxtClassModelName.DefaultButton.Click += (sender, args) => SetModelName();

            _serviceToCheckBoxMap = GetServiceToCheckBoxMap();
            _classCheckBoxGroup = GetCheckBoxGroup();
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

        private void TxtSqlSourceText_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (RbSourceTypeTableName.IsChecked != true) return;

            FormatTableName(TxtSourceSqlText);

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
                var tbl = _svcClass.ParseTableName(TxtSourceSqlText.Text);

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

            if (string.IsNullOrWhiteSpace(strName))
                strName = TxtClassEntityName.DefaultText;
            else
                strName += "Entity";

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

        private void BtnPathDefault_Click(object sender, RoutedEventArgs e)
        {
            SetPathAsDefault();
        }

        private void BtnFileNameDefault_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtPath.Text))
                SetPathAsDefault();

            TxtFileName.Text = GetDefaultClassName(true);
        }

        private void TxtClassEntityName_TextChanged(object sender, TextChangedEventArgs e)
        {
            SetFileName();

            if (!string.IsNullOrWhiteSpace(TxtClassModelName.Text)) return;

            SetModelName();
        }

        private void SetModelName()
        {
            var entity = TxtClassEntityName.Text;

            if (entity.EndsWith("Entity", StringComparison.InvariantCultureIgnoreCase))
                entity = entity.TrimEnd("Entity".ToCharArray());

            TxtClassModelName.Text = entity + "Model";
        }

        private void SetFileName()
        {
            //On startup this control is null and so it throws an exception
            if (TxtFileName == null) return;

            TxtFileName.Text = TxtClassEntityName.Text + ".cs";
        }

        private void TxtFileName_TextChanged(object sender, TextChangedEventArgs e)
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
                var obj = GetParameters();

                if (obj == null) return;

                var win = new ResultWindow(obj.ClassOptions.EntityName, _svcQueryToClass.GenerateClass(obj));

                win.Show();

                ResultWindows.Add(win);
            }
            catch (Exception ex)
            {
                B.Error(ex);
            }
        }

        private SourceSqlType GetSourceType()
        {
            return RbSourceTypeQuery.IsChecked.GetValueOrDefault() ? SourceSqlType.Query : SourceSqlType.TableName;
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

        private void TxtClassEntityName_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TxtClassEntityName.TextBox.SelectAll();
        }
    }
}