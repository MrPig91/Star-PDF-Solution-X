using CommunityToolkit.Mvvm.ComponentModel;
using StarPDFSolutionLibrary.Services.Editors;

namespace Star_PDF_Solution_X.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public CombineFilesViewModel CombineFilesVM { get; }
        public SplitFileViewModel SplitFileVM { get; }
        public MainWindowViewModel()
        {
            IPDFEditorService pdfService = new PDFSharpEditorService();
            CombineFilesVM = new(pdfService);
            SplitFileVM = new(pdfService);
        }
    }
}
