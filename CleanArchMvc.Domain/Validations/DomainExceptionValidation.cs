namespace CleanArchMvc.Domain.Validations
{
    public class DomainExceptionValidation : Exception
    {
        public DomainExceptionValidation(string messageError) : base(messageError) { }

        public static void When(bool hasError, string message)
        {
            if (hasError)
                throw new DomainExceptionValidation(message);
        }
    }
}
