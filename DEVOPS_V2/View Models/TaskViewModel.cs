using DEVOPS_V2.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;

namespace DEVOPS_V2.View_Models
{
    class TaskViewModel : BaseViewModel
    {
        #region Task Model Variables
        // XAML Command
        public ICommand GetData { get; set; }
        public ICommand PostData { get; set; }

        public ObservableCollection<Value> Collection
        {
            get
            {
                return collection;
            }
            set
            {
                SetProperty(ref collection, value);
            }
        }
        public Fields Data
        {
            get
            {
                return data;
            }
            set
            {
                SetProperty(ref data, value);
            }
        }
        public Uri_var Model
        {
            get
            {
                return model;
            }
            set
            {
                SetProperty(ref model, value);
            }
        }
        private ObservableCollection<Value> collection;
        private Fields data;
        private Uri_var model;
        #endregion
        public TaskViewModel()
        {
            collection = new ObservableCollection<Value>();

            Data = new Fields();
            Model = new Uri_var();
            PostData = new RelayCommand(async () => await ApiPostAsync());
            GetData = new RelayCommand(async () => await ApiGetAsync());
        }
        #region CREATE WIT TASK
        private async Task ApiPostAsync()
        {
            await Task.Delay(0);
            var url = string.Join("?", string.Join("/", model.BaseUrl, model.Organisasi, model.Project, "_apis/wit/workitems", "$Issue"), model.Api_Ver);
            using (var httpclient = new HttpClient())
            {
                var data = new HttpResponseMessage();

                httpclient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", "", model.PAT))));

                var content = new List<SendItem> {
                    new SendItem {
                        Op = "add",
                        Path = "/fields/System.Title",
                        From = null,
                        Value = Data.SystemTitle,
                    },
                    new SendItem {
                        Op = "add",
                        Path = "/fields/System.Description",
                        From = null,
                        Value = Data.SystemDescription.Replace("\r\n", "<br>"),
                    }
                };
                string jsonstr = JsonConvert.SerializeObject(content).ToString();
                var request = new StringContent(jsonstr, Encoding.UTF8, "application/json-patch+json");
                await httpclient.PostAsync(url, request);
            }
        }
        #endregion
        #region GET WIT TASK
        private async Task<ObservableCollection<Value>> ApiGetAsync()
        {
            //var url = $"{model.BaseUrl}/{model.Organisasi}/{model.Project}/{model.Team}/_apis/wit/wiql?{model.Api_Ver}";
            var url = $"{model.BaseUrl}/{model.Organisasi}/{model.Project}/TEST1Team/_apis/wit/wiql?{model.Api_Ver}";
            using (var httpclient = new HttpClient())
            {
                var data = new HttpResponseMessage();

                httpclient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", "", model.PAT))));
                httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", "", "zkoadtck5epdguan2byd6k4f4ezppruhctx2xdjlro24vv5dyiqq"))));

                var content = new Wiql
                {
                    Query = "Select [System.Id] From WorkItems Where [System.WorkItemType] = 'User Story'",
                };
                var jsonstr = JsonConvert.SerializeObject(content);
                var request = new StringContent(jsonstr, Encoding.UTF8, "application/json");

                data = await httpclient.PostAsync(url, request);

                var tmp = data.Content.ReadAsStringAsync().Result;
                var workitemlist = JsonConvert.DeserializeObject<WkQuery>(tmp);

                var ids = new List<int>();
                foreach (var model in workitemlist.WorkItems)
                {
                    ids.Add(model.Id);
                }

                // GET https://dev.azure.com/muhamadhafizanas/TEST1/_apis/wit/workitems?ids=13&fields=System.Title&api-version=6.0
                //var uri = string.Join("/", model.BaseUrl, model.Organisasi, model.Project, "_apis/wit/workitems?ids=");
                var uri = string.Join("/", model.BaseUrl, "muhamadhafizanas", "TEST1", "_apis/wit/workitems?ids=");
                uri += string.Join("&", string.Join(",", ids), "fields=System.Title,System.State,System.Description,System.Boardcolumn,System.CreatedDate", model.Api_Ver);

                data = await httpclient.GetAsync(uri);
                string tmpsub = data.Content.ReadAsStringAsync().Result.ToString();
                string Jstring = tmpsub.Replace("</div>", "\n").Replace("<div>", "").Replace("<br>", "\n");
                var list = JsonConvert.DeserializeObject<ValueRoot>(Jstring);

                if (list.Value.Count > 0)
                {
                    collection.Clear();
                    foreach (var item in list.Value)
                    {
                        collection.Add(item);
                    }
                }
                return collection;
            }
        }
        #endregion
    }
}