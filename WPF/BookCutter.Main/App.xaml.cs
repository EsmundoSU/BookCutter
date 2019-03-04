using System.Collections.Generic;
using System.Windows;

namespace BookCutter.Main
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Global access to photos from app scope
        /// </summary>
        public List<ImageModel> Photos { get; set; }
    }
}
