using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarPDFSolutionLibrary.Services.Editors;

namespace Star_PDF_Solution_X.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public CombineFilesViewModel CombineFilesVM { get; }
        public SplitFileViewModel SplitFileVM { get; }

        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set { _errorMessage = value; OnPropertyChanged(); }
        }
        public MainWindowViewModel()
        {
            IPDFEditorService pdfService = new PDFSharpEditorService();
            CombineFilesVM = new(pdfService);
            SplitFileVM = new(pdfService);

            CombineFilesVM.ErrorAdded += ErrorAdded;
            SplitFileVM.ErrorAdded += ErrorAdded;
            ErrorMessage = "";
        }
        private void ClearErrors()
        {
            ErrorMessage = "";
        }
        private ICommand _clearErrorsCommand;
        public ICommand ClearErrorsCommand
        {
            get
            {
                if (_clearErrorsCommand == null)
                    _clearErrorsCommand = new RelayCommand(ClearErrors);
                return _clearErrorsCommand;
            }
        }
        private void ErrorAdded(object? sender, string e)
        {
            if (string.IsNullOrWhiteSpace(ErrorMessage))
                ErrorMessage = e;
            else
                ErrorMessage += $"\n{e}";
        }
    }
}
