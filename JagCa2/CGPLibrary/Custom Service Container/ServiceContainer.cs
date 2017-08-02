using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JagCa2;

namespace CGPLibrary
{
    //services - this is how we could write our own service container
    public class ServiceContainer : IServiceProvider
    {
        Dictionary<Type, object> myservices
            = new Dictionary<Type, object>();

        //must implement AddService and RemoveService. Why?
        //because we are implementing IServiceProvider
        public void AddService(Type Service, object Provider)
        {
            if (ContainsService(Service))
            {
                //how to throw an exception in c#
                throw new Exception("add error here...");
            }
            else
            {
                myservices.Add(Service, Provider);
            }
        }

        // Get a service from the service container
        public object GetService(Type Service)
        {
            // If we have this type of service, return it
            foreach (Type type in myservices.Keys)
            {
                if (type == Service)
                {
                    return myservices[type];
                }
            }

            //if we get to here then the service wasn't found; so throw exception
            throw new Exception("BOOM! SERVICE CONTAINER ERROR!! EXPLOSIONS?");
        }

        public void RemoveService(Type Service)
        {
            if (myservices.ContainsKey(Service))
            {
                myservices.Remove(Service);
            }
        }

        public bool ContainsService(Type Service)
        {
            return myservices.ContainsKey(Service);
        }

    }
}
