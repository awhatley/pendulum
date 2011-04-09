using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Pendulum
{
    public class TaskRepository
    {
        private const string FileName = @"PendulumTasks.xml";

        public IEnumerable<ScheduledTask> GetPendingTasks()
        {
            var serializer = new DataContractSerializer(typeof(ScheduledTask));
            return LoadXml().Elements()
                .Select(x => serializer.ReadObject(x.CreateReader()))
                .Cast<ScheduledTask>()
                .Where(task => task.Status != TaskStatus.Running)
                .Where(task => task.IsEnabled)
                .Where(task => task.NextRunTime < DateTime.UtcNow)
                .Where(task => task.StartDate < DateTime.UtcNow)
                .Where(task => task.ExpirationDate > DateTime.UtcNow);
        }

        public void Save(ScheduledTask task)
        {
            ValidateTask(task);
            var element = new XDocument();
            using(var writer = element.CreateWriter())
                new DataContractSerializer(typeof(ScheduledTask)).WriteObject(writer, task);
            element.Root.RemoveAttributes();
            var document = LoadXml();
            var existing = document.Elements().FirstOrDefault(x => x.Element(XName.Get("Name", "http://schemas.datacontract.org/2004/07/Pendulum")).Value == task.Name);
            if(existing == null)
                document.Add(element.Root);
            else
                existing.ReplaceWith(element.Root);
            document.Save(FileName);
        }

        private XElement LoadXml()
        {
            var document = !File.Exists(FileName) 
                ? new XDocument(new XElement(XName.Get("Pendulum", "http://schemas.datacontract.org/2004/07/Pendulum")))
                : XDocument.Load(FileName);

            return document.Root;
        }

        private void ValidateTask(ScheduledTask task)
        {
            if(String.IsNullOrEmpty(task.Name))
                throw new ArgumentException("Task name is required.", "task");

            if(task.BindingType == null)
                throw new ArgumentException("Task binding type is required.", "task");

            if(task.EndpointAddress == null)
                throw new ArgumentException("Task endpoint address is required.", "task");
        }
    }
}