namespace catat.utility;

public class Response<T> 
{
    public string Message { get; set; }
    public T? Data { get; set; }
    public bool Success { get; set; }
}
