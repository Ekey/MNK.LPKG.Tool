using System;

namespace MNK.Unpacker
{
    class LpkgHeader
    {
        public UInt32 dwMagic { get; set; } // LCD1 (0x3144434C)
        public Int32 dwTableDecompressedSize { get; set; }
    }

    class LpkgTableHeader
    {
        public UInt32 dwMagic { get; set; } // LCAC (0x4341434C)
        public Int32 dwUnknown1 { get; set; } // 120 (Version?)
        public Int32 dwTotalFiles { get; set; }
        public Int32 dwTotalLpkgs { get; set; } // 15
    }
}
