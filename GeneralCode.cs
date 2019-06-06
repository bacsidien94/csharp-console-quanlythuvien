using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quanlythuvien
{

    class GeneralCode
    {
        //các thiết lập đơn giá để tính tiền quá hạn theo loại sách
        int freeDays;
        public int FreeRentingDay { get { return freeDays; } set { freeDays = value; } }
        double standardRateVN; public double StandardRateVN { get { return standardRateVN; } set { standardRateVN = value; } }
        double standardRateEN; public double StandardRateEN { get { return standardRateEN; } set { standardRateEN = value; } }

        public GeneralCode(int _freeRentingDays, double _standardRateVN, double _standardRateEN)
        {
            this.freeDays = _freeRentingDays;
            this.standardRateVN = _standardRateVN;
            this.standardRateEN = _standardRateEN;
        }
    }

}
