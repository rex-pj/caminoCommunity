using Coco.Business.ValidationStrategies.Models;
using System;
using System.Collections.Generic;

namespace Coco.Business.ValidationStrategies.Interfaces
{
    public interface IValidationStrategy
    {
        IEnumerable<ErrorObject> Errors { get; set; }
        bool IsValid<T>(T value);
        IEnumerable<ErrorObject> GetErrors(Exception e);
    }
}
