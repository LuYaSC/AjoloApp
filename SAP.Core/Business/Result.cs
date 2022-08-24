using System;

namespace SAP.Core.Business
{
    public class Result<BODY>
    {
        private Result()
        {
            IsOk = true;
            Message = null;
        }

        private Result(BODY body) : this() => Body = body;

        private Result(string message, bool isOk) : this()
        {
            Message = message;
            IsOk = isOk;
        }

        public bool IsOk { get; private set; }

        public string Message { get; private set; }

        public BODY Body { get; private set; }

        public static Result<BODY> SetOk(BODY body)
        {
            GC.Collect();
            var memoryToDelete = GC.GetTotalMemory(true);
            GC.RemoveMemoryPressure(memoryToDelete);
            var result = new Result<BODY>(body);
            return result;
        }

        public static Result<BODY> SetError(string message)
        {
            GC.Collect();
            var memoryToDelete = GC.GetTotalMemory(true);
            GC.RemoveMemoryPressure(memoryToDelete);
            var result = new Result<BODY>(message, false);
            return result;
        }
    }
}
