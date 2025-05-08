using System.Text.Json;
using ListaDeTarefas.Models;

namespace ListaDeTarefas.Helpers;

public static class StorageHelper
{
    // Caminho do arquivo JSON onde as tarefas serão salvas
    private static string Caminho => Path.Combine(FileSystem.AppDataDirectory, "tarefas.json");

    // Método para salvar a lista de tarefas no arquivo JSON
    public static async Task SalvarAsync(IEnumerable<Tarefa> tarefas)
    {
        var json = JsonSerializer.Serialize(tarefas);
        await File.WriteAllTextAsync(Caminho, json);
    }

    // Método para carregar a lista de tarefas do arquivo JSON
    public static async Task<List<Tarefa>> CarregarAsync()
    {
        if (!File.Exists(Caminho))
            return new List<Tarefa>();

        var json = await File.ReadAllTextAsync(Caminho);
        return JsonSerializer.Deserialize<List<Tarefa>>(json) ?? new List<Tarefa>();
    }
}
