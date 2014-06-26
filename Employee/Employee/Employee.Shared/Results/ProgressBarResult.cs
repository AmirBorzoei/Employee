using System;
using Caliburn.Micro;

namespace Employees.Shared.Results
{
    public interface IProgressBarService
    {
        void ProgressBarShow();
        void ProgressBarHide();
    }

    public class ProgressBarResult : IResult
    {
        private readonly bool _start;

        public ProgressBarResult(bool start = false)
        {
            _start = start;
        }

        public event EventHandler<ResultCompletionEventArgs> Completed = delegate { };

        public void Execute(ActionExecutionContext context)
        {
            var progressBarService = IoC.Get<IProgressBarService>();
            if (progressBarService != null)
            {
                if (_start)
                    progressBarService.ProgressBarShow();
                else
                    progressBarService.ProgressBarHide();
            }

            Completed(this, new ResultCompletionEventArgs());
        }

        public static IResult Show()
        {
            return new ProgressBarResult(true);
        }

        public static IResult Hide()
        {
            return new ProgressBarResult();
        }
    }
}