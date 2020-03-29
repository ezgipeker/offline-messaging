namespace OfflineMessaging.Domain.Dtos.Base
{
    /// <summary>
    /// Base Response Dto For Common Usage
    /// </summary>
    public abstract class BaseResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
