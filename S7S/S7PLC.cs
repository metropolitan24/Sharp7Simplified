using Sharp7;

namespace Sharp7Simplified
{
    public class S7PLC
    {
        private bool _connectionSucceeded;
        private S7Client _client;

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
    }
}