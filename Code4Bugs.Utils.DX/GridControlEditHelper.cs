using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Code4Bugs.Utils.DX
{
    public sealed class GridControlEditHelper
    {
        private readonly List<GridControl> _gridControls = new List<GridControl>();

        public void Register(GridControl gridControl)
        {
            _gridControls.Add(gridControl);
        }

        public void PerformCut()
        {
            PerformAction(view =>
            {
                var selectedRows = view.GetSelectedRows();
                CopyToClipboard(selectedRows, view);
                PerformDelete();
            });
        }

        public void PerformCopy()
        {
            PerformAction(view =>
            {
                var selectedRows = view.GetSelectedRows();
                CopyToClipboard(selectedRows, view);
            });
        }

        public void PerformDelete()
        {
            PerformAction(view =>
            {
                var selectedRows = view.GetSelectedRows();
                var removeMethod = view.DataSource.GetType().GetMethod("Remove");
                var removeList = selectedRows.Select(view.GetRow).ToList();
                foreach (var item in removeList)
                {
                    removeMethod?.Invoke(
                        view.DataSource,
                        BindingFlags.Default,
                        null,
                        new[] { item },
                        CultureInfo.CurrentCulture);
                }
                view.GridControl.RefreshDataSource();
            });
        }

        public void PerformClearAll()
        {
            PerformAction(view =>
            {
                var clearMethod = view.DataSource.GetType().GetMethod("Clear");
                clearMethod?.Invoke(
                    view.DataSource,
                    BindingFlags.Default,
                    null,
                    new object[] { },
                    CultureInfo.CurrentCulture);
                view.GridControl.RefreshDataSource();
            });
        }

        private void PerformAction(Action<GridView> action)
        {
            var focusedControl = _gridControls.Find(control => control.Focused);
            if (focusedControl != null)
            {
                var view = (GridView)focusedControl.MainView;
                action(view);
            }
        }

        private static void CopyToClipboard(IEnumerable<int> selectedRows, GridView view)
        {
            var dataList = selectedRows.Select(view.GetRow).ToList();

            var json = "";
            if (dataList.Count > 1)
                json = JsonConvert.SerializeObject(dataList, Formatting.Indented);
            else if (dataList.Count == 1)
                json = JsonConvert.SerializeObject(dataList[0], Formatting.Indented);

            if (!string.IsNullOrEmpty(json))
                Clipboard.SetText(json);
        }
    }
}