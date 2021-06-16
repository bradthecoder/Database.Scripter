#region Using Statements
using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
#endregion

namespace DatabaseScripter
{
    public class Worker
    {
        public string _serverName;
        public string _databaseName;
        public string _baseOutputFolder;

        public Worker(string serverName, string databaseName, string baseOutputFolder)
        {
            _serverName = serverName;
            _databaseName = databaseName;
            _baseOutputFolder = baseOutputFolder;
        }

        public void ScriptDatabase()
        {
            // there are many ways to connect to a SQL Server
            // the following simply uses the logged in user's credentials, 
            // along with a server name and database name.
            Server server = new Server(_serverName);
            Database database = server.Databases[_databaseName];

            ScriptingOptions scriptOptions = this.BuildScriptingOptions();

            this.ScriptTables(database, scriptOptions);
            this.ScriptViews(database, scriptOptions);
            this.ScriptFunctions(database, scriptOptions);
            this.ScriptStoredProcedures(database, scriptOptions);
        }

        private void ScriptTables(Database database, ScriptingOptions scriptOptions)
        {
            foreach (Table dbObject in database.Tables)
            {
                if (!dbObject.IsSystemObject)
                {
                    string fullOutputPath = this.BuildFullOutputPath(dbObject.Owner, "Tables", dbObject.Name);
                    string fileData = string.Empty;
                    StringCollection data = dbObject.Script(scriptOptions);
                    foreach (string line in data)
                    {
                        fileData += line + Environment.NewLine;
                    }
                    WriteFile(fullOutputPath, fileData);
                }
            }
        }

        private void ScriptViews(Database database, ScriptingOptions scriptOptions)
        {
            foreach (View dbObject in database.Views)
            {
                if (!dbObject.IsSystemObject)
                {
                    string fullOutputPath = BuildFullOutputPath(dbObject.Owner, "Views", dbObject.Name);
                    string fileData = string.Empty;
                    StringCollection data = dbObject.Script(scriptOptions);
                    foreach (string line in data)
                    {
                        fileData += line + Environment.NewLine;
                    }
                    WriteFile(fullOutputPath, fileData);
                }
            }
        }

        private void ScriptFunctions(Database database, ScriptingOptions scriptOptions)
        {
            foreach (UserDefinedFunction dbObject in database.UserDefinedFunctions)
            {
                if (!dbObject.IsSystemObject)
                {
                    string fullOutputPath = BuildFullOutputPath(dbObject.Owner, "Functions", dbObject.Name);
                    string fileData = string.Empty;
                    StringCollection data = dbObject.Script(scriptOptions);
                    foreach (string line in data)
                    {
                        fileData += line + Environment.NewLine;
                    }
                    WriteFile(fullOutputPath, fileData);
                }
            }
        }

        private void ScriptStoredProcedures(Database database, ScriptingOptions scriptOptions)
        {
            foreach (StoredProcedure dbObject in database.StoredProcedures)
            {
                if (!dbObject.IsSystemObject)
                {
                    string fullOutputPath = BuildFullOutputPath(dbObject.Owner, "Stored Procedures", dbObject.Name);
                    string fileData = string.Empty;
                    StringCollection data = dbObject.Script(scriptOptions);
                    foreach (string line in data)
                    {
                        fileData += line + Environment.NewLine;
                    }
                    WriteFile(fullOutputPath, fileData);
                }
            }
        }

        private ScriptingOptions BuildScriptingOptions()
        {
            ScriptingOptions scriptOptions = new ScriptingOptions();
            scriptOptions.ScriptSchema = true;
            scriptOptions.Triggers = true;
            scriptOptions.Indexes = true;
            scriptOptions.ClusteredIndexes = true;
            scriptOptions.NonClusteredIndexes = true;
            scriptOptions.EnforceScriptingOptions = true;
            //scriptOptions.ScriptDrops = true;
            //scriptOptions.IncludeIfNotExists = true;
            scriptOptions.AllowSystemObjects = false;
            return scriptOptions;
        }

        private string BuildFullOutputPath(string dbObjectOwner, string objectTypeName, string dbObjectName)
        {
            //if(dbObjectOwner == "sys")
            //{
            //    Console.WriteLine($"System object {objectTypeName} {dbObjectName} ");
            //}
            string fullOutputPath = _baseOutputFolder + "\\" + _databaseName + "\\" + dbObjectOwner + "\\" + objectTypeName;
            if (!System.IO.Directory.Exists(fullOutputPath))
            {
                System.IO.Directory.CreateDirectory(fullOutputPath);
            }
            fullOutputPath += "\\" + dbObjectName + ".sql";
            return fullOutputPath;
        }

        private void WriteFile(string fullOutputPath, string fileData)
        {
            if (System.IO.File.Exists(fullOutputPath))
            {
                //read file and compare; only overwrite if different
                string existingContent = string.Empty;
                using (var sr = new System.IO.StreamReader(fullOutputPath))
                {
                    existingContent = sr.ReadToEnd();
                }
                if (existingContent != fileData)
                {
                    System.IO.File.WriteAllText(fullOutputPath, fileData);
                }
            }
            else
            {
                System.IO.File.WriteAllText(fullOutputPath, fileData);
            }
        }
    }
}
