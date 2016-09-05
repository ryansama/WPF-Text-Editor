using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;

namespace TextEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public string CurrentFilePath;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenCmdExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            //MessageBox.Show("The open command has been invoked on target object.");
            Stream openFileStream = Stream.Null;
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                InitialDirectory = "C:\\",
                Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*",
                FilterIndex = 2,
                RestoreDirectory = true
            };


            bool? userClickedOK = openFileDialog.ShowDialog();

            if (userClickedOK == true)
            {
                try
                {
                    if ((openFileStream = openFileDialog.OpenFile()) != null)
                    {
                        using (openFileStream)
                        {
                            EditableTextBox.Text = File.ReadAllText(openFileDialog.FileName);
                        }
                        CurrentFilePath = openFileDialog.FileName;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }

            TextEditorWindow.Title = "Text Editor - " + CurrentFilePath;

        }

        private void OpenCmdCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void SaveCmdExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            //Check if file already exists
            if (CurrentFilePath == null)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    FileName = "Document",
                    DefaultExt = ".txt",
                    Filter = "Text documents (.txt)|*.txt"
                };

                bool? userClickedOK = saveFileDialog.ShowDialog();

                if (userClickedOK == true)
                {
                    CurrentFilePath = saveFileDialog.FileName;
                    File.WriteAllText(CurrentFilePath, EditableTextBox.Text);
                }
            }
            else
            {
                File.WriteAllText(CurrentFilePath, EditableTextBox.Text);
            }

            TextEditorWindow.Title = "Text Editor - " + CurrentFilePath;
        }

        private void SaveCmdCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void NewCmdExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            CurrentFilePath = null;
            EditableTextBox.Text = String.Empty;
            TextEditorWindow.Title = "Text Editor - (new document)";
        }

        private void NewCmdCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
    }
}
