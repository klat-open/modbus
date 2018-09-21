using DevExpress.Skins;
using System.Collections.Generic;
using Code4Bugs.Utils.DevExpress.Editors;
using Code4Bugs.Utils.DevExpress.Forms.Internal;
using Code4Bugs.Utils.DevExpress.Forms.Settings;

namespace Code4Bugs.Utils.DevExpress.Forms
{
    public partial class LookAndFeelSettingsForm : _LookAndFeelSettingsForm
    {
        public LookAndFeelSettingsForm()
        {
            InitializeComponent();
        }

        private string[] GetSkinNames()
        {
            var skins = SkinManager.Default.Skins;
            var skinNames = new List<string>();
            for (var i = 0; i < skins.Count; i++)
            {
                skinNames.Add(skins[i].SkinName);
            }
            return skinNames.ToArray();
        }

        protected override void LoadSettingToUi(LookAndFeelSettings settings)
        {
            base.LoadSettingToUi(settings);
            cbbSkin.Initialize(GetSkinNames(), settings.SkinName);
        }

        protected override void LoadSettingsFromUi(LookAndFeelSettings settings)
        {
            base.LoadSettingsFromUi(settings);
            settings.SkinName = cbbSkin.GetString();
        }
    }
}