using System.Collections.Generic;
using System;

public class ListTask
{
    public DateTime CreationDate { get; set; }
    public string Name { get; set; }
    public List<ListTask> SubTasks { get; set; }
    public bool IsDone { get; set; }
}