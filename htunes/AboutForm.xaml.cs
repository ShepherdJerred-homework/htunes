using System.Windows;

namespace htunes {
    /// <summary>
    /// Interaction logic for AboutForm.xaml
    /// </summary>
    public partial class AboutForm {
        public AboutForm() {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e) {
            DialogResult = true;
        }
    }
}