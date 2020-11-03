# How to update the table summary value when the cell in edit mode in WinForms DataGrid (SfDataGrid) ?

How to update the table summary value when the cell in edit mode in WinForms DataGrid (SfDataGrid) ?

# About the sample

In SfDataGrid, you can update the summary values when you are changing the value by overriding OnInitializeEditElement method and UiElement.TextChanged event in GridNumericCellRenderer.

```c#
this.sfDataGrid.CellRenderers.Remove("Numeric");
this.sfDataGrid.CellRenderers.Add("Numeric", new GridNumericCellRendererExt(this.sfDataGrid));
public class GridNumericCellRendererExt : GridNumericCellRenderer
{
    RowColumnIndex RowColumnIndex { get; set; }
    SfDataGrid DataGrid { get; set; }
    public GridNumericCellRendererExt(SfDataGrid dataGrid)
    {
        this.DataGrid = dataGrid;
    }
    protected override void OnInitializeEditElement(DataColumnBase column, RowColumnIndex rowColumnIndex, SfNumericTextBox uiElement)
    {
        base.OnInitializeEditElement(column, rowColumnIndex, uiElement);
        uiElement.TextChanged += UiElement_TextChanged;

        this.RowColumnIndex = rowColumnIndex;
    }

    private void UiElement_TextChanged(object sender, EventArgs e)
    {
        UpdateSummaryValues(this.RowColumnIndex.RowIndex, this.RowColumnIndex.ColumnIndex);
    }

    private void UpdateSummaryValues(int rowIndex, int columnIndex)
    {
        string editEelementText = string.IsNullOrEmpty(this.CurrentCellRendererElement.Text) ? "0" : this.CurrentCellRendererElement.Text;
        columnIndex = this.TableControl.ResolveToGridVisibleColumnIndex(columnIndex);
        if (columnIndex < 0)
            return;
        var mappingName = DataGrid.Columns[columnIndex].MappingName;
        var recordIndex = this.TableControl.ResolveToRecordIndex(rowIndex);
        if (recordIndex < 0)
            return;
        if (DataGrid.View.TopLevelGroup != null)
        {
            var record = DataGrid.View.TopLevelGroup.DisplayElements[recordIndex];
            if (!record.IsRecords)
                return;
            var data = (record as RecordEntry).Data;              
            data.GetType().GetProperty(mappingName).SetValue(data, (int.Parse(editEelementText)));
        }
        else
        {
            var record1 = DataGrid.View.Records.GetItemAt(recordIndex);
            record1.GetType().GetProperty(mappingName).SetValue(record1, (int.Parse(editEelementText)));
        }
    }
}
```
## Requirements to run the demo
 Visual Studio 2015 and above versions
