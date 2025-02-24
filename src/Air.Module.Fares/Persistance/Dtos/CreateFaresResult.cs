namespace Air.Module.Fares.Persistance.Dtos
{
    public sealed partial class CreateFaresResult
    {
        public bool Success { get; init; }
    }

    public enum ResultCode
    {
        Unknown,
        Success,
        Failed
    }
}
