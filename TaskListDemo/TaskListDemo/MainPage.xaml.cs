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
using Microsoft.Live;
using Microsoft.Phone.BackgroundTransfer;
using System.IO.IsolatedStorage;
using System.IO;

namespace TaskListDemo {
  public partial class MainPage : PhoneApplicationPage, INotifyPropertyChanged {
    LiveConnectSession session;
    string sourceName = "tasks.sdf";
    string fileName = "tasks.sdf";

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
      using (var db = new TaskDataContext()) {
        if (!db.DatabaseExists()) {
          db.CreateDatabase();
        }
        if (TaskList == null) {
          TaskList = new ObservableCollection<TaskModel>(from task in db.Tasks select task);
        }
      }
      Tasks.DataContext = this;
    }

    private void NewTask_Click(object sender, RoutedEventArgs e) {
      TaskModel task = new TaskModel();
      task.Title = Guid.NewGuid().ToString();
      using (var db = new TaskDataContext()) {
        db.Tasks.InsertOnSubmit(task);
        db.SubmitChanges();
        TaskList = new ObservableCollection<TaskModel>(from t in db.Tasks select t);
      }
    }

    private void ClearTasks_Click(object sender, RoutedEventArgs e) {
      using (var db = new TaskDataContext()) {
        db.Tasks.DeleteAllOnSubmit(db.Tasks);
        db.SubmitChanges();
      }
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

    /// <summary>Обработчик клика на кнопку бэкапа в SkyDrive</summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ToSkyDrive_Click(object sender, RoutedEventArgs e) {
      BackupToSkyDrive();
    }
    private object _obj = new object();
    void BackupToSkyDrive() {
      IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication();
      // переносим файл sourceName в папку \shared\transfers\.
      var fileToSave = new IsolatedStorageFileStream(sourceName,
        FileMode.OpenOrCreate,
        FileAccess.ReadWrite,
        FileShare.None,
        store);

      var sourceFile = store.OpenFile("\\shared\\transfers\\" + fileName,
        FileMode.OpenOrCreate);

      fileToSave.CopyTo(sourceFile);
      fileToSave.Flush();
      sourceFile.Close();
      fileToSave.Close();

      //IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication();

      //var fileToSave = new IsolatedStorageFileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, store);
      //var sourceFile = store.OpenFile("\\shared\\transfers\\" + fileName, FileMode.OpenOrCreate);

      //if (sourceFile == null) {
      //  MessageBox.Show("not found");
      //}
      //else {
      //  fileToSave.CopyTo(sourceFile);
      //  fileToSave.Flush();
      //}
      //sourceFile.Close();
      //fileToSave.Close();

      // вся "магия" здесь :)
      try {
        LiveConnectClient client = new LiveConnectClient(session);
        // удаляем все запросы, чтоб не было ошибок дублей 
        // (добавьте Microsoft.Phone.BackgroundTransfer)
        if (BackgroundTransferService.Requests.Count() > 0) {
          foreach (var request in BackgroundTransferService.Requests) {
            BackgroundTransferService.Remove(request);
          }
        }
        // в колбэке можно, например, скрыть прогресс-бар.
        client.BackgroundUploadCompleted += client_BackgroundUploadCompleted;
        client.BackgroundUploadAsync("me/skydrive",
          new Uri("/shared/transfers/" + fileName,
          UriKind.RelativeOrAbsolute),
          OverwriteOption.Overwrite);

      }
      catch (Exception ex) {
        MessageBox.Show(ex.Message);
      }
    }

    void client_BackgroundUploadCompleted(object sender, LiveOperationCompletedEventArgs e) {
      MessageBox.Show("backuping complited");    
    }

    /// <summary>Восстановление из SkyDrive</summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void FromSkyDrive_Click(object sender, RoutedEventArgs e) {
      RestoreFromSkyDrive();
    }

    void RestoreFromSkyDrive() {
      LiveConnectClient client = new LiveConnectClient(session);
      string id = string.Empty;

      // колбэк получения файлов
      client.GetCompleted += (obj, args) => {
        try {
          List<object> items;
          // в args.Result["data"] лежат данные о файлах
          items = args.Result["data"] as List<object>;
          // переберем все файлы
          foreach (object item in items) {
            Dictionary<string, object> file = item as Dictionary<string, object>;
            // если нашли наш файл (с искомым именем)
            if (file["name"].ToString() == fileName) {
              // то запомним его id
              id = file["id"].ToString();
              // удалим все активные запросы, чтоб избежать ошибок 
              // (добавьте Microsoft.Phone.BackgroundTransfer)
              if (BackgroundTransferService.Requests.Count() > 0) {
                foreach (var request in BackgroundTransferService.Requests) {
                  BackgroundTransferService.Remove(request);
                }
              }

              // в колбэке, например, скроем прогресс-бар
              client.BackgroundDownloadCompleted += client_BackgroundDownloadCompleted;
              // отправим запрос на получение содержимого файла
              client.BackgroundDownloadAsync(String.Format("{0}/content", id),
                new Uri("\\shared\\transfers\\" + fileName,
                UriKind.RelativeOrAbsolute));
            }
          }
        }
        catch (Exception ex) {
          MessageBox.Show(ex.Message);
        }
      };

      if (client != null) {
        client.GetAsync("me/skydrive/files");
      }
    }

    void client_BackgroundDownloadCompleted(object sender, LiveOperationCompletedEventArgs e) {
      if (e.Error == null) {
        var stream = e.Result;

        using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication()) {
          var fileToSave = new IsolatedStorageFileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, storage);
          var sourceFile = storage.FileExists("\\shared\\transfers\\" + fileName) ? storage.OpenFile("\\shared\\transfers\\" + fileName, FileMode.Open) : null;

          if (sourceFile == null) {
            MessageBox.Show("not found");
          }
          else {
            sourceFile.CopyTo(fileToSave);
            sourceFile.Flush();
          }

          sourceFile.Close();
          fileToSave.Close();
        }
      }
      else {
        MessageBox.Show(e.Error.Message);
      }
      MessageBox.Show("restoring complited");
    }

    /// <summary>Обработчик кнопки логина в SkyDrive</summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void signInButton_SessionChanged(object sender, Microsoft.Live.Controls.LiveConnectSessionChangedEventArgs e) {
      if (e.Session != null && e.Status == LiveConnectSessionStatus.Connected) {
        // все хорошо - можем работать.
        session = e.Session;
      }
      else {
        session = null;
      }
    }
  }
}