using Caliburn.Micro;

namespace Employees.Shared.Services
{
    public interface IProgressBarService
    {
        void ProgressBarShow();
        void ProgressBarHide();
    }

    public class ProgressBarService
    {
        public static void Show()
        {
            var progressBarService = IoC.Get<IProgressBarService>();
            if (progressBarService != null)
                progressBarService.ProgressBarShow();
        }

        public static void Hide()
        {
            var progressBarService = IoC.Get<IProgressBarService>();
            if (progressBarService != null)
                progressBarService.ProgressBarHide();
        }
    }
}