namespace ByeByeDPI.Models
{
    public class UpdateInfoWrapper
    {
        public UpdateInfo UpdateInfo { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }

        public UpdateInfoWrapper(bool success, string errorMessage = null, UpdateInfo updateInfo = null)
        {
            Success = success;
            ErrorMessage = errorMessage ?? string.Empty;
            UpdateInfo = updateInfo;
        }
    }
}
