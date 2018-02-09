using System;
using System.IO;

namespace RGSS_Extractor
{
	internal class RGSS3A_Parser : Parser
	{
		public RGSS3A_Parser(BinaryReader file) : base(file)
		{
		}

		public string read_filename(int len)
		{
			byte[] array = this.inFile.ReadBytes(len);
			for (int i = 0; i < len; i++)
			{
				byte[] expr_18_cp_0 = array;
				int expr_18_cp_1 = i;
				expr_18_cp_0[expr_18_cp_1] ^= (byte)(this.magickey >> 8 * (i % 4));
			}
			return base.get_string(array);
		}

		public void parse_table()
		{
			while (true)
			{
				long num = (long)this.inFile.ReadInt32();
				num ^= (long)this.magickey;
				if (num == 0L)
				{
					break;
				}
				long num2 = (long)this.inFile.ReadInt32();
				int num3 = this.inFile.ReadInt32();
				int num4 = this.inFile.ReadInt32();
				num2 ^= (long)this.magickey;
				num3 ^= this.magickey;
				num4 ^= this.magickey;
				string name = this.read_filename(num4);
				Entry entry = new Entry();
				entry.offset = num;
				entry.name = name;
				entry.size = num2;
				entry.datakey = num3;
				this.entries.Add(entry);
			}
		}

		public override void parse_file()
		{
			this.magickey = this.inFile.ReadInt32() * 9 + 3;
			this.parse_table();
		}
	}
}
