using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AudioHide
{

    public partial class AudioHide : Form
    {
        
        public AudioHide()
        {
            InitializeComponent();

        }

        private void Ecnode_Click(object sender, EventArgs e)
        {
            _instorage = InputURL.Text;
            _keystorage = KeyURL.Text;
            _outstorage = OutURL.Text;
            Audio SrcFile = new Audio(_instorage);
            Audio KeyFile = new Audio(_keystorage);
            //Audio OutFile = new Audio("C:\\Users\\BIT\\Documents\\programming\\AudioHide\\AudioHide\\TestCase\\Out\\sample.wav");
            Stream MessageStream = null;
            String Message = EmbededData.Text;
            BinaryWriter messageWriter = new BinaryWriter(new MemoryStream());
            /* 4bits for message's length*/
            messageWriter.Write(Message.Length);
            messageWriter.Write(Encoding.ASCII.GetBytes(Message));
            messageWriter.Seek(0, SeekOrigin.Begin);
            MessageStream = messageWriter.BaseStream;

            try
            {
                if (SrcFile.Hide(this.Method.Text[0] - '1', KeyFile.stream, MessageStream, _outstorage))
                {
                    MessageBox.Show("Sucess!");
                    SrcFile.Dispose();
                    KeyFile.Dispose();
                    MessageStream.Close();
                }

            }

            catch
            {
                MessageBox.Show("No files.");
            }

            return;
        }

        private void Decode_Click(object sender, EventArgs e)
        {
            _instorage = InputURL.Text;
            _keystorage = KeyURL.Text;
            _outstorage = OutURL.Text;
            Audio OutFile = new Audio(_outstorage);
            Stream MessageStream = new MemoryStream();
            FileStream KeyStream = new FileStream(_keystorage,FileMode.Open);

            try
            {
                if (OutFile.Extract(this.Noise.Text[0] - '1', KeyStream, MessageStream))
                {
                    MessageBox.Show("Sucess!");
                    MessageStream.Seek(0, SeekOrigin.Begin);
                    OutData.Text = new StreamReader(MessageStream).ReadToEnd();
                    OutFile.Dispose();
                    MessageStream.Close();
                    KeyStream.Close();
                }

            }

            catch
            {
                MessageBox.Show("Cannot decode it!");
            }

            return;
        }
    }

    public class Audio
    {
        public String URL;          /* 文件地址 */
        public bool isopen;
        public FileStream stream;       /* 输入流 */
        private int bytesPerSample; /* 采样率 */
        private long m_DataPos;
        private int m_Length;

        public Audio()
        {
            this.URL = "";
            this.isopen = false;
        }

        public void Dispose()
        {
            if (stream != null)
                stream.Close();
            GC.SuppressFinalize(this);
        }

        public Audio(String url)
        {
            this.URL = url;
            this.isopen = false;
            if (url != "")
            {
                try
                {
                    this.stream = new FileStream(this.URL, FileMode.Open);
                }
                catch
                {
                    MessageBox.Show("Failed");
                }
            }
        }

        private static int GetKeyValue(Stream keyStream)
        {
            int keyValue;
            if ((keyValue = keyStream.ReadByte()) < 0)
            {
                keyStream.Seek(0, SeekOrigin.Begin);
                keyValue = keyStream.ReadByte();
            }
            /* 0 和 1111 1111 危险 */
            /* 没写对应规则 可以补上 这里用的是key序列+1*/
            return ++keyValue;
        }

        public int Copy(byte[] buffer, int ofset, int count, Stream output)
        {
            int toread = (int)Math.Min(count, stream.Length - stream.Position);
            int read = stream.Read(buffer, ofset, toread);
            output.Write(buffer, ofset, read);
            return read;
        }

        public int Read(byte[] buffer, int ofset, int count)
        {
            int toread = (int)Math.Min(count, stream.Length - stream.Position);
            int read = stream.Read(buffer, ofset, toread);
            return read;
        }

        public bool Hide(int mode, FileStream KeyStream, Stream MessageStream,string name)
        {
            FileStream OutWave = new FileStream(name, FileMode.Create);
            SteganoWave.WaveStream AudioStream = new SteganoWave.WaveStream(stream, OutWave); /* 冗余 */
            bytesPerSample = AudioStream.Format.wBitsPerSample / 8;

            /* Extra output informations */
            //FileStream _fs = new FileStream(AudioHide.storage + "Message\\record01.txt", FileMode.Create);
            //StreamWriter _sw = new StreamWriter(_fs);

            switch (mode)
            {

                /* src/out/key/mes OK
                 * src --- this.stream
                 * key --- KeyStream
                 * message --- MessageStream
                 * out --- OutWave
                 */
                #region Improved LSB algorithm
                case 0:
                    byte[] waveBuffer = new byte[bytesPerSample];
			        byte message, bit, waveByte;
			        int messageBuffer; //receives the next byte of the message or -1
			        int keyByte; //distance of the next carrier sample

                    while ((messageBuffer = MessageStream.ReadByte()) >= 0)
                    {
                        
                        message = (byte)messageBuffer;
                        for (int bitIndex = 0; bitIndex < 8; bitIndex++)
                        {
                            keyByte = GetKeyValue(KeyStream);
                            /* copy the previous sample point */
                            for (int i = 0; i < keyByte - 1; i++)
                            {
                                Copy(waveBuffer, 0, waveBuffer.Length, OutWave);
                            }
                            /* modify the last bit of the selected point */
                            stream.Read(waveBuffer, 0, waveBuffer.Length);
                            waveByte = (byte)(waveBuffer[bytesPerSample - 1]%2);
                            bit = (byte)(((message & (byte)(1 << bitIndex)) > 0) ? 1 : 0);
                            if ((bit ^ waveByte) == 1)
                            {
                                waveByte = (byte)(waveByte > 0 ? -1 : 1);
                                waveBuffer[bytesPerSample - 1] += waveByte;
                            }
                            OutWave.Write(waveBuffer, 0, bytesPerSample);
                        }
                    }

			        //copy the rest of the wave without changes
			        waveBuffer = new byte[stream.Length - stream.Position];
			        stream.Read(waveBuffer, 0, waveBuffer.Length);
                    OutWave.Write(waveBuffer, 0, waveBuffer.Length);
                    break;
                #endregion

                #region parity
                case 1:
                    /* Echo hiding methods*/
                    break;
                #endregion

                #region MCLF
                case 2:
                    /* MCLF methods*/
                    break;
                #endregion
                default:
                    break;
            }

            /*_sw.Flush();
            _sw.Close();
            _fs.Flush();
            _fs.Close();*/
            OutWave.Close();
            return true;
        }

        public bool Extract(int mode, FileStream KeyStream, Stream MessageStream)
        {
            
            SteganoWave.WaveStream AudioStream = new SteganoWave.WaveStream(stream); /* 冗余 */
            bytesPerSample = AudioStream.Format.wBitsPerSample / 8;
            AudioStream.Seek(0, SeekOrigin.Begin);

            /* Extra output informations */
            //FileStream _fs = new FileStream(AudioHide.storage + "Message\\record01.txt", FileMode.Create);
            //StreamWriter _sw = new StreamWriter(_fs);

            switch (mode)
            {

                /* src/out/key/mes OK
                 * src --- this.stream
                 * key --- KeyStream
                 * message --- MessageStream
                 * out --- OutWave
                 */
                #region Improved LSB algorithm
                case 0:
                    byte[] waveBuffer = new byte[bytesPerSample];
                    int cnt = 0;
                    int keyByte; //distance of the next carrier sample
                    int mes,Mes = 0;
                    int[] length = {0,0,0,0};
                    long Length = 1000000;

                    while ( AudioStream.ReadByte() >= 0 && MessageStream.Length < Length )
                    {
                        Mes = 0;
                        //if (AudioStream.Position == 1)
                        //    AudioStream.Seek(0, SeekOrigin.Begin);
                        AudioStream.Seek(-1, SeekOrigin.Current);
                        for (int bitIndex = 0; bitIndex < 8; bitIndex++)
                        {
                            cnt++;
                            /* 这里看编码规则解码 值应减1 */
                            keyByte = GetKeyValue(KeyStream);
                            /* jump out of stream */
                            for (int i = 0; i < keyByte; i++)
                            {
                                AudioStream.Read(waveBuffer, 0, waveBuffer.Length);
                            }
                            mes = (int)(waveBuffer[waveBuffer.Length - 1] % 2 == 0 ? 0 : 1);
                            
                            /* 4 Bytes for message's length */
                            if (cnt < 32)
                            {
                                length[(cnt - 1) / 8] += (1 & mes) << ((cnt - 1) % 8);
                                
                            }
                            else if (cnt == 32)
                            {
                                Length = ( length[3] << 24 ) + ( length[2] << 16 ) + 
                                         ( length[1] << 8 ) + length[0];
                            }
                            else
                            {
                                Mes += (1 & mes) << bitIndex;
                            }

                        }
                        if (cnt > 32)
                        {
                            MessageStream.WriteByte((byte)Mes);
                        }
                    }
                    break;
                #endregion

                #region parity
                case 1:
                    /* Echo hiding methods*/
                    break;
                #endregion

                #region MCLF
                case 2:
                    /* MCLF methods*/
                    break;
                #endregion
                default:
                    break;
            }

            /*_sw.Flush();
            _sw.Close();
            _fs.Flush();
            _fs.Close();*/

            AudioStream.Dispose();
            return true;
        }

    }

}

