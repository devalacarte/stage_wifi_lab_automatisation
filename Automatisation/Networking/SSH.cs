/*
 //password based authentication
 //PasswordAuthenticationMethod passAuth = new PasswordAuthenticationMethod(_user, _password);
 //private key based authentication
 //PrivateKeyAuthenticationMethod pkAuth = new PrivateKeyAuthenticationMethod("username", new PrivateKeyFile[]{ new PrivateKeyFile(@"..\openssh.key","passphrase")});
 //ConnectionInfo cinfo = new ConnectionInfo(_host, _port, _user,new AuthenticationMethod[]{passAuth,pkAuth});
*/

using Renci.SshNet;
using System.ComponentModel;

namespace Automatisation.Networking
{
    
    public class SSHMessage
    {
        private string _sender;

        public string Sender
        {
            get { return _sender; }
            set { if (_sender != value) { _sender = value; } }
        }
        private string _receiver;

        public string Receiver
        {
            get { return _receiver; }
            set { if (_receiver != value) { _receiver = value; } }
        }
        private string _message;

        public string Message
        {
            get { return _message; }
            set { if (_message != value) { _message = value; } }
        }
        public SSHMessage(string sender, string receiver, string message)
        {
            Sender = sender;
            Receiver = receiver;
            Message = message;
        }
    }
    
}