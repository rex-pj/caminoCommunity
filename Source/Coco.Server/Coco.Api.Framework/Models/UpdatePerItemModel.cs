using Coco.Api.Framework.Commons.Enums;

namespace Coco.Api.Framework.Models
{
    public class UpdatePerItemModel
    {
        public object Key { get; set; }
        public string PropertyName { get; set; }
        public object Value { get; set; }
        public bool CanEdit { get; set; }
        public EditableTypeEnum Type { get; set; }
    }
}
