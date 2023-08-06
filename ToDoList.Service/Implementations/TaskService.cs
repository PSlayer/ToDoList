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
            model.Validate();
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
                Description = $"{ex.Message}"
            };
        }

    }

    public async Task<IBaseResponse<bool>> EndTask(long id)
    {
        try
        {
            var task = await _taskRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);
            if (task == null)
            {
                return new BaseResponse<bool>()
                {
                    StatusCode = StatusCode.TaskNotFound,
                    Description = "Задача не найдена"
                };

            }

            task.DoneTask = true;
            await _taskRepository.Update(task);
            return new BaseResponse<bool>()
            {
                StatusCode = StatusCode.OK,
                Description = "Задача завершена"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"[TaskService.EndTask]: {ex.Message}");
            return new BaseResponse<bool>()
            {
                StatusCode = StatusCode.IternalServerError,
                Description = $"{ex.Message}"
            };
        }
    }
    public async Task<IBaseResponse<bool>> DeleteTask(long id)
    {
        try
        {
            var task = await _taskRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);
            if (task == null)
            {
                return new BaseResponse<bool>()
                {
                    StatusCode = StatusCode.TaskNotFound,
                    Description = "Задача не найдена"
                };

            }
            await _taskRepository.Delete(task);
            return new BaseResponse<bool>()
            {
                StatusCode = StatusCode.OK,
                Description = "Задача удалена"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"[TaskService.EndTask]: {ex.Message}");
            return new BaseResponse<bool>()
            {
                StatusCode = StatusCode.IternalServerError,
                Description = $"{ex.Message}"
            };
        }
    }
    public async Task<IBaseResponse<IEnumerable<TaskViewModel>>> GetTasks()
    {
        try
        {
            var tasks = await _taskRepository.GetAll()
                .Select(x => new TaskViewModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    DoneTask = x.DoneTask == true ? "Готова" : "Не готова",
                    Created = x.Created.ToLongDateString()
                })
                .ToListAsync();
            return new BaseResponse<IEnumerable<TaskViewModel>>()
            {
                Data = tasks,
                StatusCode = StatusCode.OK
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"[TaskService.GetTask]: {ex.Message}");
            return new BaseResponse<IEnumerable<TaskViewModel>>()
            {
                StatusCode = StatusCode.IternalServerError,
                Description = $"{ex.Message}"
            };
        }
    }
}