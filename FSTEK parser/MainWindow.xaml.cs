using FSTEK_parser.Model;
using FSTEK_parser.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FSTEK_parser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string PATH = $"{Environment.CurrentDirectory}\\thrlist.xlsx";

        private FileDownload _downloadService;
        private FileIOService _fileIOservice;
        private PagingCollectionView _view;

        public MainWindow()
        {
            InitializeComponent();
        }
        private void OnNextClicked(object sender, RoutedEventArgs e)
        {
            this._view.MoveToNextPage();
        }

        private void OnPreviousClicked(object sender, RoutedEventArgs e)
        {
            this._view.MoveToPreviousPage();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _downloadService = new FileDownload();
            _fileIOservice = new FileIOService(PATH);
            _downloadService.Download();
            _view = _fileIOservice.LoadData();
            if(_view != null)
            {
                thrList.ItemsSource = _view;
            }

        }

        private void dngList_Loaded(object sender, RoutedEventArgs e)
        {
            _fileIOservice = new FileIOService(PATH);
            _view = _fileIOservice.LoadData();
            if(_view != null)
            {
                thrList.ItemsSource = _view;
            }

        }
    }
}
