namespace Website.Services.Interpreters;

public interface IPostInterpreter
{
    string Interpret(string postData, string token);
}