namespace SteganoWave
{
    public enum WaveFormats
    {
        Pcm = 1,
        Float = 3
    }
    public class WaveFormat
    {
        public short wFormatTag;
        public short nChannels;
        public int nSamplesPerSec;
        public int nAvgBytesPerSec;
        public short nBlockAlign;
        public short wBitsPerSample;
        public short cbSize;

        public WaveFormat(int rate, int bits, int channels)
        {
            wFormatTag = (short)WaveFormats.Pcm;
            nChannels = (short)channels;
            nSamplesPerSec = rate;
            wBitsPerSample = (short)bits;
            cbSize = 0;

            nBlockAlign = (short)(channels * (bits / 8));
            nAvgBytesPerSec = nSamplesPerSec * nBlockAlign;
        }
    }


	public class WaveStream : Stream, IDisposable
	{
		private Stream m_Stream;
		private long m_DataPos;
		private int m_Length;

		private WaveFormat m_Format;

		public WaveFormat Format
		{
			get { return m_Format; }
		}

		private string ReadChunk(BinaryReader reader) {
			byte[] ch = new byte[4];
			reader.Read(ch, 0, ch.Length);
			return System.Text.Encoding.ASCII.GetString(ch);
		}

		private void ReadHeader() {
			BinaryReader Reader = new BinaryReader(m_Stream);
			if (ReadChunk(Reader) != "RIFF")
				throw new Exception("Invalid file format");

			Reader.ReadInt32(); // File length minus first 8 bytes of RIFF description, we don't use it

			if (ReadChunk(Reader) != "WAVE")
				throw new Exception("Invalid file format");

			if (ReadChunk(Reader) != "fmt ")
				throw new Exception("Invalid file format");

			int len = Reader.ReadInt32();
			if (len < 16) // bad format chunk length
				throw new Exception("Invalid file format");

			m_Format = new WaveFormat(22050, 16, 2); // initialize to any format
			m_Format.wFormatTag = Reader.ReadInt16();
			m_Format.nChannels = Reader.ReadInt16();
			m_Format.nSamplesPerSec = Reader.ReadInt32();
			m_Format.nAvgBytesPerSec = Reader.ReadInt32();
			m_Format.nBlockAlign = Reader.ReadInt16();
			m_Format.wBitsPerSample = Reader.ReadInt16(); 

			// advance in the stream to skip the wave format block 
			len -= 16; // minimum format size
			while (len > 0) {
				Reader.ReadByte();
				len--;
			}

			// assume the data chunk is aligned
			while(m_Stream.Position < m_Stream.Length && ReadChunk(Reader) != "data")
				;

			if (m_Stream.Position >= m_Stream.Length)
				throw new Exception("Invalid file format");

			m_Length = Reader.ReadInt32();
			m_DataPos = m_Stream.Position;

			Position = 0;
		}

