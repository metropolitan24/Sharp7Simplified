using Sharp7;

namespace Sharp7Simplified
{
    public class S7PLC
    {
        private bool _connectionSucceeded;
        private S7Client _client;
        private int k;

        public bool ConnectionState
        {
            get { return _connectionSucceeded; }
            set { _connectionSucceeded = value; }
        }


        public S7PLC(string ip)
        {
            _client = new S7Client();
            int connectionResult = _client.ConnectTo(ip, 0, 1);

            if(connectionResult == 0) { _connectionSucceeded = true; }
            else
            {
                _connectionSucceeded = false; Console.WriteLine(_client.ErrorText(connectionResult));
            }
        }

        public int Read(int db, int nodepos, out object tag) 
        {
            try
            {
                byte[] bufferDB = new byte[18];
                int readResult = _client.DBRead(db, 0, 18, bufferDB);
                if (readResult != 0)
                {
                    Console.WriteLine(_client.LastError);
                }
                tag = 0;
                switch (Type.GetTypeCode(tag.GetType()))
                {
                    case TypeCode.Boolean:
                        tag = S7.GetBitAt(bufferDB, nodepos, 0);
                        break;
                    case TypeCode.Byte:
                        tag = S7.GetByteAt(bufferDB, nodepos);
                        break;
                    case TypeCode.Char:
                        tag = S7.GetCharsAt(bufferDB, nodepos, 1);
                        break;
                    case TypeCode.Int32:
                        tag = S7.GetIntAt(bufferDB, nodepos);
                        break;
                    case TypeCode.UInt32:
                        tag = S7.GetUIntAt(bufferDB, nodepos);
                        break;
                }

                return 1;
            }
            catch(Exception e)
            {
                tag = 0;
                return 0;
            }
        }

        public int Write(int db, int nodepos, object writeVar)
        {
            try
            {
                byte[] writeBuffer = new byte[12];
                switch (Type.GetTypeCode(writeVar.GetType()))
                {
                    case TypeCode.Boolean:
                        S7.SetBitAt(writeBuffer, nodepos, 0, (bool)writeVar);
                        break;
                    case TypeCode.Byte:
                        S7.SetByteAt(writeBuffer, nodepos, 0);
                        break;
                    case TypeCode.Char:
                        S7.SetCharsAt(writeBuffer, nodepos, writeVar.ToString());
                        break;
                    case TypeCode.Int32:
                        S7.SetIntAt(writeBuffer, nodepos, (short)writeVar);
                        break;
                    case TypeCode.UInt32:
                        S7.SetUIntAt(writeBuffer, nodepos, (ushort)writeVar);
                        break;
                }

                return 1;
            }
            catch (Exception e)
            {
                return 0;
            }
        }
    }
}
