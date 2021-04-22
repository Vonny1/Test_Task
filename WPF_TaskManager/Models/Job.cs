using Newtonsoft.Json;
using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WPF_TaskManager.Models
{
    public partial class Job
    {
        public Job()
        {
            InverseParent = new HashSet<Job>();
        }

        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? PlannedTimeMin { get; set; }
        public int? ActualTimeMin { get; set; }
        public string Asignee { get; set; }
        public DateTime? RegDate { get; set; }
        public DateTime? CompleteDate { get; set; }
        public int? StatusId { get; set; }
        public int? PlannedTimeSum { get; set; }
        public int? ActualTimeSum { get; set; }

        [JsonIgnore]
        public virtual Job Parent { get; set; }
        public virtual Status Status { get; set; }
        public virtual ICollection<Job> InverseParent { get; set; }
    }
}
