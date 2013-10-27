using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Data.Linq.Mapping;
using System.ComponentModel;

namespace TaskListDemo.Data {
  [Table]
  public class TaskModel : INotifyPropertyChanged, INotifyPropertyChanging {
    [Column(IsPrimaryKey = true,
      IsDbGenerated = true,
      DbType = "INT NOT NULL Identity",
      CanBeNull = false,
      AutoSync = AutoSync.OnInsert)]
    public int Id {
      get { 
        return _id; 
      }
      set {
        if (_id != value) {
          NotifyPropertyChanging("Id");
          _id = value;
          NotifyPropertyChanged("Id");
        }
      }
    }
    private int _id;

    [Column]
    public string Title {
      get {
        return _title;
      }
      set {
        if (_title != value) {
          NotifyPropertyChanging("Title");
          _title = value;
          NotifyPropertyChanged("Title");
        }
      }
    }
    private string _title;

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
