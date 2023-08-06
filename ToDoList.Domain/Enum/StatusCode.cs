namespace ToDoList.Domain.Enum;

public enum StatusCode
{
    OK = 200,
    TaskNotFound = 2,
    IternalServerError = 500,
    TaskAlreadyYet = 1
}