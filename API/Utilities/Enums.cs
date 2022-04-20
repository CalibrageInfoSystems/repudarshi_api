using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
   public class Enums
    {
        public enum ActivityRights
        {
            CanViewSettings = 1,
            CanManageSettings = 2,
            CanViewAreaExtension = 3,
            CanManageAreaExtension = 4,
            CanViewConversion = 5,
            CanManageConversion = 6,
            CanViewCropMaintenance = 7,
            CanManageCropMaintenance = 8,
            CanViewComplaints = 9,
            CanManageComplaints = 10,
            CanViewCollectionCenter = 11,
            CanManageCollectionCenter = 12
        };
        public enum Moduletype
        {
            Plantation=303
        }
        public enum Module
        {
            Farmer= 181,
            Plot=182,
            Collection= 183,
            Consignment = 184,
            PartialUprootment = 283,
            PlotSplit = 237,
            CollectionOrConsignment=306,
            AdvancedDetails=311,
            NurserySaplingDetails=312,
            CollectionFarmer=313,
            CollectionFarmerPersonalDetails = 314,
            CollectionFarmerBank=315,
            CollectionFarmerRegistation=316,
            EditCollectionFarmerCollection=317,
            DeletePlot=320,
            DeleteFarmer=321,
            ConsignmentSent=326,
            ConsignmentReceived=327
        };

        public enum PlotStatus
        {
            GeoBoundariesPending = 258,
            GeoBoundariesDone = 259,
            Underplanting=308
        };

        public enum WeighbridgeTokenCategory
        {
            Collection = 203,
            Consignment = 204
        };

        public enum ReasonType
        {
            PlotSplit = 280,
            PartialUprootment = 281,
            Uprootment = 282,
            OwnershipChange= 284,
            Underplanting =309,
            DeletePlot=318,
            DeleteFarmer=319
        };

        public enum ComplaintCriticality
        {
            VeryHigh = 98,
            High = 97,
            Medium = 96,
            Low = 95
        };

        public enum AlertType
        {
            ComplaintAssigned=285,
            ComplaintRaised=286,
            NewConversion=287,
            NewPlotIdentified=288,
            PlotSplit =289,
            Partialuprootment=290,
            PlotApprovedforConversion=291,
            PestIdentified=292,
            DiseaseIdentified=293,
            NutrientDeficiencyIdentified=294,
            SaplingsLifted=295,
            Retakegeoboundaries=296,
            PlotRejected=297,
            PlotDeclined=298,
            Pushmessagefromsomeone=299,
            Underplanting=310
        };
        public enum StatusType
        {
            Prospective= 81,
            ReadytoConvert= 82,
            Approved= 83,
            Declined= 84,
            Converted= 85,
            Uprooted= 86,
            NotPlanted= 87,
            CurrentCropNonYielding= 88,
            CurrentCropYielding= 89,
            GeoBoundariesPending=258,
            GeoBoundariesDone= 259,
            Rejected=323
        }
        public enum ComplaintStatusType
        {
            Assigned=199,
            Reassigned=200,
            Resolved=201,
            Done=202,
            Created=254,
            WorkInProgress=255
        }
        public enum GeoCategories
        {
            GeoBoundaries=206,
            GeoTag=207
        }

    }
}
