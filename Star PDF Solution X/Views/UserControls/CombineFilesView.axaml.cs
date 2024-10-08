using System.Collections.Generic;
using System.Linq;
using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Star_PDF_Solution_X.ViewModels;

namespace Star_PDF_Solution_X;

public partial class CombineFilesView : UserControl
{
    public CombineFilesView()
    {
        InitializeComponent();
        AddHandler(DragDrop.DragEnterEvent, Grid_DragEnter);
        AddHandler(DragDrop.DragLeaveEvent, Grid_DragLeave);
        combineFilesBorder.AddHandler(DragDrop.DragEnterEvent, dragDropBorder_DragEnter);
        combineFilesBorder.AddHandler(DragDrop.DragLeaveEvent, dragDropBorder_DragLeave);
        addFilesBorder.AddHandler(DragDrop.DragEnterEvent, dragDropBorder_DragEnter);
        addFilesBorder.AddHandler(DragDrop.DragLeaveEvent, dragDropBorder_DragLeave);

        combineFilesBorder.AddHandler(DragDrop.DropEvent, combineFilesBorder_Drop);
        addFilesBorder.AddHandler(DragDrop.DropEvent, addFilesBorder_Drop);
    }

    private void Grid_DragEnter(object? sender, DragEventArgs e)
    {
        dragDropGrid.IsVisible = true;
        addFilesBorder.IsVisible = true;
        combineFilesBorder.IsVisible = true;
    }
    private void dragDropBorder_DragEnter(object? sender, DragEventArgs e)
    {
        if (sender is not null && sender is Border)
        {
            var border = sender as Border;
            border.Classes.Remove("DragLeave");
            border.Classes.Add("DragEnter");
        }
    }
    private void dragDropBorder_DragLeave(object? sender, DragEventArgs e)
    {
        if (sender is not null && sender is Border)
        {
            var border = sender as Border;
            border.Classes.Remove("DragEnter");
            border.Classes.Add("DragLeave");
        }
    }

    private void Grid_DragLeave(object? sender, DragEventArgs e)
    {
        dragDropGrid.IsVisible = false;
        addFilesBorder.IsVisible = false;
        combineFilesBorder.IsVisible = false;
    }

    private void addFilesBorder_Drop(object? sender, DragEventArgs e)
    {
        try
        {
            var files = e.Data.GetFiles();

            if (files?.Count() > 0)
            {
                var viewModel = this.DataContext as CombineFilesViewModel;
                if (viewModel is null)
                    return;
                string fileName = string.Empty;
                List<string> pdfsToAdd = new();
                files.Where(f => f.Path.LocalPath.EndsWith(".pdf")).ToList().ForEach(f => pdfsToAdd.Add(f.Path.LocalPath));

                if (pdfsToAdd.Count > 0)
                {
                    foreach (var file in pdfsToAdd)
                    {
                        viewModel.SourceFiles.Add(new(file));
                    }
                    viewModel.SelectedSourceFile = viewModel.SourceFiles.FirstOrDefault();
                }
            }

            dragDropGrid.IsVisible = false;
        }
        catch (Exception ex) { }
    }

    private void combineFilesBorder_Drop(object? sender, DragEventArgs e)
    {
        try
        {
            var files = e.Data.GetFiles();

            if (files?.Count() > 0)
            {
                var viewModel = this.DataContext as CombineFilesViewModel;
                if (viewModel is null)
                    return;
                string fileName = string.Empty;
                List<string> pdfsToCombine = new();
                files.Where(f => f.Path.LocalPath.EndsWith(".pdf")).ToList().ForEach(f => pdfsToCombine.Add(f.Path.LocalPath));
                if (pdfsToCombine.Count > 0)
                {
                    viewModel.SourceFiles.Clear();
                    foreach (var file in pdfsToCombine)
                    {
                        viewModel.SourceFiles.Add(new(file));
                    }
                    viewModel.SelectedSourceFile = viewModel.SourceFiles.FirstOrDefault();
                    viewModel.NaturalSortCommand.Execute(null);
                    viewModel.CombineFilesCommand.Execute(null);
                }
            }
            dragDropGrid.IsVisible = false;
        }
        catch (Exception ex) { }
    }
}