using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.WindowsAzure.MobileServices;
using MvvmHelpers;
using Newtonsoft.Json;

namespace AuthTestThree.Models
{
    public class TodoItem : ObservableObject
    {
        string id;
        string name;
        bool done;

        [JsonProperty(PropertyName = "id")]
        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        [JsonProperty(PropertyName = "text")]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [JsonProperty(PropertyName = "complete")]
        public bool Done
        {
            get { return done; }
            set { done = value; }
        }

        [Version]
        public string Version { get; set; }
    }
}