using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using TaskListDemo.Data;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace TaskListDemo {
  public partial class MainPage : PhoneApplicationPage, INotifyPropertyChanged {
    TaskDataContext db = new TaskDataContext();

    private ObservableCollection<TaskModel> _tasks1;
    public ObservableCollection<TaskModel> TaskList {
      get { return _tasks1; }
      set {
        if (_tasks1 == value) return;
        _tasks1 = value;
        NotifyPropertyChanged("TaskList");
      }
    }
    // Constructor
    public MainPage() {
      InitializeComponent();
      Init();
    }

    private void Init() {
      if (!db.DatabaseExists()) {
        db.CreateDatabase();
      }
      if (TaskList == null) {
        TaskList = new ObservableCollection<TaskModel>(from task in db.Tasks select task);
      }
      Tasks.DataContext = this;
    }

    private void NewTask_Click(object sender, RoutedEventArgs e) {
      TaskModel task = new TaskModel();
      task.Title = Guid.NewGuid().ToString();
      db.Tasks.InsertOnSubmit(task);
      db.SubmitChanges();
      TaskList = new ObservableCollection<TaskModel>(from t in db.Tasks select t);
    }

    #region "Реализация интерфейсов"
    public event PropertyChangedEventHandler PropertyChanged;

    private void NotifyPropertyChanged(string propertyName) {
      if (PropertyChanged != null) {
        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
      }
    }

    public event PropertyChangingEventHandler PropertyChanging;

    private void NotifyPropertyChanging(string propertyName) {
      if (PropertyChanging != null) {
        PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
      }
    }
    #endregion
  }
}