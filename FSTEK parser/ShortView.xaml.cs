using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FSTEK_parser
{
    /// <summary>
    /// Interaction logic for ShortView.xaml
    /// </summary>
    public partial class ShortView : Page 
    {
        private PagingCollectionView _view;
        public ShortView(PagingCollectionView view)
        {
            InitializeComponent();
            _view = view;
        }

        private void DataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            thrShortList.ItemsSource = _view.SourceCollection;
        }
    }
}
