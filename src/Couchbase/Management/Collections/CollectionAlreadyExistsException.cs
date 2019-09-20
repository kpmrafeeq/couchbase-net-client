using System;

namespace Couchbase.Management.Collections
{
    public class CollectionAlreadyExistsException : Exception
    {
        public CollectionAlreadyExistsException(string scopeName, string collectionName)
            : base($"Collection with name {collectionName} already exists in scope {scopeName}")
        { }
    }
}