using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Web;
using System.IO;


namespace face
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public static string base641;
        public static string base642;
        public static string accesstoken;
        private void Form1_Load(object sender, EventArgs e)
        {
            accesstoken = getaccess();
            base641 = Convert.ToBase64String(ImgToBase64String(@"D:\3.bmp"));
            getList();


           
            
        }



        public string getaccess()
        {
            string clientId = "qrLX5PLAM2ZaurpAtuG6Q1b5";
            String clientSecret = "xgn1y66IXAXg7enuZWqG2KmDgTdXI1kd";
            String authHost = "https://aip.baidubce.com/oauth/2.0/token";
            HttpClient client = new HttpClient();
            List<KeyValuePair<String, String>> paraList = new List<KeyValuePair<string, string>>();
            paraList.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
            paraList.Add(new KeyValuePair<string, string>("client_id", clientId));
            paraList.Add(new KeyValuePair<string, string>("client_secret", clientSecret));

            HttpResponseMessage response = client.PostAsync(authHost, new FormUrlEncodedContent(paraList)).Result;
            string result = response.Content.ReadAsStringAsync().Result;
            //  textBox1.Text = result;"expires_in"

            dynamic resObj = JsonConvert.DeserializeObject(result);
            label2.Text = resObj.expires_in/86400;
            //Console.WriteLine(resObj.expires_in + "\n");
            Console.WriteLine(resObj.access_token + "\n");
            return (resObj.access_token);
        }

        public static string add()
        {
            string token = accesstoken;
            string host = "https://aip.baidubce.com/rest/2.0/face/v3/faceset/user/add?access_token=" + token;
            Encoding encoding = Encoding.Default;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(host);
            request.Method = "post";
            request.KeepAlive = true;
            String str = "{\"image\":\"" + base641 + "\",\"image_type\":\"BASE64\",\"group_id\":\"test\",\"user_id\":\"shiyifan\",\"user_info\":\"syf\",\"quality_control\":\"LOW\",\"liveness_control\":\"NORMAL\",\"action_type\":\"APPEND\"}";
            byte[] buffer = encoding.GetBytes(str);
            request.ContentLength = buffer.Length;
            request.GetRequestStream().Write(buffer, 0, buffer.Length);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.Default);
            string result = reader.ReadToEnd();
           // Console.WriteLine("人脸注册:");
           // Console.WriteLine(result);
           
            return result;
        }



        public  string  getList()
        {
            string token = accesstoken;
            string host = "https://aip.baidubce.com/rest/2.0/face/v3/faceset/face/getlist?access_token=" + token;
            Encoding encoding = Encoding.Default;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(host);
            request.Method = "post";
            request.KeepAlive = true;
            String str = "{\"user_id\":\"shiyifan\",\"group_id\":\"test\"}";
            byte[] buffer = encoding.GetBytes(str);
            request.ContentLength = buffer.Length;
            request.GetRequestStream().Write(buffer, 0, buffer.Length);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.Default);
            string result = reader.ReadToEnd();
            // Console.WriteLine("获取用户人脸列表:");
            //Console.WriteLine(result);
            JObject resObj = JObject.Parse(result);


            //  var token1 = from what in resObj["result"]["face_list"].Children()
            //                select (string)what["face_token"];

            //JToken success = resObj["result"]["face_list"][0]["face_token"];
            JToken success = resObj["result"]["face_list"];
            JArray length = JArray.Parse(success.ToString());

            // foreach (var token2 in token1)
            //    Console.WriteLine(token2);
            for(int i=0;i< length.Count();i++)
            {
                // textBox1.lin
                textBox1.Text = textBox1.Text + resObj["result"]["face_list"][i]["face_token"].ToString() +System.Environment.NewLine;
            }
            return (result);
        }

        static Byte[] ImgToBase64String(string imagePath)

        {
  
            FileStream fs = new FileStream(imagePath, FileMode.Open);
            byte[] byteData = new byte[fs.Length];
            fs.Read(byteData, 0, byteData.Length);
            fs.Close();

            return byteData;

        }


    }
}
