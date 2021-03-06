using DevExpress.Skins;
using System.Collections.Generic;
using Code4Bugs.Utils.DX.Editors;
using Code4Bugs.Utils.DX.Forms.Internal;
using Code4Bugs.Utils.DX.Forms.Settings;

namespace Code4Bugs.Utils.DX.Forms
{
    public partial class LookAndFeelSettingsForm : _LookAndFeelSettingsForm
    {
        private static readonly string[] DefaultFlatSkinNames = new string [] { "Visual Studio 2013 Dark", "Visual Studio 2013 Light" };

        public bool UseFlatSkin { get; set; }

        public LookAndFeelSettingsForm()
        {
            InitializeComponent();
        }
        
        private string[] GetSkinNames()
        {
            if (UseFlatSkin) return DefaultFlatSkinNames;

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