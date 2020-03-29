using System;
using System.Collections.Generic;
using System.Text;

namespace IRS.DAL.ModelInterfaces
{
    public interface IProtectable
    {
        /// <summary> If this flag is set the record can not be deleted </summary>
		bool Protected { get; set; }
    }
}
