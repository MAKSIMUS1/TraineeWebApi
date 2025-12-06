using FluentValidation;
using System.Text.Json;
using WebApiTrainingProject.DTOs.Request;

namespace WebApiTrainingProject.DTOs.DTOValidators
{
    public class NodeGraphUpdateValidator : AbstractValidator<NodeGraphUpdateDto>
    {
        public NodeGraphUpdateValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name cannot be empty");

            RuleFor(x => x.JsonData)
                .Must(BeValidJson)
                .WithMessage("JsonData must contain valid JSON");
        }

        private bool BeValidJson(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return false;

            try
            {
                JsonDocument.Parse(json);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
