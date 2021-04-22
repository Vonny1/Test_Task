using Newtonsoft.Json;
using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace JobManagerMVC_2.Models
{
    public partial class Job
    {
        public Job()
        {
            InverseParent = new HashSet<Job>();
        }

        public int Id { get; set; }
        public int? ParentId { get; set; } //Id родительской задачи
        public string Name { get; set; } //Название
        public string Description { get; set; } //Описание
        public int? PlannedTimeMin { get; set; } //Планируемая начальная трудоемкость в минутах
        public int? ActualTimeMin { get; set; } 
        public string Asignee { get; set; } //Исполнители
        public DateTime? RegDate { get; set; } //Дата регистрации
        public DateTime? CompleteDate { get; set; } //Дата выполнения
        public int? StatusId { get; set; } // Статус задачи
        public int? PlannedTimeSum { get; set; } //Планируемая трудоемкость, вычисляемая из сумм подзадач
        public int? ActualTimeSum { get; set; }

        [JsonIgnore]
        public virtual Job Parent { get; set; }
        public virtual Status Status { get; set; }
        public virtual ICollection<Job> InverseParent { get; set; }
    }
}
