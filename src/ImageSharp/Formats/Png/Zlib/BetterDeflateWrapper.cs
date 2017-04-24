namespace ImageSharp.Formats
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    /// <summary>
    /// None
    /// </summary>
    public class BetterDeflateWrapper : Stream
    {
        /// <summary>
        /// The raw stream containing the uncompressed image data.
        /// </summary>
        private readonly Stream rawStream;
        private ZlibInflateStream compress;
        private int bytesRemaining;

        /// <summary>
        /// Initializes a new instance of the <see cref="BetterDeflateWrapper"/> class.
        /// This is the constrctor
        /// </summary>
        /// <param name="rawStream">stream</param>
        public BetterDeflateWrapper(Stream rawStream)
        {
            this.rawStream = rawStream;
        }

        /// <summary>
        /// Gets a value indicating whether tests
        /// </summary>
        public bool StillBytesToProcess => this.bytesRemaining > 0;

        /// <inheritdoc/>
        public override bool CanRead => true;

        /// <inheritdoc/>
        public override bool CanSeek => throw new NotImplementedException();

        /// <inheritdoc/>
        public override bool CanWrite => throw new NotImplementedException();

        /// <inheritdoc/>
        public override long Length => throw new NotImplementedException();

        /// <inheritdoc/>
        public override long Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        /// <summary>
        /// Gets this is the inflate stream
        /// </summary>
        internal ZlibInflateStream InflateStream => this.compress;

        /// <summary>
        /// test
        /// </summary>
        /// <param name="bytes">blabla</param>
        public void AllocateNewBytes(int bytes)
        {
            this.bytesRemaining = bytes;
            if (this.InflateStream == null)
            {
                this.compress = new ZlibInflateStream(this);
            }
        }

        /// <inheritdoc/>
        public override int ReadByte()
        {
            this.bytesRemaining--;
            return this.rawStream.ReadByte();
        }

        /// <inheritdoc/>
        public override void Flush()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override int Read(byte[] buffer, int offset, int count)
        {
            if (this.bytesRemaining == 0)
            {
                return 0;
            }

            int bytesToRead = Math.Min(count, this.bytesRemaining);
            this.bytesRemaining -= bytesToRead;
            return this.rawStream.Read(buffer, offset, bytesToRead);
        }

        /// <inheritdoc/>
        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }
    }
}
