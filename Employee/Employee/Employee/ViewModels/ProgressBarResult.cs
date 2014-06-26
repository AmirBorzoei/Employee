using System;
using Caliburn.Micro;

namespace Employees.ViewModels
{
    //internal class ProgressBarResult : IResult
    //{
    //    private readonly bool _start;

    //    public ProgressBarResult(bool start = false)
    //    {
    //        _start = start;
    //    }

    //    public event EventHandler<ResultCompletionEventArgs> Completed = delegate { };

    //    public void Execute(ActionExecutionContext context)
    //    {
    //        var shell = IoC.Get<IShellViewModel>();
    //        if (shell != null)
    //        {
    //            if (_start)
    //                shell.ProgressBarShow();
    //            else
    //                shell.ProgressBarHide();
    //        }

    //        Completed(this, new ResultCompletionEventArgs());
    //    }

    //    public static IResult Show()
    //    {
    //        return new ProgressBarResult(true);
    //    }

    //    public static IResult Hide()
    //    {
    //        return new ProgressBarResult();
    //    }
    //}
}