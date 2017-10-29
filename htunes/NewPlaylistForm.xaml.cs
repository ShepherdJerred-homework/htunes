using System.Windows;

namespace htunes
{
    /// <summary>
    /// Interaction logic for NewPlaylistForm.xaml
    /// </summary>
    public partial class NewPlaylistForm : Window
    {
        public NewPlaylistForm()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
