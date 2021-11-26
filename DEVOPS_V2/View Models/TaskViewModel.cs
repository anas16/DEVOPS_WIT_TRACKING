using DEVOPS_V2.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DEVOPS_V2.View_Models
{
    class TaskViewModel : BaseViewModel
    {
        #region Task Model Variables
        // XAML Command
        public bool LoginState { get; set; }

        public ICommand GetData { get; set; }
        public ICommand PostData { get; set; }
        public ICommand StartData { get; set; }
        public ICommand LoadData { get; set; }

        public ObservableCollection<GetStart.Start_Value> Starts
        {
            get
            {
                return starts;
            }
            set
            {
                SetProperty(ref starts, value);
            }
        }

        public GetStart.Start_Value Start
        {
            get
            {
                return start;
            }
            set
            {
                SetProperty(ref start, value);
            }
        }

        public ObservableCollection<Types> Types
        {
            get
            {
                return types;
            }
            set
            {
                SetProperty(ref types, value);
            }
        }

        public Types Type
        {
            get
            {
                return type;
            }
            set
            {
                SetProperty(ref type, value);
            }
        }

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

        public event Action<bool> OnStartChanged;
        public event Action<bool> OnUploadSuccess;
        public event Action<bool> OnGetFailed;

        private ObservableCollection<Value> collection;
        private ObservableCollection<GetStart.Start_Value> starts;
        private ObservableCollection<Types> types;
        private GetStart.Start_Value start;
        private Types type;
        private Fields data;
        private Uri_var model;
        #endregion

        public TaskViewModel()
        {
            collection = new ObservableCollection<Value>();
            starts = new ObservableCollection<GetStart.Start_Value>();
            types = new ObservableCollection<Types>();

            Start = new GetStart.Start_Value();
            Type = new Types();
            Data = new Fields();
            Model = new Uri_var();

            PostData = new RelayCommand(async () => await ApiPostAsync());
            GetData = new RelayCommand(async () => await ApiGetAsync());

            StartData = new RelayCommand(async () => await StartAsync());
            LoadData = new RelayCommand(async () => await LoadComboData());
            LoadData.Execute(null);

            Model.PAT = "zkoadtck5epdguan2byd6k4f4ezppruhctx2xdjlro24vv5dyiqq";
            Model.Organisasi = "muhamadhafizanas";
            //Model.Project = "TEST1";
        }

        private async Task StartAsync()
        {
            var url = $"https://dev.azure.com/{Model.Organisasi}/_apis/teams?api-version=6.0-preview.3";
            using var httpclient = new HttpClient();
            var flag = false;
            var data = new HttpResponseMessage();

            httpclient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", "", Model.PAT))));

            try
            {
                data = await httpclient.GetAsync(url);
                string tmpapi = data.Content.ReadAsStringAsync().Result.ToString();

                var list = JsonConvert.DeserializeObject<GetStart.Start_Root>(tmpapi);
                flag = list.count > 0;
            }
            catch (Exception)
            {
                flag = false;
            }
            OnStartChanged?.Invoke(flag);
        }

        private async Task LoadComboData()
        {
            if (App.Model.PAT != null)
            {
                Model = App.Model;
                var url = $"https://dev.azure.com/{Model.Organisasi}/_apis/teams?api-version=6.0-preview.3";
                using var httpclient = new HttpClient();
                var data = new HttpResponseMessage();

                httpclient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", "", Model.PAT))));

                data = await httpclient.GetAsync(url);
                string tmpapi = data.Content.ReadAsStringAsync().Result.ToString();

                var list = JsonConvert.DeserializeObject<GetStart.Start_Root>(tmpapi);
                foreach (var item in list.value)
                {
                    starts.Add(item);
                }

                types.Add(new Types { id = 1, name = "User Story" });
                types.Add(new Types { id = 2, name = "Task" });
                types.Add(new Types { id = 3, name = "Bug" });
            }
        }
        #region CREATE WIT TASK
        private async Task ApiPostAsync()
        {
            await Task.Delay(0);
            var url = string.Join("?", string.Join("/", model.BaseUrl, model.Organisasi, Start.projectName, "_apis/wit/workitems", "$" + type.name.Replace(" ", "%20")), model.Api_Ver);
            var state = false;
            using var httpclient = new HttpClient();
            try
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
                state = true;
            }
            catch (Exception)
            {
                state = false;
            }
            OnUploadSuccess?.Invoke(state);
        }
        #endregion
        #region GET WIT TASK
        private async Task<ObservableCollection<Value>> ApiGetAsync()
        {
            var url = $"{Model.BaseUrl}/{Model.Organisasi}/{Start.projectName}/{Start.name}/_apis/wit/wiql?{Model.Api_Ver}";
            using var httpclient = new HttpClient();
            try
            {
                var data = new HttpResponseMessage();

                httpclient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", "", Model.PAT))));

                var content = new Wiql
                {
                    Query = $"Select [System.Id] From WorkItems Where [System.WorkItemType] = '{type.name}' And [System.TeamProject] = '{Start.projectName}'",
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
                var uri = string.Join("/", model.BaseUrl, model.Organisasi, Start.projectName, "_apis/wit/workitems?ids=");
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
            }
            catch (Exception)
            {
                collection.Clear();
            }
            return collection;
        }
        #endregion
    }
}