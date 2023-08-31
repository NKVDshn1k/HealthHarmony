using DynamicData.Binding;
using HealthHarmony.Base;
using HealthHarmony.Model;
using ReactiveUI;
using System;
using System.ComponentModel;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;
using System.Windows.Input;
using System.Threading.Tasks;

namespace HealthHarmony.ViewModel;

public class MainVindowViewModel : ReactiveObject
{
    public DbDomainUnionUsersModel UsersModel { get; }
    private CollectionViewSource _itemSourceList;
    private ICollectionView _usersListView;

    public ICollectionView UsersListView
    {
        get => _usersListView;
        set => this.RaiseAndSetIfChanged(ref _usersListView, value);
    }

    private bool _isAdditing;
    private User _editableUser;

    private int _firstRowToSwapIndex;
    private int _secondRowToSwapIndex;
    private bool _isDragging = false;
    private bool _isFirstSetted = false;

    public MainVindowViewModel()
    {
        UsersModel = new DbDomainUnionUsersModel();
        _itemSourceList = new CollectionViewSource();

        Load();

        this.WhenAnyValue(
            x => x.NameFilter,
            x => x.SernameFilter,
            x => x.PhoneFilter,
            x => x.EmailFilter)
            .Throttle(TimeSpan.FromMilliseconds(500))
            .ObserveOnDispatcher()
            .Subscribe(x => UsersListView?.Refresh());

        this.WhenPropertyChanged(x => x.SelectedUserIndex)
            .Where(x => _isDragging && !_isFirstSetted && SelectedUserIndex != -1)
            .Subscribe(x =>
            {
                _firstRowToSwapIndex = SelectedUserIndex;
                _isFirstSetted = true;
            });

        IsAdditingOrEditing = false;

        ShowAddUserFormCommand = ReactiveCommand.CreateFromTask(ShowAddUserForm);
        ShowEditeUserFormCommand = ReactiveCommand.CreateFromTask(ShowEditeUserForm);
        CloseFormCommand = ReactiveCommand.CreateFromTask<object>(FormClosing);
        DeleteUserCommand = ReactiveCommand.CreateFromTask(DeleteUser);
        ReloadCommand = ReactiveCommand.CreateFromTask(Reload);
    }

    //вызываеться из MainWindow
    public void SubscribeToSortDirectionsClear(DataGrid dataGrid)
    {
        this.WhenPropertyChanged(x => x.AllowSort)
        .Subscribe(x =>
        {
            if (!AllowSort)
            {
                //Убираем стрелочки 
                foreach (var c in dataGrid.Columns)
                    c.SortDirection = null;

                //Убираем саму сортировку
                UsersListView?.SortDescriptions.Clear();
            }

        });
    }

    private async Task Load()
    {
        await Reload();
        this.WhenPropertyChanged(x => x.IsFiltering)
            .Subscribe(x =>
            {
                if (IsFiltering)
                    UsersListView.Filter = UsersFilter;
                else
                    UsersListView.Filter = null;
            });
    }
    private async Task Reload()
    {
        await UsersModel.Reload();
        _itemSourceList.Source = UsersModel.Collection;
        UsersListView = _itemSourceList.View;
        UsersListView.Filter = UsersFilter;
    }



    #region Fields

    private string _nameFilter;
    public string NameFilter
    {
        get => _nameFilter;
        set => this.RaiseAndSetIfChanged(ref _nameFilter, value);
    }

    private string _sernameFilter;
    public string SernameFilter
    {
        get => _sernameFilter;
        set => this.RaiseAndSetIfChanged(ref _sernameFilter, value);
    }

    private string _phoneFilter;
    public string PhoneFilter
    {
        get => _phoneFilter;
        set => this.RaiseAndSetIfChanged(ref _phoneFilter, value);
    }

    private string _emailFilter;
    public string EmailFilter
    {
        get => _emailFilter;
        set => this.RaiseAndSetIfChanged(ref _emailFilter, value);
    }

    private bool _isFiltering;
    public bool IsFiltering
    {
        get => _isFiltering;
        set => this.RaiseAndSetIfChanged(ref _isFiltering, value);
    }

    private bool _allowSort;
    public bool AllowSort
    {
        get => _allowSort;
        set => this.RaiseAndSetIfChanged(ref _allowSort, value);
    }

    private bool _isAdditingOrEditing;
    public bool IsAdditingOrEditing
    {
        get => _isAdditingOrEditing;
        set => this.RaiseAndSetIfChanged(ref _isAdditingOrEditing, value);
    }

    private string _formText;
    public string FormText
    {
        get => _formText;
        set => this.RaiseAndSetIfChanged(ref _formText, value);
    }

    private bool _isAdding;
    public bool IsAdding
    {
        get => _isAdding;
        set => this.RaiseAndSetIfChanged(ref _isAdding, value);
    }

