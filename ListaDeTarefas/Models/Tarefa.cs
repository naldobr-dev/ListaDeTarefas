using System.ComponentModel;
using System.Windows.Input;
using System.Text.Json.Serialization;

namespace ListaDeTarefas.Models;

public class Tarefa : INotifyPropertyChanged
{
    private bool concluida;

    public string Nome { get; set; }

    public bool Concluida
    {
        get => concluida;
        set
        {
            if (concluida != value)
            {
                concluida = value;
                OnPropertyChanged(nameof(Concluida));

                // Notifica que essa tarefa foi modificada
                ConcluidaChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    // Evento para notificar mudança de estado
    public event EventHandler? ConcluidaChanged;

    [JsonIgnore]
    public ICommand DeleteCommand { get; set; }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string nome)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nome));
    }
}
