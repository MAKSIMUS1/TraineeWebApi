using FluentValidation;
using WebApiTrainingProject.DTOs.Request;

namespace WebApiTrainingProject.DTOs.DTOValidators
{
    public class UpdateProjectDtoValidator : AbstractValidator<UpdateProjectDto>
    {
        public UpdateProjectDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Название проекта не может быть пустым.");
        }
    }
}
