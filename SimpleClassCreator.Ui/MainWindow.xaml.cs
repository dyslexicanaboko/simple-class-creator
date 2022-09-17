using SimpleClassCreator.Lib.DataAccess;
using SimpleClassCreator.Lib.Services;
using SimpleClassCreator.Ui.Helpers;
using SimpleClassCreator.Ui.Profile;
using SimpleClassCreator.Ui.Services;
using System.Collections.Generic;
using System.Windows;
using SimpleClassCreator.Lib.Services.CodeFactory;

namespace SimpleClassCreator.Ui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
        : Window
    {
        private readonly List<IUsesResultWindow> _hasResultWindows;

        public MainWindow(INameFormatService classService,
            IQueryToClassService queryToClassService,
            IQueryToMockDataService queryToMockDataService,
            IGeneralDatabaseQueries repository,
            IProfileManager profileManager,
            IDtoGenerator dtoGenerator,
            IMetaViewModelService metaViewModelService,
            ICSharpCompilerService compilerService)
        {
            InitializeComponent();

            //I am probably doing this wrong, but I don't care right now. I will have to circle back and try to do it right later.
            //The MVVM model seems like a lot of extra unnecessary work.
            CtrlQueryToClass.Dependencies(
                classService,
                queryToClassService,
                repository,
                profileManager);

            CtrlQueryToMockData.Dependencies(
                classService,
                queryToMockDataService,
                repository,
                profileManager);

            CtrlDtoMaker.Dependencies(
                dtoGenerator,
                metaViewModelService,
                queryToClassService,
                compilerService);

            _hasResultWindows = new List<IUsesResultWindow>
            {
                CtrlQueryToMockData,
                CtrlDtoMaker,
                CtrlQueryToClass
            };
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
            =>_hasResultWindows.ForEach(x => x.CloseResultWindows());

        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {
            using (var obj = new AboutSimpleClassCreator())
            {
                obj.ShowDialog();
            }
        }
    }
}
