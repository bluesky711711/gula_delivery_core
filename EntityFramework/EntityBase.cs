using ErpCore.RepoFramework;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace ErpCore.EntityFramework
{
    public abstract class EntityBase : ObjectStateBase
    {
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public DateTime? DeleteAt { get; set; }

        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
      
        public string DeleteBy { get; set; }

        public bool? IsDeleted { get; set; } = false;

       
    }

    //public abstract class EntityBase2 
    //{
    //    public DateTime? CreatedAt { get; set; }
    //    public DateTime? UpdatedAt { get; set; }

    //    public DateTime? DeleteAt { get; set; }

    //    public string CreatedBy { get; set; }
    //    public string UpdatedBy { get; set; }

    //    public string DeleteBy { get; set; }

    //    public bool IsDeleted { get; set; } = false;


    //}
}
