using CommunityToolkit.Mvvm.ComponentModel;

namespace Star_PDF_Solution_X.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string _greeting;

        public MainWindowViewModel()
        {
            Greeting = "Hellow World!";
        }
    }
}
