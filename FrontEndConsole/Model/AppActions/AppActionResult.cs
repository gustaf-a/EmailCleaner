namespace FrontEndConsole.Model.AppActions
{
    internal class AppActionResult
    {
        private readonly bool _isSuccess;
        private readonly Exception _exception;

        public ApplicationAction? NextAction;

        public List<string> FilePaths;

        public AppActionResult(Exception exception)
        {
            _exception = exception ?? new Exception("Null exception received to AppActionResult.");
            _isSuccess = false;
        }

        public AppActionResult(ApplicationAction nextAction)
        {
            NextAction = nextAction;
            _isSuccess = true;
        }

        public AppActionResult(ApplicationAction nextAction, List<string> filePaths) : this(nextAction)
        {
            FilePaths = filePaths;
        }

        public void EnsureSuccess()
        {
            if (_isSuccess)
                return;

            throw _exception;
        }
    }
}
