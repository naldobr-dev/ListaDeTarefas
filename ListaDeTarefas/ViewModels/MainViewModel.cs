using ListaDeTarefas.Helpers;
using ListaDeTarefas.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace ListaDeTarefas.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    public ObservableCollection<Tarefa> Tarefas { get; set; } = new();

    // Comando para adicionar uma nova tarefa
    private string novaTarefa;
    public string NovaTarefa
    {
        get => novaTarefa;
        set
        {
            // Verifica se o valor é diferente do atual
            if (novaTarefa != value)
            {
                // Se o valor for diferente, atualiza a propriedade
                novaTarefa = value;
                // Notifica a mudança da propriedade
                OnPropertyChanged(nameof(NovaTarefa));
            }
        }
    }

    // Comando para adicionar uma nova tarefa
    public ICommand AdicionarCommand { get; }
    // Comando para remover as tarefas concluídas
    public ICommand LimparConcluidasCommand { get; }

    public MainViewModel()
    {
        AdicionarCommand = new Command(AdicionarTarefa);
        LimparConcluidasCommand = new Command(LimparConcluidas);

        CarregarTarefas();
    }

    // Método para carregar as tarefas do arquivo JSON
    private async void CarregarTarefas()
    {
        var lista = await StorageHelper.CarregarAsync();
        foreach (var tarefa in lista)
        {
            tarefa.DeleteCommand = new Command(() => RemoverTarefa(tarefa));
            tarefa.ConcluidaChanged += (_, __) => Salvar();
            Tarefas.Add(tarefa);
        }
    }

    // Método para adicionar uma nova tarefa
    private void AdicionarTarefa()
    {
        if (string.IsNullOrWhiteSpace(NovaTarefa)) return;

        var tarefa = new Tarefa
        {
            Nome = NovaTarefa,
            Concluida = false
        };

        tarefa.DeleteCommand = new Command(() => Tarefas.Remove(tarefa));
        tarefa.ConcluidaChanged += (_, __) => Salvar();

        Tarefas.Add(tarefa);
        NovaTarefa = string.Empty;

        Salvar();
    }

    // Método para remover uma tarefa
    private void RemoverTarefa(Tarefa tarefa)
    {
        Tarefas.Remove(tarefa);
        Salvar();
    }

    // Método para limpar as tarefas concluídas
    private void LimparConcluidas()
    {
        var concluidas = Tarefas.Where(t => t.Concluida).ToList();
        foreach (var t in concluidas)
            Tarefas.Remove(t);

        Salvar();
    }

    // Método para salvar as tarefas no arquivo JSON
    private async void Salvar()
    {
        await StorageHelper.SalvarAsync(Tarefas);
    }

    // Evento para notificar mudança de propriedade
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string nome)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nome));
    }
}