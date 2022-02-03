namespace RetroBot.Application.Contracts.Services;

public class ServiceResult<TData>
{
    public bool IsSuccess { get; }
    public TData Data { get; }
    public string ErrorMessage { get; }

    private ServiceResult(bool isSuccess, TData data, string errorMessage)
    {
        IsSuccess = isSuccess;
        Data = data;
        ErrorMessage = errorMessage;
    }
    
    public static ServiceResult<TData> Success<TData>(TData data)
    {
        return new ServiceResult<TData>(true, data, string.Empty);
    }

    public static ServiceResult<TData> Fail<TData>(string errorMessage)
    {
        return new ServiceResult<TData>(false, default, errorMessage);
    }
}