using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;

namespace Star_PDF_Solution_X.Utilities
{
    public static class FileSelectorUtility
    {
        public static async Task<IReadOnlyList<IStorageFile>?> SelectFiles()
        {
            if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop ||
                desktop.MainWindow?.StorageProvider is not { } provider)
                throw new NullReferenceException("Missing StorageProvider instance.");

            var files = await provider.OpenFilePickerAsync(new FilePickerOpenOptions()
            {
                Title = "Open Text File",
                AllowMultiple = true,
                FileTypeFilter = new[] { FilePickerFileTypes.Pdf }
            });

            return files;
        }
    }
}
