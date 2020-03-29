using System;
using System.Collections.Generic;
using System.Text;

namespace IRS.DAL.ModelInterfaces
{
    public interface INomenclature: IDbModel, IProtectable, IPseudoDeletable
	{
		string Name { get; set; }

        string Description { get; set; }
    }
}
