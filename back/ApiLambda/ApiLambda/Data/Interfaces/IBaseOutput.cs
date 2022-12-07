namespace ApiLambda.Data.Interfaces
{
    public interface IBaseOutput
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}

