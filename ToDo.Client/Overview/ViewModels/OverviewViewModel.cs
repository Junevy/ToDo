using System.Collections.ObjectModel;
using ToDo.Client.Models;
using ToDo.WebAPI.DTOs;

namespace ToDo.Client.Overview.ViewModels
{
    public class OverviewViewModel
    {
        public ObservableCollection<PriorityDTO> TestList { get; set; } = [];

        public OverviewViewModel()
        {
            //TestList.Add("hello");
            //TestList.Add("hello");
            //TestList.Add("hello");
            //TestList.Add("hello");
            //TestList.Add("hello");
            //TestList.Add("hello");
            //TestList.Add("hello");
        }
    }
}
