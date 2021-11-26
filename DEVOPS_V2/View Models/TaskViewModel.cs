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
    internal class TaskViewModel : BaseViewModel
    {
        #region Task Model Variables
        // XAML Command

        public ICommand GetData { get; set; }
        public ICommand PostData { get; set; }
        public ICommand StartData { get; set; }
        public ICommand LoadData { get; set; }

        public ObservableCollection<GetStart.Start_Value> Starts
        {
            get => starts;
            set => SetProperty(ref starts, value);
        }
        public GetStart.Start_Value Start
        {
            get => start;
            set => SetProperty(ref start, value);
        }
        public ObservableCollection<Types> Types
        {
            get => types;
            set => SetProperty(ref types, value);
        }
        public Types Type
        {
            get => type;
            set => SetProperty(ref type, value);
        }
        public ObservableCollection<Value> Collection
        {
            get => collection;
            set => SetProperty(ref collection, value);
        }
        public Fields Data
        {
            get => data;
            set => SetProperty(ref data, value);
        }
        public Uri_var Model
        {
            get => model;
            set => SetProperty(ref model, value);
        }

        public event Action<bool> OnStartChanged;
        public event Action<bool> OnUploadSuccess;

        private ObservableCollection<Value> collection;
        private ObservableCollection<GetStart.Start_Value> starts;
        private ObservableCollection<Types> types;
        private GetStart.Start_Value start;
        private Types type;
        private Fields data;
        private Uri_var model;
        #endregion
        #region Constructors
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

            StartData = new RelayCommand(async () => await AuthtAsync());
            LoadData = new RelayCommand(async () => await LoadComboData());
            LoadData.Execute(null);
        }
        #endregion
        #region Start Auth Task
        private async Task AuthtAsync()
        {
            string url = $"{Model.BaseUrl}/{Model.Organisasi}/_apis/teams?api-version=6.0-preview.3";
            using HttpClient httpclient = new HttpClient();
            bool flag = false;
            HttpResponseMessage data = new HttpResponseMessage();

            httpclient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", "", Model.PAT))));

            try
            {
                data = await httpclient.GetAsync(url);
                string tmpapi = data.Content.ReadAsStringAsync().Result.ToString();

                GetStart.Start_Root list = JsonConvert.DeserializeObject<GetStart.Start_Root>(tmpapi);
                flag = list.count > 0;
            }
            catch (Exception)
            {
                flag = false;
            }
            OnStartChanged?.Invoke(flag);
        }
        #endregion
        #region Combobox data load
        private async Task LoadComboData()
        {
            if (App.Model.PAT != null)
            {
                Model = App.Model;
                string url = $"{Model.BaseUrl}/{Model.Organisasi}/_apis/teams?api-version=6.0-preview.3";
                using HttpClient httpclient = new HttpClient();
                HttpResponseMessage data = new HttpResponseMessage();

                httpclient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", "", Model.PAT))));

                data = await httpclient.GetAsync(url);
                string tmpapi = data.Content.ReadAsStringAsync().Result.ToString();

                GetStart.Start_Root list = JsonConvert.DeserializeObject<GetStart.Start_Root>(tmpapi);
                foreach (GetStart.Start_Value item in list.value)
                {
                    starts.Add(item);
                }

                types.Add(new Types { id = 1, name = "User Story" });
                types.Add(new Types { id = 2, name = "Task" });
                types.Add(new Types { id = 3, name = "Bug" });
            }
        }
        #endregion
        #region CREATE WIT TASK
        private async Task ApiPostAsync()
        {
            await Task.Delay(0);
            string url = string.Join("?", string.Join("/", model.BaseUrl, model.Organisasi, Start.projectName, "_apis/wit/workitems", "$" + type.name.Replace(" ", "%20")), model.Api_Ver);
            bool state = false;
            using HttpClient httpclient = new HttpClient();
            try
            {
                HttpResponseMessage data = new HttpResponseMessage();

                httpclient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", "", model.PAT))));

                List<SendItem> content = new List<SendItem> {
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
                StringContent request = new StringContent(jsonstr, Encoding.UTF8, "application/json-patch+json");
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
            string url = $"{Model.BaseUrl}/{Model.Organisasi}/{Start.projectName}/{Start.name}/_apis/wit/wiql?{Model.Api_Ver}";
            using HttpClient httpclient = new HttpClient();
            try
            {
                HttpResponseMessage data = new HttpResponseMessage();

                httpclient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", "", Model.PAT))));

                Wiql content = new Wiql
                {
                    Query = $"Select [System.Id] From WorkItems Where [System.WorkItemType] = '{type.name}' And [System.TeamProject] = '{Start.projectName}'",
                };
                string jsonstr = JsonConvert.SerializeObject(content);
                StringContent request = new StringContent(jsonstr, Encoding.UTF8, "application/json");

                data = await httpclient.PostAsync(url, request);

                string tmp = data.Content.ReadAsStringAsync().Result;
                WkQuery workitemlist = JsonConvert.DeserializeObject<WkQuery>(tmp);

                List<int> ids = new List<int>();
                foreach (WkItem model in workitemlist.WorkItems)
                {
                    ids.Add(model.Id);
                }

                // GET https://dev.azure.com/{ORG}/{PROJECT}/_apis/wit/workitems?ids={Ids}&fields={Fields}&{API_Version}
                string uri = string.Join("/", model.BaseUrl, model.Organisasi, Start.projectName, "_apis/wit/workitems?ids=");
                uri += string.Join("&", string.Join(",", ids), "fields=System.Title,System.State,System.Description,System.Boardcolumn,System.CreatedDate", model.Api_Ver);

                data = await httpclient.GetAsync(uri);
                string tmpsub = data.Content.ReadAsStringAsync().Result.ToString();
                string Jstring = tmpsub.Replace("</div>", "\n").Replace("<div>", "").Replace("<br>", "\n");
                ValueRoot list = JsonConvert.DeserializeObject<ValueRoot>(Jstring);

                if (list.Value.Count > 0)
                {
                    collection.Clear();
                    foreach (Value item in list.Value)
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