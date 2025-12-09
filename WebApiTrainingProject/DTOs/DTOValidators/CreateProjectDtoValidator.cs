using FluentValidation;
using WebApiTrainingProject.DTOs.Request;

namespace WebApiTrainingProject.DTOs.DTOValidators
{
    public class CreateProjectDtoValidator : AbstractValidator<CreateProjectDto>
    {
        public CreateProjectDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("The project name cannot be empty.");
        }
    }
}
