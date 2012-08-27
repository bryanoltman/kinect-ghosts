﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17929
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Ghosts
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
	
	
	public partial class GhostDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertGhostSkeletonSequence(GhostSkeletonSequence instance);
    partial void UpdateGhostSkeletonSequence(GhostSkeletonSequence instance);
    partial void DeleteGhostSkeletonSequence(GhostSkeletonSequence instance);
    partial void InsertGhostJoint(GhostJoint instance);
    partial void UpdateGhostJoint(GhostJoint instance);
    partial void DeleteGhostJoint(GhostJoint instance);
    partial void InsertGhostSkeleton(GhostSkeleton instance);
    partial void UpdateGhostSkeleton(GhostSkeleton instance);
    partial void DeleteGhostSkeleton(GhostSkeleton instance);
    #endregion
		
		public GhostDataContext() : 
				base(global::Ghosts.Properties.Settings.Default.GhostDBConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public GhostDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public GhostDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public GhostDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public GhostDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<GhostSkeletonSequence> GhostSkeletonSequences
		{
			get
			{
				return this.GetTable<GhostSkeletonSequence>();
			}
		}
		
		public System.Data.Linq.Table<GhostJoint> GhostJoints
		{
			get
			{
				return this.GetTable<GhostJoint>();
			}
		}
		
		public System.Data.Linq.Table<GhostSkeleton> GhostSkeletons
		{
			get
			{
				return this.GetTable<GhostSkeleton>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="")]
	public partial class GhostSkeletonSequence : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _ID;
		
		private System.Nullable<System.DateTime> _StartDate;
		
		private System.Nullable<System.DateTime> _EndDate;
		
		private EntitySet<GhostSkeleton> _GhostSkeletons;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIDChanging(int value);
    partial void OnIDChanged();
    partial void OnStartDateChanging(System.Nullable<System.DateTime> value);
    partial void OnStartDateChanged();
    partial void OnEndDateChanging(System.Nullable<System.DateTime> value);
    partial void OnEndDateChanged();
    #endregion
		
		public GhostSkeletonSequence()
		{
			this._GhostSkeletons = new EntitySet<GhostSkeleton>(new Action<GhostSkeleton>(this.attach_GhostSkeletons), new Action<GhostSkeleton>(this.detach_GhostSkeletons));
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ID", IsPrimaryKey=true, IsDbGenerated=true)]
		public int ID
		{
			get
			{
				return this._ID;
			}
			set
			{
				if ((this._ID != value))
				{
					this.OnIDChanging(value);
					this.SendPropertyChanging();
					this._ID = value;
					this.SendPropertyChanged("ID");
					this.OnIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_StartDate")]
		public System.Nullable<System.DateTime> StartDate
		{
			get
			{
				return this._StartDate;
			}
			set
			{
				if ((this._StartDate != value))
				{
					this.OnStartDateChanging(value);
					this.SendPropertyChanging();
					this._StartDate = value;
					this.SendPropertyChanged("StartDate");
					this.OnStartDateChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_EndDate")]
		public System.Nullable<System.DateTime> EndDate
		{
			get
			{
				return this._EndDate;
			}
			set
			{
				if ((this._EndDate != value))
				{
					this.OnEndDateChanging(value);
					this.SendPropertyChanging();
					this._EndDate = value;
					this.SendPropertyChanged("EndDate");
					this.OnEndDateChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="GhostSkeletonSequence_GhostSkeleton", Storage="_GhostSkeletons", ThisKey="ID", OtherKey="SequenceID")]
		public EntitySet<GhostSkeleton> GhostSkeletons
		{
			get
			{
				return this._GhostSkeletons;
			}
			set
			{
				this._GhostSkeletons.Assign(value);
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
		
		private void attach_GhostSkeletons(GhostSkeleton entity)
		{
			this.SendPropertyChanging();
			entity.GhostSkeletonSequence = this;
		}
		
		private void detach_GhostSkeletons(GhostSkeleton entity)
		{
			this.SendPropertyChanging();
			entity.GhostSkeletonSequence = null;
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="")]
	public partial class GhostJoint : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private float _X;
		
		private float _Y;
		
		private float _Z;
		
		private int _ID;
		
		private int _SkeletonID;
		
		private int _JointType;
		
		private EntityRef<GhostSkeleton> _GhostSkeleton;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnXChanging(float value);
    partial void OnXChanged();
    partial void OnYChanging(float value);
    partial void OnYChanged();
    partial void OnZChanging(float value);
    partial void OnZChanged();
    partial void OnIDChanging(int value);
    partial void OnIDChanged();
    partial void OnSkeletonIDChanging(int value);
    partial void OnSkeletonIDChanged();
    partial void OnJointTypeChanging(int value);
    partial void OnJointTypeChanged();
    #endregion
		
		public GhostJoint()
		{
			this._GhostSkeleton = default(EntityRef<GhostSkeleton>);
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_X")]
		public float X
		{
			get
			{
				return this._X;
			}
			set
			{
				if ((this._X != value))
				{
					this.OnXChanging(value);
					this.SendPropertyChanging();
					this._X = value;
					this.SendPropertyChanged("X");
					this.OnXChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Y")]
		public float Y
		{
			get
			{
				return this._Y;
			}
			set
			{
				if ((this._Y != value))
				{
					this.OnYChanging(value);
					this.SendPropertyChanging();
					this._Y = value;
					this.SendPropertyChanged("Y");
					this.OnYChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Z")]
		public float Z
		{
			get
			{
				return this._Z;
			}
			set
			{
				if ((this._Z != value))
				{
					this.OnZChanging(value);
					this.SendPropertyChanging();
					this._Z = value;
					this.SendPropertyChanged("Z");
					this.OnZChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ID", IsPrimaryKey=true, IsDbGenerated=true)]
		public int ID
		{
			get
			{
				return this._ID;
			}
			set
			{
				if ((this._ID != value))
				{
					this.OnIDChanging(value);
					this.SendPropertyChanging();
					this._ID = value;
					this.SendPropertyChanged("ID");
					this.OnIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_SkeletonID")]
		public int SkeletonID
		{
			get
			{
				return this._SkeletonID;
			}
			set
			{
				if ((this._SkeletonID != value))
				{
					if (this._GhostSkeleton.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnSkeletonIDChanging(value);
					this.SendPropertyChanging();
					this._SkeletonID = value;
					this.SendPropertyChanged("SkeletonID");
					this.OnSkeletonIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_JointType")]
		public int JointType
		{
			get
			{
				return this._JointType;
			}
			set
			{
				if ((this._JointType != value))
				{
					this.OnJointTypeChanging(value);
					this.SendPropertyChanging();
					this._JointType = value;
					this.SendPropertyChanged("JointType");
					this.OnJointTypeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="GhostSkeleton_GhostJoint", Storage="_GhostSkeleton", ThisKey="SkeletonID", OtherKey="ID", IsForeignKey=true)]
		public GhostSkeleton GhostSkeleton
		{
			get
			{
				return this._GhostSkeleton.Entity;
			}
			set
			{
				GhostSkeleton previousValue = this._GhostSkeleton.Entity;
				if (((previousValue != value) 
							|| (this._GhostSkeleton.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._GhostSkeleton.Entity = null;
						previousValue.GhostJoints.Remove(this);
					}
					this._GhostSkeleton.Entity = value;
					if ((value != null))
					{
						value.GhostJoints.Add(this);
						this._SkeletonID = value.ID;
					}
					else
					{
						this._SkeletonID = default(int);
					}
					this.SendPropertyChanged("GhostSkeleton");
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
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="")]
	public partial class GhostSkeleton : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _ID;
		
		private int _SequenceID;
		
		private EntitySet<GhostJoint> _GhostJoints;
		
		private EntityRef<GhostSkeletonSequence> _GhostSkeletonSequence;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIDChanging(int value);
    partial void OnIDChanged();
    partial void OnSequenceIDChanging(int value);
    partial void OnSequenceIDChanged();
    #endregion
		
		public GhostSkeleton()
		{
			this._GhostJoints = new EntitySet<GhostJoint>(new Action<GhostJoint>(this.attach_GhostJoints), new Action<GhostJoint>(this.detach_GhostJoints));
			this._GhostSkeletonSequence = default(EntityRef<GhostSkeletonSequence>);
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ID", IsPrimaryKey=true, IsDbGenerated=true)]
		public int ID
		{
			get
			{
				return this._ID;
			}
			set
			{
				if ((this._ID != value))
				{
					this.OnIDChanging(value);
					this.SendPropertyChanging();
					this._ID = value;
					this.SendPropertyChanged("ID");
					this.OnIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_SequenceID")]
		public int SequenceID
		{
			get
			{
				return this._SequenceID;
			}
			set
			{
				if ((this._SequenceID != value))
				{
					if (this._GhostSkeletonSequence.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnSequenceIDChanging(value);
					this.SendPropertyChanging();
					this._SequenceID = value;
					this.SendPropertyChanged("SequenceID");
					this.OnSequenceIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="GhostSkeleton_GhostJoint", Storage="_GhostJoints", ThisKey="ID", OtherKey="SkeletonID")]
		public EntitySet<GhostJoint> GhostJoints
		{
			get
			{
				return this._GhostJoints;
			}
			set
			{
				this._GhostJoints.Assign(value);
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="GhostSkeletonSequence_GhostSkeleton", Storage="_GhostSkeletonSequence", ThisKey="SequenceID", OtherKey="ID", IsForeignKey=true)]
		public GhostSkeletonSequence GhostSkeletonSequence
		{
			get
			{
				return this._GhostSkeletonSequence.Entity;
			}
			set
			{
				GhostSkeletonSequence previousValue = this._GhostSkeletonSequence.Entity;
				if (((previousValue != value) 
							|| (this._GhostSkeletonSequence.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._GhostSkeletonSequence.Entity = null;
						previousValue.GhostSkeletons.Remove(this);
					}
					this._GhostSkeletonSequence.Entity = value;
					if ((value != null))
					{
						value.GhostSkeletons.Add(this);
						this._SequenceID = value.ID;
					}
					else
					{
						this._SequenceID = default(int);
					}
					this.SendPropertyChanged("GhostSkeletonSequence");
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
		
		private void attach_GhostJoints(GhostJoint entity)
		{
			this.SendPropertyChanging();
			entity.GhostSkeleton = this;
		}
		
		private void detach_GhostJoints(GhostJoint entity)
		{
			this.SendPropertyChanging();
			entity.GhostSkeleton = null;
		}
	}
}
#pragma warning restore 1591