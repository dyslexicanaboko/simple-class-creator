using System;
using SimpleClassCreator.DataAccess;
using SimpleClassCreator.Services;
using SimpleInjector;

namespace SimpleClassCreatorUI
{    
    static class Program
    {
        [STAThread]
        static void Main()
        {
            var container = Bootstrap();

            // Any additional other configuration, e.g. of your desired MVVM toolkit.

            RunApplication(container);
        }

        private static Container Bootstrap()
        {
            // Create the container as usual.
            var container = new Container();

            // Register your types, for instance:
            container.Register<IGeneralDatabaseQueries, GeneralDatabaseQueries>();
            container.Register<IQueryToClassRepository, QueryToClassRepository>();
            container.Register<IDtoGenerator, DtoGenerator>();
            container.Register<IQueryToClassService, QueryToClassService>();

            // Register your windows and view models:
            container.Register<QueryToClassWindow>();

            container.Verify();

            return container;
        }

        private static void RunApplication(Container container)
        {
            try
            {
                var app = new App();
                //app.InitializeComponent();
                
                var mainWindow = container.GetInstance<QueryToClassWindow>();
                
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
