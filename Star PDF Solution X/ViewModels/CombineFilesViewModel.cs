﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.Input;
using StarPDFSolutionLibrary.Services.Editors;
using System.Windows.Input;
using System.IO;
using Star_PDF_Solution_X.Utilities;
using Star_PDF_Solution_X.Extensions;

namespace Star_PDF_Solution_X.ViewModels
{
    public class CombineFilesViewModel : ViewModelBase
    {
        public event EventHandler<string> ErrorAdded;
        private IPDFEditorService _pdfEditorService;
        private Progress<double> _progressUpdater = new();

        public bool IsNaturalSortEnabled;
        public ObservableCollection<StarPDFDocumentViewModel> SourceFiles { get; } = new();
        private StarPDFDocumentViewModel _selectedSourceFile;
        public StarPDFDocumentViewModel SelectedSourceFile
        {
            get { return _selectedSourceFile; }
            set { _selectedSourceFile = value; OnPropertyChanged(); }
        }

        private StarPDFDocumentViewModel? _outputFile;
        public StarPDFDocumentViewModel? OutputFile
        {
            get => _outputFile;
            set { _outputFile = value; OnPropertyChanged(); }
        }

        private double? _progress;

        public double? Progress
        {
            get { return _progress; }
            set { _progress = value; OnPropertyChanged(); }
        }
        public List<string> Errors { get; } = new();

        public CombineFilesOptionsViewModel Options { get; } = new();
        public void AddError(string error)
        {
            Errors.Add(error);
            ErrorAdded?.Invoke(this, error);
        }
        private async void SelectFiles()
        {
            try
            {
                SourceFiles.Clear();
                var files = await FileSelectorUtility.SelectFiles();

                if (files is null)
                    return;

                foreach (var file in files)
                    SourceFiles.Add(new(file.Path.LocalPath));
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
        private async void CombineFiles()
        {
            try
            {
                if (SourceFiles.Count == 0)
                {
                    SourceFiles.Clear();
                    var files = await FileSelectorUtility.SelectFiles();

                    if (files is null)
                        return;

                    foreach (var file in files)
                        SourceFiles.Add(new(file.Path.LocalPath));

                    if (SourceFiles.Count == 0)
                        return;
                }
                var filePaths = new List<string>();
                foreach (StarPDFDocumentViewModel file in SourceFiles)
                    filePaths.Add(file.FilePath);

                OutputFile = new(await _pdfEditorService.CombineFilesAsync(filePaths, options: Options.GetPDFOptions(), progress: _progressUpdater));
                if (Options.OpenFile)
                    Process.Start(new ProcessStartInfo(OutputFile.FilePath) { UseShellExecute = true });
                if (Options.DeleteSourceFiles)
                {
                    foreach (var file in SourceFiles)
                        File.Delete(file.FilePath);

                    SourceFiles.Clear();
                }
                Progress = null;
            }
            catch (Exception ex) { AddError(ex.Message); }
        }

        private ICommand _combineFilesCommand;
        public ICommand CombineFilesCommand
        {
            get
            {
                if (_combineFilesCommand == null)
                    _combineFilesCommand = new RelayCommand(CombineFiles);
                return _combineFilesCommand;
            }
        }
        public void MoveUp()
        {
            try
            {
                if (SelectedSourceFile is null)
                    return;

                var oldIndex = SourceFiles.IndexOf(SelectedSourceFile);
                var newIndex = oldIndex - 1;
                if (oldIndex == 0)
                    return;
                SourceFiles.Move(oldIndex, newIndex);

                SelectedSourceFile = SourceFiles[newIndex];
            }
            catch (Exception ex) { AddError(ex.Message); }
        }
        private ICommand _moveUpCommand;

        public ICommand MoveUpCommand
        {
            get
            {
                if (_moveUpCommand == null)
                    _moveUpCommand = new RelayCommand(MoveUp);
                return _moveUpCommand;
            }
        }

        public void MoveDown()
        {
            try
            {
                if (SelectedSourceFile is null)
                    return;
                var oldIndex = SourceFiles.IndexOf(SelectedSourceFile);
                var newIndex = oldIndex + 1;
                if (oldIndex == SourceFiles.Count - 1)
                    return;
                SourceFiles.Move(oldIndex, newIndex);
                SelectedSourceFile = SourceFiles[newIndex];
            }
            catch (Exception ex) { AddError(ex.Message); }
        }
        private ICommand _moveDownCommand;

        public ICommand MoveDownCommand
        {
            get
            {
                if (_moveDownCommand == null)
                    _moveDownCommand = new RelayCommand(MoveDown);
                return _moveDownCommand;
            }
        }
        private void NaturalSort()
        {
            try
            {
                SourceFiles.Sort();
            }
            catch (Exception ex) { AddError(ex.Message); }
        }
        private ICommand _naturalSortCommand;
        public ICommand NaturalSortCommand
        {
            get
            {
                if (_naturalSortCommand is null)
                    _naturalSortCommand = new RelayCommand(NaturalSort);
                return _naturalSortCommand;
            }
        }
        public void Remove()
        {
            try
            {
                if (SelectedSourceFile is null)
                    return;
                var index = SourceFiles.IndexOf(SelectedSourceFile);
                if (SourceFiles.Contains(SelectedSourceFile))
                    SourceFiles.Remove(SelectedSourceFile);

                index = index == 0 ? 0 : index - 1;
                SelectedSourceFile = SourceFiles[index];
            }
            catch (Exception ex) { AddError(ex.Message); }
        }
        private ICommand _removeCommand;
        public ICommand RemoveCommand
        {
            get
            {
                if (_removeCommand is null)
                    _removeCommand = new RelayCommand(Remove);
                return _removeCommand;
            }
        }
        public void Clear()
        {
            try
            {
                SourceFiles.Clear();
            }
            catch (Exception ex) { AddError(ex.Message); }
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

        private void OpenFile()
        {
            try
            {
                if (File.Exists(OutputFile?.FilePath) == false)
                    return;
                Process.Start(new ProcessStartInfo(OutputFile.FilePath) { UseShellExecute = true });
            }
            catch (Exception ex) { AddError(ex.Message); }
        }
        private ICommand _openFileCommand;
        public ICommand OpenFileCommand
        {
            get
            {
                if (_openFileCommand is null)
                    _openFileCommand = new RelayCommand(OpenFile);
                return _openFileCommand;
            }
        }

        public CombineFilesViewModel(IPDFEditorService pdfEditorService)
        {
            _pdfEditorService = pdfEditorService;
        }

        public CombineFilesViewModel() { }
    }
}
