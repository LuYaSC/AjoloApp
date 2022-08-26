using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP.Core.Business
{
    public class TrimAllStringProperty : IMappingAction<object, object>
    {
        public void Process(object source, object destination, ResolutionContext context)
        {
            var stringProperties = destination.GetType().GetProperties().Where(p => p.PropertyType == typeof(string));
            foreach (var stringProperty in stringProperties)
            {
                string currentValue = (string)stringProperty.GetValue(destination, null);
                if (currentValue != null)
                    stringProperty.SetValue(destination, currentValue.Trim().ToUpper(), null);
            }
        }
    }
}
