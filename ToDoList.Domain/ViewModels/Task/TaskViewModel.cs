using System.ComponentModel.DataAnnotations;

namespace ToDoList.Domain.ViewModels.Task;

public class TaskViewModel
{
    public long Id { get; set; }
    
    [Display(Name = "Название")]
    public string Name { get; set; }
    
    [Display(Name = "Статус")]
    public string DoneTask { get; set; }
    
    [Display(Name = "Дата создания")]
    public string Created { get; set; }
}