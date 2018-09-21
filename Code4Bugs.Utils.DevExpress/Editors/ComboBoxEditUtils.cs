using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Linq;
using Code4Bugs.Utils.Types;

namespace Code4Bugs.Utils.DevExpress.Editors
{
    public static class ComboBoxEditUtils
    {
        public static void Initialize<T>(this ComboBoxEdit comboBox, IEnumerable<T> items, T selected = default)
        {
            comboBox.Properties.Items.Clear();
            if (items != null)
            {
                comboBox.Properties.Items.AddRange(items.ToList());
                comboBox.SelectItem(selected);
            }
        }

        public static void Initialize<T>(this ComboBoxEdit comboBox, T selected = default) where T : Enum
        {
            comboBox.Properties.Items.Clear();
            var items = Enum.GetNames(typeof(T));
            comboBox.Properties.Items.AddRange(items);
            comboBox.SelectItem(selected);
        }

        public static void Initialize(this ComboBoxEdit comboBox, int rangeFrom, int rangeTo, int selected = -1)
        {
            comboBox.Properties.Items.Clear();
            var items = Enumerable.Range(rangeFrom, rangeTo - rangeFrom + 1);
            comboBox.Properties.Items.AddRange(items.ToList());
            comboBox.SelectItem(selected < rangeFrom ? rangeFrom : selected);
        }

        public static void SelectItem(this ComboBoxEdit comboBox, object selected, int selectedDefaultIndex = 0)
        {
            for (var i = 0; i < comboBox.Properties.Items.Count; i++)
            {
                var item = comboBox.Properties.Items[i];
                if (selected != null && item != null && item.ToString() == selected.ToString())
                {
                    comboBox.SelectedIndex = i;
                    break;
                }
            }

            if (comboBox.SelectedIndex == -1) comboBox.SelectedIndex = selectedDefaultIndex;
        }

        public static T GetEnum<T>(this ComboBoxEdit comboBox)
        {
            return comboBox.SelectedItem.ToString().ToEnum<T>();
        }

        public static int GetInt(this ComboBoxEdit comboBox)
        {
            return int.Parse(comboBox.SelectedItem.ToString());
        }

        public static float GetFloat(this ComboBoxEdit comboBox)
        {
            return float.Parse(comboBox.SelectedItem.ToString());
        }

        public static string GetString(this ComboBoxEdit comboBox)
        {
            return comboBox.SelectedItem.ToString();
        }
    }
}