    private bool _isEditing;
    public bool IsEditing
    {
        get => _isEditing;
        set => this.RaiseAndSetIfChanged(ref _isEditing, value);
    }

    private string _currentSidebarTaskName;
    public string CurrentSidebarTaskName
    {
        get => _currentSidebarTaskName;
        set => this.RaiseAndSetIfChanged(ref _currentSidebarTaskName, value);
    }

    private string _nameSidebar;
    public string NameSidebar
    {
        get => _nameSidebar;
        set => this.RaiseAndSetIfChanged(ref _nameSidebar, value);
    }

    private string _sernameSidebar;
    public string SernameSidebar
    {
        get => _sernameSidebar;
        set => this.RaiseAndSetIfChanged(ref _sernameSidebar, value);
    }

    private string _phoneSidebar;
    public string PhoneSidebar
    {
        get => _phoneSidebar;
        set => this.RaiseAndSetIfChanged(ref _phoneSidebar, value);
    }

    private string _emailSidebar;
    public string EmailSidebar
    {
        get => _emailSidebar;
        set => this.RaiseAndSetIfChanged(ref _emailSidebar, value);
    }

    private User _selectedUser;
    public User SelectedUser
    {
        get => _selectedUser;
        set => this.RaiseAndSetIfChanged(ref _selectedUser, value);
    }

    private int _selectedUserIndex;
    public int SelectedUserIndex
    {
        get => _selectedUserIndex;
        set => this.RaiseAndSetIfChanged(ref _selectedUserIndex, value);
    }


    #endregion

    #region Commands

    public ICommand ReloadCommand { get; }
    public ICommand ShowAddUserFormCommand { get; }
    private async Task ShowAddUserForm() =>
        await Task.Run(() =>
        {
            IsAdditingOrEditing = true;
            FormText = "Add";
            _isAdditing = true;

            NameSidebar = null;
            SernameSidebar = null;
            PhoneSidebar = null;
            EmailSidebar = null;
        });

    public ICommand ShowEditeUserFormCommand { get; }
    private async Task ShowEditeUserForm() =>
        await Task.Run(() =>
        {

            if (SelectedUser == null)
                return;

            IsAdditingOrEditing = true;
            FormText = "Edite";
            _isAdditing = false;
            _editableUser = SelectedUser;

            NameSidebar = SelectedUser.Name;
            SernameSidebar = SelectedUser.Sername;
            PhoneSidebar = SelectedUser.Phone;
            EmailSidebar = SelectedUser.Email;
        });


    public ICommand CloseFormCommand { get; }
    private async Task FormClosing(object param)
    {
        bool isConfirm = (bool)param;
        if (!isConfirm)
        {
            IsAdditingOrEditing = false;
            return;
        }

        UserDataPackage package = new UserDataPackage()
        {
            Id = _isAdditing ? 0 : SelectedUser?.Id ?? 0,
            Name = NameSidebar,
            Sername = SernameSidebar,
            Phone = PhoneSidebar,
            Email = EmailSidebar
        };

        var validation = package.Validate();

        if (!validation.isValide)
        {
            MessageBox.Show(validation.whyNot);
            return;
        }

        if (_isAdditing)
            await UsersModel.Add(package);
        else
            await UsersModel.Edite(SelectedUser,package);
        

        IsAdditingOrEditing = false;
    }

    public ICommand DeleteUserCommand { get; }

    private async Task DeleteUser()
    {
        if (SelectedUser != null &&
            MessageBox.Show("Are you shure you want to delete selected item?",
            "Are you shure?", MessageBoxButton.YesNo)
            == MessageBoxResult.Yes)
        {
            await UsersModel.Delete(SelectedUser);
        }
    }

    #endregion

    private bool UsersFilter(object User)
    {
        User user = User as User;

        var filtering = (string field, string filter) =>
            string.IsNullOrEmpty(filter) || field.Contains(filter);

        return
            filtering(user.Name, NameFilter) &&
            filtering(user.Email, EmailFilter) &&
            filtering(user.Sername, SernameFilter) &&
            filtering(user.Phone, PhoneFilter);
    }


    #region EventsFuncs

    public void OnMouseLeftButtonDown(MouseButtonEventArgs e)
    {
        if (IsFiltering || AllowSort)
            return;

        _isDragging = true;
    }
    public void OnMouseLeftButtonUp(MouseButtonEventArgs e)
    {
        if (!_isDragging || AllowSort || SelectedUserIndex == -1)
            goto end;

        _isDragging = false;

        _secondRowToSwapIndex = SelectedUserIndex;

        if (_firstRowToSwapIndex != _secondRowToSwapIndex)
        {
            UsersModel.Swap(_firstRowToSwapIndex, _secondRowToSwapIndex);
            //Сброс выделений
            UsersListView.Refresh();
        }
    end:
        _isFirstSetted = false;
        _isDragging = false;
    }

    #endregion
}
