using System.Windows;
using System.Windows.Controls;

namespace veteran_logistic.Authentication.Views;

/// <summary>
/// Interaction logic for LoginView.xaml
/// </summary>
public partial class LoginView : UserControl
{
    public LoginView()
    {
        InitializeComponent();
    }

    private void OnPasswordChanged(object sender, RoutedEventArgs e)
    {
        if (sender is PasswordBox passwordBox && DataContext is ViewModels.LoginViewModel viewModel)
        {
            viewModel.Password = passwordBox.Password;
        }
    }
}
