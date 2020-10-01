﻿using Kermalis.EndianBinaryIO;
using System.Collections;
using System.Collections.Generic;

namespace Kermalis.DLS2
{
    // LIST Chunk - Page 40 of spec
    public sealed class ListChunk : DLSChunk, IReadOnlyList<DLSChunk>
    {
        /// <summary>Length 4</summary>
        public string Identifier { get; set; }
        private readonly List<DLSChunk> _children;

        public int Count => _children.Count;
        public DLSChunk this[int index] => _children[index];

        internal ListChunk(EndianBinaryReader reader) : base("LIST", reader)
        {
            long endOffset = reader.BaseStream.Position + Size;
            Identifier = reader.ReadString(4, false);
            _children = DLS.GetAllChunks(reader, endOffset);
        }

        internal override void UpdateSize()
        {
            Size = 4; // Identifier
            foreach (DLSChunk c in _children)
            {
                c.UpdateSize();
                Size += c.Size + 8;
            }
        }

        internal override void Write(EndianBinaryWriter writer)
        {
            base.Write(writer);
            writer.Write(Identifier, 4);
            foreach (DLSChunk c in _children)
            {
                c.Write(writer);
            }
        }

        public IEnumerator<DLSChunk> GetEnumerator()
        {
            return _children.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _children.GetEnumerator();
        }
    }
}
