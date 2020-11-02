using Syncfusion.Data;
using Syncfusion.WinForms.DataGrid;
using Syncfusion.WinForms.DataGrid.Enums;
using Syncfusion.WinForms.DataGrid.Renderers;
using Syncfusion.WinForms.DataGrid.Styles;
using Syncfusion.WinForms.GridCommon.ScrollAxis;
using Syncfusion.WinForms.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Update_Live
{
    public partial class Form1 : Form
    {
        SfDataGrid sfDataGrid;
        OrderInfoCollection order;
        OrderInfo orderInfo;
        public Form1()
        {
            InitializeComponent();
            sfDataGrid = new SfDataGrid();
            order = new OrderInfoCollection();
            sfDataGrid.Dock = DockStyle.Fill;
            this.Controls.Add(sfDataGrid);
            sfDataGrid.ShowGroupDropArea = true;
            sfDataGrid.AllowGrouping = true;
            sfDataGrid.DataSource = order.Orders;
            GridTableSummaryRow tableSummaryRow1 = new GridTableSummaryRow();
            tableSummaryRow1.Name = "TableSummary";
            tableSummaryRow1.ShowSummaryInRow = false;
            tableSummaryRow1.Position = VerticalPosition.Bottom;
            GridSummaryColumn summaryColumn1 = new GridSummaryColumn();
            summaryColumn1.Name = "PriceAmount";
            summaryColumn1.Format = "{Sum:c}";
            summaryColumn1.MappingName = "UnitPrice";
            summaryColumn1.SummaryType = SummaryType.DoubleAggregate;
            tableSummaryRow1.SummaryColumns.Add(summaryColumn1);
            this.sfDataGrid.TableSummaryRows.Add(tableSummaryRow1);
            this.sfDataGrid.LiveDataUpdateMode = LiveDataUpdateMode.AllowSummaryUpdate;
            this.sfDataGrid.CellRenderers.Remove("Numeric");
            this.sfDataGrid.CellRenderers.Add("Numeric", new GridNumericCellRendererExt(this.sfDataGrid));
        }
    }



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
}
