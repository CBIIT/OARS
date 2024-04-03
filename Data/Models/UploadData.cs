using Blazorise;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace TheradexPortal.Data.Models
{
    public class ETCTNUploadRequest //: IValidatableObject
    {
        public string RequestId { get; set; }

        [Required]
        public string Protocol { get; set; }

        [Required]
        public string ReceivingSite { get; set; }

        [Required]
        public string SourceSite { get; set; }

        [Required]
        public string CRF { get; set; }

        [RequiredIf(nameof(CRF), "RECEIVING_STATUS")]
        public string Assay { get; set; }

        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    if (CRF == "RECEIVING_STATUS")
        //    {
        //        yield return new ValidationResult("Assay is required");
        //    }
        //}

        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    if (Genre == Genre.Classic && ReleaseDate.Year > _classicYear)
        //    {
        //        yield return new ValidationResult(
        //            $"Classic movies must have a release year no later than {_classicYear}.",
        //            new[] { nameof(ReleaseDate) });
        //    }
        //}
    }

    //public class RequiredIfCustomAttribute(string otherProperty, object targetValue) : ValidationAttribute
    //{
    //    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    //    {
    //        var otherPropertyValue = validationContext.ObjectType
    //                                                  .GetProperty(otherProperty)?
    //                                                  .GetValue(validationContext.ObjectInstance);

    //        if (otherPropertyValue is null || !otherPropertyValue.Equals(targetValue)) return ValidationResult.Success;

    //        if (value is null || string.IsNullOrWhiteSpace(value.ToString()))
    //        {
    //            return new ValidationResult(ErrorMessage ?? "This field is required.");
    //        }

    //        return ValidationResult.Success;
    //    }
    //}

    public class RequiredIfAttribute : ValidationAttribute
    {
        private readonly string _propertyName;
        private readonly object? _isValue;

        public RequiredIfAttribute(string propertyName, object? isValue)
        {
            _propertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
            _isValue = isValue;
        }

        public override string FormatErrorMessage(string name)
        {
            var errorMessage = $"{name} is required when {_propertyName} is {_isValue}";
            return ErrorMessage ?? errorMessage;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            ArgumentNullException.ThrowIfNull(validationContext);
            var property = validationContext.ObjectType.GetProperty(_propertyName);

            if (property == null)
            {
                throw new NotSupportedException($"Can't find {_propertyName} on searched type: {validationContext.ObjectType.Name}");
            }

            var requiredIfTypeActualValue = property.GetValue(validationContext.ObjectInstance);

            if (requiredIfTypeActualValue == null && _isValue != null)
            {
                return ValidationResult.Success;
            }

            if (requiredIfTypeActualValue == null || requiredIfTypeActualValue.Equals(_isValue))
            {
                return value == null
                    ? new ValidationResult(FormatErrorMessage(validationContext.DisplayName))
                    : ValidationResult.Success;
            }

            return ValidationResult.Success;
        }
    }

    public class UploadFileModel
    {
        public string OriginalFileName { get; set; }
        public string S3Key { get; set; }
    }

    public class MedidataDictionaryModel
    {
        public string UserData { get; set; }
        public string CodedData { get; set; }
    }

    public class CRFModel
    {
        public string FormOID { get; set; }
        public string FormName { get; set; }
    }

    public class FileMetadata
    {
        [JsonProperty("requestId")]
        public string RequestId { get; set; }

        [JsonProperty("userId")]
        public int UserId { get; set; }

        [JsonProperty("protocol")]
        public string Protocol { get; set; }

        [JsonProperty("sourceSite")]
        public string SourceSite { get; set; }

        [JsonProperty("receivingSite")]
        public string ReceivingSite { get; set; }

        [JsonProperty("assay")]
        public string Assay { get; set; }

        [JsonProperty("crf")]
        public string CRF { get; set; }

        [JsonProperty("filePath")]
        public string FilePath { get; set; }

        [JsonProperty("originalFileName")]
        public string OriginalFileName { get; set; }

        [JsonProperty("bucket")]
        public string Bucket { get; set; }
    }
}
