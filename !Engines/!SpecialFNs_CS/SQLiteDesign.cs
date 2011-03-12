using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;

namespace SQLiteDesign
{
	class OpenTable
	{
		public SQLiteDataAdapter	Adapter;
		public DataTable			Table;
	}
	
	class ReplaceCommandBuilder
	{
		
	}

	class SQLiteDatabase
	{
		protected SQLiteConnection	m_DbConnection;
		protected DataSet			m_Dataset;
		protected List<OpenTable>	m_OpenTables;

		public SQLiteDatabase(SQLiteConnection a_DbConnection, DataSet a_DataSet)
		{
			m_DbConnection = a_DbConnection;
			m_Dataset = a_DataSet;
		}

		public DataSet DataSet
		{
			get
			{
				return m_Dataset;
			}
		}
		
		protected virtual string NetTypeToSQLiteType(Type a_NetType)
		{
			switch (a_NetType.Name)
			{
				case "String":
					return "text";
				case "UInt64":
				case "UInt32":
				case "UInt16":
				case "UInt8":
				case "Int64":
				case "Int32":
				case "Int16":
				case "Int8":
					return "integer";
				case "Double":
				case "double":
					return "real";
			}

			string errorText = "Bad type in NetTypeToSQLiteType:" + a_NetType.Name;
			Debug.Assert(false, errorText);
			throw new global::System.ArgumentException(errorText);
		}
		
		// WARNING this method is not finished and ignores many features
		protected string GetCreateTableSql(DataTable a_Table)
		{
			StringBuilder resultBuilder = new StringBuilder();
			resultBuilder.Append("CREATE TABLE IF NOT EXISTS ");
			resultBuilder.Append(a_Table.TableName);
			resultBuilder.Append(" (");

			for (int i = 0; i < a_Table.Columns.Count; i++)
			{
				DataColumn currColumn = a_Table.Columns[i];
				if (i != 0)
					resultBuilder.Append(", ");
				
				resultBuilder.Append(currColumn.ColumnName);
				resultBuilder.Append(" ");
				resultBuilder.Append(NetTypeToSQLiteType(currColumn.DataType));
			}

			if (0 != a_Table.PrimaryKey.Count())
			{
				resultBuilder.Append(", PRIMARY KEY (");
				
				for (int i = 0; i < a_Table.PrimaryKey.Count(); i++)
				{
					DataColumn currColumn = a_Table.PrimaryKey[i];
					if (i != 0)
						resultBuilder.Append(", ");
					
					resultBuilder.Append(currColumn.ColumnName);
				}

				resultBuilder.Append(")");
			}

			resultBuilder.Append(");");
			return resultBuilder.ToString();
		}

		// Summary:
		//     Creates tables in Database according to the design
		public void CreateTables()
		{
			foreach (DataTable currTable in m_Dataset.Tables)
			{
				SQLiteCommand createTableCmd = m_DbConnection.CreateCommand();
				createTableCmd.CommandText = GetCreateTableSql(currTable);
				createTableCmd.ExecuteNonQuery();
			}
		}

		// Summary:
		//	Load tables from database into a DataSet
		//	WARNING consumes a lot of memory since it loads the entire database into memory!
		public void LoadDatabase()
		{
			if (m_OpenTables != null)
				return;
				
			m_OpenTables = new List<OpenTable>();

			foreach (DataTable currTable in m_Dataset.Tables)
			{
				OpenTable newTable = new OpenTable();
				newTable.Table = currTable;
				newTable.Adapter = new SQLiteDataAdapter("Select * from " + currTable.TableName, m_DbConnection);
				newTable.Adapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
				newTable.Adapter.Fill(m_Dataset, currTable.TableName);

				SQLiteCommandBuilder cmdBuilder = new SQLiteCommandBuilder(newTable.Adapter);

				m_OpenTables.Add(newTable);
			}
		}

		// Summary:
		//	Saves changes made to assigned DataSet after loading with LoadDatabase()
		public void SaveDataSetChanges()
		{
			if (m_OpenTables == null)
				return;

			foreach (OpenTable currTable in m_OpenTables)
			{
				currTable.Adapter.Update(currTable.Table);
			}
		}

		// Summary:
		//	Generates a command usable for replacing rows in a specified table
		private SQLiteCommand GetReplaceCommand(DataTable a_Template)
		{
			SQLiteDataAdapter adapter = new SQLiteDataAdapter("Select * from " + a_Template.TableName, m_DbConnection);
			SQLiteCommandBuilder cmdBuilder = new SQLiteCommandBuilder(adapter);
			SQLiteCommand replaceCommand = (SQLiteCommand)cmdBuilder.GetInsertCommand().Clone();
			replaceCommand.CommandText = replaceCommand.CommandText.Replace("INSERT INTO", "REPLACE INTO");

			// remove SQLiteCommandBuilder's side effects
			cmdBuilder.DataAdapter = null;
			
			return replaceCommand;
		}

		// Summary:
		//	Replaces rows into a database table
		public void ReplaceRows(DataTable a_Template, DataRowCollection a_Rows)
		{
			SQLiteCommand replaceCommand = GetReplaceCommand(a_Template);
			SQLiteTransaction transaction = m_DbConnection.BeginTransaction();
			
			foreach (DataRow currRow in a_Rows)
			{
				foreach (SQLiteParameter currParam in replaceCommand.Parameters)
				{
					currParam.Value = currRow[currParam.SourceColumn];
				}
				
				replaceCommand.ExecuteNonQuery();
			}
			
			transaction.Commit();
		}
	}
}
