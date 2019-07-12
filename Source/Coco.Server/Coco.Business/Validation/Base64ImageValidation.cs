using Coco.Business.Validation.Interfaces;

namespace Coco.Business.Validation
{
    public class Base64ImageValidation : IValidation
    {
        public bool IsValid(string value)
        {
            return true;
        }
    }
}
