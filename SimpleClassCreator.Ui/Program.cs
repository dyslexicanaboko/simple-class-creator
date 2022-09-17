using SimpleClassCreator.Lib.DataAccess;
using SimpleClassCreator.Lib.Services;
using SimpleClassCreator.Lib.Services.CodeFactory;
using SimpleClassCreator.Ui.Profile;
using SimpleClassCreator.Ui.Services;
using SimpleInjector;
using System;

namespace SimpleClassCreator.Ui
{
    static class Program
    {
        private static ProfileSaver _profileSaver;

        [STAThread]
        static void Main()
        {
            _profileSaver = new ProfileSaver();

            var container = Bootstrap();

            // Any additional other configuration, e.g. of your desired MVVM toolkit.

            RunApplication(container);
        }

        private static Container Bootstrap()
        {
            // Create the container as usual.
            var container = new Container();

            // Register your types, for instance:
            container.Register<IMetaViewModelService, MetaViewModelService>();
            container.Register<IGeneralDatabaseQueries, GeneralDatabaseQueries>();
            container.Register<IQueryToClassRepository, QueryToClassRepository>();
            container.Register<IDtoGenerator, DtoGenerator>();
            container.Register<INameFormatService, NameFormatService>();
            container.Register<IQueryToClassService, QueryToClassService>();
            container.Register<IQueryToMockDataService, QueryToMockDataService>();
            container.Register<IProfileManager>(GetProfileManager, Lifestyle.Singleton);
            container.Register<ICSharpCompilerService, CSharpCompilerService>();

            // Register your windows and view models:
            //container.Register<DtoMakerControl>();
            //container.Register<QueryToClassControl>();
            container.Register<MainWindow>();

            container.Verify();

            return container;
        }

        private static ProfileManager GetProfileManager()
        {
            var profileManager = _profileSaver.Load();

            return profileManager;
        }

        private static void RunApplication(Container container)
        {
            try
            {
                var app = new App();
                //app.InitializeComponent();
                
                var mainWindow = container.GetInstance<MainWindow>();
                
                app.Run(mainWindow);
            }
            catch (Exception ex)
            {
                //Log the exception and exit
                if (true) ;
            }
        }
    }
}
