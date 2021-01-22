using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AsyncSocketLib
{
    public class AsyncSocketServer
    {
        IPAddress mIP;
        int mPort;
        TcpListener mServer;
        List<TcpClient> mClients;

        public AsyncSocketServer()
        {
            mClients = new List<TcpClient>();
        }

        //Server inizia as ascoltare
        public async void InAscolto(IPAddress ipaddr = null, int port = 23000)
        {
            //controlli generali
            if (ipaddr == null)
            {
                ipaddr = IPAddress.Any;
            }

            if (port < 0 || port > 65535)
            {
                port = 23000;
            }

            mIP = ipaddr;
            mPort = port;

            mServer = new TcpListener(mIP, mPort);

            Debug.WriteLine("Server in ascolto su IP: {0} - Porta: {1}", mIP.ToString(), mPort.ToString());

            mServer.Start();

            Debug.WriteLine("Server avviato.");

            while (true)
            {
                TcpClient client = await mServer.AcceptTcpClientAsync();
                mClients.Add(client);
                Debug.WriteLine("Client connessi: {0}. Client connesso: {1}", mClients.Count, client.Client.RemoteEndPoint);
                RiceviMessaggio(client);
            }
        }

        public async void RiceviMessaggio(TcpClient client)
        {
            NetworkStream stream = null;
            StreamReader reader = null;

            try
            {
                stream = client.GetStream();
                reader = new StreamReader(stream);
                char[] buff = new char[512];
                int nBytes = 0;

                while (true)
                {
                    Debug.WriteLine("In attesa di un messaggio");
                    //ricezione messaggio asincrono
                    nBytes = await reader.ReadAsync(buff, 0, buff.Length);

                    if (nBytes == 0)
                    {
                        Debug.WriteLine("Client Disconnesso");
                        break;
                    }
                    string recvText = new string(buff);
                    Debug.WriteLine("N° byte: {0}. Messaggio: {1}", nBytes, recvText);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Errore: " + ex.Message);
            }
        }

        private void RimuoviClient(TcpClient client)
        {
            if (mClients.Contains(client))
            {
                mClients.Remove(client);
            }
        }

        public void InviaTutti(string messaggio)
        {
            try
            {
                foreach (TcpClient client in mClients)
                {
                    byte[] buff = Encoding.ASCII.GetBytes(messaggio);
                    client.GetStream().WriteAsync(buff, 0, buff.Length);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Errore: " + ex.Message);
            }
        }

        public void Disconnetti()
        {
            try
            {
                foreach (TcpClient client in mClients)
                {
                    client.Close();
                    RimuoviClient(client);
                }
                mServer.Stop();
                mServer = null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Errore: " + ex.Message);
            }
        }
    }
}
