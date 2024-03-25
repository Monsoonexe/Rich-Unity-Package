using RichPackage.SaveSystem;
using Sirenix.Serialization;
using System;
using System.IO;
using UnityEngine.Assertions;

namespace ScaryRobot.LostInSpace
{
    public static class SaveSystemUtilities
    {
        // can BinaryDataReader be re-used?
        private static readonly MemoryStream s_Stream = new MemoryStream(new byte[1024]);
        private static readonly BinaryDataReader s_Reader = new BinaryDataReader(s_Stream, null);

        private static readonly BinaryDataWriter s_Writer = new BinaryDataWriter(s_Stream, null);

        public static PooledReader GetReader(this ISaveStore store, string key, out BinaryDataReader reader)
        {
            Assert.IsTrue(store.KeyExists(key));
            Assert.IsTrue(s_Stream.Position == 0);

            var data = store.Load<byte[]>(key); // TODO - could optimize with LoadInto a shared array...
            s_Stream.Write(data, 0, data.Length);
            s_Stream.Position = 0;
            reader = s_Reader;
            reader.PrepareNewSerializationSession();
            return new PooledReader();
        }

        public static PooledWriter GetWriter(this ISaveStore store, string key, out BinaryDataWriter writer)
        {
            Assert.IsTrue(s_Stream.Position == 0);

            writer = s_Writer;
            writer.PrepareNewSerializationSession();
            return new PooledWriter(store, key);
        }

        public static ActionDisposable GetWriterDefault(this ISaveStore store, string key, out BinaryDataWriter writer)
        {
            // non-optimal, but convenient
            var stream = new MemoryStream(32);
            writer = new BinaryDataWriter(stream, null);
            var writer2 = writer; // lol closures
            return new ActionDisposable(() => // TODO - implement custom struct to remove closure
            {
                // todo optimize closure
                writer2.FlushToStream();
                store.Save(key, stream.ToArray());
            });
        }

        public static BinaryDataReader GetReaderDefault(this ISaveStore store, string key)
        {
            // non-optimal, but convenient
            var data = store.Load<byte[]>(key);
            var stream = new MemoryStream(data);
            return new BinaryDataReader(stream, null);
        }

        public static BinaryDataReader GetReaderDefault(this ISaveStore store, string key, out BinaryDataReader reader)
        {
            return reader = GetReaderDefault(store, key);
        }

        public struct PooledReader : IDisposable
        {
            void IDisposable.Dispose()
            {
                s_Stream.Position = 0;
            }
        }

        public struct PooledWriter : IDisposable
        {
            private readonly ISaveStore store;
            private readonly string key;

            public PooledWriter(ISaveStore store, string key)
            {
                this.store = store;
                this.key = key;
            }

            void IDisposable.Dispose()
            {
                // write stream to store
                s_Writer.FlushToStream();
                store.Save(key, s_Stream.ToArray());
                s_Stream.Position = 0; // 
            }
        }
    }
}
