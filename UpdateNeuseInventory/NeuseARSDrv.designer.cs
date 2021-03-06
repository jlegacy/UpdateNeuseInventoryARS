﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UpdateNeuseInventory
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="nssnc")]
	public partial class NeuseARSDrvDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertNeuseARSDrv(NeuseARSDrv instance);
    partial void UpdateNeuseARSDrv(NeuseARSDrv instance);
    partial void DeleteNeuseARSDrv(NeuseARSDrv instance);
    #endregion
		
		public NeuseARSDrvDataContext() : 
				base(global::UpdateNeuseInventory.Properties.Settings.Default.nssncConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public NeuseARSDrvDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public NeuseARSDrvDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public NeuseARSDrvDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public NeuseARSDrvDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<NeuseARSDrv> NeuseARSDrvs
		{
			get
			{
				return this.GetTable<NeuseARSDrv>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.NeuseARSDrv")]
	public partial class NeuseARSDrv : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private string _upc;
		
		private decimal _qty;
		
		private string _prc;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnupcChanging(string value);
    partial void OnupcChanged();
    partial void OnqtyChanging(decimal value);
    partial void OnqtyChanged();
    partial void OnprcChanging(string value);
    partial void OnprcChanged();
    #endregion
		
		public NeuseARSDrv()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_upc", DbType="NVarChar(25) NOT NULL", CanBeNull=false, IsPrimaryKey=true)]
		public string upc
		{
			get
			{
				return this._upc;
			}
			set
			{
				if ((this._upc != value))
				{
					this.OnupcChanging(value);
					this.SendPropertyChanging();
					this._upc = value;
					this.SendPropertyChanged("upc");
					this.OnupcChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_qty", DbType="Decimal(18,4) NOT NULL", IsPrimaryKey=true)]
		public decimal qty
		{
			get
			{
				return this._qty;
			}
			set
			{
				if ((this._qty != value))
				{
					this.OnqtyChanging(value);
					this.SendPropertyChanging();
					this._qty = value;
					this.SendPropertyChanged("qty");
					this.OnqtyChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_prc", DbType="NVarChar(1)")]
		public string prc
		{
			get
			{
				return this._prc;
			}
			set
			{
				if ((this._prc != value))
				{
					this.OnprcChanging(value);
					this.SendPropertyChanging();
					this._prc = value;
					this.SendPropertyChanged("prc");
					this.OnprcChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
#pragma warning restore 1591
