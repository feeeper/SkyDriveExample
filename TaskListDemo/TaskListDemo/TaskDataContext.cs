﻿using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Data.Linq;

namespace TaskListDemo.Data {
  public class TaskDataContext : DataContext {
    public static string connectionString = "Data Source=isostore:/tasks.sdf;";

    public TaskDataContext(string connectionString)
      : base(connectionString) { }

    public TaskDataContext()
      : base(connectionString) { }

    public Table<TaskModel> Tasks;
  }
}
