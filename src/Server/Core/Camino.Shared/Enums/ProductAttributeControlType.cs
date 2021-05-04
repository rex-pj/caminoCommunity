using System.ComponentModel;

namespace Camino.Shared.Enums
{
    public enum ProductAttributeControlType
    {
        [Description("Horizontal List")]
        HorizontalList = 1,

        [Description("Vertical List")]
        VerticalList = 2,

        [Description("Dropdown List")]
        DropdownList = 3,

        [Description("Radio List")]
        RadioList = 4,

        [Description("Checkboxes")]
        Checkboxes = 5,

        [Description("Color Boxes")]
        ColorBoxes = 6
    }
}
