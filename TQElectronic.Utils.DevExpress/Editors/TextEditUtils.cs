using DevExpress.XtraEditors;

namespace TQElectronic.Utils.DevExpress.Editors
{
    public static class TextEditUtils
    {
        public static int GetInt(this TextEdit textEdit, int defaultValue = 0)
        {
            var text = textEdit.Text;
            if (!string.IsNullOrEmpty(text) && int.TryParse(text, out var ret))
                return ret;
            return defaultValue;
        }

        public static float GetFloat(this TextEdit textEdit, float defaultValue = 0)
        {
            var text = textEdit.Text;
            if (!string.IsNullOrEmpty(text) && float.TryParse(text, out var ret))
                return ret;
            return defaultValue;
        }
    }
}