using System.Windows.Controls;

namespace veteran_logistic.Transactions.LoadingRegisters.Views;

/// <summary>
/// Interaction logic for EditLoadingRegisterView.xaml
/// </summary>
public partial class EditLoadingRegisterView : UserControl
{
    public EditLoadingRegisterView()
    {
        try
        {
            InitializeComponent();
        }
        catch (System.Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"XAML Load Error: {ex.Message}");
            if (ex.InnerException != null)
            {
                System.Diagnostics.Debug.WriteLine($"Inner Exception: {ex.InnerException.Message}");
            }
            throw;
        }
    }
}
