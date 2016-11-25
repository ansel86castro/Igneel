using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel
{
    public interface IAffectable
	{
      
        /// <summary>
        /// Inverse of the affectors`s world pose when it is linked to this Instance. This allows to tranform properly the instance by an affector
        /// </summary>
        Matrix BindAffectorPose { get; set; }

        /// <summary>
        /// Instance of an object that influences or affect the globalPose.
        /// </summary>
        IAffector Affector { get; set; }

        /// <summary>
        /// This method is called when the affector has influenced his affectable instance
        /// and the affactable needs to updates its GlobalPose. For Physics simulated objects this method is called after
        /// a simulation frame is completed
        /// </summary>
        void UpdatePose(Matrix affectorPose);
      		
	}
}
