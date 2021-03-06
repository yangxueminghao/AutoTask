﻿using AutoTask.Helper.Model;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AutoTask.Helper
{

    public static class RequestHelper
    {

        public static string POSTAsync(string host, string path, RequestBody data = null)
        {
            try
            {
                var datetime = DateTime.Now.ToString();
                var auth = path + "@" + datetime;
                var header = new RequestHeader()
                {
                    Act = path,
                    Auth = DESHelper.DESEncode(auth),
                    Method = string.Empty,
                    Source = "TS",
                    Ver = "2.0"
                };

                var client = new JsonServiceClient(host);
                client.Timeout = TimeSpan.FromDays(1);

                //Task.Factory.StartNew(() =>
                //{
                //    var response = client.Post<string>(path, new RequestInfo { Header = header, Body = data ?? new RequestBody() });
                //    Trace.WriteLine(response);
                //});

                //return string.Empty;
                var response = client.PostAsync<string>(path, new RequestInfo { Header = header, Body = data ?? new RequestBody() });
                Trace.WriteLine(response.Result);

                return response.Result;
            }
            catch (Exception e)
            {
                Trace.Write(e.Message);
            }

            return string.Empty;
        }

        /// <summary>
        /// POST请求网络地址，未测试
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string POST(string url, string data = "")
        {
            string result = string.Empty;

            Stream responseStream = null;
            Stream postStream = null;
            StreamReader readerStream = null;
            try
            {
                var request = WebRequest.Create(url);
                request.Method = "POST";
                request.Timeout = 30000;

                byte[] postData = Encoding.UTF8.GetBytes(data);
                request.ContentLength = postData.Length;
                postStream = request.GetRequestStream();
                postStream.Write(postData, 0, postData.Length);

                responseStream = request.GetResponse().GetResponseStream();
                readerStream = new StreamReader(responseStream, Encoding.UTF8);
                result = readerStream.ReadToEnd();
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
            }
            finally
            {
                if (responseStream != null)
                    responseStream.Close();

                if (postStream != null)
                    postStream.Close();

                if (readerStream != null)
                    readerStream.Close();
            }


            return result;
        }

        /// <summary>
        /// GET请求网络地址
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GET(string url)
        {
            string result = string.Empty;

            Stream responseStream = null;
            StreamReader readerStream = null;
            try
            {
                var request = WebRequest.Create(url);
                request.Method = "GET";
                request.Timeout = 30000;

                responseStream = request.GetResponse().GetResponseStream();
                readerStream = new StreamReader(responseStream, Encoding.UTF8);
                result = readerStream.ReadToEnd();
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
            }
            finally
            {
                if (responseStream != null)
                    responseStream.Close();

                if (readerStream != null)
                    readerStream.Close();
            }


            return result;
        }



    }
}
