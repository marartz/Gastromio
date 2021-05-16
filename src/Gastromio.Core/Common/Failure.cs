namespace Gastromio.Core.Common
{
    public abstract class Failure
    {
        public string Code
        {
            get { return GetType().Name; }
        }

        public string Message
        {
            get { return ToString(); }
        }
    }
}
