using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Star_PDF_Solution_X.ViewModels
{
    public class SplitFileOptionsViewModel : ViewModelBase
    {
        public SplitPDFOptions GetPDFOptions()
        {
            var splitOptions = SplitPDFOptions.None;
            if (_removeComments)
                splitOptions = splitOptions.AddFlag(SplitPDFOptions.RemoveComments);
            if (_removeBookmarks)
                splitOptions = splitOptions.AddFlag(SplitPDFOptions.RemoveBookmarks);
            return splitOptions;
        }

        private bool _openDestinationDirectory;
        public bool OpenDestinationDirectory
        {
            get { return _openDestinationDirectory; }
            set { _openDestinationDirectory = value; OnPropertyChanged(); }
        }
        private bool _deleteSourceFile;
        public bool DeleteSourceFile
        {
            get { return _deleteSourceFile; }
            set { _deleteSourceFile = value; OnPropertyChanged(); }
        }
        private bool _removeComments;
        public bool RemoveComments
        {
            get => _removeComments;
            set { _removeComments = value; OnPropertyChanged(); }
        }
        private bool _removeBookmarks;
        public bool RemoveBookmarks
        {
            get { return _removeBookmarks; }
            set { _removeBookmarks = value; }
        }


        public SplitFileOptionsViewModel()
        {
            OpenDestinationDirectory = true;
            DeleteSourceFile = false;
            RemoveComments = false;
        }
    }
}
