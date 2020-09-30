﻿using Kermalis.EndianBinaryIO;
using System.Collections.Generic;
using System.IO;

namespace Kermalis.DLS2
{
    // Pool Table Chunk - Page 54 of spec
    public sealed class PoolTableChunk : DLSChunk
    {
        private readonly uint _byteSize;
        private readonly uint _numCues;
        private readonly List<uint> _poolCues;

        internal PoolTableChunk(EndianBinaryReader reader) : base("ptbl", reader)
        {
            _byteSize = reader.ReadUInt32();
            if (_byteSize != 8)
            {
                throw new InvalidDataException(); // TODO: Support?
            }
            _numCues = reader.ReadUInt32();
            _poolCues = new List<uint>((int)_numCues);
            for (uint i = 0; i < _numCues; i++)
            {
                _poolCues.Add(reader.ReadUInt32());
            }
        }

        internal override void UpdateSize()
        {
            Size = 4 // _byteSize
                + 4 // _numCues
                + (4 * _numCues); // _poolCues
        }

        internal override void Write(EndianBinaryWriter writer)
        {
            base.Write(writer);
            writer.Write(_byteSize);
            writer.Write(_numCues);
            for (int i = 0; i < _numCues; i++)
            {
                writer.Write(_poolCues[i]);
            }
        }
    }
}
