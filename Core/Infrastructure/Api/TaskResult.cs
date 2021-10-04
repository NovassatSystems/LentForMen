namespace Core
{
    public class TaskResult
    {
        internal static TaskResult<T> Create<T>(T data, bool success, string message = "")
            => new TaskResult<T>(data, success, message);
    }

    public class TaskResult<T>
    {
        public TaskResult(T data, bool success, string message = "")
        {
            Data = data;
            Message = message;
            Success = success;
        }

        public T Data { get; private set; }

        public bool Success { get; private set; }

        public string Message { get; private set; }
    }
}
