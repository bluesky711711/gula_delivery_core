using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using static ErpCore.RepoFramework.RepoConstants;

namespace ErpCore.RepoFramework
{
   

    public interface IObjectState
    {
        [NotMapped]
        ObjectStates ObjectState { get; set; } 
    }


    public abstract class ObjectStateBase : IObjectState
    {
        [NotMapped]
        public ObjectStates ObjectState { get; set; }

    }

   
}
