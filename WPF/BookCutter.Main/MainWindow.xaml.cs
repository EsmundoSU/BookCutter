using System.Windows;

namespace BookCutter.Main
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenPhotoButton_Click(object sender, RoutedEventArgs e)
        {
            var photo = PhotoProcessing.TestFunction();
            BasicPhotoImage.Source = photo;
            
        }
    }
}
