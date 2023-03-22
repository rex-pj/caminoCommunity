using Camino.Core.Validators;
using System.Linq;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Camino.Shared.Configuration.Options;
using Camino.Shared.Exceptions;
using Camino.Shared.File;
using System.IO;

namespace Camino.Application.Validators
{
    public class FormFileValidator : BaseValidator<IFormFile, bool>
    {
        private readonly ApplicationSettings _appSettings;
        public FormFileValidator(IOptions<ApplicationSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public override bool IsValid(IFormFile value)
        {
            try
            {
                if (value == null || value.Length < 0)
                {
                    Errors = GetErrors(new ArgumentNullException(nameof(value))).ToList();
                    return false;
                }

                if (value.Length > _appSettings.MaxUploadFileSize)
                {
                    Errors = GetErrors(new CaminoApplicationException($"The maximum supported file size are {_appSettings.MaxUploadFileSize / 1024 / 1024}")).ToList();
                    return false;
                }

                if (!FileUtils.ValidateName(value.FileName))
                {
                    Errors = GetErrors(new CaminoApplicationException($"The file name contains characters are not supported")).ToList();
                    return false;
                }

                if (!FileUtils.ValidateContentType(value.ContentType))
                {
                    Errors = GetErrors(new CaminoApplicationException($"This file content type is not supported")).ToList();
                    return false;
                }

                var fileExtension = Path.GetExtension(value.FileName);
                if (!FileUtils.ValidateExtension(fileExtension))
                {
                    Errors = GetErrors(new CaminoApplicationException($"This file extension is not supported")).ToList();
                    return false;
                }

                if (!FileUtils.ValidateContentTypeAndExtension(value.ContentType, fileExtension))
                {
                    Errors = GetErrors(new CaminoApplicationException($"File content type and file extension don't matched")).ToList();
                    return false;
                }

                return true;
            }
            catch (Exception e)
            {
                Errors = GetErrors(e).ToList();
                return false;
            }
        }

        public override IEnumerable<ValidatorErrorResult> GetErrors(Exception exception)
        {
            yield return new ValidatorErrorResult()
            {
                Message = exception.Message
            };
        }
    }
}
