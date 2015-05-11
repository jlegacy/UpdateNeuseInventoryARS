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
	public partial class ARSTempDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    #endregion
		
		public ARSTempDataContext() : 
				base(global::UpdateNeuseInventory.Properties.Settings.Default.nssncConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public ARSTempDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public ARSTempDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public ARSTempDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public ARSTempDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<NeuseARSTemp> NeuseARSTemps
		{
			get
			{
				return this.GetTable<NeuseARSTemp>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.NeuseARSTemp")]
	public partial class NeuseARSTemp
	{
		
		private string _upc;
		
		private System.Nullable<decimal> _qty;
		
		public NeuseARSTemp()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_upc", DbType="NVarChar(MAX)")]
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
					this._upc = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_qty", DbType="Decimal(18,4)")]
		public System.Nullable<decimal> qty
		{
			get
			{
				return this._qty;
			}
			set
			{
				if ((this._qty != value))
				{
					this._qty = value;
				}
			}
		}
	}
}
#pragma warning restore 1591
