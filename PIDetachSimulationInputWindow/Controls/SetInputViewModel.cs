using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PIDetachSimulationInputWindow.Controls
{
    public class SetInputViewModel : ObservableObject
    {
		public EventHandler SetValueEvent;

		private int _id;
        private bool _value;
        private string _name;

		public SetInputViewModel(int id)
		{
			Id = id;
		}
        public int Id
		{
			get 
			{
				return _id; 
			}
			set 
			{
				_id = value;
			}
		}

		public bool Value
		{
			get 
			{ 
				return _value; 
			}
			set 
			{
				_value = value; 
				OnPropertyChanged();
			}
		}

		public string Name
		{
			get 
			{
				return _name; 
			}
			set
			{
				_name = value; 
				OnPropertyChanged();
			}
		}
		public ICommand SetValueCommand
		{
			get
			{
				return new RelayCommand(() =>
				{
					Value = !Value;
					SetValueEvent?.Invoke(this, EventArgs.Empty);
				});
			}
		}
	}
}
