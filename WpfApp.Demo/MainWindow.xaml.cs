using System.Windows;

namespace AppDemo
{
    /// <summary>
    /// Interaction logic for SOP.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainWindowVm();
        }
    }
}
