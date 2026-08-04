using System.Windows.Controls;
using System.Windows.Data;
using System.Collections.Specialized;
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
        this.Loaded += QueryBuilderView_Loaded;
    }

    private void QueryBuilderView_Loaded(object sender, System.Windows.RoutedEventArgs e)
    {
        if (DataContext is QueryBuilderViewModel viewModel)
        {
            ((INotifyCollectionChanged)viewModel.ResultItems).CollectionChanged += ResultItems_CollectionChanged;
        }
    }

    private void ResultItems_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (DataContext is QueryBuilderViewModel viewModel && !viewModel.ColumnsGenerated)
        {
            GenerateColumns(viewModel);
        }
    }

    private void GenerateColumns(QueryBuilderViewModel viewModel)
    {
        ResultsDataGrid.Columns.Clear();

        if (viewModel.QueryResult != null && viewModel.SelectedModule != null)
        {
            foreach (var columnId in viewModel.QueryResult.ColumnHeaders)
            {
                var field = viewModel.SelectedModule.Fields.FirstOrDefault(f => f.FieldId == columnId);
                var headerText = field?.DisplayName ?? columnId;

                var binding = new Binding("Values")
                {
                    Converter = FindResource("DictionaryValueConverter") as IValueConverter,
                    ConverterParameter = columnId
                };

                var column = new DataGridTextColumn
                {
                    Header = headerText,
                    Binding = binding
                };

                ResultsDataGrid.Columns.Add(column);
            }

            viewModel.GenerateColumns();
        }
    }

    private void ResultsDataGrid_LoadingRow(object? sender, DataGridRowEventArgs e)
    {
        if (DataContext is QueryBuilderViewModel viewModel && !viewModel.ColumnsGenerated)
        {
            GenerateColumns(viewModel);
        }
    }
}
