using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.System.Threading;
using Wiktionary.ViewModel;

namespace Wiktionary.Models
{
    class RoamingStorage
    {
        private const string filename = "roaming.xml";
        private static List<object> _data = new List<object>();

        public static List<object> Data
        {
            get { return _data; }
            set { _data = value; }
        }

        static async public Task Save<T>()
        {
            await Windows.System.Threading.ThreadPool.RunAsync((sender) => RoamingStorage.SaveAsync<T>(), Windows.System.Threading.WorkItemPriority.Normal);
        }

        static async public Task Restore<T>()
        {
            await Windows.System.Threading.ThreadPool.RunAsync((sender) => RoamingStorage.RestoreAsync<T>(), Windows.System.Threading.WorkItemPriority.Normal);
        }

        static async private Task SaveAsync<T>()
        {
            StorageFile sessionFile = await ApplicationData.Current.RoamingFolder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
            IRandomAccessStream sessionRandomAccess = await sessionFile.OpenAsync(FileAccessMode.ReadWrite);
            IOutputStream sessionOutputStream = sessionRandomAccess.GetOutputStreamAt(0);
            var sessionSerializer = new DataContractSerializer(typeof(List<object>), new Type[] { typeof(T) });
            sessionSerializer.WriteObject(sessionOutputStream.AsStreamForWrite(), _data);
            await sessionOutputStream.FlushAsync();
        }

        static async private Task RestoreAsync<T>()
        {
            StorageFile sessionFile = await ApplicationData.Current.RoamingFolder.CreateFileAsync(filename, CreationCollisionOption.OpenIfExists);
            if (sessionFile == null)
            {
                return;
            }
            IInputStream sessionInputStream = await sessionFile.OpenReadAsync();
            var sessionSerializer = new DataContractSerializer(typeof(List<object>), new Type[] { typeof(T) });
            _data = (List<object>)sessionSerializer.ReadObject(sessionInputStream.AsStreamForRead());
        }
    }
}
