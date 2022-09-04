using Microsoft.Win32;
using SimpleClassCreator.Lib.Models;
using SimpleClassCreator.Lib.Services;
using SimpleClassCreator.Ui.Helpers;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using SimpleClassCreator.Lib.Models.Meta;
using SimpleClassCreator.Ui.Services;
using SimpleClassCreator.Ui.ViewModels;

namespace SimpleClassCreator.Ui
{
    /// <summary>
    /// Interaction logic for DTOMakerControl.xaml
    /// </summary>
    public partial class DtoMakerControl : UserControl, IUsesResultWindow
    {
        private const string GhostText = "Caution: Everything is loaded by default...";
        private readonly ResultWindowManager _resultWindowManager;
        private readonly Brush _dragAndDropTargetBackgroundOriginal;
        private IDtoGenerator _generator;
        
        public DtoMakerControl()
        {
            InitializeComponent();

            //Lock in what the background color is at start
            _dragAndDropTargetBackgroundOriginal = DragAndDropTarget.Background;

            _resultWindowManager = new ResultWindowManager();

            TxtFullyQualifiedClassName.Text = GhostText;
        }

        private void BtnAssemblyOpenDialog_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog();

            var ok = ofd.ShowDialog();

            if (!ok.GetValueOrDefault())
                return;

            SetSelectedAssembly(ofd.FileName);
        }

        internal void Dependencies(IDtoGenerator generator)
        {
            _generator = generator;
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
            var strHelp = 
@"A Fully Qualified Class Name is providing the class name with its namespace separated by periods. 
Example: If you have a class named Product, but it exists in My.Project.Business,
then enter: My.Project.Business.Product, in the text box below. 
Please keep in mind casing matters.";

            MessageBox.Show(strHelp, "What is a Fully Qualified Class Name?");
        }

        private void CheckFqdnForGhostText()
        {
            if (TxtFullyQualifiedClassName.Text == GhostText) TxtFullyQualifiedClassName.Clear();
        }

        private void BtnLoadClass_Click(object sender, RoutedEventArgs e)
        {
            CheckFqdnForGhostText();

            LoadClass();
        }

        private void LoadClass()
        {
            try
            {
                _generator.LoadAssembly(TxtAssemblyFullFilePath.Text);

                var asmData = string.IsNullOrWhiteSpace(TxtFullyQualifiedClassName.Text) ?
                    _generator.GetListOfClasses() :
                    _generator.GetClassProperties(TxtFullyQualifiedClassName.Text);

                var asmViewModel = new MetaViewModelService().ToViewModel(asmData);

                //Everything is loaded via XAML bindings
                TvAssembliesAndClasses.ItemsSource = new ObservableCollection<MetaAssemblyViewModel> { asmViewModel };
                TvAssembliesAndClasses.Focus();
            }
            catch (Exception ex)
            {
                ex.ShowAsErrorMessage();
            }
        }

        private void TreeViewClasses_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var tvi = e.Source as TreeViewItem;

            if (!(tvi?.Header is IMetaClass c)) return;

            TxtFullyQualifiedClassName.Text = c.FullName;

            LoadClass();
        }

        //TODO: Many to dos
        /*   primitives, dark blue -> RGB  86, 156, 214 - #569cd6 - visual studio keyword
         *   classes   , teal      -> RGB  78, 201, 176 - #4ec9b0 - estimated
         *   Interfaces, grnyellow -> RGB 184, 215, 163 - #b8d7a3 - estimated
         *   enums     , grnyellow -> RGB 184, 215, 163 - #b8d7a3 - estimated
         *
         * Select/Deselect all - Have to make a shift over to an observable collection among other things
         * Select multiple using CTRL and SHIFT keys as normal
         * Generate DTO button should read from the Tree View to take full advantage of it
         * 
         * Drag and drop still doesn't work. Something is blocking it from happening.
         * 
         * How to handle large assemblies?
         * This should be asynchronous with a way to cancel the task
         * Progress bar can be shown if any of this is measurable
         * 
         * Extract interface option. What other options make sense? */
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

            var win = new ResultWindow("Dto", _generator.MakeDto(TxtFullyQualifiedClassName.Text, p));
            
            win.Show();

            _resultWindowManager.Add(win);
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

        public void CloseResultWindows() => _resultWindowManager.CloseAll();

        private void BtnHelp_OnClick(object sender, RoutedEventArgs e)
            => LblClassName_MouseDoubleClick(sender, null);

        private void BtnResetTree_OnClick(object sender, RoutedEventArgs e)
        {
            TxtFullyQualifiedClassName.Text = GhostText;
            
            TvAssembliesAndClasses.ItemsSource = Array.Empty<MetaAssembly>();
        }

        private void TxtFullyQualifiedClassName_OnGotFocus(object sender, RoutedEventArgs e)
            => CheckFqdnForGhostText();

        private void CbPropertiesSelectAll_OnChecked(object sender, RoutedEventArgs e)
        {
            if (true) ;
        }

        private void CbPropertiesSelectAll_OnUnchecked(object sender, RoutedEventArgs e)
        {
            if (true) ;
        }
    }
}
