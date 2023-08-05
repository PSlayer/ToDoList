using ToDoList.Domain.Enum;

namespace ToDoList.Domain.Entity;

public class TaskEntity
{
    public long Id { get; set; }
    
    public string Name { get; set; }
    
    public bool DoneTask { get; set; }
    
    public DateTime Created { get; set; }
    
}