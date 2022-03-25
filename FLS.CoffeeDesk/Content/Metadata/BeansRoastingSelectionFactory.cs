using System;
using EPiServer.Cms.Shell.UI.ObjectEditing.EditorDescriptors.SelectionFactories;
using EPiServer.Framework.Localization;

namespace FLS.CoffeeDesk.Content.Metadata
{
    public class BeansRoastingSelectionFactory : EnumSelectionFactory
    {
        public BeansRoastingSelectionFactory(LocalizationService localizationService) : base(localizationService)
        {
        }

        protected override string GetStringForEnumValue(int value)
        {
            return ((BeansRoasting)value).ToString();
        }

        protected override Type EnumType => typeof(BeansRoasting);
    }
}