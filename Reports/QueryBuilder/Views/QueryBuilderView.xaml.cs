using System.Windows.Controls;
using System.Windows.Data;
using veteran_logistic.Reports.QueryBuilder.ViewModels;

namespace veteran_logistic.Reports.QueryBuilder.Views;

/// <summary>
/// Interaction logic for QueryBuilderView.xaml
/// </summary>
public partial class QueryBuilderView : UserControl
{
    public QueryBuilderView()
    {
        InitializeComponent();
    }

    private void ResultsDataGrid_LoadingRow(object? sender, DataGridRowEventArgs e)
    {
        if (DataContext is QueryBuilderViewModel viewModel && !viewModel.ColumnsGenerated)
        {
            ResultsDataGrid.Columns.Clear();

            if (viewModel.QueryResult != null && viewModel.SelectedModule != null)
            {
                foreach (var columnId in viewModel.QueryResult.ColumnHeaders)
                {
                    var field = viewModel.SelectedModule.Fields.FirstOrDefault(f => f.FieldId == columnId);
                    var headerText = field?.DisplayName ?? columnId;

                    var column = new DataGridTextColumn
                    {
                        Header = headerText,
                        Binding = new Binding($"Values[{columnId}]")
                    };

                    ResultsDataGrid.Columns.Add(column);
                }

                viewModel.GenerateColumns();
            }
        }
    }
}
