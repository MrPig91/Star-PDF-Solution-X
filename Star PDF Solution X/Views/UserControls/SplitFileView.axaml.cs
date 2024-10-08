using System.Collections.Generic;
using System.Linq;
using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Star_PDF_Solution_X.ViewModels;

namespace Star_PDF_Solution_X;

public partial class SplitFileView : UserControl
{
    public SplitFileView()
    {
        InitializeComponent();
        AddHandler(DragDrop.DragEnterEvent, Grid_DragEnter);
        AddHandler(DragDrop.DragLeaveEvent, Grid_DragLeave);
        AddHandler(DragDrop.DropEvent, Grid_Drop);
    }

    private void Grid_Drop(object? sender, DragEventArgs e)
    {
        try
        {
            var files = e.Data.GetFiles();

            if (files?.Count() > 0)
            {
                var viewModel = this.DataContext as SplitFileViewModel;
                if (viewModel is null)
                    return;
                string fileName = string.Empty;
                List<string> pdfsToSplit = new();
                files.Where(f => f.Path.LocalPath.EndsWith(".pdf")).ToList().ForEach(f => pdfsToSplit.Add(f.Path.LocalPath));

                if (pdfsToSplit.Count > 0)
                {
                    viewModel.SourceFilePaths.Clear();
                    pdfsToSplit.ForEach(f => viewModel.SourceFilePaths.Add(f));
                    viewModel.SplitFileCommand.Execute(null);
                }
            }
        }
        catch (Exception ex) { }

        dragDropBorder.IsVisible = false;
    }

    private void Grid_DragEnter(object? sender, DragEventArgs e)
    {
        dragDropBorder.IsVisible = true;
        dragDropBorder.Classes.Add("DragEnter");
    }

    private void Grid_DragLeave(object? sender, DragEventArgs e)
    {
        dragDropBorder.IsVisible = false;
        dragDropBorder.Classes.Remove("DragEnter");
    }
}