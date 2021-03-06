﻿namespace Infusion
{
    public struct Color
    {
        public Color(ushort id)
        {
            Id = id;
        }

        public override bool Equals(object obj)
        {
            if (obj is Color)
            {
                var otherColor = (Color) obj;

                return Equals(otherColor);
            }

            return false;
        }

        public bool Equals(Color other)
        {
            return Id == other.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public static bool operator ==(Color color1, Color color2) => color1.Equals(color2);

        public static bool operator !=(Color color1, Color color2) => !color1.Equals(color2);

        public static explicit operator Color(ushort id)
        {
            return new Color(id);
        }

        public static implicit operator ushort(Color color)
        {
            return color.Id;
        }

        public override string ToString() => $"0x{Id:X4}";

        public ushort Id { get; }
    }
}