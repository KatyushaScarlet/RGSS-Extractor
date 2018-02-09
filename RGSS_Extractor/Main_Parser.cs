using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RGSS_Extractor
{
	public class Main_Parser
	{
		private Parser parser;

		private Parser get_parser(int version, BinaryReader inFile)
		{
			if (version == 1)
			{
				return new RGSSAD_Parser(inFile);
			}
			if (version == 3)
			{
				return new RGSS3A_Parser(inFile);
			}
			return null;
		}

		public List<Entry> parse_file(string path)
		{
			BinaryReader binaryReader = new BinaryReader(File.OpenRead(path));
			string @string = Encoding.UTF8.GetString(binaryReader.ReadBytes(6));
			if (@string != "RGSSAD")
			{
				return null;
			}
			binaryReader.ReadByte();
			int version = (int)binaryReader.ReadByte();
			this.parser = this.get_parser(version, binaryReader);
			if (this.parser == null)
			{
				return null;
			}
			this.parser.parse_file();
			return this.parser.entries;
		}

		public byte[] get_filedata(Entry e)
		{
			return this.parser.read_data(e.offset, e.size, e.datakey);
		}

		public void export_file(Entry e)
		{
			this.parser.write_file(e);
		}

		public void export_archive()
		{
			if (this.parser == null)
			{
				return;
			}
			this.parser.write_entries();
		}

		public void close_file()
		{
			this.parser.close_file();
		}
	}
}
