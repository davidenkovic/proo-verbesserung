using Core.Dtos;
using Core.Entities;
using Persistence;
using RoomBooking.Wpf.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WpfApp.Common;
using WpfApp.Views;

namespace WpfApp.ViewModels
{
    public class RoomTypeDescription
    {
        public RoomType? RoomType { get; set; } 
        public string Description { get; set; } = string.Empty;

    }

    public class MainWindowViewModel : BaseViewModel
    {

        public RoomDTO SelectedRow { get; set; }

        private RoomTypeDescription _selectedRow;
        public RoomTypeDescription SelectedType 
        {
            get => _selectedRow;
            set
            {
                _selectedRow = value;
                OnPropertyChanged();
                LoadDataAsync();
            }
        }
        private string _roomNumber;

        public string RoomNumber
        {
            get => _roomNumber;
            set
            {
                _roomNumber = value;
                OnPropertyChanged();
                LoadDataAsync();
            }
        }

        

        public ICommand CheckInCommand { get; set; }


        public ObservableCollection<RoomDTO> Rooms { get; set; }

        public ObservableCollection<Customer> Customers { get; set; }
        public List<RoomTypeDescription> RoomTypes { get; } = new()
        {
            new() { RoomType = null, Description = "Alle" },
            new() { RoomType = RoomType.Standard, Description = RoomType.Standard.ToString() },
            new() { RoomType = RoomType.Premium, Description = RoomType.Premium.ToString() },
            new() { RoomType = RoomType.Deluxe, Description = RoomType.Deluxe.ToString() },
            new() { RoomType = RoomType.Suite, Description = RoomType.Suite.ToString() },
        };


        public MainWindowViewModel(IWindowController windowController) : base(windowController)
        {
            Rooms = new ObservableCollection<RoomDTO>();
            Customers = new ObservableCollection<Customer>();
            SelectedType = new RoomTypeDescription();
            SelectedType.Description = "Alle";
            SelectedType.RoomType = null;

            RoomNumber = null;

            CheckInCommand = new RelayCommand( async _ => await CheckIn(),_ => SelectedRow != null);
        }

        private void CheckinBooking()
        {
            MessageBox.Show($"Todo", "TODO");
        }


        public async Task LoadDataAsync()
        {
            await using var uow = new UnitOfWork();
            var temp = await uow.Bookings.GetCustomers();
            foreach (var item in temp)
            {
                Customers.Add(item);
            }
            Rooms.Clear();

            if (SelectedType.Description == "Alle" && RoomNumber == null)
            {
                Rooms.Clear();
                List<RoomDTO> list = await uow.Rooms.GetRoomsAsync();

                foreach (RoomDTO room in list)
                {
                    Rooms.Add(room);
                }
            }
            if (SelectedType.Description != "Alle" && RoomNumber == null)
            {
                Rooms.Clear();
                List<RoomDTO> list = await uow.Rooms.GetRoomsAsync();

                foreach (RoomDTO room in list)
                {
                    if (room.RoomType == SelectedType.RoomType)
                    {
                        Rooms.Add(room);
                    }
                }
            }
            if (SelectedType.Description == "Alle" && RoomNumber != null)
            {
                Rooms.Clear();
                List<RoomDTO> list = await uow.Rooms.GetRoomsAsync();

                foreach (RoomDTO room in list)
                {
                    if (room.RoomNumber.Contains(RoomNumber))
                    {
                        Rooms.Add(room);
                    }
                }
            }
            if (SelectedType.Description != "Alle" && RoomNumber != null)
            {
                Rooms.Clear();
                List<RoomDTO> list = await uow.Rooms.GetRoomsAsync();

                foreach (RoomDTO room in list)
                {
                    if (room.RoomType == SelectedType.RoomType && room.RoomNumber.Contains(RoomNumber))
                    {
                        Rooms.Add(room);

                    }
                }
            }

        }
        
        public async Task CheckIn()
        {
            var controller = new WindowController(new CheckoutBookingWindow());
            var viewModel = new CheckInBookingViewModel(controller, SelectedRow, Customers);
            controller.ShowDialog(viewModel);
            await LoadDataAsync();
        }

    }
}
