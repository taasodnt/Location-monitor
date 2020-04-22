using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

//http://163.18.53.144/F459/php/c%23_serverbackend/C%23_GetPhoneMac.php

namespace WindowsFormsApp4.MyClasses
{
    public class DataSourceManager
    {
        private  readonly HttpClient client = new HttpClient();
        private  Dictionary<string, Dictionary<string, Point>> locationOfBeacons = new Dictionary<string, Dictionary<string, Point>>();
        private  Dictionary<string, bool> beaconListBuffer = new Dictionary<string, bool>();
        private System.Timers.Timer timer;
        private  Dictionary<string, string> firefighters;
        public  object firefightersLock = new object();
        public  Task getFirefighterTask;

        public Dictionary<string, Dictionary<string, Point>> getLocationOfBeacons()
        {
            return locationOfBeacons;
        }

        public List<string> getBeaconFromList()
        {
            return new List<string>(beaconListBuffer.Keys);
        }

        public void close()
        {
            timer.Stop();
        }

        public void fetchDataFromServer()
        {
            getNewBeaconList();
        }

        public string[] getFirefighters()
        {
            lock (firefightersLock)
            {
                Console.WriteLine("消防員人數: " + firefighters.Keys.Count);
                string[] result = new string[firefighters.Keys.Count];
                int indexOfFirefighters = 0;
                foreach(string firefighter in firefighters.Keys)
                {
                    result[indexOfFirefighters++] = firefighter;
                }
                return result;
            }
        }

