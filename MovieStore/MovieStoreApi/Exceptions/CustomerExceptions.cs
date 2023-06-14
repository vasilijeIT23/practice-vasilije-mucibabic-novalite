namespace MovieStoreApi.Exceptions
{
    public class InvalidInputValueException : Exception
    {
        public InvalidInputValueException() : base(String.Format("Input request values not valid.")) { }
    }

    public class CustomerDoesntExistException : Exception
    {
        public CustomerDoesntExistException() : base(String.Format("Customer with given id doesnt exist. ")) { }
    }

    public class MovieDoesntExistException : Exception
    {
        public MovieDoesntExistException() : base(String.Format("Movie with given id doesnt exist. ")) { }
    }

    public class RequirementsNotSatisfiedException : Exception
    {
        public RequirementsNotSatisfiedException() : base(String.Format("Customer doesnt satisfy requirements needed for this action to be succesfully completed. ")) { }
    }
}
