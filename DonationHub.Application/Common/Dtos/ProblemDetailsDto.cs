

using System.Text.Json.Serialization;

namespace DonationHub.Application.Common.Dtos
{
    public class ProblemDetailsDto
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IReadOnlyDictionary<string , string[]>? Errors { get; set; }
        public string Title { get; set; } = default!;

        public string Code { get; set; } = default!;

        public int Status { get; set; }

        public string TraceId { get; set; } = default!;

        public string Type { get; set; } = default!;
    };
}
