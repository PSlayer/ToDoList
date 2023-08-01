namespace ToDoList.Domain.Entity;

public class TaskEntity
{
    public long Id { get; set; }
    
    public string Name { get; set; }
    
    public bool doneTask { get; set; }
}