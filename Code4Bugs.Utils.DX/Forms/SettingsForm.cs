using System;
using System.ComponentModel;
using System.Reflection;
using Code4Bugs.Utils.Types;

namespace Code4Bugs.Utils.DX.Forms
{
    public partial class SettingsForm<T> : ChooserForm where T : class
    {
        private T _settings;

        public T Settings
        {
            get => _settings;
            set
            {
                _settings = value;

                if (_settings != null)
                    LoadSettingToUi(_settings);
                else
                    EmptySettings();
            }
        }

        public bool IsImmutable { get; set; }

        public SettingsForm()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!DesignMode)
                LoadTitle();
        }

        private void LoadTitle()
        {
            var settingsType = typeof(T);
            var descriptionAttribute = settingsType.GetCustomAttribute<DescriptionAttribute>();
            if (descriptionAttribute != null)
                Text = descriptionAttribute.Description;
            else
                Text = settingsType.Name.SplitCamelCase();
        }

        protected virtual void LoadSettingToUi(T settings)
        {
        }

        protected virtual void EmptySettings()
        {
        }

        protected override void OnAccept()
        {
            if (!VerifySettings()) return;
            if (_settings == null || IsImmutable)
                _settings = Activator.CreateInstance<T>();
            LoadSettingsFromUi(_settings);
            base.OnAccept();
        }

        protected virtual void LoadSettingsFromUi(T settings)
        {
        }

        protected virtual bool VerifySettings()
        {
            return true;
        }
    }
}