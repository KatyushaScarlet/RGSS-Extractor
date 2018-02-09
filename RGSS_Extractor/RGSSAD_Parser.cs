using System;
using System.IO;

namespace RGSS_Extractor
{
	internal class RGSSAD_Parser : Parser
	{
		public RGSSAD_Parser(BinaryReader file) : base(file)
		{
		}

		public string read_filename(int len)
		{
			byte[] array = this.inFile.ReadBytes(len);
			for (int i = 0; i < len; i++)
			{
				byte[] expr_18_cp_0 = array;
				int expr_18_cp_1 = i;
				expr_18_cp_0[expr_18_cp_1] ^= (byte)this.magickey;
				this.magickey = this.magickey * 7 + 3;
			}
			return base.get_string(array);
		}

		public void parse_table()
		{
			while (this.inFile.BaseStream.Position != this.inFile.BaseStream.Length)
			{
				int num = this.inFile.ReadInt32();
				num ^= this.magickey;
				this.magickey = this.magickey * 7 + 3;
				string name = this.read_filename(num);
				long num2 = (long)this.inFile.ReadInt32();
				num2 ^= (long)this.magickey;
				this.magickey = this.magickey * 7 + 3;
				long position = this.inFile.BaseStream.Position;
				this.inFile.BaseStream.Seek(num2, SeekOrigin.Current);
				Entry entry = new Entry();
				entry.name = name;
				entry.offset = position;
				entry.size = num2;
				entry.datakey = this.magickey;
				this.entries.Add(entry);
			}
		}

		public override void parse_file()
		{
			uint magickey = 3735931646u;
			this.magickey = (int)magickey;
			this.parse_table();
		}
	}
}
