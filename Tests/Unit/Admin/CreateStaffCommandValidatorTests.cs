using Application.Admin.Commands.CreateStaff;
using FluentValidation.TestHelper;

namespace Tests.Unit.Admin
{
    public class CreateStaffCommandValidatorTests
    {
        private readonly CreateStaffCommandValidator _validator = new();

        [Fact]
        public void Should_HaveError_When_NameIsEmpty()
        {
            var cmd = new CreateStaffCommand(
                BusinessId: Guid.NewGuid(),
                Name: "",
                Title: "Frisör",
                ImageUrl: null,
                Bio: null,
                IsActive: true,
                QualifiedServiceIds: Array.Empty<Guid>());

            var result = _validator.TestValidate(cmd);
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Should_HaveError_When_TitleIsEmpty()
        {
            var cmd = new CreateStaffCommand(
                BusinessId: Guid.NewGuid(),
                Name: "Anna",
                Title: "",
                ImageUrl: null,
                Bio: null,
                IsActive: true,
                QualifiedServiceIds: Array.Empty<Guid>());

            var result = _validator.TestValidate(cmd);
            result.ShouldHaveValidationErrorFor(x => x.Title);
        }

        [Fact]
        public void Should_HaveError_When_ServiceIdsContainDuplicates()
        {
            var id = Guid.NewGuid();

            var cmd = new CreateStaffCommand(
                BusinessId: Guid.NewGuid(),
                Name: "Anna",
                Title: "Frisör",
                ImageUrl: null,
                Bio: null,
                IsActive: true,
                QualifiedServiceIds: new[] { id, id });

            var result = _validator.TestValidate(cmd);
            result.ShouldHaveValidationErrorFor(x => x.QualifiedServiceIds);
        }

        [Fact]
        public void Should_Pass_When_Valid()
        {
            var cmd = new CreateStaffCommand(
                BusinessId: Guid.NewGuid(),
                Name: "Anna",
                Title: "Frisör",
                ImageUrl: "https://img",
                Bio: "bio",
                IsActive: true,
                QualifiedServiceIds: new[] { Guid.NewGuid() });

            var result = _validator.TestValidate(cmd);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
