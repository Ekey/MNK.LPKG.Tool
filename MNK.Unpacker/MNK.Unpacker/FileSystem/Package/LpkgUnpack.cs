﻿using System;
using System.IO;
using System.Collections.Generic;

namespace MNK.Unpacker
{
    class LpkgUnpack
    {
        static List<LpkgEntry> m_EntryTable = new List<LpkgEntry>();

        public static void iDoIt(String m_Archive, String m_DstFolder)
        {
            LpkgHashList.iLoadProject();
            using (FileStream TLpkgStream = File.OpenRead(m_Archive))
            {
                var m_Header = new LpkgHeader();

                m_Header.dwMagic = TLpkgStream.ReadUInt32();
                m_Header.dwTableDecompressedSize = TLpkgStream.ReadInt32();

                if (m_Header.dwMagic != 0x3144434C)
                {
                    throw new Exception("[ERROR]: Invalid magic of compressed LPKG index file!");
                }

                var lpSrcTable = TLpkgStream.ReadBytes((Int32)TLpkgStream.Length - 8);
                var lpDstTable = Zlib.iDecompress(lpSrcTable);

                using (var TTableReader = new MemoryStream(lpDstTable))
                {
                    var m_TableHeader = new LpkgTableHeader();

                    m_TableHeader.dwMagic = TTableReader.ReadUInt32();
                    m_TableHeader.dwUnknown1 = TTableReader.ReadInt32();
                    m_TableHeader.dwTotalFiles = TTableReader.ReadInt32();
                    m_TableHeader.dwTotalLpkgs = TTableReader.ReadInt32();

                    if (m_TableHeader.dwMagic != 0x4341434C)
                    {
                        throw new Exception("[ERROR]: Invalid magic of decompressed LPKG index file!");
                    }

                    if (m_TableHeader.dwUnknown1 != 120)
                    {
                        throw new Exception("[ERROR]: Invalid version of decompressed LPKG index file!");
                    }

                    m_EntryTable.Clear();
                    for (Int32 i = 0; i < m_TableHeader.dwTotalFiles; i++)
                    {
                        UInt32 dwNameHash = TTableReader.ReadUInt32();
                        Int32 dwPackageID = TTableReader.ReadInt32();
                        Int64 dwOffset = TTableReader.ReadInt64();
                        Int64 dwSize = TTableReader.ReadInt64();

                        var TEntry = new LpkgEntry
                        {
                            dwNameHash = dwNameHash,
                            dwPackageID = dwPackageID,
                            dwOffset = dwOffset,
                            dwSize = dwSize,
                        };

                        m_EntryTable.Add(TEntry);
                    }

                    TTableReader.Dispose();
                }

                TLpkgStream.Dispose();
            }

            foreach (var m_Entry in m_EntryTable)
            {
                String m_FileName = LpkgHashList.iGetNameFromHashList(m_Entry.dwNameHash);
                String m_FullPath = m_DstFolder + m_FileName.Replace("/", @"\");
                String m_Package = Path.GetDirectoryName(m_Archive) + @"\" + String.Format("data.lpkg.{0}", m_Entry.dwPackageID);

                Utils.iSetInfo("[UNPACKING]: " + m_FileName);
                Utils.iCreateDirectory(m_FullPath);

                LpkgHelpers.ReadWriteFile(m_Package, m_FullPath, m_Entry.dwOffset, (Int32)m_Entry.dwSize);
            }
        }
    }
}
