namespace Website.Domain.Interpreters;

public interface IPostInterpreter
{
    string Interpret(string postData, string token);
}