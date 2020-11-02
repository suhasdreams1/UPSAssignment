using System.Windows;
using UPSAssignment.Interfaces.ViewModels;

namespace UPSAssignment.Views
{
    /// <summary>
    /// Interaction logic for AddUpdateView.xaml
    /// </summary>
    public partial class AddUpdateView : Window
    {
        public AddUpdateView(IAddUpdateViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
        }
    }
}
