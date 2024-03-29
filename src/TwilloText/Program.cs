﻿using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TwilloText
{
    public class Message
    {
        // properties of the message that we want.
        // 
        public string To { get; set; }
        public string From { get; set; }
        public string Body { get; set; }
        public string Status { get; set; }
    }
    class progarm
    {
        static void Main(string[] args)
        {
            var client = new RestClient("https://api.twilio.com/2010-04-01");
            //1- We're making a GET request to the URL now. We've tacked on .json to the end of the URL to get the response in JSON format.
            var request = new RestRequest("Accounts/ACdd14ea166a41abcac32ae9ed80d6f10c/Messages.json", Method.GET);
            client.Authenticator = new HttpBasicAuthenticator("ACdd14ea166a41abcac32ae9ed80d6f10c", "ec711bf2721a30f2d7cf4594a66a28a2");

            //2-We initialize a new RestResponse variable named response
            var response = new RestResponse();

            //3a-The request is made with an asynchronous method, and Task.Run with Wait() allows us to await asynchronous calls in a "synchronous" way. 
            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();


            // we turn a giant string stored as response.content into JSON data.
            //JasonConvert.DeserializeObject()<Jobject>(respoonse.content) converts the JSON-formatted string response.Content into a JObject.
            // JObject comes from the Newtonsoft.Json.Linq library and is a .NET object we can treat as JSON.

            // DeserializeObject went into the data for each message and found those keys to create Message objects for us. from our class message.
            JObject jsonResponse = JsonConvert.DeserializeObject<JObject>(response.Content);

            // program to convert the string jsonResponse["messages"]into a list of Message objects, 
            var messageList = JsonConvert.DeserializeObject<List<Message>>(jsonResponse["messages"].ToString());
            // for each loop through and print the properties of each Message
            foreach (var message in messageList)
            {
                // these are the property keys for our class which we gave at the top off the page.
                // property name has to match the JSON key
                Console.WriteLine("To: {0}", message.To);
                Console.WriteLine("From: {0}", message.From);
                Console.WriteLine("Body: {0}", message.Body);
                Console.WriteLine("Status: {0}", message.Status);
            }
            Console.ReadLine();
        }




    //3b- We set response equal to the response from our request, which we make in the method shown in 3b, and then cast as the type RestResponse.
        public static Task<IRestResponse> GetResponseContentAsync(RestClient theClient, RestRequest theRequest)
        {
            var tcs = new TaskCompletionSource<IRestResponse>();
            theClient.ExecuteAsync(theRequest, response => {
                tcs.SetResult(response);
            });
            return tcs.Task;
        }
        // deserializing- We can actually pull this array out as a JSON object.
    }
}
    
    
  

 
     


