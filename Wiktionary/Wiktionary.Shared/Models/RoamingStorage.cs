﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace Wiktionary.Models
{
    public class RoamingStorage
    {
        private const string Filename = "roaming.xml";
        private static List<object> _data = new List<object>();

        public static List<object> Data
        {
            get { return _data; }
            set { _data = value; }
        }

        static async public Task Save<T>()
        {
            await Windows.System.Threading.ThreadPool.RunAsync(sender => SaveAsync<T>(), Windows.System.Threading.WorkItemPriority.Normal);
        }

        static async public Task Restore<T>()
        {
            await Windows.System.Threading.ThreadPool.RunAsync(sender => RestoreAsync<T>(), Windows.System.Threading.WorkItemPriority.Normal);
        }

        static async private Task SaveAsync<T>()
        {
            StorageFile sessionFile = await ApplicationData.Current.RoamingFolder.CreateFileAsync(Filename, CreationCollisionOption.ReplaceExisting);
            IRandomAccessStream sessionRandomAccess = await sessionFile.OpenAsync(FileAccessMode.ReadWrite);
            IOutputStream sessionOutputStream = sessionRandomAccess.GetOutputStreamAt(0);
            var sessionSerializer = new DataContractSerializer(typeof(List<object>), new[] { typeof(T) });
            sessionSerializer.WriteObject(sessionOutputStream.AsStreamForWrite(), _data);
            await sessionOutputStream.FlushAsync();
        }

        static async private Task RestoreAsync<T>()
        {
            StorageFile sessionFile = await ApplicationData.Current.RoamingFolder.CreateFileAsync(Filename, CreationCollisionOption.OpenIfExists);
            if (sessionFile == null)
            {
                return;
            }
            IInputStream sessionInputStream = await sessionFile.OpenReadAsync();
            var sessionSerializer = new DataContractSerializer(typeof(List<object>), new[] { typeof(T) });
            _data = (List<object>)sessionSerializer.ReadObject(sessionInputStream.AsStreamForRead());
        }
    }
}