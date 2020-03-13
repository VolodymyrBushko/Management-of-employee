using System;
using System.Xml.Serialization;
using System.IO;
using System.Collections.ObjectModel;

namespace VolodWPF.Classes
{
    [Serializable]
    public class Company
    {
        public ObservableCollection<Worker> workers { get; set; }

        public Company()
        {
            workers = new ObservableCollection<Worker>();
        }

        public static void Serializer(Company company, String path)
        {
            using (FileStream file = new FileStream(path, FileMode.Create))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Company));
                serializer.Serialize(file, company);
            }
        }

        public static Company Deserializer(String path)
        {
            try
            {
                using (FileStream file = new FileStream(path, FileMode.Open))
                {
                    XmlSerializer deserializer = new XmlSerializer(typeof(Company));
                    return (Company)deserializer.Deserialize(file);
                }
            }
            catch { return new Company(); }
        }
    }
}