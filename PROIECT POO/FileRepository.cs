using System.Text.Json;

namespace PROIECT_POO;

public class FileRepository<T>:IRepository<T>
{
    private readonly string _path;
    private readonly ILogger<FileRepository<T>> _logger;

    public List<T> GettAll()
    {
        try
        {
            if (!File.Exists(_path))
                return new List<T>();

            return JsonSerializer.Deserialize<List<T>>(File.ReadAllText(_path));
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return new List<T>();
        }
    }
}