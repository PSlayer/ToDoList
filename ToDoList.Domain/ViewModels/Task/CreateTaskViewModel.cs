namespace ToDoList.Domain.ViewModels.Task;

public class CreateTaskViewModel
{
    public string Name { get; set; }

    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            throw new ArgumentNullException(Name, "Укажите название задачи");
        }
    }
}