using System;
using System.Collections.Generic;
using System.Text;

namespace IRS.DAL.ModelInterfaces
{
    public interface IEditLoggable
    {
        /// <summary>
		/// Timestamp of last changes
		/// </summary>
		DateTime? DateEdited { get; set; }
        /// <summary>
        /// User that made changes
        /// </summary>
        Guid? EditedByUserId { get; set; }
    }
}
