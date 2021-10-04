using System;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Microsoft.AppCenter.Crashes;

namespace Core
{
    internal static class TaskExtensions
    {
        public static async Task<TaskResult<bool>> HandleWithProgress(this Task self)
        {
            using (var progressDialog = UserDialogs.Instance.Loading(title: string.Empty, maskType: MaskType.Black))
            {
                return await Handle(self);
            }
        }

        public static async Task<TaskResult<T>> HandleWithProgress<T>(this Task<T> self)
        {
            using (var progressDialog = UserDialogs.Instance.Loading(title: string.Empty, maskType: MaskType.Black))
            {
                return await Handle(self);
            }
        }


        public static async Task<TaskResult<bool>> Handle(this Task self)
        {
            try
            {
                await self.ConfigureAwait(false);
                return TaskResult.Create(true, true);
            }
            catch (Exception ex)
            {
                return Error<bool>(ex);
            }
            finally
            {

            }
        }

        public static async Task<TaskResult<T>> Handle<T>(this Task<T> self)
        {
            try
            {
                var result = await self.ConfigureAwait(false);
                return TaskResult.Create(result, true);
            }
            catch (Exception ex)
            {
                return Error<T>(ex);
            }
            finally
            {

            }
        }

        
        static TaskResult<T> Error<T>(Exception ex, string message)
        {
            Crashes.TrackError(ex);
            Console.WriteLine(ConcactException(ex));
            return TaskResult.Create(default(T), false, message);
        }

        static TaskResult<T> Error<T>(Exception ex)
        {
            Crashes.TrackError(ex);
            Console.WriteLine(ConcactException(ex));
            return TaskResult.Create(default(T), false, Resources.ErrorServer);
        }

        public static string ConcactException(Exception ex, StringBuilder str = null)
        {
            if (str == null)
                str = new StringBuilder();

            str.AppendLine($"Message: {ex.Message}");
            str.AppendLine($"StackTrace: {ex.StackTrace}");

            if (ex.InnerException != null)
                str.AppendLine(ConcactException(ex.InnerException, str));

            return str.ToString();
        }
    }
}
