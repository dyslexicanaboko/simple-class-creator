using SimpleClassCreator.Lib.Models;
using SimpleClassCreator.Lib.Services;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SimpleClassCreator.Ui
{
    /// <summary>
    /// Interaction logic for DTOMakerControl.xaml
    /// </summary>
    public partial class DtoMakerControl : UserControl
    {
        private readonly IDtoGenerator _generator;
        
        private string AssemblyFullPath => txtAssemblyFullFilePath.Text;
        
        private string ClassFqdn => txtFullyQualifiedClassName.Text;

        public DtoMakerControl()
        {
            InitializeComponent();

            //TODO: Need to use Dependency Injection here
            _generator = new DtoGenerator();
        }

        private void btnAssemblyOpenDialog_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();

            if (!ofd.ShowDialog().Value)
                return;

            txtAssemblyFullFilePath.Text = ofd.FileName;

            lblAssemblyChosen.Content = System.IO.Path.GetFileName(ofd.FileName);
        }

        private void lblClassName_MouseEnter(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Help;
        }

        private void lblClassName_MouseLeave(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        private void lblClassName_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            string strHelp = 
@"A Fully Qualified Class Name is providing the class name with its namespace separated by periods. 
Example: If you have a class named Product, but it exists in My.Project.Business,
then enter: My.Project.Business.Product, in the text box below. 
Please keep in mind casing matters.";

            MessageBox.Show(strHelp, "What is a Fully Qualified Class Name?");
        }

        private void btnLoadClass_Click(object sender, RoutedEventArgs e)
        {
            _generator.LoadAssembly(AssemblyFullPath);

            var asm = _generator.GetClassProperties(ClassFqdn);

            LoadTreeView(asm);
        }

        private void LoadTreeView(AssemblyInfo assembly)
        { 
            var asm = new TreeViewItem();
            
            asm.Header = assembly.Name;

            foreach(var ci in assembly.Classes)
            {
                var cls = new TreeViewItem();
                cls.Header = ci.FullName;
                cls.IsExpanded = true;

                foreach(var pi in ci.Properties)
                    cls.Items.Add(MakeOption(pi));

                asm.Items.Add(cls);
            }

            tvAssebliesAndClasses.Items.Add(asm);
        }

        private StackPanel MakeOption(PropertyInfo info)
        {
            var cbx = new CheckBox();
            cbx.Name = "cbx_" + info.Name;
            cbx.IsChecked = true;

            var lbl = new Label();
            lbl.Name = "lbl_" + info.Name;
            lbl.Content = info.ToString();

            var sp = new StackPanel();
            sp.Orientation = Orientation.Horizontal;
            sp.Children.Add(cbx);
            sp.Children.Add(lbl);
            
            return sp;
        }

        private ClassParameters GetParametersFromUi()
        {
            //Not every parameter will be in use yet
            var p = new ClassParameters
            {
                IncludeCloneMethod = GetValue(cbxIncludeCloneMethod),
                ExcludeCollections = GetValue(cbxExcludeCollections),
                IncludeTranslateMethod = GetValue(cbxIncludeTranslateMethod),
                IncludeIEquatableOfTMethods = GetValue(cbxIncludeIEquatableOfTMethod)
            };

            return p;
        }

        private bool GetValue(CheckBox cb)
        {
            var value = cb.IsChecked.GetValueOrDefault();

            return value;
        }

        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            var p = GetParametersFromUi();

            _generator.LoadAssembly(AssemblyFullPath);

            var win = new ResultWindow(_generator.MakeDto(ClassFqdn, p));
            
            win.Show();
        }
    }
}
