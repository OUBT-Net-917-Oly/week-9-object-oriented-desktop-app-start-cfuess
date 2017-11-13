using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace EmployeeTracker.Data
{
    public class FileManager
    {
        //declare field variables
        private XmlSerializer _serializer;
        private string _filepath = @"C:\Temp\TrackerDb.xml";

        public FileManager()
        {
            _serializer = new XmlSerializer(typeof(DataStore));
        }

        // this method returns a Datastore object with whatever was saved last time.
        public DataStore Load()
        {
            // declare and instantiate the store object, has scope only in this method
            DataStore store = new DataStore();
            try
            {
                if (File.Exists(_filepath))
                {
                    // use the .NET FileStream object to get data out of the file 
                    FileStream stream = new FileStream(_filepath, FileMode.Open);

                    // use the XmlSerializer to take the xml data and turn it back
                    // into a DataStore object
                    store = (DataStore)_serializer.Deserialize(stream);

                    // important: close the stream
                    stream.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return store;
        }

        public void Save(DataStore store)
        {
            // the store object is passed in via the method parameter.
            // it has scope only in this method, so it is independent of the store object in 
            // the method above
            try
            {
                // declare and instantiate objects
                FileStream stream = new FileStream(_filepath, FileMode.CreateNew);
                TextWriter writer = new StreamWriter(stream);

                // use the seralizer to transform the object to XML
                _serializer.Serialize(writer, store);

                // important: close the writer and stream objects
                writer.Close();
                stream.Close();
            }
            catch (System.IO.IOException ex)
            {
                // catch the more specific error type see if it is because
                // it already exists
                if (ex.Message.Contains("already exists"))
                {
                    // if it does exist delete it and try saving again
                    File.Delete(_filepath);

                    // getting recursive (calling itself)
                    Save(store);
                }
                else
                {
                    // some other kind of IOException occured, tell the user
                    MessageBox.Show(ex.Message);
                }
            }
            catch (Exception ex)
            {
                // some other kind of error happened, tell the user
                MessageBox.Show(ex.Message);
            }
        }

    }
}