        public async Task getFirefighterMac()
        {
            try
            {
                Console.WriteLine("getFirefighterMac");
                var data = await client.GetAsync("http://163.18.53.144/F459/php/c%23_serverbackend/C%23_GetPhoneMac.php");
                data.EnsureSuccessStatusCode();
                var response = await data.Content.ReadAsStringAsync();
                string dataString = response.ToString();
                Console.WriteLine("getFirefighterMac " + dataString);
                lock (firefightersLock)
                {
                    if (dataString != null && dataString != "")
                    {
                        String[] tmp = splitString(dataString, ',');
                        foreach (string item in tmp)
                        {
                            Console.WriteLine(item);
                            if (!firefighters.ContainsKey(item))
                            {
                                firefighters.Add(item, "");
                            }
                        }
                    }
                }
            }
            catch(Exception e)
            {

            }   
        }
        public async Task getNewBeaconList()
        {
            try
            {
                var jsonData = await client.GetAsync("http://163.18.53.144/F459/php/c%23_serverbackend/C%23_GetBeaconMac.php");
                jsonData.EnsureSuccessStatusCode();
                var response = await jsonData.Content.ReadAsStringAsync();
                string originString = response.ToString();
                Console.WriteLine(originString);
                if (originString != null && originString != "")
                {
                    String[] tmp = splitString(originString, ',');
                    foreach (string item in tmp)
                    {
                        Console.WriteLine(item);
                        if (!beaconListBuffer.ContainsKey(item))
                        {
                            beaconListBuffer.Add(item, false);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("exception");
                Console.WriteLine(e.Message);
            }
        }

        private static T convertJsonData<T>(string rawData) where T : new()
        {
            // if string with JSON data is not empty, deserialize it to class and return its instance 
            return !string.IsNullOrEmpty(rawData) ? JsonConvert.DeserializeObject<T>(rawData) : new T();

        }

        public DataSourceManager()
        {
           // getFirefighterTask = getFirefighterMac();
           // getNewBeaconList();

            locationOfBeacons.Add("F4", new Dictionary<string, Point>());
            locationOfBeacons.Add("f459", new Dictionary<string, Point>());
            timer = new System.Timers.Timer(5000);
            timer.Elapsed += new System.Timers.ElapsedEventHandler(dataUpdate);
            timer.AutoReset = true;
            firefighters = new Dictionary<string, string>();
           // timer.Start();
        }

        public  bool addBeacon(string floor, string mac, Point location)
        {
            if (beaconListBuffer[mac] == false)
            {
                beaconListBuffer[mac] = true;
                /*  if (!locationOfBeacons.ContainsKey(floor))
                  {
                      Dictionary<string, Point> beacons = new Dictionary<string, Point>();
                      beaconListBuffer[mac] = true;
                      beacons.Add(mac, location);
                      locationOfBeacons.Add(floor, beacons);
                      return true;
                  }*/

                Dictionary<string, Point> theFloorBeacons = locationOfBeacons[floor];
                if (!theFloorBeacons.ContainsKey(mac))
                {
                    theFloorBeacons.Add(mac, location);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

            /*   Dictionary<string, Point> beacons = new Dictionary<string, Point>();
               beacons.Add(mac, location);
               locationOfBeacons.Add(floor, beacons);

               Dictionary<string, Point> theFloorBeacons = locationOfBeacons[floor];

               theFloorBeacons.Add(mac, location);*/
        }

        private  void dataUpdate(object source, System.Timers.ElapsedEventArgs e)
        {
            Console.WriteLine("dataUpdate");
            getData();
        }

        private  async Task getData() //get compare result
        {
            try
            {
                var jsonData = await client.GetAsync("http://163.18.53.144/f459/php/C%23_ServerBackend/C%23_GetJsonResult.php");
                jsonData.EnsureSuccessStatusCode();
                var response = await jsonData.Content.ReadAsStringAsync();
                var myData = convertJsonData<List<CompareResult>>(response);
                for (int i = 0; i < myData.Count; i++)
                {
                    string phoneMac = myData[i].PHONE_MAC;
                    string beaconMac = myData[i].BEACON_MAC;
                    lock (firefightersLock)
                    {
                        if (!firefighters.ContainsKey(phoneMac))
                        {
                            Console.WriteLine("add new firefighter");
                            firefighters.Add(phoneMac, beaconMac);
                        }
                        else
                        {
                            if (beaconMac != string.Empty)
                            {
                                Console.WriteLine("modify courrent firefighter");
                                firefighters[phoneMac] = beaconMac;
                                Console.WriteLine(beaconMac);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("exception");
                Console.WriteLine(e.Message);
            }
        }

        /*  private async Task getCompareResult()
          {
              foreach (string phoneMac in firefighters.Keys)
              {
                  Console.WriteLine(phoneMac + " foreach");
                  var values = new Dictionary<string, string> { { "data", phoneMac } };
                  var content = new FormUrlEncodedContent(values);
                  var response = await client.PostAsync("http://163.18.53.144/F459/php/C%23_ServerBackend/C%23_GetResult.php", content);
                  response.EnsureSuccessStatusCode();
                  var responseString = await response.Content.ReadAsStringAsync();
                  string originString = responseString.ToString(); 
                  if (originString != "")
                  {
                      try
                      {
                          string[] handledString = splitString(originString);
                          string mac = handledString[0];
                          string onWhichBeacon = handledString[1];
                          firefighters[mac] = onWhichBeacon;

                          Console.WriteLine("Raw data:" + responseString);
                          Console.WriteLine("Phone mac:" + mac);
                          Console.WriteLine("Beacon mac:" + onWhichBeacon);
                      }
                      catch (IndexOutOfRangeException e)
                      {
                          Console.WriteLine(e.Message);
                      }
                  }
              }
          }*/

        private static string[] splitString(string sourceString, char splitChar)
        {
            string[] splitedString = sourceString.Split(splitChar);
            foreach (string tmp in splitedString)
            {
                Console.WriteLine(tmp + " from splitString");
            }
            return splitedString;
        }

        public Dictionary<string, Point> firefighterLocation(string floor)
        {
            Dictionary<string, Point> result = new Dictionary<string, Point>();
            lock (firefightersLock)
            {
                foreach (string firefighter in firefighters.Keys)
                {
                  //  Dictionary<string, Point> theFloor = locationOfBeacons[floor];
                    if (locationOfBeacons.ContainsKey(floor) && firefighters.ContainsKey(firefighter))
                    {
                        Dictionary<string, Point> theFloor = locationOfBeacons[floor];
                        Console.WriteLine("firefighters.ContainsKey(firefighter)");
                        if (theFloor.ContainsKey(firefighters[firefighter]))
                        {
                            Point location = theFloor[firefighters[firefighter]];
                            result.Add(firefighter, location);
                        }
                    }
                }
            }
            return result;
        }

        public int numberOfFirefighter()
        {
            lock (firefightersLock)
            {
                return firefighters.Keys.Count;
            }
        }
    }
}
