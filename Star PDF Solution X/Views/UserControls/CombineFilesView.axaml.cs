using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;

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
}