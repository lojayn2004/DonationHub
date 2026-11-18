namespace DonationHub.API.Dtos.Campaigns
{
    public class UpdateStatusDto
    {
        public string status { get; set; }
        public byte[] rowVersion { get; set; }
    }

}
