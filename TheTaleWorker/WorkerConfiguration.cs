using System;
using System.Collections.Generic;
using System.Text;

namespace TheTaleWorker
{
    [Serializable]
    public class WorkerConfiguration
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string ConnectionString { get; set; }

        public string EmailFrom { get; set; }
        public string EmailTo { get; set; }
        public string EmailLogin { get; set; }
        public string EmailPassword { get; set; }
    }
}