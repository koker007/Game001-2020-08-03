using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class TimeWorld : MonoBehaviour
{
    static bool randomized = false;
    static int numServer = 0;
    static DateTime utcDateTime = DateTime.MinValue;
    static float timeLastUpdate = 0; //��������� ����� (������ ���������) ���������� ������� (�����������)
    public static DateTime GetFastestNISTDate()
    {
        //var result = DateTime.MinValue;
        //DateTime utcDateTime = DateTime.MinValue;

        // Initialize the list of NIST time servers
        // http://tf.nist.gov/tf-cgi/servers.cgi
        string[] servers = new string[] {
                "time-a-g.nist.gov",
                "time-b-g.nist.gov",
                "time-c-g.nist.gov",
                "time-d-g.nist.gov",
                "time-e-g.nist.gov",
                "time-a-wwv.nist.gov",
                "time-b-wwv.nist.gov",
                "time-c-wwv.nist.gov",
                "time-d-wwv.nist.gov",
                "time-e-wwv.nist.gov",
                "time-a-b.nist.gov",
                "time-b-b.nist.gov",
                "time-c-b.nist.gov",
                "time-d-b.nist.gov",
                "time-e-b.nist.gov",
                "time-e-b.nist.gov",
                "time.nist.gov",
                "ntp-b.nist.gov",
                "ntp-wwv.nist.gov",
                "ntp-d.nist.gov",
                "ut1-wwv.nist.gov"
            };

        /*
                string[] servers = new string[] {
                "nist1-ny.ustiming.org",
                "nist1-nj.ustiming.org",
                "nist1-pa.ustiming.org",
                "time-a.nist.gov",
                "time-b.nist.gov",
                "nist1.aol-va.symmetricom.com",
                "nist1.columbiacountyga.gov",
                "nist1-chi.ustiming.org",
                "nist.expertsmi.com",
                "nist.netservicesgroup.com"
            };
        */


        //���� ����� ��� �� ���� ��������
        if (utcDateTime == DateTime.MinValue)
        {
            // Try 5 servers in random order to spread the load 
            //������������ ������� ���� ����� �� ������
            if (!randomized)
            {
                for (int now = 0; now < servers.Length; now++)
                {
                    int rand = UnityEngine.Random.Range(0, servers.Length);
                    if (now != rand)
                    {
                        string bufferRand = servers[rand];
                        servers[rand] = servers[now];
                        servers[now] = bufferRand;
                    }
                }
                randomized = true;
            }

            //�������� ���� ������ ��� �� ���������� � ����� �� ��������
            if (numServer < servers.Length)
            {
                numServer++;
                try
                {
                    // Connect to the server (at port 13) and get the response
                    //������������ � ������� (���� 13) � �������� �����
                    string serverResponse = string.Empty;
                    using (var reader = new StreamReader(new System.Net.Sockets.TcpClient(servers[numServer - 1], 13).GetStream()))
                    {
                        serverResponse = reader.ReadToEnd();
                    }

                    Debug.Log(servers[numServer - 1]);
                    // If a response was received
                    //���� ��� ������� �����
                    if (!string.IsNullOrEmpty(serverResponse))
                    {
                        Debug.Log(serverResponse);

                        // Split the response string ("55596 11-02-14 13:54:11 00 0 0 478.1 UTC(NIST) *")
                        //��������� ������ ������ ("55596 11-02-14 13:54:11 00 0 0 478.1 UTC (NIST) *")
                        string[] tokens = serverResponse.Split(' ');

                        // Check the number of tokens
                        //��������� ���������� �������
                        if (tokens.Length >= 6)
                        {
                            // Check the health status
                            //��������� ��������� ��������
                            string health = tokens[5];
                            if (health == "0")
                            {
                                // Get date and time parts from the server response
                                //�������� ����� ���� � ������� �� ������ �������
                                string[] dateParts = tokens[1].Split('-');
                                string[] timeParts = tokens[2].Split(':');

                                // Create a DateTime instance
                                //������� ��������� DateTime
                                utcDateTime = new DateTime(
                                    Convert.ToInt32(dateParts[0]) + 2000,
                                    Convert.ToInt32(dateParts[1]), Convert.ToInt32(dateParts[2]),
                                    Convert.ToInt32(timeParts[0]), Convert.ToInt32(timeParts[1]),
                                    Convert.ToInt32(timeParts[2]));

                                // Convert received (UTC) DateTime value to the local timezone
                                //�������������� ����������� �������� DateTime (UTC) � ������� ������� ����
                                //result = utcDateTime.ToLocalTime();

                                //return result;
                                return utcDateTime;
                                // Response successfully received; exit the loop

                            }
                        }

                    }
                    else
                    {
                        Debug.Log(servers[numServer - 1] + "IS BAD");
                    }
                }
                catch
                {
                    // Ignore exception and try the next server
                    // ������������ ���������� � ����������� ��������� ������
                }
            }
            //���� ������� �� ������ �����������
            else
            {
                utcDateTime = DateTime.UtcNow;
            }


            //���� ����� ������ ��� ������������
            if (utcDateTime != DateTime.MinValue)
                timeLastUpdate = Time.unscaledTime;
        }

        //return result;
        return utcDateTime;
    }

    public static double GetSecondsFrom2020(DateTime dateTime)
    {
        double result = 0;
        if (dateTime != DateTime.MinValue)
        {
            result = dateTime.Subtract(new DateTime(2020, 1, 1)).TotalSeconds;
        }

        return result;
    }

    // Update is called once per frame
    void Update()
    {
        TestGetTime();
    }


    void TestGetTime()
    {
        if (utcDateTime == DateTime.MinValue)
        {
            GetFastestNISTDate();
            timeLastUpdate = Time.unscaledTime;
        }
        //���� ����� ���� ������ ������� ��� ������
        else
        {
            utcDateTime = utcDateTime.AddSeconds(Time.unscaledTime - timeLastUpdate);
            timeLastUpdate = Time.unscaledTime;
        }
    }

    public static DateTime GetTimeWorld()
    {
        return utcDateTime;
    }
}
