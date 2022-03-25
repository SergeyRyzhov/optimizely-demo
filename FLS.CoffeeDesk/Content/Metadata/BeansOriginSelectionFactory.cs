using System.Collections.Generic;
using EPiServer.Shell.ObjectEditing;

namespace FLS.CoffeeDesk.Content.Metadata
{
    public class BeansOriginSelectionFactory : ISelectionFactory
    {
        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            return new ISelectItem[]
            {
                new SelectItem() { Text = "Brazil", Value = "BRA" },
                new SelectItem() { Text = "Vietnam", Value = "VNM" },
                new SelectItem() { Text = "Colombia", Value = "COL" },
                new SelectItem() { Text = "Indonesia", Value = "IDN" }
            };
        }
    }
}