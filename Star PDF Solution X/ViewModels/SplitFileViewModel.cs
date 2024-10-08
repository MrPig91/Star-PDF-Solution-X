using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using StarPDFSolutionLibrary.Models;
using StarPDFSolutionLibrary.Services.Editors;
using System.Windows.Input;
using System.IO;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using Avalonia;
using Star_PDF_Solution_X.Utilities;

namespace Star_PDF_Solution_X.ViewModels
{
    public class SplitFileViewModel : ViewModelBase
    {
        public event EventHandler<string> ErrorAdded;
        private IPDFEditorService _pdfEditorService;
        private Progress<Tuple<StarPDFDocument, double>> _progressUpdater = new();
        public ObservableCollection<string> SourceFilePaths { get; } = new();
        private string? _selectedSourceFilePath;
        public string? SelectedSourceFilePath
        {
            get => _selectedSourceFilePath;
            set { _selectedSourceFilePath = value; OnPropertyChanged(); }
        }
        public ObservableCollection<StarPDFDocumentViewModel> OutputFiles { get; } = new();
        public List<string> Errors { get; } = new();
        public SplitFileOptionsViewModel Options { get; } = new();
        private double? _progress;

        public double? Progress
        {
            get { return _progress; }
            set { _progress = value; OnPropertyChanged(); }
        }
        private double? _multiFileProgress;
        public double? MultiFileProgress
        {
            get => _multiFileProgress;
            set { _multiFileProgress = value; OnPropertyChanged(); }
        }
        public void AddError(string error)
        {
            Errors.Add(error);
            ErrorAdded?.Invoke(this, error);
        }
        private void Clear()
        {
            SourceFilePaths.Clear();
            OutputFiles.Clear();
        }
        private ICommand _clearCommand;
        public ICommand ClearCommand
        {
            get
            {
                if (_clearCommand is null)
                    _clearCommand = new RelayCommand(Clear);
                return _clearCommand;
            }
        }

        private async void SelectFiles()
        {
            try
            {
                SourceFilePaths.Clear();
                var files = await FileSelectorUtility.SelectFiles();

                if (files is null)
                    return;

                foreach (var file in files)
                    SourceFilePaths.Add(new(file.Path.LocalPath));
            }
            catch (Exception ex) { AddError(ex.Message); }
        }
        private ICommand? _selectFilesCommand;
        public ICommand SelectFilesCommand
        {
            get
            {
                if (_selectFilesCommand == null)
                    _selectFilesCommand = new RelayCommand(SelectFiles);
                return _selectFilesCommand;
            }
        }

        private async void SplitFile()
        {
            try
            {
                var sourceFiles = SourceFilePaths;
                double completeFileCount = 0;
                OutputFiles.Clear();

                if (SourceFilePaths.Count == 0)
                {
                    SourceFilePaths.Clear();
                    var files = await FileSelectorUtility.SelectFiles();

                    if (files is null)
                        return;

                    foreach (var file in files)
                        SourceFilePaths.Add(new(file.Path.LocalPath));

                    if (SourceFilePaths.Count == 0)
                        return;
                }

                if (SourceFilePaths.Count() > 1)
                {
                    MultiFileProgress = 0;
                }

                foreach (var sourceFile in SourceFilePaths)
                {
                    SelectedSourceFilePath = sourceFile;
                    var outputfiles = await _pdfEditorService.SplitAsync(sourceFile, options: Options.GetPDFOptions(), progress: _progressUpdater);

                    if (Options.OpenDestinationDirectory)
                        Process.Start(new ProcessStartInfo(Path.GetDirectoryName(outputfiles.First().FilePath)) { UseShellExecute = true });
                    if (Options.DeleteSourceFile)
                        File.Delete(sourceFile);

                    completeFileCount++;
                    if (MultiFileProgress is not null)
                        MultiFileProgress = completeFileCount / (double)SourceFilePaths.Count;
                }

                if (Options.OpenDestinationDirectory && OutputFiles.Count > 0)
                    Process.Start(new ProcessStartInfo(Path.GetDirectoryName(OutputFiles.First().FilePath)) { UseShellExecute = true });

                Progress = null;
                MultiFileProgress = null;
            }
            catch (Exception ex) { AddError(ex.Message); }
        }

        private ICommand? _splitFileCommand;
        public ICommand SplitFileCommand
        {
            get
            {
                if (_splitFileCommand == null)
                    _splitFileCommand = new RelayCommand(SplitFile);
                return _splitFileCommand;
            }
        }

        public SplitFileViewModel(IPDFEditorService pdfEditorService)
        {
            _pdfEditorService = pdfEditorService;
            _progressUpdater.ProgressChanged += _progressUpdater_ProgressChanged;
        }

        public SplitFileViewModel() { }

        private void _progressUpdater_ProgressChanged(object? sender, Tuple<StarPDFDocument, double> e)
        {
            Progress = e.Item2;
            OutputFiles.Add(new(e.Item1));
        }
    }
}