		/// <summary>ReadChunk(reader) - Changed to CopyChunk(reader, writer)</summary>
		/// <param name="reader">source stream</param>
		/// <returns>four characters</returns>
		private string CopyChunk(BinaryReader reader, BinaryWriter writer)
		{
			byte[] ch = new byte[4];
			reader.Read(ch, 0, ch.Length);
			
			//copy the chunk
			writer.Write(ch);
			
			return System.Text.Encoding.ASCII.GetString(ch);
		}

		/// <summary>ReadHeader() - Changed to CopyHeader(destination)</summary>
		private void CopyHeader(Stream destinationStream)
		{
			BinaryReader reader = new BinaryReader(m_Stream);
			BinaryWriter writer = new BinaryWriter(destinationStream);
			
			if (CopyChunk(reader, writer) != "RIFF")
				throw new Exception("Invalid file format");

			writer.Write( reader.ReadInt32() ); // File length minus first 8 bytes of RIFF description
			
			if (CopyChunk(reader, writer) != "WAVE")
				throw new Exception("Invalid file format");

			if (CopyChunk(reader, writer) != "fmt ")
				throw new Exception("Invalid file format");

			int len = reader.ReadInt32();
			if (len < 16){ // bad format chunk length
				throw new Exception("Invalid file format");
			}else{
				writer.Write(len);
			}

			m_Format = new WaveFormat(22050, 16, 2); // initialize to any format
			m_Format.wFormatTag = reader.ReadInt16();
			m_Format.nChannels = reader.ReadInt16();
			m_Format.nSamplesPerSec = reader.ReadInt32();
			m_Format.nAvgBytesPerSec = reader.ReadInt32();
			m_Format.nBlockAlign = reader.ReadInt16();
			m_Format.wBitsPerSample = reader.ReadInt16(); 

			//copy format information
			writer.Write( m_Format.wFormatTag );
			writer.Write( m_Format.nChannels );
			writer.Write( m_Format.nSamplesPerSec );
			writer.Write( m_Format.nAvgBytesPerSec );
			writer.Write( m_Format.nBlockAlign );
			writer.Write( m_Format.wBitsPerSample ); 


			// advance in the stream to skip the wave format block 
			len -= 16; // minimum format size
			writer.Write( reader.ReadBytes(len) );
			len = 0;
			/*while (len > 0)
			{
				reader.ReadByte();
				len--;
			}*/

			// assume the data chunk is aligned
			while(m_Stream.Position < m_Stream.Length && CopyChunk(reader, writer) != "data")
				;

			if (m_Stream.Position >= m_Stream.Length)
				throw new Exception("Invalid file format");

			m_Length = reader.ReadInt32();
			writer.Write( m_Length );
			
			m_DataPos = m_Stream.Position;
			Position = 0;
		}

