using System.Collections.Generic;
using System;
using System.ComponentModel;

public class ListTask : INotifyPropertyChanged
{
    private DateTime _creationDate;
    private string _name;
    private List<ListTask> _subTasks;
    private bool _isDone;

    public DateTime CreationDate
    {
        get { return _creationDate; }
        set
        {
            if (_creationDate != value)
            {
                _creationDate = value;
                OnPropertyChanged(nameof(CreationDate));
            }
        }
    }

    public string Name
    {
        get { return _name; }
        set
        {
            if (_name != value)
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
    }

    public List<ListTask> SubTasks
    {
        get { return _subTasks; }
        set
        {
            if (_subTasks != value)
            {
                _subTasks = value;
                OnPropertyChanged(nameof(SubTasks));
            }
        }
    }

    public bool IsDone
    {
        get { return _isDone; }
        set
        {
            if (_isDone != value)
            {
                _isDone = value;
                OnPropertyChanged(nameof(IsDone));
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
