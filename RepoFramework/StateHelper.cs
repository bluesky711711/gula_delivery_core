using System;
using System.Data.Entity;
using static ErpCore.RepoFramework.RepoConstants;

namespace ErpCore.RepoFramework
{
    public class StateHelper
    {
        public static EntityState ConvertState(ObjectStates state)
        {
            switch (state)
            {
                case ObjectStates.Added:
                    return EntityState.Added;

                case ObjectStates.Modified:
                    return EntityState.Modified;

                case ObjectStates.Deleted:
                    return EntityState.Deleted;

                default:
                    return EntityState.Unchanged;
            }
        }

        public static ObjectStates ConvertState(EntityState state)
        {
            switch (state)
            {
                case EntityState.Detached:
                    return ObjectStates.Unchanged;

                case EntityState.Unchanged:
                    return ObjectStates.Unchanged;

                case EntityState.Added:
                    return ObjectStates.Added;

                case EntityState.Deleted:
                    return ObjectStates.Deleted;

                case EntityState.Modified:
                    return ObjectStates.Modified;

                default:
                    throw new ArgumentOutOfRangeException("state");
            }
        }
    }
}