		/// <summary>Write a new header</summary>
		public static Stream CreateStream(Stream waveData, WaveFormat format) {
			MemoryStream stream = new MemoryStream();
			BinaryWriter writer = new BinaryWriter(stream);

			writer.Write(System.Text.Encoding.ASCII.GetBytes("RIFF".ToCharArray()));
			
			writer.Write((Int32)(waveData.Length + 36)); //File length minus first 8 bytes of RIFF description

			writer.Write(System.Text.Encoding.ASCII.GetBytes("WAVEfmt ".ToCharArray()));
		
			writer.Write((Int32)16); //length of following chunk: 16

			writer.Write((Int16)format.wFormatTag);
			writer.Write((Int16)format.nChannels);
			writer.Write((Int32)format.nSamplesPerSec);
			writer.Write((Int32)format.nAvgBytesPerSec);
			writer.Write((Int16)format.nBlockAlign);
			writer.Write((Int16)format.wBitsPerSample);

			writer.Write(System.Text.Encoding.ASCII.GetBytes("data".ToCharArray()));
			
			writer.Write((Int32)waveData.Length);

			waveData.Seek(0, SeekOrigin.Begin);
			byte[] b = new byte[waveData.Length];
			waveData.Read(b, 0, (int)waveData.Length);
			writer.Write(b);

			writer.Seek(0, SeekOrigin.Begin);
			return stream;
		}


		public WaveStream(Stream sourceStream, Stream destinationStream)
		{
			m_Stream = sourceStream;
			CopyHeader(destinationStream);
		}
		
		public WaveStream(Stream sourceStream) {
			m_Stream = sourceStream;
			ReadHeader();
		}

		~WaveStream()
		{
			Dispose();
		}
		
		public void Dispose()
		{
			if (m_Stream != null)
				m_Stream.Close();
			GC.SuppressFinalize(this);
		}

		public override bool CanRead
		{
			get { return true; }
		}
		public override bool CanSeek
		{
			get { return true; }
		}
		public override bool CanWrite
		{
			get { return false; }
		}
		public override long Length
		{
			get { return m_Length; }
		}

		/// <summary>Length of the data (in samples)</summary>
		public long CountSamples {
			get { return (long)((m_Length - m_DataPos) / (m_Format.wBitsPerSample/8)); }
		}

		public override long Position
		{
			get { return m_Stream.Position - m_DataPos; }
			set { Seek(value, SeekOrigin.Begin); }
		}
		public override void Close()
		{
			Dispose();
		}
		public override void Flush()
		{
		}
		public override void SetLength(long len)
		{
			throw new InvalidOperationException();
		}
		public override long Seek(long pos, SeekOrigin o)
		{
			switch(o)
			{
				case SeekOrigin.Begin:
					m_Stream.Position = pos + m_DataPos;
					break;
				case SeekOrigin.Current:
					m_Stream.Seek(pos, SeekOrigin.Current);
					break;
				case SeekOrigin.End:
					m_Stream.Position = m_DataPos + m_Length - pos;
					break;
			}
			return this.Position;
		}

		public override int Read(byte[] buf, int ofs, int count)
		{
			int toread = (int)Math.Min(count, m_Length - Position);
			return m_Stream.Read(buf, ofs, toread);
		}

		/// <summary>Read - Changed to Copy</summary>
		/// <param name="buf">Buffer to receive the data</param>
		/// <param name="ofs">Offset</param>
		/// <param name="count">Count of bytes to read</param>
		/// <param name="destination">Where to copy the buffer</param>
		/// <returns>Count of bytes actually read</returns>
		public int Copy(byte[] buf, int ofs, int count, Stream destination) {
			int toread = (int)Math.Min(count, m_Length - Position);
			int read = m_Stream.Read(buf, ofs, toread);
			destination.Write(buf, ofs, read);

			if(m_Stream.Position != destination.Position){
				Console.WriteLine();
			}

			return read;
		}

		public override void Write(byte[] buf, int ofs, int count)
		{
			throw new InvalidOperationException();
		}
	}
}
