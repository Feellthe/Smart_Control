using Ambilight.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Ambilight.Ampoules
{
    public enum mode { RGB,White,Scene};
    public enum scene {Ocean = 1,Romance = 2,Sunset = 3,Party = 4,Fireplace = 5,Cozy = 6,Forest = 7,Pastel_Colors = 8,Wake_up = 9,Bedtime = 10,Warm_white = 11,Daylight = 12,Cool_white = 13,Night_light = 14,Focus = 15,Relax = 16,True_colors = 17,TV_time = 18,Plantgrowth = 19,Spring = 20,Summer = 21,Fall = 21,Deepdive = 23,Jungle = 24,Mojito = 25,Club = 26,Christmas = 27,Halloween = 28,Candlelight = 29,Golden_white = 30,Pulse = 31,Steampunk = 32,Rhythm = 1000};
    public class WIZ_A60 : Connected_Device, Standard
    {
        private int Color_R=0;
        private int Color_G=0;
        private int Color_B=0;
        public const string model = "A60";
        public const string brand = "WIZ";
        internal int Intensity=50;
        internal int White_Color=4500;
        private int Scene_Speed=50;
        public string UDP_response;
        public mode _currentMode;
        public scene _currentScene;
        

        public void Update_Status()
        {
            if (_currentMode == mode.RGB)
            {
                
                Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram,ProtocolType.Udp);
                IPAddress ip_adress = IPAddress.Parse(ip);

                IPEndPoint endPoint = new IPEndPoint(ip_adress, port);
                byte[] send_buffer = Encoding.ASCII.GetBytes("{\"method\":\"setPilot\",\"params\":{\"r\":" + Color_R + ",\"g\":" + Color_G + ",\"b\":" + Color_B + "}}");
                
                try
                {
                    sock.SendTo(send_buffer, endPoint);
                }
                    
                catch (System.Net.Sockets.SocketException e)
                {
                    Console.WriteLine("Erreur acces reseau");
                }

            }
            else if (_currentMode == mode.White)
            {
                Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                IPAddress ip_adress = IPAddress.Parse(ip);
                
                IPEndPoint endPoint = new IPEndPoint(ip_adress, port);
                byte[] send_buffer = Encoding.ASCII.GetBytes("{\"method\":\"setPilot\",\"params\":{\"temp\":" + White_Color + ",\"dimming\":"+ Intensity + "}}");
                sock.SendTo(send_buffer, endPoint);


            }
            else if (_currentMode == mode.Scene)
            {
                Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                IPAddress ip_adress = IPAddress.Parse(ip);

                IPEndPoint endPoint = new IPEndPoint(ip_adress, port);
                byte[] send_buffer = Encoding.ASCII.GetBytes("{\"method\":\"setPilot\",\"params\":{\"sceneId\":" + (int)_currentScene + ",\"dimming\":" + Intensity + ",\"speed\":" + Scene_Speed + "}}");
                sock.SendTo(send_buffer, endPoint);
            }
        }

        public void Set_Red(int red)
        {
            Color_R = red;
            _currentMode = mode.RGB;
        }
        public void Set_Green(int green)
        {
            Color_G = green;
            _currentMode = mode.RGB;
        }
        public void Set_Blue(int blue)
        {
            Color_B = blue;
            _currentMode = mode.RGB;
        }

        public void Set_Scene(scene scene)
        {
            _currentScene = scene;
            _currentMode = mode.Scene;


        }

        public void Set_White_Color(int color)
        {
            if (color <= 6200 && color >= 2200)
            {
                White_Color = color;
                _currentMode = mode.White;
            }
            
        }
        public void Set_Scene_Speed(int speed)
        {
            if (speed >= 10 && speed <= 100)
            {
                Scene_Speed = speed;
                _currentMode = mode.Scene;
            }
            
        }

        public void Set_Intensity(int intensity)
        {
            if (intensity >= 10 && intensity <= 100)
            {
                Intensity = intensity;
            }
        }

        static void SendUdp(int srcPort, string dstIp, int dstPort, byte[] data)
        {
            using (UdpClient c = new UdpClient(srcPort))
                c.Send(data, data.Length, dstIp, dstPort);
                
        }

        

        public void Turn_ON()
        {
            //Turning ON the light 
        }

        public void Turn_OFF()
        {
            //Turning OFF the light 
        }
    }

   
}
