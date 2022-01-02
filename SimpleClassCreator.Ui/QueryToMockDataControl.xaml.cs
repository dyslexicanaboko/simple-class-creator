using SimpleClassCreator.Lib;
using SimpleClassCreator.Lib.DataAccess;
using SimpleClassCreator.Lib.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SimpleClassCreator.Lib.Exceptions;
using SimpleClassCreator.Ui.Profile;
using B = SimpleClassCreator.Ui.UserControlExtensions;

namespace SimpleClassCreator.Ui
{
    /// <summary>
    ///     Interaction logic for QueryToMockDataControl.xaml
    /// </summary>
    public partial class QueryToMockDataControl : UserControl
    {
        private INameFormatService _svcClass;
        //private IQueryToMockDataService _svcQueryToMockData;
        private IGeneralDatabaseQueries _generalRepo;

        private static string DefaultPath => AppDomain.CurrentDomain.BaseDirectory;

        private List<ResultWindow> ResultWindows { get; }
        
        // Empty constructor Required by WPF
        public QueryToMockDataControl()
        {
            InitializeComponent();

            ResultWindows = new List<ResultWindow>();
        }

        public void Dependencies(
            INameFormatService classService, 
            //IQueryToMockDataService queryToClassService,
            IGeneralDatabaseQueries repository,
            IProfileManager profileManager)
        {
            _svcClass = classService;
            //_svcQueryToMockData = queryToClassService;
            _generalRepo = repository;

            ConnectionStringCb.Dependencies(profileManager, _generalRepo);
        }

        private void TxtSqlSourceText_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (RbSourceTypeTableName.IsChecked != true) return;

            try
            {
                FormatTableName(TxtSourceSqlText);
            }
            catch (Exception ex)
            {
                B.ShowWarningMessage($"The table name you provided could not be formatted.\nPlease select the Query radio button if your source is not just a table name.\n\nError: {ex.Message}");
            }
        }

        private void FormatTableName(TextBox target)
        {
            var strName = target.Text;

            if (string.IsNullOrWhiteSpace(strName))
                return;

            target.Text = _svcClass.FormatTableQuery(strName);
        }
        
        private async void BtnGenerate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //var obj = GetParameters();

                //if (obj == null) return;
                
                PbGenerator.IsIndeterminate = true;

                //var results = await Task.Run(() => _svcQueryToMockData.Generate(obj));

                //foreach (var g in results)
                //{
                //    ShowResultWindow(g.Filename, g.Contents);
                //}
            }
            catch (NonUniqueColumnException nucEx)
            {
                B.ShowWarningMessage(nucEx.Message);
            }
            catch (Exception ex)
            {
                B.ShowErrorMessage(ex);
            }
            finally
            {
                PbGenerator.IsIndeterminate = false;
            }
        }

        private void ShowResultWindow(string title, string contents)
        {
            var win = new ResultWindow(title, contents);

            win.Show();

            ResultWindows.Add(win);
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
    }
}