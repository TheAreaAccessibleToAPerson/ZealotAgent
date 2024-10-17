namespace Zealot
{
    public struct MessageHeader
    {
        public const int LENGHT = 7;

        public const int LENGTH_BYTE_INDEX = 0;

        public const int TYPE_1BYTE_INDEX = 1;
        public const int TYPE_2BYTE_INDEX = 2;

        public const int MESSAGE_LENGTH_1BYTE_INDEX = 3;
        public const int MESSAGE_LENGTH_2BYTE_INDEX = 4;
        public const int MESSAGE_LENGTH_3BYTE_INDEX = 5;
        public const int MESSAGE_LENGTH_4BYTE_INDEX = 6;
    }
}