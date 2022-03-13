using System;
using System.Collections;
using System.Collections.Generic;

namespace HelpLine.BuildingBlocks.Application.Search
{
    public class Sort : IEquatable<Sort>
    {
        public bool Asc { get; set; } = true;
        public IEnumerable<string> Path { get; set; }

        public Sort()
        {
        }

        public Sort(bool asc, params string[] path)
        {
            Asc = asc;
            Path = path;
        }

        public bool Equals(Sort? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Asc == other.Asc && string.Join(".", Path).ToUpperInvariant() == string.Join(".", other.Path).ToUpperInvariant();
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Sort) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Asc, Path);
        }
    }
}
