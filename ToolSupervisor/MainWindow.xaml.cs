using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using ToolSupervisor.Utilities;

namespace ToolSupervisor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Private Fields

        private ObservableCollection<ManagedTool> managedTools;
        private DispatcherTimer refreshTimer;

        #endregion

        #region Constructor

        public MainWindow()
        {
            InitializeComponent();
            InitializeTools();
            InitializeTimer();

            this.Closing += (o, a) => { StorageHelper.SaveTools(managedTools.ToList()); };
        }

        #endregion

        #region Event Handlers

        private void btn_refresh_Click(object sender, RoutedEventArgs e)
        {
            RefreshTools();
        }

        private void btn_turnOn_Click(object sender, RoutedEventArgs e)
        {
            var tool = listBox_tools.SelectedItem as ManagedTool;

            ProcessHelper.StartProcess(tool.Path);
        }

        private void btn_turnOff_Click(object sender, RoutedEventArgs e)
        {
            var tool = listBox_tools.SelectedItem as ManagedTool;

            ProcessHelper.KillProcess(tool.Title);
        }

        private void btn_turnOnAll_Click(object sender, RoutedEventArgs e)
        {
            foreach (var tool in managedTools)
                ProcessHelper.StartProcess(tool.Path);
        }

        private void btn_turnOffAll_Click(object sender, RoutedEventArgs e)
        {
            foreach (var tool in managedTools)
                ProcessHelper.KillProcess(tool.Path);
        }

        private void btn_removeOne_Click(object sender, RoutedEventArgs e)
        {
            managedTools.Remove(listBox_tools.SelectedItem as ManagedTool);
            StorageHelper.SaveTools(managedTools.ToList());
        }

        private void btn_removeAll_Click(object sender, RoutedEventArgs e)
        {
            managedTools.Clear();
            StorageHelper.SaveTools(managedTools.ToList());
        }

        private void listBox_tools_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ProcessHelper.StartProcess(System.IO.Path.GetDirectoryName((listBox_tools.SelectedItem as ManagedTool).Path));
        }


        private void listBox_tools_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                AddToManagedTools(files[0]);
            }
        }

        private void listBox_tools_DragOver(object sender, DragEventArgs e)
        {
            bool dropEnabled = true;

            if (e.Data.GetDataPresent(DataFormats.FileDrop, true))
            {
                string[] filenames = e.Data.GetData(DataFormats.FileDrop, true) as string[];

                dropEnabled = filenames.All(fn => System.IO.Path.GetExtension(fn).Equals(".exe", StringComparison.OrdinalIgnoreCase));
            }
            else
            {
                dropEnabled = false;
            }

            if (!dropEnabled)
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;
            }
        }

        #endregion

        #region Private Methods

        private void InitializeTimer()
        {
            refreshTimer = new DispatcherTimer();
            refreshTimer.Interval = TimeSpan.FromSeconds(5);
            refreshTimer.Tick += (o, a) => RefreshTools();
            refreshTimer.Start();
        }

        private void InitializeTools()
        {
            managedTools = new ObservableCollection<ManagedTool>(StorageHelper.LoadTools());
            listBox_tools.ItemsSource = managedTools;
        }

        private void RefreshTools()
        {
            foreach (var tool in managedTools)
            {
                if (File.Exists(tool.Path))
                {
                    if (ProcessHelper.IsProcessRunning(tool.Filename))
                        tool.Status = ToolStatus.Running;
                    else
                        tool.Status = ToolStatus.Stopped;
                }
                else
                    tool.Status = ToolStatus.NotFound;
            }
        }

        private void AddToManagedTools(string filePath)
        {
            managedTools.Add(new ManagedTool()
            {
                Title = System.IO.Path.GetFileNameWithoutExtension(filePath),
                Path = filePath,
                Filename = System.IO.Path.GetFileName(filePath),
                Status = ToolStatus.Unknown
            });

            StorageHelper.SaveTools(managedTools.ToList());
        }

        #endregion
    }
}
