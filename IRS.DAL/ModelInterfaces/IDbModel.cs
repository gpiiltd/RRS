using System;
using System.Collections.Generic;
using System.Text;

namespace IRS.DAL.ModelInterfaces
{
    public interface IDbModel
    {
        /// <summary>
		/// The primary key. Required in Entity framework.
		/// </summary>
		Guid Id { get; set; }
    }
}
