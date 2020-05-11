using System;
using AN.Integration.SyncToDynamics.Job.Extensions;

namespace AN.Integration.SyncToDynamics.Job.Services
{
    public sealed class DataInstance : IDataInstance
    {
        public T GetInstanceForUpsert<T>(object body)
        {
            var value = body.GetFirstPropertyValueByInterface<T>() ??
                        throw new ArgumentException($"{body.GetTypeName()} doesn't contain" +
                                                    $" {nameof(T)} property");
            return value;
        }

        public T GetInstanceForDelete<T>(object body)
        {
            var recordKey = body.GetSinglePropertyValue<T>();
            var genericArgument = body.GetSingleGenericArgument();
            var value = (T) Activator.CreateInstance(genericArgument, recordKey) ??
                        throw new Exception("Unable to create instance of" +
                                            $" {genericArgument.GetTypeName()}");
            return value;
        }
    }
}