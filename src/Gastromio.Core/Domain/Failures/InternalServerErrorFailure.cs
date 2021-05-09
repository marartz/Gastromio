using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class InternalServerErrorFailure : Failure
    {
        public override string ToString()
        {
            return "Es ist ein technischer Fehler aufgetreten. Bitte versuchen Sie es erneut bzw. kontaktieren Sie uns, wenn das Problem anhält.";
        }
    }
}
