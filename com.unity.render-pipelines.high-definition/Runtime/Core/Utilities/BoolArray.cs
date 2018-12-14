using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityEngine.Experimental.Rendering
{
    public interface ICheapBoolArray
    {
        uint capacity { get; }
        bool allFalse { get; }
        bool allTrue { get; }
        bool this[uint index] { get; set; }
    }

    [Serializable]
    public struct CheapBoolArray8 : ICheapBoolArray
    {
        [SerializeField]
        byte data;

        public uint capacity => 8u;
        public bool allFalse => data == 0u;
        public bool allTrue => data == byte.MaxValue;

        public bool this[uint index]
        {
            get => (data & (1u << (int)index)) != 0u;
            set
            {
                if (value)
                    data = (byte)(data | (1u << (int)index));
                else
                    data = (byte)(data & ~(1u << (int)index));
            }
        }

        public CheapBoolArray8(byte initValue) => data = initValue;
        public CheapBoolArray8(IEnumerable<uint> bitIndexTrue)
        {
            data = (byte)0u;
            if (bitIndexTrue == null)
                return;
            for (int index = bitIndexTrue.Count() - 1; index >= 0; --index)
                data += (byte)(1u << (int)bitIndexTrue.ElementAt(index));
        }

        public static CheapBoolArray8 operator |(CheapBoolArray8 a, CheapBoolArray8 b) => new CheapBoolArray8((byte)(a.data | b.data));
        public static CheapBoolArray8 operator &(CheapBoolArray8 a, CheapBoolArray8 b) => new CheapBoolArray8((byte)(a.data & b.data));
    }

    [Serializable]
    public struct CheapBoolArray16 : ICheapBoolArray
    {
        [SerializeField]
        ushort data;

        public uint capacity => 16u;
        public bool allFalse => data == 0u;
        public bool allTrue => data == ushort.MaxValue;

        public bool this[uint index]
        {
            get => (data & (1u << (int)index)) != 0u;
            set
            {
                if (value)
                    data = (ushort)(data | (1u << (int)index));
                else
                    data = (ushort)(data & ~(1u << (int)index));
            }
        }

        public CheapBoolArray16(ushort initValue) => data = initValue;
        public CheapBoolArray16(IEnumerable<uint> bitIndexTrue)
        {
            data = (ushort)0u;
            if (bitIndexTrue == null)
                return;
            for (int index = bitIndexTrue.Count() - 1; index >= 0; --index)
                data += (ushort)(1u << (int)bitIndexTrue.ElementAt(index));
        }

        public static CheapBoolArray16 operator |(CheapBoolArray16 a, CheapBoolArray16 b) => new CheapBoolArray16((ushort)(a.data | b.data));
        public static CheapBoolArray16 operator &(CheapBoolArray16 a, CheapBoolArray16 b) => new CheapBoolArray16((ushort)(a.data & b.data));
    }

    [Serializable]
    public struct CheapBoolArray32 : ICheapBoolArray
    {
        [SerializeField]
        uint data;

        public uint capacity => 32u;
        public bool allFalse => data == 0u;
        public bool allTrue => data == uint.MaxValue;

        public bool this[uint index]
        {
            get => (data & (1u << (int)index)) != 0u;
            set
            {
                if (value)
                    data = data | (1u << (int)index);
                else
                    data = data & ~(1u << (int)index);
            }
        }

        public CheapBoolArray32(uint initValue) => data = initValue;
        public CheapBoolArray32(IEnumerable<uint> bitIndexTrue)
        {
            data = 0u;
            if (bitIndexTrue == null)
                return;
            for (int index = bitIndexTrue.Count() - 1; index >= 0; --index)
                data += 1u << (int)bitIndexTrue.ElementAt(index);
        }

        public static CheapBoolArray32 operator |(CheapBoolArray32 a, CheapBoolArray32 b) => new CheapBoolArray32(a.data | b.data);
        public static CheapBoolArray32 operator &(CheapBoolArray32 a, CheapBoolArray32 b) => new CheapBoolArray32(a.data & b.data);
    }

    [Serializable]
    public struct CheapBoolArray64 : ICheapBoolArray
    {
        [SerializeField]
        ulong data;

        public uint capacity => 64u;
        public bool allFalse => data == 0uL;
        public bool allTrue => data == ulong.MaxValue;

        public bool this[uint index]
        {
            get => (data & (1uL << (int)index)) != 0uL;
            set
            {
                if (value)
                    data = data | (1uL << (int)index);
                else
                    data = data & ~(1uL << (int)index);
            }
        }

        public CheapBoolArray64(ulong initValue) => data = initValue;
        public CheapBoolArray64(IEnumerable<uint> bitIndexTrue)
        {
            data = 0L;
            if (bitIndexTrue == null)
                return;
            for (int index = bitIndexTrue.Count() - 1; index >= 0; --index)
                data += 1uL << (int)bitIndexTrue.ElementAt(index);
        }

        public static CheapBoolArray64 operator |(CheapBoolArray64 a, CheapBoolArray64 b) => new CheapBoolArray64(a.data | b.data);
        public static CheapBoolArray64 operator &(CheapBoolArray64 a, CheapBoolArray64 b) => new CheapBoolArray64(a.data & b.data);
    }

    [Serializable]
    public struct CheapBoolArray128 : ICheapBoolArray
    {
        [SerializeField]
        CheapBoolArray64 data1;
        [SerializeField]
        CheapBoolArray64 data2;

        public uint capacity => 128u;
        public bool allFalse => data1.allFalse && data2.allFalse;
        public bool allTrue => data1.allTrue && data2.allTrue;

        public bool this[uint index]
        {
            get => (index < 64u) ? data1[index] : data2[index - 64u];
            set
            {
                if (index < 64u)
                    data1[index] = value;
                else
                    data2[index - 64u] = value;
            }
        }

        public CheapBoolArray128(ulong initValue1, ulong initValue2)
        {
            data1 = new CheapBoolArray64(initValue1);
            data2 = new CheapBoolArray64(initValue2);
        }
        public CheapBoolArray128(IEnumerable<uint> bitIndexTrue)
        {
            if (bitIndexTrue == null)
            {
                data1 = new CheapBoolArray64(0uL);
                data2 = new CheapBoolArray64(0uL);
                return;
            }
            var groups = bitIndexTrue.GroupBy(idx => idx < 128u);
            data1 = new CheapBoolArray64(groups.First());
            data2 = new CheapBoolArray64(groups.Last());
        }
        private CheapBoolArray128(CheapBoolArray64 initValue1, CheapBoolArray64 initValue2)
        {
            data1 = initValue1;
            data2 = initValue2;
        }

        public static CheapBoolArray128 operator |(CheapBoolArray128 a, CheapBoolArray128 b) => new CheapBoolArray128(a.data1 | b.data1, a.data2 | b.data2);
        public static CheapBoolArray128 operator &(CheapBoolArray128 a, CheapBoolArray128 b) => new CheapBoolArray128(a.data1 & b.data1, a.data2 & b.data2);
    }
}
