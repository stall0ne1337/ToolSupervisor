using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolSupervisor
{
    [Serializable()]
    public class ManagedTool : INotifyPropertyChanged
    {
        public string Title { get; set; }
        public string Path { get; set; }
        public string Filename { get; set; }

        private ToolStatus status;
        public ToolStatus Status
        {
            get { return status; }
            set
            {
                status = value;
                OnPropertyChanged(nameof(Status));
            }
        }

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(String propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public enum ToolStatus
    {
        Running,
        Stopped,
        Unknown,
        NotFound
    }
}
