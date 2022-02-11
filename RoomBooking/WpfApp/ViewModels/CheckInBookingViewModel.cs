using Core.Contracts;
using Core.Dtos;
using Core.Entities;
using Persistence;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfApp.Common;

namespace WpfApp.ViewModels
{
    public class CheckInBookingViewModel : BaseViewModel
    {
        public RoomDTO SelectedRoom { get; set; }
        public DateTime CurrentDate { get; set; }

        public ICommand CancelCommand { get; set; }

        public ICommand SaveCommand { get; set; }

        private Customer _selectedCustomer;
        public Customer SelectedCustomer
        {
            get => _selectedCustomer;
            set
            {
                _selectedCustomer = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Customer> ShowCustomers { get; set; }


        public CheckInBookingViewModel(IWindowController controller, RoomDTO selectedRow, ObservableCollection<Customer> customers) : base(controller)
        {
            SelectedRoom = selectedRow;
            CurrentDate = DateTime.Now;
            CancelCommand = new RelayCommand(async _ => await Cancel(), null);
            SaveCommand = new RelayCommand(async _ => await Save(), null);
            ShowCustomers = customers;

        }

        public async Task Cancel()
        {
            Controller.CloseWindow();
        }

        public async Task Save()
        {
            await using var uow = new UnitOfWork();
            Controller.CloseWindow();
        }
    }
}
