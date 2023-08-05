using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ToDoList.DAL.Interfaces;
using ToDoList.Domain.Entity;
using ToDoList.Domain.Enum;
using ToDoList.Domain.Response;
using ToDoList.Domain.ViewModels.Task;
using ToDoList.Service.Interfaces;

namespace ToDoList.Service.Implementations;

public class TaskService : ITaskService
{
    private readonly IBaseRepository<TaskEntity> _taskRepository;
    private ILogger<TaskService> _logger;

    public TaskService(IBaseRepository<TaskEntity> taskRepository,
        ILogger<TaskService> logger)
    {
        _taskRepository = taskRepository;
        _logger = logger;
    }

    public async Task<IBaseResponse<TaskEntity>> Create(CreateTaskViewModel model)
    {
        try
        {
            _logger.LogInformation($"Запрос на создание задачи - {model.Name}");

            var task = await _taskRepository.GetAll()
                .Where(x => x.Created.Date == DateTime.Today)
                .FirstOrDefaultAsync(x => x.Name == model.Name);
            if (task != null)
            {
                return new BaseResponse<TaskEntity>()
                {
                    Description = "Задача с таким именем уже есть",
                    StatusCode = StatusCode.TaskAlreadyYet
                };
            }
                task = new TaskEntity()
                {
                    Name = model.Name,
                    DoneTask = false,
                    Created = DateTime.Now
                    
                };
                await _taskRepository.Create(task);
                _logger.LogInformation($"Задача добавлена - {task.Created} - {task.Name}");
                return new BaseResponse<TaskEntity>()
                {
                    StatusCode = StatusCode.OK,
                    Description = "Задача успешно добавлена"
                };
            
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"[TaskService.Create]: {ex.Message}");
            return new BaseResponse<TaskEntity>()
            {
                StatusCode = StatusCode.IternalServerError,
                Description = "Ошибка связи с сервером"
            };
        }

    }
}