

namespace DonationHub.Application.Common.Dtos
{
    public class ProblemDetailsDto
    {
        public IReadOnlyDictionary<string , string[]>? Details { get; set; }
        public string Title { get; set; }

        public string Code { get; set; }

        public int Status { get; set; }

        public string TraceId { get; set; }

        public string Type { get; set; }
    };
}
