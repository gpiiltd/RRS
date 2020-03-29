using System;
using System.Collections.Generic;
using System.Text;

namespace IRS.DAL.ModelInterfaces
{
    public interface ICreateLoggable
    {
        /// <summary>
		/// Timestamp of creation
		/// </summary>
		DateTime DateCreated { get; set; }
        /// <summary>
        /// User that made creation
        /// </summary>
        Guid CreatedByUserId { get; set; }
    }
}
