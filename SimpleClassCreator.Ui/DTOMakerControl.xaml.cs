using Microsoft.Win32;
using SimpleClassCreator.Lib.Models;
using SimpleClassCreator.Lib.Services;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SimpleClassCreator.Ui
{
    /// <summary>
    /// Interaction logic for DTOMakerControl.xaml
    /// </summary>
    public partial class DtoMakerControl : UserControl
    {
        private Brush _dragAndDropTargetBackgroundOriginal;
        private readonly IDtoGenerator _generator;
        
        private string ClassFqdn => TxtFullyQualifiedClassName.Text;

        public DtoMakerControl()
        {
            InitializeComponent();

            //TODO: Need to use Dependency Injection here
            _generator = new DtoGenerator();

            //Lock in what the background color is at start
            _dragAndDropTargetBackgroundOriginal = DragAndDropTarget.Background;
        }

        private void BtnAssemblyOpenDialog_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog();

            var ok = ofd.ShowDialog();

            if (ok.GetValueOrDefault())
                return;

            SetSelectedAssembly(ofd.FileName);
        }

        private void SetSelectedAssembly(string fullFilePath)
        {
            TxtAssemblyFullFilePath.Text = fullFilePath;

            LblAssemblyChosen.Content = Path.GetFileName(fullFilePath);
        }

        private void LblClassName_MouseEnter(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Help;
        }

        private void LblClassName_MouseLeave(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        private void LblClassName_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            string strHelp = 
@"A Fully Qualified Class Name is providing the class name with its namespace separated by periods. 
Example: If you have a class named Product, but it exists in My.Project.Business,
then enter: My.Project.Business.Product, in the text box below. 
Please keep in mind casing matters.";

            MessageBox.Show(strHelp, "What is a Fully Qualified Class Name?");
        }

        private void BtnLoadClass_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _generator.LoadAssembly(TxtAssemblyFullFilePath.Text);

                var asm = _generator.GetClassProperties(ClassFqdn);

                LoadTreeView(asm);
            }
            catch (Exception ex)
            {
                ex.ShowAsErrorMessage();
            }
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

            TvAssembliesAndClasses.Items.Add(asm);
        }

        private StackPanel MakeOption(PropertyInfo info)
        {
            var cbx = new CheckBox();
            cbx.Name = "Cb_" + info.Name;
            cbx.IsChecked = true;

            var lbl = new Label();
            lbl.Name = "Lbl_" + info.Name;
            lbl.Content = info.ToString();

            var sp = new StackPanel();
            sp.Orientation = Orientation.Horizontal;
            sp.Children.Add(cbx);
            sp.Children.Add(lbl);
            
            return sp;
        }

        private DtoMakerParameters GetParametersFromUi()
        {
            //Not every parameter will be in use yet
            var p = new DtoMakerParameters
            {
                IncludeCloneMethod = CbIncludeCloneMethod.IsChecked(),
                IncludeTranslateMethod = CbIncludeTranslateMethod.IsChecked(),
                IncludeIEquatableOfTMethods = CbIncludeIEquatableOfTMethod.IsChecked()
            };

            return p;
        }

        private void BtnGenerate_Click(object sender, RoutedEventArgs e)
        {
            var p = GetParametersFromUi();

            _generator.LoadAssembly(TxtAssemblyFullFilePath.Text);

            var win = new ResultWindow("Dto", _generator.MakeDto(ClassFqdn, p));
            
            win.Show();
        }

        private void DragAndDropTarget_OnDrop(object sender, DragEventArgs e)
        {
            try
            {
                if (!e.Data.GetDataPresent(DataFormats.FileDrop))
                    return;

                var arr = e.Data.GetData(DataFormats.FileDrop) as string[];

                if (arr == null) return;

                SetSelectedAssembly(arr.First());
            }
            catch (Exception ex)
            {
                ex.ShowAsErrorMessage();
            }
        }

        private void DragAndDropTarget_OnMouseEnter(object sender, MouseEventArgs e)
            => DragAndDropTarget.Background = Brushes.Yellow;

        private void DragAndDropTarget_OnMouseLeave(object sender, MouseEventArgs e)
            => DragAndDropTarget.Background = _dragAndDropTargetBackgroundOriginal;
    }
}
