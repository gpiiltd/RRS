using IRS.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRS.API.Dtos
{
    public class MediaResource
    {
        public Guid? Id { get; set; }
        public string FileName { get; set; }

        public string Uri { get; set; }

        public DateTime DateUploaded { get; set; }

        public DateTime? DateEdited { get; set; }
        public Guid? IncidenceId { get; set; }

        public FileUploadChannels? FileUploadChannel { get; set; }

        public string Description { get; set; }

        public bool IsVideo { get; set; }
    }
}
