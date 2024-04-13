using System;
using System.IO;
using System.Text;

namespace RichPackage.IO
{
    public class StringStream : Stream
    {
        private readonly StringBuilder stringBuilder;
        private long position;

        public StringStream() : this(new StringBuilder())
        {
        }

        public StringStream(StringBuilder sb)
        {
            stringBuilder = sb ?? new StringBuilder();
            position = 0;
        }

        public override bool CanRead => false;
        public override bool CanSeek => true;
        public override bool CanWrite => true;
        public override long Length => stringBuilder.Length;

        public override long Position
        {
            get => position;
            set => position = value;
        }

        public override void Flush()
        {
            // No need to flush for StringBuilder
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException("Reading is not supported.");
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Begin:
                    position = offset;
                    break;
                case SeekOrigin.Current:
                    position += offset;
                    break;
                case SeekOrigin.End:
                    position = stringBuilder.Length + offset;
                    break;
            }
            return position;
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException("Setting length is not supported.");
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            string data = Encoding.UTF8.GetString(buffer, offset, count);
            Write(data);
        }

        public void Write(string str)
        {
            stringBuilder.Insert((int)position, str);
            position += str.Length;
        }

        public override string ToString() => stringBuilder.ToString();
    }
}
