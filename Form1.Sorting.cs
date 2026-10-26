using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Student_Manager
{
    public partial class Form1
    {
        private string currentSortColumnName;
        private SortOrder currentSortOrder = SortOrder.None;

        private void EnableNaturalColumnSorting()
        {
            dataGridView1.ColumnHeaderMouseClick -= DataGridView1_ColumnHeaderMouseClick;
            dataGridView1.DataBindingComplete -= DataGridView1_DataBindingComplete;
            dataGridView1.ColumnHeaderMouseClick += DataGridView1_ColumnHeaderMouseClick;
            dataGridView1.DataBindingComplete += DataGridView1_DataBindingComplete;
            ConfigureSortableColumns();
        }

        private void DataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            ConfigureSortableColumns();
            ApplySortGlyph();
        }

        private void ConfigureSortableColumns()
        {
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.Programmatic;
            }
        }

        private void DataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex < 0 || dataGridView1.Columns.Count <= e.ColumnIndex)
            {
                return;
            }

            DataTable source = dataGridView1.DataSource as DataTable;
            if (source == null)
            {
                DataView view = dataGridView1.DataSource as DataView;
                source = view == null ? null : view.ToTable();
            }

            if (source == null || source.Rows.Count == 0)
            {
                return;
            }

            DataGridViewColumn clickedColumn = dataGridView1.Columns[e.ColumnIndex];
            string columnName = clickedColumn.DataPropertyName;
            if (string.IsNullOrWhiteSpace(columnName))
            {
                columnName = clickedColumn.Name;
            }

            SortOrder requestedOrder = string.Equals(currentSortColumnName, columnName, StringComparison.Ordinal)
                && currentSortOrder == SortOrder.Ascending
                    ? SortOrder.Descending
                    : SortOrder.Ascending;

            List<IndexedRow> rows = source.AsEnumerable()
                .Select((row, index) => new IndexedRow(row, index))
                .ToList();
            rows.Sort(new NaturalRowComparer(columnName, requestedOrder));

            DataTable sortedTable = source.Clone();
            foreach (IndexedRow row in rows)
            {
                sortedTable.ImportRow(row.Row);
            }

            currentSortColumnName = columnName;
            currentSortOrder = requestedOrder;
            dataGridView1.DataSource = sortedTable;
            ApplySortGlyph();
            dataGridView1.ClearSelection();
        }

        private void ApplySortGlyph()
        {
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.HeaderCell.SortGlyphDirection = SortOrder.None;
            }

            if (string.IsNullOrWhiteSpace(currentSortColumnName))
            {
                return;
            }

            DataGridViewColumn sortedColumn = dataGridView1.Columns
                .Cast<DataGridViewColumn>()
                .FirstOrDefault(column =>
                    string.Equals(column.DataPropertyName, currentSortColumnName, StringComparison.Ordinal)
                    || string.Equals(column.Name, currentSortColumnName, StringComparison.Ordinal));

            if (sortedColumn != null)
            {
                sortedColumn.HeaderCell.SortGlyphDirection = currentSortOrder;
            }
        }

        private sealed class IndexedRow
        {
            public IndexedRow(DataRow row, int originalIndex)
            {
                Row = row;
                OriginalIndex = originalIndex;
            }

            public DataRow Row { get; private set; }
            public int OriginalIndex { get; private set; }
        }

        private sealed class NaturalRowComparer : IComparer<IndexedRow>
        {
            private readonly string columnName;
            private readonly SortOrder sortOrder;

            public NaturalRowComparer(string columnName, SortOrder sortOrder)
            {
                this.columnName = columnName;
                this.sortOrder = sortOrder;
            }

            public int Compare(IndexedRow left, IndexedRow right)
            {
                object leftValue = left.Row[columnName];
                object rightValue = right.Row[columnName];
                bool leftEmpty = IsEmpty(leftValue);
                bool rightEmpty = IsEmpty(rightValue);

                // Empty values always stay at the bottom in either direction.
                if (leftEmpty || rightEmpty)
                {
                    if (leftEmpty && rightEmpty)
                    {
                        return left.OriginalIndex.CompareTo(right.OriginalIndex);
                    }

                    return leftEmpty ? 1 : -1;
                }

                int result = CompareValues(leftValue, rightValue);
                if (sortOrder == SortOrder.Descending)
                {
                    result = -result;
                }

                return result == 0 ? left.OriginalIndex.CompareTo(right.OriginalIndex) : result;
            }

            private static bool IsEmpty(object value)
            {
                if (value == null || value == DBNull.Value)
                {
                    return true;
                }

                if (value is DateTime && (DateTime)value == DateTime.MinValue)
                {
                    return true;
                }

                return string.IsNullOrWhiteSpace(Convert.ToString(value, CultureInfo.CurrentCulture));
            }

            private static int CompareValues(object left, object right)
            {
                if (left is DateTime && right is DateTime)
                {
                    return ((DateTime)left).CompareTo((DateTime)right);
                }

                if (left is bool && right is bool)
                {
                    return ((bool)left).CompareTo((bool)right);
                }

                string leftText = Convert.ToString(left, CultureInfo.CurrentCulture).Trim();
                string rightText = Convert.ToString(right, CultureInfo.CurrentCulture).Trim();
                decimal leftNumber;
                decimal rightNumber;
                if (decimal.TryParse(leftText, NumberStyles.Number, CultureInfo.CurrentCulture, out leftNumber)
                    && decimal.TryParse(rightText, NumberStyles.Number, CultureInfo.CurrentCulture, out rightNumber))
                {
                    return leftNumber.CompareTo(rightNumber);
                }

                return CompareNaturalText(leftText, rightText);
            }

            private static int CompareNaturalText(string left, string right)
            {
                string[] leftParts = Regex.Split(left, "(\\d+)");
                string[] rightParts = Regex.Split(right, "(\\d+)");
                int partCount = Math.Min(leftParts.Length, rightParts.Length);

                for (int index = 0; index < partCount; index++)
                {
                    long leftNumber;
                    long rightNumber;
                    bool leftIsNumber = long.TryParse(leftParts[index], out leftNumber);
                    bool rightIsNumber = long.TryParse(rightParts[index], out rightNumber);
                    int partResult;

                    if (leftIsNumber && rightIsNumber)
                    {
                        partResult = leftNumber.CompareTo(rightNumber);
                    }
                    else
                    {
                        partResult = StringComparer.CurrentCultureIgnoreCase.Compare(leftParts[index], rightParts[index]);
                    }

                    if (partResult != 0)
                    {
                        return partResult;
                    }
                }

                int lengthResult = leftParts.Length.CompareTo(rightParts.Length);
                return lengthResult != 0
                    ? lengthResult
                    : StringComparer.CurrentCultureIgnoreCase.Compare(left, right);
            }
        }
    }
}
