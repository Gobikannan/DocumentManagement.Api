using FluentValidation;

namespace DocumentManagement.Application.Documents.Queries.FetchDocumentLocation
{
    public class FetchDocumentLocationQueryValidator : AbstractValidator<FetchDocumentLocationQuery>
    {
        public FetchDocumentLocationQueryValidator()
        {
            RuleFor(v => v.RowKey).NotEmpty().WithMessage("RowKey is required.");
        }
    }
}
