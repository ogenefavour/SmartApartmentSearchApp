using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Smart.Apartment.Application.Contracts.Infrastructure
{
    public interface IUploadService
    {
        Task BulkDocumentUpload();
    }
}
